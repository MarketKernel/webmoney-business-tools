using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class FindCertificateFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public string Control1Identifier
        {
            get => (string) GetValue(0);
            set => SetValue("Identifier", value);
        }

        public bool Control2GetBlAndTl => (bool) GetValue(1);
        public bool Control3GetBlAndTlForAttachedWmids => (bool) GetValue(2);
        public bool Control4CheckComplaintsAndComments => (bool) GetValue(3);

        public bool Control5OfflineSearch
        {
            get => (bool) GetValue(4);
            set => SetValue("OfflineSearch", value);
        }

        public FindCertificateFormValuesWrapper()
        {
        }

        public FindCertificateFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}