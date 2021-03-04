using CheckoutApp.Models;
using CheckoutApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApp.Services.Interfaces
{
    public interface ICardPaymentService : IService<CardPayment>
    {
        Task<CardPaymentViewModel> ProcessPayment(CardPaymentViewModel model);
        List<CardPayment> GetAllCardPayment();
        CardPayment GetCardPaymentById(Guid id);
    }
}
