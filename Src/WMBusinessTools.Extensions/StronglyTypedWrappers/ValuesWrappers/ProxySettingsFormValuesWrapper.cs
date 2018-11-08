using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class ProxySettingsFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public const string Control1ModeValueNone = "None";
        public const string Control1ModeValueShared = "Shared";
        public const string Control1ModeValueCustom = "Custom";

        public string Control1Mode
        {
            get => (string) GetValue(0);
            set => SetValue("Mode", value);
        }

        public string Control2Address
        {
            get => (string) GetValue(1);
            set => SetValue("Address", value);
        }

        public int Control3Port
        {
            get => (int) (decimal) GetValue(2);
            set => SetValue("Port", (decimal) value);
        }

        public bool Control4AuthenticationRequired
        {
            get => (bool) GetValue(3);
            set => SetValue("AuthenticationRequired", value);
        }

        public string Control5Username
        {
            get => (string) GetValue(4);
            set => SetValue("Username", value);
        }

        public string Control6Password
        {
            get => (string) GetValue(5);
            set => SetValue("Password", value);
        }

        public ProxySettingsFormValuesWrapper()
        {
        }

        public ProxySettingsFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}