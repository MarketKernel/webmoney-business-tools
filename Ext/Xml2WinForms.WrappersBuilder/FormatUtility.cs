using System;
using System.Globalization;

namespace Xml2WinForms.WrappersBuilder
{
    internal static class FormatUtility
    {
        public static string BuildPropertyName(string name, string desc, int index)
        {
            if (null == desc)
                throw new ArgumentNullException(nameof(desc));

            var propertyName = "Control" + (index + 1);

            if (!string.IsNullOrEmpty(name))
            {
                if (Char.IsLower(name[0]))
                    propertyName += ToCamelCase(name);
                else
                    propertyName += name;

                return propertyName;
            }

            propertyName += ToCamelCase(desc);

            return propertyName;
        }

        public static string ToCamelCase(string value)
        {
            if (null == value)
                throw new ArgumentNullException(nameof(value));

            value = value.Replace(":", string.Empty);
            value = value.Replace("'", string.Empty);
            value = value.Replace("-", string.Empty);
            value = value.Replace("_", " ");

            value = value.ToLower();

            int leftBracketIndex = value.IndexOf("(", StringComparison.Ordinal);

            if (-1 != leftBracketIndex)
                value = value.Substring(0, leftBracketIndex);

            var textInfo = new CultureInfo("en-US", false).TextInfo;
            value = textInfo.ToTitleCase(value);

            value = value.Replace(" ", string.Empty);

            return value;
        }
    }
}
