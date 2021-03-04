using CheckoutApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApp.Services.Interfaces
{
    public interface IPaymentStateService : IService<PaymentState>
    {
        List<PaymentState> GetAllPaymentState();
        PaymentState GetPaymentStateById(Guid id);
    }
}
