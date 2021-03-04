using CheckoutApp.ViewModel;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CheckoutApp.Extensions
{
    public static class Helper
    {
        public static string GetInnerMessage(this Exception original)
        {
            if (original == null) return null;
            var ex = original;
            while (ex.InnerException != null) ex = ex.InnerException;
            return ex.Message;
        }
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            var entities = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return entities;
        }

        public static string GenerateTransaction(string[] states)
        {
            string[] status = states;

            Random rand = new Random();

            int index = rand.Next(status.Length);

            var result = status[index];

            return result;
        }

        public static string NormalizeCardNumber(string cardNumber)
        {
            if (cardNumber == null)
                cardNumber = String.Empty;

            StringBuilder sb = new StringBuilder();

            foreach (char c in cardNumber)
            {
                if (Char.IsDigit(c))
                    sb.Append(c);
            }

            return sb.ToString();
        }

        public static bool IsCardNumberValid(string cardNumber)
        {
            int i, checkSum = 0;

            // Compute checksum of every other digit starting from right-most digit
            for (i = cardNumber.Length - 1; i >= 0; i -= 2)
                checkSum += (cardNumber[i] - '0');

            // Now take digits not included in first checksum, multiple by two,
            // and compute checksum of resulting digits
            for (i = cardNumber.Length - 2; i >= 0; i -= 2)
            {
                int val = ((cardNumber[i] - '0') * 2);
                while (val > 0)
                {
                    checkSum += (val % 10);
                    val /= 10;
                }
            }

            // Number is valid if sum of both checksums MOD 10 equals 0
            return ((checkSum % 10) == 0);
        }

        private static CardTypeInfo[] _cardTypeInfo =
        {
          new CardTypeInfo("^(51|52|53|54|55)", 16, CardType.MasterCard),
          new CardTypeInfo("^(4)", 16, CardType.VISA),
          new CardTypeInfo("^(4)", 13, CardType.VISA),
          new CardTypeInfo("^(34|37)", 15, CardType.Amex),
        };

        public static CardType GetCardType(string cardNumber)
        {
            foreach (CardTypeInfo info in _cardTypeInfo)
            {
                if (cardNumber.Length == info.Length &&
                    Regex.IsMatch(cardNumber, info.RegEx))
                    return info.Type;
            }

            return CardType.Unknown;
        }
        public static bool HasCreditCardExpired(string expiryDate)
        {
            var dateParts = expiryDate.Split('/');  

            var year = int.Parse(dateParts[1]);
            var month = int.Parse(dateParts[0]);
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month); 

            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);
            var expiry = cardExpiry < DateTime.Now;

            return expiry;
        }
    }
}
