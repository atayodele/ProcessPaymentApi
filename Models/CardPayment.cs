using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApp.Models
{
    public class CardPayment : BaseEntity
    {
        public static CardPayment Create(String cardNo, String cardName, String cvv, decimal amount, String expiry)
        {
            return new CardPayment()
            {
                CardNumber = cardNo,
                CardHolder = cardName,
                ExpirationDate = expiry,
                SecurityCode = cvv,
                Amount = amount
            };
        }
        public string CardNumber { get; set; }
        public string CardHolder { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public decimal Amount { get; set; }
        public Guid PaymentState_Id { get; set; }
        [ForeignKey(nameof(PaymentState_Id))] 
        public PaymentState PaymentState { get; set; }
    }
}
