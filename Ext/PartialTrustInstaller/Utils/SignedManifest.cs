using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PartialTrustInstaller.Utils
{
    internal class SignedManifest
    {
        private const string Namespace = "http://www.w3.org/2000/09/xmldsig#";
        
        private const string DigestValueExpression = "ds:Signature/ds:SignedInfo/ds:Reference[@URI=\"\"]/ds:DigestValue";
        private const string SignatureElementExpression = "ds:Signature";
        private const string SignedInfoExpression = "ds:Signature/ds:SignedInfo";
        private const string SignatureValueExpression = "ds:Signature/ds:SignatureValue";

        private readonly XDocument _xDocument;
        private readonly XmlNamespaceManager _namespaceManager;

        public string Version { get; }
        public DateTime Date { get; }
        public string Digest { get; }
        public string PackageUrl { get; }

        public SignedManifest(XDocument xDocument, int osMajorVersion)
        {
            _xDocument = xDocument ?? throw new ArgumentNullException(nameof(xDocument));

            var packageElement =
                (from element in xDocument.Root?.Elements()
                    let osVersionValue = element.Attribute("OSMajorVersion")?.Value
                    where osVersionValue != null
                    let osVersion = int.Parse(osVersionValue)
                    where osVersion <= osMajorVersion
                    orderby osVersion descending
                    select element).FirstOrDefault();

            if (null == packageElement)
                throw new InvalidOperationException("null == packageElement");

            // Version
            var version = packageElement.Attribute("Version")?.Value;
            Version = version ?? throw new InvalidOperationException("null == version");

            // Date
            var dateValue = packageElement.Attribute("ReleaseDate")?.Value;

            if (null == dateValue)
                throw new InvalidOperationException("null == dateValue");

            if (!DateTime.TryParseExact(dateValue, "MM/dd/yyyy", CultureInfo.InvariantCulture.DateTimeFormat,
                DateTimeStyles.AdjustToUniversal, out var date))
                throw new InvalidOperationException("dateValue == " + dateValue);

            Date = date;

            // Digest
            var digestValue = packageElement.Attribute("Digest")?.Value;
            Digest = digestValue ?? throw new InvalidOperationException("null == digestValue");

            // PackageUrl
            var packageUrl = packageElement.Value;
            PackageUrl = packageUrl;

            var namespaceManager = new XmlNamespaceManager(new NameTable());
            namespaceManager.AddNamespace("ds", Namespace);

            _namespaceManager = namespaceManager;
        }

        public bool CheckSignature(RSACryptoServiceProvider publicKey)
        {
            if (null == publicKey)
                throw new ArgumentNullException(nameof(publicKey));

            var expectedDigest = ComputeDigest();
            var digest = ObtainDigest();

            if (!expectedDigest.SequenceEqual(digest))
                return false;

            return VerifySignature(publicKey);
        }

        private byte[] ComputeDigest()
        {
            var xDocument = new XDocument(_xDocument);

            var signatureElement = xDocument.Root?.XPathSelectElement(SignatureElementExpression, _namespaceManager);

            if (null == signatureElement)
                throw new CryptographicException("null == signatureElement");

            signatureElement.Remove();

            var canonizedXml = Canonicalizer.Canonize(xDocument);

            byte[] hash;

            using (var sha256 = new SHA256Managed())
            {
                hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(canonizedXml));
            }

            return hash;
        }

        private byte[] ObtainDigest()
        {
            var documentRoot = _xDocument.Root;

            var digestValueElement = documentRoot?.XPathSelectElement(DigestValueExpression, _namespaceManager);

            if (null == digestValueElement)
                throw new CryptographicException("null == digestValueElement");

            byte[] digest;

            try
            {
                digest = Convert.FromBase64String(digestValueElement.Value);
            }
            catch (Exception exception) when (exception is FormatException || exception is ArgumentNullException)
            {
                throw new CryptographicException("Wrong format of digest value!");
            }

            return digest;
        }

        private bool VerifySignature(RSACryptoServiceProvider publicKey)
        {
            var signatureValueElement =
                _xDocument.Root?.XPathSelectElement(SignatureValueExpression, _namespaceManager);

            if (null == signatureValueElement)
                throw new CryptographicException("null == signatureValueElement");

            byte[] signature;

            try
            {
                signature = Convert.FromBase64String(signatureValueElement.Value);
            }
            catch (Exception exception) when (exception is FormatException || exception is ArgumentNullException)
            {
                throw new CryptographicException("Wrong format of signature value!");
            }

            var signedInfoElement = _xDocument.Root.XPathSelectElement(SignedInfoExpression, _namespaceManager);

            var xDocument = new XDocument();
            xDocument.Add(signedInfoElement);

            var canonizedSignedInfo = Canonicalizer.Canonize(xDocument);

            return publicKey.VerifyData(Encoding.UTF8.GetBytes(canonizedSignedInfo), new SHA256Managed(), signature);
        }
    }
}