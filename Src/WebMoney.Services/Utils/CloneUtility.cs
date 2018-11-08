using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace WebMoney.Services.Utils
{
    internal static class CloneUtility
    {
        public static void CloneProperties(object o)
        {
            foreach (var property in o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var value = property.GetValue(o, null);

                if (null == value)
                    continue;

                if (value is ICloneable cloneable)
                {
                    property.SetValue(o, cloneable.Clone(), null);
                    continue;
                }

                if (value is Collection<int> intCollection)
                {
                    property.SetValue(o, new Collection<int>(intCollection), null);
                    continue;
                }

                if (value is ValueType)
                    continue;

                throw new InvalidOperationException("type=" + value.GetType());
            }
        }
    }
}