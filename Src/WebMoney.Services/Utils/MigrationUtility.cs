using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace WebMoney.Services.Utils
{
    internal static class MigrationUtility
    {
        //var xml = MigrationUtility.Decompress(Resources.GetString("Target"));
        //xml = xml.Replace("Schema=\"WMBTUSER\"", $"Schema=\"{Schema}\"");
        //return MigrationUtility.Compress(xml);

        public static string Decompress(string compressed)
        {
            if (null == compressed)
                throw new ArgumentNullException(nameof(compressed));

            using (var memoryStream = new MemoryStream(Convert.FromBase64String(compressed)))
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress, false))
            using (var streamReader = new StreamReader(gZipStream))
            {
                return streamReader.ReadToEnd();
            }
        }

        public static string Compress(string value)
        {
            if (null == value)
                throw new ArgumentNullException(nameof(value));

            var bytes = Encoding.UTF8.GetBytes(value);
            byte[] compressedBytes;

            using (var memoryStream = new MemoryStream())
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, false))
            {
                gZipStream.Write(bytes, 0, bytes.Length);

                gZipStream.Flush();
                gZipStream.Close();

                compressedBytes = memoryStream.ToArray();
            }

            return Convert.ToBase64String(compressedBytes);
        }
    }
}
