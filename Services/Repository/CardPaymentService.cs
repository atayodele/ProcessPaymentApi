using CheckoutApp.Exceptions;
using CheckoutApp.Extensions;
using CheckoutApp.Models;
using CheckoutApp.Services.Interfaces;
using CheckoutApp.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CheckoutApp.Services.Repository
{
    public class CardPaymentService : Service<CardPayment>, ICardPaymentService
    {
        private readonly IPaymentStateService _paymentStateServ;
        private readonly IGatewayService _gatewayServ;
        private readonly IPaymentModelStateService _paymentModelState;

        public CardPaymentService(
            IPaymentStateService paymentStateServ,
            IGatewayService gatewayServ,
            IPaymentModelStateService paymentModelState,
            IUnitOfWork iuow) : base(iuow)
        {
            _paymentStateServ = paymentStateServ;
            _gatewayServ = gatewayServ;
            _paymentModelState = paymentModelState;
        }


        public List<CardPayment> GetAllCardPayment()
        {
            return this.GetAll().Include(x => x.PaymentState).ToList();
        }

        public CardPayment GetCardPaymentById(Guid id)
        {
            var cardList = GetAllCardPayment();
            var card = cardList.FirstOrDefault(x => x.Id == id);
            return card;
        }

        public async Task<CardPaymentViewModel> ProcessPayment(CardPaymentViewModel model)
        {
            var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearCheck = new Regex(@"^20[0-9]{2}$");
            var cvvCheck = new Regex(@"^\d{3}$");

            if (!cvvCheck.IsMatch(model.Cvv)) throw new BadRequestException("Invalid Security code");

            var dateParts = model.ExpirationDate.Split('/');
            if (!monthCheck.IsMatch(dateParts[0]) || !yearCheck.IsMatch(dateParts[1])) throw new BadRequestException("Invalid Date Format");
            if (model.Amount <= 0) throw new BadRequestException("Amount cannot be zero(0)");
            // NormalizeCardNumber
            var normalizeCard = Helper.NormalizeCardNumber(model.CardNumber);
            // IsCardNumberValid
            var isCardValid = Helper.IsCardNumberValid(normalizeCard);
            //if card not valid throw error
            if(isCardValid == false) throw new BadRequestException("Invalid credit card");
            // HasCreditCardExpired
            var expiry = Helper.HasCreditCardExpired(model.ExpirationDate);
            // if expired, throw error
            if (expiry == true) throw new BadRequestException("Card Already expiry!");
            // GetCardType
            var cardType = Helper.GetCardType(normalizeCard);
            //if card is unknown throw error
            if (cardType == 0) throw new BadRequestException("Unknwon Credit Card!");
            //perform transaction
            var transaction = _paymentModelState.GetAllPaymentState();
            string[] state = transaction.Select(x => x.Name).ToArray();
            var transactionStatus = Helper.GenerateTransaction(state);
            if (model.Amount <= 20)
            {
                //perform ICheapPaymentGateway
                var gateway = _gatewayServ.GetGatewayById("ICheapPaymentGateway");
                if (gateway == null) throw new AnyErrorException("ICheapPaymentGateway is unavailable");
                await UseICheapPaymentGateway(model.CardNumber, model.CardHolder, model.Cvv, model.Amount, model.ExpirationDate, transactionStatus, gateway.Id);
            }
            else if (model.Amount >= 21 && model.Amount <= 500)
            {
                //perform IExpensivePaymentGateway
                var gateway = _gatewayServ.GetGatewayById("IExpensivePaymentGateway");
                if(gateway == null)
                {
                    //status of IExpensivePaymentGateway is false, not available use ICheapPaymentGateway
                    var cheapGateway = await UseICheapPaymentGateway(model.CardNumber, model.CardHolder, model.Cvv, model.Amount, model.ExpirationDate, transactionStatus, gateway.Id);
                }
                else
                {
                    //use IExpensivePaymentGateway
                    var expensiveGateway = await UseIExpensivePaymentGateway(model.CardNumber, model.CardHolder, model.Cvv, model.Amount, model.ExpirationDate, transactionStatus, gateway.Id);
                } 
            }
            else
            {
                //perform PremiumPaymentService
                var gateway = _gatewayServ.GetGatewayById("PremiumPaymentGateway");
                if (gateway == null) throw new AnyErrorException("PremiumPaymentGateway is unavailable");
                var expensiveGateway = await UsePremiumPaymentGateway(model.CardNumber, model.CardHolder, model.Cvv, model.Amount, model.ExpirationDate, transactionStatus, gateway.Id);
                if(expensiveGateway.Status != "processed")
                {
                    //retry 3 times
                    int i = 0;
                    do
                    {
                        var getState = _paymentModelState.GetAllPaymentState();
                        string[] stateList = transaction.Select(x => x.Name).ToArray();
                        var Status = Helper.GenerateTransaction(stateList);
                        i++;

                        if (i >= 3 || Status == "processed")
                        {
                            //perform update
                            var perform = await UpdatePaymentGateway(expensiveGateway.Id, i, Status);
                            break;
                        }
                        await UpdatePaymentGateway(expensiveGateway.Id, i, Status);
                    } while (i < 5);
                }
            }
            return model;
        }

        private async Task<int> UseICheapPaymentGateway(string cardNo, string cardName, string cvv, decimal amount, string expiry, string status, Guid id)
        {
            var paymentState = new PaymentState();
            paymentState.Retry_Count = 1;
            paymentState.Status = status;
            paymentState.Gateway_Id = id;

            var payment = CardPayment.Create(cardNo, cardName, cvv, amount, expiry);
            payment.Id = Guid.NewGuid();
            payment.CreatedOn = DateTimeOffset.Now;
            payment.PaymentState_Id = paymentState.Id;

            this.UnitOfWork.BeginTransaction();
            await _paymentStateServ.AddAsync(paymentState);
            await this.AddAsync(payment);
            await this.UnitOfWork.CommitAsync();

            return 1;
        }
        private async Task<int> UseIExpensivePaymentGateway(string cardNo, string cardName, string cvv, decimal amount, string expiry, string status, Guid id)
        {
            var paymentState = new PaymentState();
            paymentState.Retry_Count = 1;
            paymentState.Status = status;
            paymentState.Gateway_Id = id;

            var payment = CardPayment.Create(cardNo, cardName, cvv, amount, expiry);
            payment.Id = Guid.NewGuid();
            payment.CreatedOn = DateTimeOffset.Now;
            payment.PaymentState_Id = paymentState.Id;

            this.UnitOfWork.BeginTransaction();
            await _paymentStateServ.AddAsync(paymentState);
            await this.AddAsync(payment);
            await this.UnitOfWork.CommitAsync();

            return 1;
        }
        private async Task<TrasactionModel> UsePremiumPaymentGateway(string cardNo, string cardName, string cvv, decimal amount, string expiry, string status, Guid id)
        {
            var paymentState = new PaymentState();
            paymentState.Retry_Count = 1;
            paymentState.Status = status;
            paymentState.Gateway_Id = id;

            var payment = CardPayment.Create(cardNo, cardName, cvv, amount, expiry);
            payment.Id = Guid.NewGuid();
            payment.CreatedOn = DateTimeOffset.Now;
            payment.PaymentState_Id = paymentState.Id;

            this.UnitOfWork.BeginTransaction();
            await _paymentStateServ.AddAsync(paymentState);
            await this.AddAsync(payment);
            await this.UnitOfWork.CommitAsync();

            return new TrasactionModel
            {
                Id = payment.Id,
                Status = paymentState.Status,
                Count = paymentState.Retry_Count
            };
        }
        private async Task<int> UpdatePaymentGateway(Guid id, int count, string status)
        {
            var card = this.GetCardPaymentById(id);

            card.PaymentState.Retry_Count = count;
            card.PaymentState.Status = status;

            this.UnitOfWork.BeginTransaction();
            await this.UpdateAsync(card);
            await this.UnitOfWork.CommitAsync();

            return 1;
        }
    }
}
