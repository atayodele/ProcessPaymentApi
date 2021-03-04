using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckoutApp.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckoutApp.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult HandleError(Exception ex, string customErrorMessage = null)
        {
            ApiResponse<string> rsp = new ApiResponse<string>();
            rsp.Code = ApiResponseCodes.ERROR;
#if DEBUG
            rsp.Description = $"Error: {(ex?.InnerException?.Message ?? ex.Message)} --> {ex?.StackTrace}";
            return Ok(rsp);
#else
             rsp.Description = customErrorMessage ?? "An error occurred while processing your request!";
             return Ok(rsp);
#endif
        }
    }
}