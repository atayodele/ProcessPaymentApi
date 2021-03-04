using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApp.ViewModel
{
    public class CardPaymentViewModel
    {
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string CardHolder { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{yyyy-MM}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public string ExpirationDate { get; set; } 
        [Required]
        public string Cvv { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
    public class TrasactionModel
    {
        public Guid Id { get; set; }
        public int Count { get; set; }
        public string Status { get; set; }
    }
}
