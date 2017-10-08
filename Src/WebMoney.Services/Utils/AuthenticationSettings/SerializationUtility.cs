using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WebMoney.Services.Utils
{
    internal static class SerializationUtility
    {
        public static byte[] Serialize(object o)
        {
            if (null == o)
                throw new ArgumentNullException(nameof(o));

            byte[] bytes;

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, o);
                bytes = memoryStream.ToArray();
            }

            return bytes;
        }

        public static object Deserialize(byte[] bytes)
        {
            if (null == bytes)
                throw new ArgumentNullException(nameof(bytes));

            object o;

            using (var memoryStream = new MemoryStream(bytes))
            {
                var binaryFormatter = new BinaryFormatter();
                o = binaryFormatter.Deserialize(memoryStream);
            }

            return o;
        }
    }
}
