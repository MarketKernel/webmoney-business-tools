using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class RejectInvoiceFormValuesWrapper : StronglyTypedValuesWrapper
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

        public decimal? Control3Amount
        {
            get => (decimal?) GetValue(2);
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

        public decimal? Control7PaymentPeriod
        {
            get => (decimal?) GetValue(6);
            set => SetValue("PaymentPeriod", value);
        }

        public decimal? Control8ProtectionPeriod
        {
            get => (decimal?) GetValue(7);
            set => SetValue("ProtectionPeriod", value);
        }

        public RejectInvoiceFormValuesWrapper()
        {
        }

        public RejectInvoiceFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}