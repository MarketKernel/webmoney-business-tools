using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using LocalizationAssistant;

namespace Xml2WinForms.Utils
{
    public static class DisplayContentHelper
    {
        public static string GetText(PropertyInfo propertyInfo, object value)
        {
            if (null == propertyInfo)
                throw new ArgumentNullException(nameof(propertyInfo));

            if (null == value)
                return string.Empty;

            if (value is bool)
                return Translator.Instance.Translate("xml2winforms", value.ToString());

            if (value is Enum)
                return Translator.Instance.Translate("xml2winforms", value.ToString());

            string dataFormatString = null;
            var attributes = propertyInfo.GetCustomAttributes(typeof(DisplayFormatAttribute), false);

            if (1 == attributes.Length)
            {
                var displayFormatAttribute = (DisplayFormatAttribute) attributes[0];
                dataFormatString = displayFormatAttribute.DataFormatString;
            }

            if (value is DateTime)
            {
                if (null != dataFormatString)
                    return ((DateTime) value).ToLocalTime().ToString(dataFormatString, CultureInfo.CurrentCulture);
                return ((DateTime) value).ToLocalTime().ToString(CultureInfo.CurrentCulture);
            }

            return null != dataFormatString
                ? string.Format(CultureInfo.CurrentUICulture, dataFormatString, value)
                : value.ToString();
        }

        public static Color SelectColor(string value)
        {
            if (null == value)
                throw new ArgumentNullException(nameof(value));

            var values = value.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            if (3 != values.Length)
                return Color.FromName(value);

            return Color.FromArgb(int.Parse(values[0], CultureInfo.InvariantCulture.NumberFormat),
                int.Parse(values[1], CultureInfo.InvariantCulture.NumberFormat),
                int.Parse(values[2], CultureInfo.InvariantCulture.NumberFormat));
        }
    }
}
