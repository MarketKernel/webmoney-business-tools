using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class PasswordSettingsFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public string Control1OldPassowrd => (string) GetValue(0);

        public string Control2Password
        {
            get => (string) GetValue(1);
            set => SetValue("Password", value);
        }

        public string Control3PasswordConfirmation
        {
            get => (string) GetValue(2);
            set => SetValue("PasswordConfirmation", value);
        }

        public bool Control4LoginWithoutPassword => (bool) GetValue(3);

        public PasswordSettingsFormValuesWrapper()
        {
        }

        public PasswordSettingsFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));

            ApplyOutcomeValues(values);
        }
    }
}