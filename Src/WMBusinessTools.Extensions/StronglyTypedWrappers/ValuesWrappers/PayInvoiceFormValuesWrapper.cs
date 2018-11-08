using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class PayInvoiceFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public const string Control1TargetIdentifierCommandFindPassport = "FindPassport";

        public string Control1TargetIdentifier
        {
            get => (string) GetValue(0);
            set => SetValue("TargetIdentifier", value);
        }

        public string Control2TargetPurse
        {
            get => (string) GetValue(1);
            set => SetValue("TargetPurse", value);
        }

        public decimal Control3Amount
        {
            get => (decimal) GetValue(2);
            set => SetValue("Amount", value);
        }

        public string Control4Description
        {
            get => (string) GetValue(3);
            set => SetValue("Description", value);
        }

        public string Control5OrderId
        {
            get => (string) GetValue(4);
            set => SetValue("OrderId", value);
        }

        public string Control6Address
        {
            get => (string) GetValue(5);
            set => SetValue("Address", value);
        }

        public string Control7PaymentPeriod
        {
            get => (string) GetValue(6);
            set => SetValue("PaymentPeriod", value);
        }

        public string Control8MaxProtectionPeriod
        {
            get => (string) GetValue(7);
            set => SetValue("MaxProtectionPeriod", value);
        }

        public int Control9TransferId
        {
            get => (int) (decimal) GetValue(8);
            set => SetValue("TransferId", (decimal) value);
        }

        public string Control10PayFrom => (string) GetValue(9);
        public bool Control11UsePaymentProtection => (bool?) GetValue(10) ?? false;

        public byte Control12ProtectionPeriod
        {
            get => (byte) (decimal) GetValue(11);
            set => SetValue("ProtectionPeriod", (decimal) value);
        }

        public string Control13ProtectionCode
        {
            get => (string) GetValue(12);
            set => SetValue("ProtectionCode", value);
        }

        public bool Control14ProtectionByTime => (bool) GetValue(13);

        public PayInvoiceFormValuesWrapper()
        {
        }

        public PayInvoiceFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}