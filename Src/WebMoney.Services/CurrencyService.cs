using System;
using System.Collections.Generic;
using System.Globalization;
using log4net;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.XmlInterfaces.BasicObjects;

namespace WebMoney.Services
{
    public sealed class CurrencyService : ICurrencyService
    {
        public const string Prefix = "WM";

        private static readonly ILog Logger = LogManager.GetLogger(typeof(CurrencyService));

        private readonly Dictionary<string, CurrencyCapabilities> _currencyRegister;

        public CurrencyService()
        {
            _currencyRegister = new Dictionary<string, CurrencyCapabilities>
            {
                {
                    "Z",
                    CurrencyCapabilities.Actual | CurrencyCapabilities.CreatePurse | CurrencyCapabilities.Transfer |
                    CurrencyCapabilities.Invoice
                },
                {
                    "E",
                    CurrencyCapabilities.Actual | CurrencyCapabilities.CreatePurse | CurrencyCapabilities.Transfer |
                    CurrencyCapabilities.Invoice
                },
                {
                    "R",
                    CurrencyCapabilities.Actual | CurrencyCapabilities.CreatePurse | CurrencyCapabilities.Transfer |
                    CurrencyCapabilities.Invoice
                },
                {
                    "U",
                    CurrencyCapabilities.Actual | CurrencyCapabilities.CreatePurse | CurrencyCapabilities.Transfer |
                    CurrencyCapabilities.Invoice
                },
                {
                    "B",
                    CurrencyCapabilities.Actual | CurrencyCapabilities.CreatePurse | CurrencyCapabilities.Transfer |
                    CurrencyCapabilities.Invoice
                },
                {
                    "G",
                    CurrencyCapabilities.Actual | CurrencyCapabilities.CreatePurse | CurrencyCapabilities.Transfer |
                    CurrencyCapabilities.Invoice
                },
                {"D", CurrencyCapabilities.Actual | CurrencyCapabilities.CreatePurse | CurrencyCapabilities.Invoice},
                {"C", CurrencyCapabilities.Actual | CurrencyCapabilities.CreatePurse},
                {
                    "X",
                    CurrencyCapabilities.Actual | CurrencyCapabilities.CreatePurse | CurrencyCapabilities.Transfer |
                    CurrencyCapabilities.Invoice
                },
                {
                    "K",
                    CurrencyCapabilities.Actual | CurrencyCapabilities.CreatePurse | CurrencyCapabilities.Transfer |
                    CurrencyCapabilities.Invoice
                },
                {
                    "V",
                    CurrencyCapabilities.Actual | CurrencyCapabilities.CreatePurse | CurrencyCapabilities.Transfer |
                    CurrencyCapabilities.Invoice
                },
                {
                    "H",
                    CurrencyCapabilities.Actual | CurrencyCapabilities.CreatePurse | CurrencyCapabilities.Transfer |
                    CurrencyCapabilities.Invoice
                }
            };
        }

        public string ObtainCurrencyByAccountNumber(string accountNumber)
        {
            if (null == accountNumber)
                throw new ArgumentNullException(nameof(accountNumber));

            return Purse.CurrencyToLetter(Purse.Parse(accountNumber).Type).ToString();
        }

        public bool CheckCapabilities(string currency, CurrencyCapabilities capabilitiesToCheck)
        {
            if (null == currency)
                throw new ArgumentNullException(nameof(currency));

            if (CurrencyCapabilities.None == capabilitiesToCheck)
                return true;

            CurrencyCapabilities capabilities;

            if (!_currencyRegister.TryGetValue(currency, out capabilities))
            {
                Logger.ErrorFormat("Currency {0} not supported.", currency);
                return false;
            }

            foreach (var flag in GetFlags(capabilitiesToCheck))
            {
                if (!capabilities.HasFlag(flag))
                    return false;
            }

            return true;
        }

        public string AddPrefix(string currency)
        {
            if (null == currency)
                throw new ArgumentNullException(nameof(currency));

            return string.Format(CultureInfo.InvariantCulture, "{0}{1}", Prefix, currency);
        }

        public string RemovePrefix(string currency)
        {
            if (null == currency)
                throw new ArgumentNullException(nameof(currency));

            if (!currency.StartsWith(Prefix,StringComparison.Ordinal))
                throw new ArgumentException();

            return currency.Remove(0, Prefix.Length);
        }

        public IReadOnlyCollection<string> SelectCurrencies(CurrencyCapabilities currencyCapabilities)
        {
            var currencies = new List<string>();

            foreach (WmCurrency wmCurrency in Enum.GetValues(typeof(WmCurrency)))
            {
                if (0 == wmCurrency)
                    continue;

                if ((int)wmCurrency >= 200)
                    continue;

                string currency = Purse.CurrencyToLetter(wmCurrency).ToString();

                if (CurrencyCapabilities.None != currencyCapabilities)
                {
                    if (!CheckCapabilities(currency, currencyCapabilities))
                        continue;
                }

                currencies.Add(currency);
            }

            return currencies;
        }

        private static IEnumerable<CurrencyCapabilities> GetFlags(CurrencyCapabilities flags)
        {
            foreach (CurrencyCapabilities singleFlag in Enum.GetValues(typeof(CurrencyCapabilities)))
                if (flags.HasFlag(singleFlag))
                    yield return singleFlag;
        }
    }
}
