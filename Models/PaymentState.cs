using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApp.Models
{
    public class PaymentState : BaseEntity
    {
        public Guid Gateway_Id { get; set; }
        [ForeignKey(nameof(Gateway_Id))]
        public GatewayMethod GatewayMethod { get; set; }
        public int Retry_Count { get; set; }
        public string Status { get; set; }
    }
}
