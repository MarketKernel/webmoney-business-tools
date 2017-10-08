using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;

namespace WebMoney.Services.Utils
{
    internal static class Encryptor
    {
        private const int Iterations = 0x01;
        private static readonly byte[] Salt = { 0xA6, 0x2E, 0x75, 0xF8, 0xB9, 0xC3, 0x92, 0xB1 };

        public static byte[] Encrypt(byte[] value, SecureString securePassword)
        {
            if (null == value)
                throw new ArgumentNullException(nameof(value));

            if (null == securePassword)
                throw new ArgumentNullException(nameof(securePassword));

            if (0 == securePassword.Length)
                throw new ArgumentOutOfRangeException(nameof(securePassword));

            byte[] encryptedValue;

            using (var aes = CreateAes(securePassword))
            {
                encryptedValue = Encrypt(value, aes);
                aes.Clear();
            }

            return encryptedValue;
        }

        public static byte[] Decrypt(byte[] encryptedValue, SecureString securePassword)
        {
            if (null == encryptedValue)
                throw new ArgumentNullException(nameof(encryptedValue));

            if (null == securePassword)
                throw new ArgumentNullException(nameof(securePassword));

            if (0 == securePassword.Length)
                throw new ArgumentOutOfRangeException(nameof(securePassword));

            byte[] value;

            using (var aes = CreateAes(securePassword))
            {
                value = Decrypt(encryptedValue, aes);
                aes.Clear();
            }

            return value;
        }

        public static byte[] Protect(byte[] value)
        {
            if (null == value)
                throw new ArgumentNullException(nameof(value));

            return ProtectedData.Protect(value, null, DataProtectionScope.CurrentUser);
        }

        public static byte[] Unprotect(byte[] protectedValue)
        {
            if (null == protectedValue)
                throw new ArgumentNullException(nameof(protectedValue));

            return ProtectedData.Unprotect(protectedValue, null, DataProtectionScope.CurrentUser);
        }

        private static Aes CreateAes(SecureString securePassword)
        {
            Aes aes;

            byte[] password = null;
            Rfc2898DeriveBytes deriveBytes = null;

            try
            {
                password = securePassword.ToByteArray();
                deriveBytes = new Rfc2898DeriveBytes(password, Salt, Iterations);

                aes = new AesManaged
                {
                    KeySize = 256,
                    Key = deriveBytes.GetBytes(32),
                    IV = deriveBytes.GetBytes(16),
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.ISO10126
                };
            }
            finally
            {
                if (null != password)
                    Array.Clear(password, 0, password.Length);

                deriveBytes?.Dispose();
            }

            return aes;
        }

        private static byte[] Encrypt(byte[] value, SymmetricAlgorithm algorithm)
        {
            byte[] result;

            using (var targetStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(targetStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(value, 0, value.Length);
                    cryptoStream.Flush();
                }

                targetStream.Flush();
                result = targetStream.ToArray();
            }

            return result;
        }

        private static byte[] Decrypt(byte[] value, SymmetricAlgorithm algorithm)
        {
            byte[] result;

            using (var sourceStream = new MemoryStream(value))
            using (var cryptoStream = new CryptoStream(sourceStream, algorithm.CreateDecryptor(),
                CryptoStreamMode.Read))
            using (var targetStream = new MemoryStream())
            {
                var buffer = new byte[1024];

                while (true)
                {
                    int length = cryptoStream.Read(buffer, 0, buffer.Length);

                    if (length > 0)
                        targetStream.Write(buffer, 0, length);
                    else
                        break;
                }

                targetStream.Flush();
                result = targetStream.ToArray();
            }

            return result;
        }
    }
}
