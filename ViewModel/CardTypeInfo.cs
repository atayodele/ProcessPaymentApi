using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApp.ViewModel
{
    public class CardTypeInfo
    {
        public CardTypeInfo(string regEx, int length, CardType type)
        {
            RegEx = regEx;
            Length = length;
            Type = type;
        }

        public string RegEx { get; set; }
        public int Length { get; set; }
        public CardType Type { get; set; }
    }
    public enum CardType
    {
        Unknown = 0,
        MasterCard = 1,
        VISA = 2,
        Amex = 3,
    }
}
