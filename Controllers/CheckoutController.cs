using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckoutApp.Services.Interfaces;
using CheckoutApp.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CheckoutApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : BaseController
    {
        private readonly IPaymentStateService _paymentState;
        private readonly ICardPaymentService _cardPaymentServ;
        private readonly ILogger<CheckoutController> _ILogger;

        public CheckoutController(IPaymentStateService paymentState,
            ICardPaymentService cardPaymentServ,
            ILogger<CheckoutController> ILogger)
        {
            _paymentState = paymentState;
            _cardPaymentServ = cardPaymentServ;
            _ILogger = ILogger;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(CardPaymentViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var operation = await Operation.Run(async () =>
                {
                    return await _cardPaymentServ.ProcessPayment(model);
                });
                return Ok(operation);

            }
            catch (Exception ex)
            {
                _ILogger.LogError($"Something Went Wrong: {ex.Message}");
                return HandleError(ex);
            }
        }
    }
}
