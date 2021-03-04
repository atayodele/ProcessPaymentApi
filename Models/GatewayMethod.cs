using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApp.Models
{
    public class GatewayMethod : BaseEntity
    {
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
