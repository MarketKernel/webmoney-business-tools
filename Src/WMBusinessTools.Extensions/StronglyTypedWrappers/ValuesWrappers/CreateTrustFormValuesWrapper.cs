using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class CreateTrustFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public const string Control1MasterIdentifierCommandFindPassport = "FindPassport";

        public string Control1MasterIdentifier
        {
            get => (string) GetValue(0);
            set => SetValue("MasterIdentifier", value);
        }

        public string Control2PurseNumber
        {
            get => (string) GetValue(1);
            set => SetValue("PurseNumber", value);
        }

        public bool Control3InvoiceAllowed
        {
            get => (bool) GetValue(2);
            set => SetValue("InvoiceAllowed", value);
        }

        public bool Control4BalanceAllowed
        {
            get => (bool) GetValue(3);
            set => SetValue("BalanceAllowed", value);
        }

        public bool Control5HistoryAllowed
        {
            get => (bool) GetValue(4);
            set => SetValue("HistoryAllowed", value);
        }

        public bool Control6TransferAllowed
        {
            get => (bool) GetValue(5);
            set => SetValue("TransferAllowed", value);
        }

        public decimal Control7DailyAmountLimit
        {
            get => (decimal) GetValue(6);
            set => SetValue("DailyAmountLimit", value);
        }

        public decimal Control8DayAmountLimit
        {
            get => (decimal) GetValue(7);
            set => SetValue("DayAmountLimit", value);
        }

        public decimal Control9WeeklyAmountLimit
        {
            get => (decimal) GetValue(8);
            set => SetValue("WeeklyAmountLimit", value);
        }

        public decimal Control10MonthlyAmountLimit
        {
            get => (decimal) GetValue(9);
            set => SetValue("MonthlyAmountLimit", value);
        }

        public CreateTrustFormValuesWrapper()
        {
        }

        public CreateTrustFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}