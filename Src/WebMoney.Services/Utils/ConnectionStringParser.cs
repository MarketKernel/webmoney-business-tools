using System;

namespace WebMoney.Services.Utils
{
    internal static class ConnectionStringParser
    {
        public static string TryGetValue(string connectionString, string parameterName)
        {
            if (null == connectionString)
                throw new ArgumentNullException(nameof(connectionString));

            var parts = connectionString.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                var nameValue = part.Trim().Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                if (2 != nameValue.Length)
                    throw new InvalidOperationException("2 != nameValue.Length");

                var name = nameValue[0].Trim();
                var value = nameValue[1].Trim();

                if (name.Equals(parameterName, StringComparison.OrdinalIgnoreCase))
                    return value;
            }

            return null;
        }
    }
}
