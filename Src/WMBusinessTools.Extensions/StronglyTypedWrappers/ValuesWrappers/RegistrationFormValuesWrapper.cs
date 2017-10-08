using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class RegistrationFormValuesWrapper
    {
        public sealed class Step1 : StronglyTypedValuesWrapper
        {
            public const string Control1AuthenticationTypeValueClassic = "Classic";
            public const string Control1AuthenticationTypeValueLight = "Light";
            public const string Control3BackupFileCommandSelectFile = "SelectFile";
            public string Control1AuthenticationType => (string) GetValue(0);

            public string Control2Identifier
            {
                get => (string) GetValue(1);
                set => SetValue("Identifier", value);
            }

            public string Control3BackupFile
            {
                get => (string) GetValue(2);
                set => SetValue("BackupFile", value);
            }

            public string Control4BackupFilePassword
            {
                get => (string) GetValue(3);
                set => SetValue("BackupFilePassword", value);
            }

            public object Control5Certificate
            {
                get => GetValue(4);
                set => SetValue("Certificate", value);
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
            public const string Control5ConnectionStringCommandBuildConnectionString = "BuildConnectionString";
            public bool Control1LoginWithoutPassword => (bool) GetValue(0);

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

            public bool Control4ChangeConnectionString => ((bool?) GetValue(3)) ?? false;

            public string Control5ConnectionString
            {
                get => (string) GetValue(4);
                set => SetValue("ConnectionString", value);
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