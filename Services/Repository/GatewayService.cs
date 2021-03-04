using CheckoutApp.Models;
using CheckoutApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApp.Services.Repository
{
    public class GatewayService : Service<GatewayMethod>, IGatewayService
    {
        public GatewayService(IUnitOfWork iuow) : base(iuow)
        {

        }

        public List<GatewayMethod> GetAllGatewayMethod()
        {
            return this.GetAll().OrderBy(x => x.Name).Where(x => x.Status == true).ToList();
        }

        public GatewayMethod GetGatewayById(string name)
        {
            var gatewayList = GetAllGatewayMethod();
            var gateway = gatewayList.FirstOrDefault(x => x.Name == name);
            return gateway;
        }
    }
}
