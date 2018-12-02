param (
[String]$sourceFilePath,
[String]$targetFilePath,
[String]$thumbprint
)

$ReferencingAssemblies = ("C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.XML.dll",
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Security.dll")

$Source = @"
using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

public class ManifestSigner
{
	public static int SignManifest(string sourceFilePath, string targetFilePath, string thumbprint)
	{
		if (null == sourceFilePath)
		    throw new ArgumentNullException("sourceFilePath");
			
		if (null == targetFilePath)
		    throw new ArgumentNullException("targetFilePath");
			
		if (null == thumbprint)
		    throw new ArgumentNullException("thumbprint");
		
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

	private static void SignXmlFile(string sourceFilePath, string targetFilePath, RSA privateKey)
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

		xmlDocument.DocumentElement.AppendChild(xmlDocument.ImportNode(xmlDigitalSignature, true));

		if (xmlDocument.FirstChild is XmlDeclaration)
			xmlDocument.RemoveChild(xmlDocument.FirstChild);

		using (var xmlTextWriter = new XmlTextWriter(targetFilePath, new UTF8Encoding(false)))
		{
			xmlDocument.WriteTo(xmlTextWriter);
			xmlTextWriter.Close();
		}
	}
}
"@

if (-not("ManifestSigner" -as [type]))
{
    Add-Type -TypeDefinition $Source -ReferencedAssemblies $ReferencingAssemblies -Language CSharp
}

[ManifestSigner]::SignManifest($sourceFilePath, $targetFilePath, $thumbprint)

pause