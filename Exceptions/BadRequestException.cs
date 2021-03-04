using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CheckoutApp.Extensions.Constants;

namespace CheckoutApp.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message)
        {
            Code = ResponseCodes.Failed;
            httpStatusCode = System.Net.HttpStatusCode.BadRequest;
        }
    }
}
