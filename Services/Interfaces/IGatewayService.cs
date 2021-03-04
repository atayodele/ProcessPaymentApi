using CheckoutApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApp.Services.Interfaces
{
    public interface IGatewayService : IService<GatewayMethod>
    {
        List<GatewayMethod> GetAllGatewayMethod();
        GatewayMethod GetGatewayById(string name); 
    }
}
