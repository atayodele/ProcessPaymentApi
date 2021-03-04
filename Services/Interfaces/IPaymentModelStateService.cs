using CheckoutApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApp.Services.Interfaces
{
    public interface IPaymentModelStateService : IService<PaymentModelState>
    {
        Task<PaymentModelState> GetPaymentStateById(Guid id);
        List<PaymentModelState> GetAllPaymentState();
        Task<string> GetRandomTrasactionStates();
    }
}
