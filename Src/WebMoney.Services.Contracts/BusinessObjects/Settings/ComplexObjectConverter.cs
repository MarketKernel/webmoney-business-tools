using System;
using System.ComponentModel;
using System.Globalization;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    internal sealed class ComplexObjectConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return false;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            if (destinationType == typeof(string))
                return null;

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
