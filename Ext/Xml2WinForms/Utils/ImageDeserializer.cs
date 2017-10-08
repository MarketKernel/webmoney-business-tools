using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Xml2WinForms.Utils
{
    public static class ImageSerializer
    {
        public static byte[] Serialize(Bitmap bitmap)
        {
            if (null == bitmap)
                throw new ArgumentNullException(nameof(bitmap));

            byte[] array;

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, bitmap);
                array = memoryStream.ToArray();
            }

            return array;
        }

        public static Bitmap Deserialize(byte[] bitmapBytes)
        {
            if (null == bitmapBytes)
                throw new ArgumentNullException(nameof(bitmapBytes));

            Bitmap bitmap;

            using (var memoryStream = new MemoryStream(bitmapBytes))
            {
                var binaryFormatter = new BinaryFormatter();
                bitmap = (Bitmap)binaryFormatter.Deserialize(memoryStream);
            }

            return bitmap;
        }
    }
}
