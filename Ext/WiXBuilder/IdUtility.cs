using System;

namespace WiXBuilder
{
    internal static class IdUtility
    {
        public static string BuildId(string name)
        {
            if (null == name)
                throw new ArgumentNullException(nameof(name));

            var id = name.Replace("-", "_");
            id = id.Replace(" ", "_");

            if (Char.IsDigit(id[0]))
                id = "_" + id;

            return id;
        }
    }
}
