using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace PartialTrustInstaller.Utils
{
    /// <summary>
    /// Очень упрощенная канонизация XML (нет полноценной поддержки пространств имен и пр.).
    /// </summary>
    internal static class Canonicalizer
    {
        internal static string Canonize(XDocument xDocument)
        {
            if (null == xDocument)
                throw new ArgumentNullException(nameof(xDocument));

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xDocument.ToString(SaveOptions.OmitDuplicateNamespaces));

            var stringBuilder = new StringBuilder();

            using (var stringWriter = new StringWriter(stringBuilder))
            using (var xmlWriter = new XmlTextWriter(stringWriter))
            {

                ProcessNode(xmlWriter, xmlDocument.DocumentElement);
                xmlWriter.Flush();
                xmlWriter.Close();
            }

            return stringBuilder.ToString();
        }

        private static void ProcessNode(XmlWriter xmlWriter, XmlNode node)
        {
            if (node.NodeType == XmlNodeType.Text)
            {
                xmlWriter.WriteString(node.Value);
                return;
            }

            xmlWriter.WriteStartElement(node.Name);

            var attributes = node.Attributes?.Cast<XmlAttribute>().OrderBy(a => a.Name);

            if (null != attributes)
                foreach (var attribute in attributes)
                {
                    xmlWriter.WriteAttributeString(attribute.Name, attribute.Value);
                }

            foreach (XmlNode childNode in node.ChildNodes)
            {
                ProcessNode(xmlWriter, childNode);
            }

            xmlWriter.WriteFullEndElement();
        }
    }
}
