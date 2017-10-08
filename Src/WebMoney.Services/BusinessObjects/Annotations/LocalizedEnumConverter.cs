using System;
using System.ComponentModel;
using System.Globalization;
using LocalizationAssistant;

namespace WebMoney.Services.BusinessObjects.Annotations
{
    internal sealed class LocalizedEnumConverter : EnumConverter
    {
        public LocalizedEnumConverter(Type type)
            : base(type)
        {
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            if (value is Enum && destinationType == typeof(string))
                return Translator.Instance.Translate("Settings", value.ToString());

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var propertyType = context.PropertyDescriptor?.PropertyType;

                if (null != propertyType)
                {
                    var enumValues = Enum.GetValues(propertyType);

                    foreach (var enumValue in enumValues)
                    {
                        var stringValue = Translator.Instance.Translate("Settings", enumValue.ToString());

                        if (stringValue.Equals(value))
                            return enumValue;
                    }
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}