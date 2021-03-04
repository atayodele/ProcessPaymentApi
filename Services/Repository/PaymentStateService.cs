using CheckoutApp.Extensions;
using CheckoutApp.Models;
using CheckoutApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApp.Services.Repository
{
    public class PaymentStateService : Service<PaymentState>, IPaymentStateService
    {
        public PaymentStateService(IUnitOfWork iuow) : base(iuow)
        {

        }

        public List<PaymentState> GetAllPaymentState()
        {
            return this.GetAll().ToList();
        }

        public PaymentState GetPaymentStateById(Guid id)
        {
            var gatewayList = GetAllPaymentState();
            var gateway = gatewayList.FirstOrDefault(x => x.Id == id);
            return gateway;
        }
    }
}
