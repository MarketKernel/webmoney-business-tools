using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace ManifestSigner
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Using: source target thumbprint");
                return 1;
            }

            string sourceFilePath = args[0];
            string targetFilePath = args[1];
            string thumbprint = args[2];

            var privateKey = ObtainPrivateKey(thumbprint);

            SignXmlFile(sourceFilePath, targetFilePath, privateKey);

            return 0;
        }

        private static RSA ObtainPrivateKey(string thumbprint)
        {
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.MaxAllowed);
            var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

            var certificate = certificates.Cast<X509Certificate2>().First();

            return (RSA)certificate.PrivateKey;
        }

        public static void SignXmlFile(string sourceFilePath, string targetFilePath, RSA privateKey)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(new XmlTextReader(sourceFilePath));

            var signedXml = new SignedXml(xmlDocument)
            {
                SigningKey = privateKey
            };

            var reference = new Reference { Uri = string.Empty };

            var xmlDsigEnvelopedSignatureTransform = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(xmlDsigEnvelopedSignatureTransform);

            signedXml.AddReference(reference);

            signedXml.ComputeSignature();

            var xmlDigitalSignature = signedXml.GetXml();

            xmlDocument.DocumentElement?.AppendChild(xmlDocument.ImportNode(xmlDigitalSignature, true));

            if (xmlDocument.FirstChild is XmlDeclaration)
                xmlDocument.RemoveChild(xmlDocument.FirstChild);

            using (var xmlTextWriter = new XmlTextWriter(targetFilePath, new UTF8Encoding(false)))
            {
                xmlDocument.WriteTo(xmlTextWriter);
                xmlTextWriter.Close();
            }
        }
    }
}
