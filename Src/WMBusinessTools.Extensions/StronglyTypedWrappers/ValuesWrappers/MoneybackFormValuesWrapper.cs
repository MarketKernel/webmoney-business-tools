using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class MoneybackFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public const string Control1SourcePurseCommandFindIdentifier = "FindIdentifier";

        public string Control1SourcePurse
        {
            get => (string) GetValue(0);
            set => SetValue("SourcePurse", value);
        }

        public string Control2TargetPurse
        {
            get => (string) GetValue(1);
            set => SetValue("TargetPurse", value);
        }

        public string Control3Amount
        {
            get => (string) GetValue(2);
            set => SetValue("Amount", value);
        }

        public string Control4Description
        {
            get => (string) GetValue(3);
            set => SetValue("Description", value);
        }

        public decimal Control5ReturnAmount
        {
            get => (decimal) GetValue(4);
            set => SetValue("ReturnAmount", value);
        }

        public MoneybackFormValuesWrapper()
        {
        }

        public MoneybackFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}