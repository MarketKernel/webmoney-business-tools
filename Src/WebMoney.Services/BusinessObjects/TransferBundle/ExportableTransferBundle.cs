using System.Collections.Generic;
using System.Xml.Serialization;

namespace WebMoney.Services.BusinessObjects
{
    [XmlRoot("payments", Namespace = "http://tempuri.org/ds.xsd")]
    public sealed class ExportableTransferBundle
    {
        [XmlElement(ElementName = "payment")]
        public List<ExportableTransfer> Transfers { get; }

        public ExportableTransferBundle()
        {
            Transfers = new List<ExportableTransfer>();
        }
    }
}