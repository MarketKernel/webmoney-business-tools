using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace PartialTrustInstaller.Utils
{
    internal static class HashUtility
    {
        public static byte[] ComputeHash(string filePath)
        {
            if (null == filePath)
                throw new ArgumentNullException(nameof(filePath));

            byte[] hash;

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                hash = new SHA256Managed().ComputeHash(fileStream);
                fileStream.Close();
            }

            return hash;
        }

        public static byte[] FromHexString(string value)
        {
            if (null == value)
                throw new ArgumentNullException(nameof(value));

            if (0 == value.Length)
                return new byte[] { };

            if (0 != value.Length % 2)
                throw new ArgumentException();

            return Enumerable.Range(0, value.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(value.Substring(x, 2), 16))
                .ToArray();
        }
    }
}
