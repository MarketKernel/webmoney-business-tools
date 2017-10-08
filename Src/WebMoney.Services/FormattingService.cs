using System;
using System.Globalization;
using WebMoney.Services.Contracts;

namespace WebMoney.Services
{
    public sealed class FormattingService : IFormattingService
    {
        public const string IdentifierTemplate = "{0:000000000000}";
        public const string AmountTemplate = "{0:0.00}";

        public string FormatIdentifier(long identifier)
        {
            return string.Format(CultureInfo.CurrentCulture, IdentifierTemplate, identifier);
        }

        public string FormatAmount(decimal amount)
        {
            return string.Format(CultureInfo.CurrentCulture, AmountTemplate, amount);
        }

        public string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToLocalTime().ToString(CultureInfo.CurrentCulture);
        }

        public string FormatDate(DateTime dateTime)
        {
            return dateTime.ToLocalTime().ToString("yyyy MMMM dd", CultureInfo.CurrentCulture);
        }
    }
}