using System;
using System.Globalization;
using WMBusinessTools.Extensions.Templates.Controls;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class AccountDropDownListItem
    {
        public string Number { get; }
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
        public decimal? AvailableAmount { get; set; }
        public decimal? RecommendedAmount { get; set; }

        public AccountDropDownListItem(string number)
        {
            Number = number ?? throw new ArgumentNullException(nameof(number));
        }

        public static AccountDropDownListItem FromTemplate(AccountDropDownListItemTemplate itemTemplate)
        {
            if (null == itemTemplate)
                throw new ArgumentNullException(nameof(itemTemplate));

            return new AccountDropDownListItem(itemTemplate.Number)
            {
                Name = itemTemplate.Name,
                Amount = itemTemplate.Amount,
                AvailableAmount = itemTemplate.AvailableAmount,
                RecommendedAmount = itemTemplate.RecommendedAmount,
                Currency = itemTemplate.Currency
            };
        }

        public override string ToString()
        {
            if (null == Name)
                return Number;

            if (null == Amount)
                return string.Format(CultureInfo.InvariantCulture, "{0} [{1}]", Number, Name);

            return string.Format(CultureInfo.InvariantCulture, "{0} [{1}]     {2:0.00}", Number, Name, Amount);
        }
    }
}
