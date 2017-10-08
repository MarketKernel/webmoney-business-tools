using System;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.BusinessObjects
{
    internal sealed class SuspectedClientInfo : ISuspectedClientInfo
    {
        public ExchangeType ExchangeType { get; }
        public bool Output { get; set; }
        public string Currency { get; }
        public decimal Amount { get; }
        public long Identifier { get; }
        public string PassportNumber { get; private set; }
        public string SecondName { get; private set; }
        public string FirstName { get; private set; }
        public string BankName { get; private set; }
        public string BankAccount { get; private set; }
        public string CardNumber { get; private set; }
        public PaymentSystem PaymentSystem { get; private set; }
        public string PaymentSystemClientId { get; private set; }
        public string Phone { get; private set; }
        public CryptoCurrency CryptoCurrency { get; private set; }
        public string CryptoCurrencyAddress { get; private set; }

        private SuspectedClientInfo(ExchangeType exchangeType, string currency, decimal amount, long identifier)
        {
            ExchangeType = exchangeType;
            Currency = currency ?? throw new ArgumentNullException(nameof(amount));
            Amount = amount;
            Identifier = identifier;
        }

        public static SuspectedClientInfo CreateCashPaymentVerification(string currency, decimal amount,
            long identifier, string passportNumber, string firstName, string secondName)
        {
            if (null == currency)
                throw new ArgumentNullException(nameof(currency));

            if (null == passportNumber)
                throw new ArgumentNullException(nameof(passportNumber));

            if (null == firstName)
                throw new ArgumentNullException(nameof(firstName));

            if (null == secondName)
                throw new ArgumentNullException(nameof(secondName));

            var suspectedClientInfo =
                new SuspectedClientInfo(ExchangeType.Cash, currency, amount, identifier)
                {
                    PassportNumber = passportNumber,
                    FirstName = firstName,
                    SecondName = secondName
                };

            return suspectedClientInfo;
        }

        public static SuspectedClientInfo CreateOfflineSystemPaymentVerification(string currency, decimal amount,
            long identifier, string firstName, string secondName)
        {
            if (null == currency)
                throw new ArgumentNullException(nameof(currency));

            if (null == firstName)
                throw new ArgumentNullException(nameof(firstName));

            if (null == secondName)
                throw new ArgumentNullException(nameof(secondName));

            var suspectedClientInfo =
                new SuspectedClientInfo(ExchangeType.OfflineSystem, currency, amount, identifier)
                {
                    FirstName = firstName,
                    SecondName = secondName
                };

            return suspectedClientInfo;
        }

        public static SuspectedClientInfo CreateBankAccountPaymentVerification(string currency, decimal amount,
            long identifier, string firstName, string secondName, string bankName, string bankAccount)
        {
            if (null == currency)
                throw new ArgumentNullException(nameof(currency));

            if (null == firstName)
                throw new ArgumentNullException(nameof(firstName));

            if (null == secondName)
                throw new ArgumentNullException(nameof(secondName));

            if (null == bankName)
                throw new ArgumentNullException(nameof(bankName));

            if (null == bankAccount)
                throw new ArgumentNullException(nameof(bankAccount));

            var suspectedClientInfo =
                new SuspectedClientInfo(ExchangeType.BankAccount, currency, amount, identifier)
                {
                    FirstName = firstName,
                    SecondName = secondName,
                    BankName = bankName,
                    BankAccount = bankAccount
                };

            return suspectedClientInfo;
        }

        public static SuspectedClientInfo CreateBankCardPaymentVerification(string currency, decimal amount,
            long identifier, string firstName, string secondName, string bankName, string cardNumber)
        {
            if (null == currency)
                throw new ArgumentNullException(nameof(currency));

            if (null == firstName)
                throw new ArgumentNullException(nameof(firstName));

            if (null == secondName)
                throw new ArgumentNullException(nameof(secondName));

            if (null == bankName)
                throw new ArgumentNullException(nameof(bankName));

            if (null == cardNumber)
                throw new ArgumentNullException(nameof(cardNumber));

            var suspectedClientInfo =
                new SuspectedClientInfo(ExchangeType.Bankcard, currency, amount, identifier)
                {
                    FirstName = firstName,
                    SecondName = secondName,
                    BankName = bankName,
                    CardNumber = cardNumber
                };

            return suspectedClientInfo;
        }

        public static SuspectedClientInfo CreateInternetSystemPaymentVerification(string currency, decimal amount,
            long identifier, PaymentSystem paymentSystem, string paymentSystemClientId)
        {
            if (null == currency)
                throw new ArgumentNullException(nameof(currency));

            if (null == paymentSystemClientId)
                throw new ArgumentNullException(nameof(paymentSystemClientId));

            var suspectedClientInfo =
                new SuspectedClientInfo(ExchangeType.InternetSystem, currency, amount, identifier)
                {
                    PaymentSystem = paymentSystem,
                    PaymentSystemClientId = paymentSystemClientId
                };

            return suspectedClientInfo;
        }

        public static SuspectedClientInfo CreateSmsPaymentVerification(string currency, decimal amount,
            long identifier, string phone)
        {
            if (null == currency)
                throw new ArgumentNullException(nameof(currency));

            if (null == phone)
                throw new ArgumentNullException(nameof(phone));

            var suspectedClientInfo =
                new SuspectedClientInfo(ExchangeType.Sms, currency, amount, identifier) {Phone = phone};

            return suspectedClientInfo;
        }

        public static SuspectedClientInfo CreateMobilePaymentVerification(string currency, decimal amount,
            long identifier, string phone)
        {
            if (null == currency)
                throw new ArgumentNullException(nameof(currency));

            if (null == phone)
                throw new ArgumentNullException(nameof(phone));

            var suspectedClientInfo =
                new SuspectedClientInfo(ExchangeType.Mobile, currency, amount, identifier) {Phone = phone};

            return suspectedClientInfo;
        }

        public static SuspectedClientInfo CreateBlockchainPaymentVerification(string currency, decimal amount,
            long identifier, CryptoCurrency cryptoCurrency, string cryptoCurrencyAddress)
        {
            if (null == currency)
                throw new ArgumentNullException(nameof(currency));

            if (null == cryptoCurrencyAddress)
                throw new ArgumentNullException(nameof(cryptoCurrencyAddress));

            var suspectedClientInfo =
                new SuspectedClientInfo(ExchangeType.Blockchain, currency, amount, identifier)
                {
                    CryptoCurrency = cryptoCurrency,
                    CryptoCurrencyAddress = cryptoCurrencyAddress
                };

            return suspectedClientInfo;
        }
    }
}