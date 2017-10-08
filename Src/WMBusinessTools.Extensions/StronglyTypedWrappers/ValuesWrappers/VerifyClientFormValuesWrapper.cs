using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class VerifyClientFormValuesWrapper
    {
        public sealed class Step1 : StronglyTypedValuesWrapper
        {
            public const string Control6WmidCommandFindPassport = "FindPassport";

            public const string Control1InstrumentValueCash = "Cash";
            public const string Control1InstrumentValueOfflinesystem = "OfflineSystem";
            public const string Control1InstrumentValueBankaccount = "BankAccount";
            public const string Control1InstrumentValueBankcard = "Bankcard";
            public const string Control1InstrumentValueInternetsystem = "InternetSystem";
            public const string Control1InstrumentValueSms = "Sms";
            public const string Control1InstrumentValueMobile = "Mobile";
            public const string Control1InstrumentValueBlockchain = "Blockchain";

            public const string Control2DirectionValueOutput = "Output";
            public const string Control2DirectionValueInput = "Input";

            public const string Control13PaymentSystemValuePaypal = "PayPal";
            public const string Control13PaymentSystemValueSkrill = "Skrill";
            public const string Control13PaymentSystemValueAlipay = "Alipay";
            public const string Control13PaymentSystemValueQiwi = "Qiwi";
            public const string Control13PaymentSystemValueYandexmoney = "YandexMoney";

            public const string Control16CryptoCurrencyValueBitcoin = "Bitcoin";
            public const string Control16CryptoCurrencyValueBitcoincash = "BitcoinCash";

            public string Control1Instrument => (string) GetValue(0);
            public string Control2Direction => (string) GetValue(1);
            public string Control3PurseType => (string) GetValue(2);

            public decimal Control4Amount
            {
                get => (decimal) GetValue(3);
                set => SetValue("Amount", value);
            }

            public int Control5OrderNumber => (int) (decimal) GetValue(4);
            public string Control6Wmid => (string) GetValue(5);

            public string Control7FirstName
            {
                get => (string) GetValue(6);
                set => SetValue("FirstName", value);
            }

            public string Control8SecondName
            {
                get => (string) GetValue(7);
                set => SetValue("SecondName", value);
            }

            public string Control9PassportNumber
            {
                get => (string) GetValue(8);
                set => SetValue("PassportNumber", value);
            }

            public string Control10BankName
            {
                get => (string) GetValue(9);
                set => SetValue("BankName", value);
            }

            public string Control11BankAccount
            {
                get => (string) GetValue(10);
                set => SetValue("BankAccount", value);
            }

            public string Control12CardNumber
            {
                get => (string) GetValue(11);
                set => SetValue("CardNumber", value);
            }

            public string Control13PaymentSystem
            {
                get => (string) GetValue(12);
                set => SetValue("PaymentSystem", value);
            }

            public string Control14PaymentSystemClientId
            {
                get => (string) GetValue(13);
                set => SetValue("PaymentSystemClientId", value);
            }

            public string Control15Phone
            {
                get => (string) GetValue(14);
                set => SetValue("Phone", value);
            }

            public string Control16CryptoCurrency
            {
                get => (string) GetValue(15);
                set => SetValue("CryptoCurrency", value);
            }

            public string Control17CryptoCurrencyAddress
            {
                get => (string) GetValue(16);
                set => SetValue("CryptoCurrencyAddress", value);
            }

            public Step1()
            {
            }

            public Step1(List<object> values)
            {
                if (null == values)
                    throw new ArgumentNullException(nameof(values));

                ApplyOutcomeValues(values);
            }
        }

        public sealed class Step2 : StronglyTypedValuesWrapper
        {
            public object Control1Result
            {
                get => GetValue(0);
                set => SetValue("Result", value);
            }

            public Step2()
            {
            }

            public Step2(List<object> values)
            {
                if (null == values)
                    throw new ArgumentNullException(nameof(values));

                ApplyOutcomeValues(values);
            }
        }
    }
}