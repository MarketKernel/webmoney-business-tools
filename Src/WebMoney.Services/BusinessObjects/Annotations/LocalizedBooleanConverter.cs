using System;
using System.ComponentModel;
using System.Globalization;
using LocalizationAssistant;

namespace WebMoney.Services.BusinessObjects.Annotations
{
    internal sealed class LocalizedBooleanConverter : BooleanConverter
    {
        private readonly string _trueValue;
        private readonly string _falseValue;

        public LocalizedBooleanConverter()
        {
            _trueValue = Translator.Instance.Translate("Settings", "yes");
            _falseValue = Translator.Instance.Translate("Settings", "no");
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            if (value is bool && destinationType == typeof(string))
                return (bool) value ? _trueValue : _falseValue;

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                if (_trueValue.Equals((string) value, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (_falseValue.Equals((string)value, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
