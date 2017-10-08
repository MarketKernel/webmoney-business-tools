﻿using System;
using System.Collections.Generic;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal sealed class IncomingInvoiceFilterFormValuesWrapper : StronglyTypedValuesWrapper
    {
        public DateTime Control1FromTime
        {
            get => (DateTime) GetValue(0);
            set => SetValue("FromTime", value);
        }

        public DateTime Control2ToTime
        {
            get => (DateTime) GetValue(1);
            set => SetValue("ToTime", value);
        }

        public IncomingInvoiceFilterFormValuesWrapper()
        {
        }

        public IncomingInvoiceFilterFormValuesWrapper(List<object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));
            ApplyOutcomeValues(values);
        }
    }
}