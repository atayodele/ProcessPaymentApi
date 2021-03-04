using static CheckoutApp.Extensions.Constants;

namespace CheckoutApp.Exceptions
{
    public class AnyErrorException : BaseException
    {

        public AnyErrorException(string message) : base(message)
        {
            Code = ResponseCodes.NotFound;
            httpStatusCode = System.Net.HttpStatusCode.InternalServerError;
        }
    }
}
