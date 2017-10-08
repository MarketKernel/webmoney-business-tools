using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace WebMoney.Services.Utils
{
    internal static class SecureStringExtensions
    {
        public static void FromByteArray(this SecureString secureString, byte[] byteArray, Encoding encoding = null)
        {
            if (null == secureString)
                throw new ArgumentNullException(nameof(secureString));

            if (null == byteArray)
                throw new ArgumentNullException(nameof(byteArray));

            if (secureString.IsReadOnly())
                throw new InvalidOperationException("secureString.IsReadOnly()");

            if (null == encoding)
                encoding = Encoding.UTF8;

            char[] chars = null;

            try
            {
                chars = encoding.GetChars(byteArray);

                foreach (var c in chars)
                {
                    secureString.AppendChar(c);
                }
            }
            finally
            {
                if (null != chars)
                    for (int i = 0; i < chars.Length; i++)
                    {
                        chars[i] = Char.MinValue;
                    }
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Unrestricted = true)]
        public static byte[] ToByteArray(this SecureString secureString, Encoding encoding = null)
        {
            if (null == secureString)
                throw new ArgumentNullException(nameof(secureString));

            if (0 == secureString.Length)
                throw new ArgumentOutOfRangeException(nameof(secureString));

            if (null == encoding)
                encoding = Encoding.UTF8;

            var memory = IntPtr.Zero;
            byte[] password = null;

            try
            {
                password = new byte[secureString.Length * 2];

                memory = Marshal.SecureStringToCoTaskMemUnicode(secureString);
                Marshal.Copy(memory, password, 0, password.Length);

                return Encoding.Convert(Encoding.Unicode, encoding, password);
            }
            finally
            {
                Marshal.ZeroFreeCoTaskMemUnicode(memory);

                if (null != password)
                    Array.Clear(password, 0, password.Length);
            }
        }
    }
}
