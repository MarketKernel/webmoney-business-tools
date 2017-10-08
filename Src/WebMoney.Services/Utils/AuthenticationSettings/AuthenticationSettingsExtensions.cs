using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Cryptography;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts.Exceptions;

namespace WebMoney.Services.Utils
{
    internal static class AuthenticationSettingsExtensions
    {
        public static void Save(this AuthenticationSettings authenticationSettings, string path,
            SecureString securePassword)
        {
            if (null == path)
                throw new ArgumentNullException(nameof(path));

            var bytes = SerializationUtility.Serialize(authenticationSettings);

            if (null != securePassword && securePassword.Length > 0)
                bytes = Encryptor.Encrypt(bytes, securePassword);

            if (!ApplicationUtility.IsUnix)
                bytes = Encryptor.Protect(bytes);

            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                fileStream.Write(bytes, 0, bytes.Length);

                fileStream.Close();
            }
        }

        public static AuthenticationSettings Load(string path, SecureString securePassword)
        {
            if (null == path)
                throw new ArgumentNullException(nameof(path));

            byte[] bytes;

            using (var sourceStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var targetStream = new MemoryStream())
            {
                byte[] buffer = new byte[1024];

                int bytesRead;

                while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    targetStream.Write(buffer, 0, bytesRead);
                }

                targetStream.Flush();
                bytes = targetStream.ToArray();
            }

            if (!ApplicationUtility.IsUnix)
                try
                {
                    bytes = Encryptor.Unprotect(bytes);
                }
                catch (CryptographicException exception)
                {
                    throw new WrongPasswordException("Wrong password or user profile has been changed!", exception);
                }

            if (null != securePassword && securePassword.Length > 0)
            {
                try
                {
                    bytes = Encryptor.Decrypt(bytes, securePassword);
                }
                catch (CryptographicException exception)
                {
                    throw new WrongPasswordException("Wrong password!", exception);
                }
            }

            try
            {
                return (AuthenticationSettings) SerializationUtility.Deserialize(bytes);
            }
            catch (SerializationException exception)
            {
                throw new WrongPasswordException("Wrong password!", exception);
            }
        }
    }
}
