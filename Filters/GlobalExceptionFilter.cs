using CheckoutApp.Exceptions;
using CheckoutApp.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace CheckoutApp.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {

        public void OnException(ExceptionContext context)
        {
            var (status, message) = GetStatusCode(context.Exception);

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.ContentType = "application/json";

            context.Result = new JsonResult(new Operation { Succeeded = false, Message = message });
        }

        private static (HttpStatusCode code, string message) GetStatusCode(Exception exception)
        {
            switch (exception)
            {
                case AnyErrorException nfex:
                    return (code: HttpStatusCode.NotFound, message: nfex.GetInnerMessage());
                case BadRequestException bex:
                    return (code: HttpStatusCode.BadRequest, message: bex.GetInnerMessage());
                default:
                    return (code: HttpStatusCode.InternalServerError, message: exception.GetInnerMessage());
            }
        }
    }
}
