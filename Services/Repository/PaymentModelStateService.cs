using CheckoutApp.Extensions;
using CheckoutApp.Models;
using CheckoutApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApp.Services.Repository
{
    public class PaymentModelStateService : Service<PaymentModelState>, IPaymentModelStateService
    {
        public PaymentModelStateService(IUnitOfWork iuow) : base(iuow)
        {

        }
        public List<PaymentModelState> GetAllPaymentState()
        {
            return this.GetAll().OrderBy(x => x.Name).ToList();
        }

        public async Task<PaymentModelState> GetPaymentStateById(Guid id)
        {
            return await this.GetByIdAsync(id);
        }

        public Task<string> GetRandomTrasactionStates()
        {
            var paymentState = this.GetAllPaymentState();
            string[] state = paymentState.Select(x => x.Name).ToArray();
            var result = Helper.GenerateTransaction(state);
            return Task.FromResult(result);
        }
    }
}
