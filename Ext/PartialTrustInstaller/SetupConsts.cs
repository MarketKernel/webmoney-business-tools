using System;
using System.IO;
using System.Security.Cryptography;

namespace PartialTrustInstaller
{
    internal static class SetupConsts
    {
        public const string ManifestUrl = "http://www.webmoney-business-tools.com/dist/Manifest.xml";
        public const string AppStartupFile = "WMBusinessTools.exe";

        private const string CompanyName = "MarketKernel";
        private const string ProductName = "WMBusinessTools";
        private const string PublicKeyXmlValue =
            "<RSAKeyValue><Modulus>t4tbqMQlla+1BdMWWl6LWU8rU9DBv4wUVwPhozEuYQz4ZcXpmDbg8KIuZqTLXEjBUUM/D/pQEkrTN1nSPlI6XI0kAd+AT7ox7hwMhs6501Iawtmd/78CHFRp+UZqZiQL/i7cQ7Ms8FYSxIm3VmsHhdA2FzkfjbW4Jod55znVv/hW+C1e2Bz0PzXENgEx0WvImH9yo+Gj6xqyPnk9Zk1IsxH6UauaFUd45DIlz63uXJ6QyoJJkE+3yE/X7NiMvzAShNRGX6Kqm99pKrCQdsXTcgDLnTBuID24X3l69y82NsyeiWgVxPvnlllVg6QABVPeAaX6rJ3PpXMR2BkZ2SpMOQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        public static readonly string AppDirectory;
        public static readonly RSACryptoServiceProvider PublicKey;

        static SetupConsts()
        {
            RSACryptoServiceProvider.UseMachineKeyStore = false;
            var publicKey = new RSACryptoServiceProvider();
            publicKey.FromXmlString(PublicKeyXmlValue);

            PublicKey = publicKey;

            var appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                CompanyName, ProductName);
            AppDirectory = appDirectory;
        }
    }
}