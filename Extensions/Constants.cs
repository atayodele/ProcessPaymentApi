using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApp.Extensions
{
    public static class Constants
    { 
        public static class ResponseCodes
        {
            public const string Successful = "00";
            public const string Unknown = "500";
            public const string NotFound = "01";
            public const string Duplicate = "02";
            public const string Failed = "03";
        }
    }
}
