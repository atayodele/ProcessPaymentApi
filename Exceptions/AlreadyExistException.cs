using static CheckoutApp.Extensions.Constants;

namespace CheckoutApp.Exceptions
{
    public class AlreadyExistException : BaseException
    {
        public AlreadyExistException(string message) : base(message)
        {
            Code = ResponseCodes.Duplicate;
            httpStatusCode = System.Net.HttpStatusCode.BadRequest;
        }
    }
}
