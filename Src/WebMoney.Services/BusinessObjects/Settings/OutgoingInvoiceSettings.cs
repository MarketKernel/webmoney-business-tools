using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using AutoMapper;
using WebMoney.Services.BusinessObjects.Annotations;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    public sealed class OutgoingInvoiceSettings : IOutgoingInvoiceSettings, ICloneable, IEquatable<OutgoingInvoiceSettings>
    {
        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display secondary system ID")]
        [XmlAttribute("secondaryIdVisibility")]
        public bool SecondaryIdVisibility { get; set; }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display order ID")]
        [XmlAttribute("orderIdVisibility")]
        public bool OrderIdVisibility { get; set; }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display source purse")]
        [XmlAttribute("sourcePurseVisibility")]
        public bool SourcePurseVisibility { get; set; } = true;

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display target purse")]
        [XmlAttribute("targetPurseVisibility")]
        public bool TargetPurseVisibility { get; set; } = true;

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display description")]
        [XmlAttribute("descriptionVisibility")]
        public bool DescriptionVisibility { get; set; } = true;

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display address")]
        [XmlAttribute("addressVisibility")]
        public bool AddressVisibility { get; set; }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display protection period")]
        [XmlAttribute("protectionPeriodVisibility")]
        public bool ProtectionPeriodVisibility { get; set; }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display expiration period")]
        [XmlAttribute("expirationPeriodVisibility")]
        public bool ExpirationPeriodVisibility { get; set; }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display transfer system ID")]
        [XmlAttribute("transferPrimaryIdVisibility")]
        public bool TransferPrimaryIdVisibility { get; set; }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display creation time")]
        [XmlAttribute("creationTimeVisibility")]
        public bool CreationTimeVisibility { get; set; } = true;

        [Browsable(false)]
        [XmlArray(ElementName = "columnOrders")]
        [XmlArrayItem("columnOrder")]
        public Collection<int> ColumnOrders { get; private set; }

        [Browsable(false)]
        [XmlArray(ElementName = "columnWidths")]
        [XmlArrayItem("columnWidth")]
        public Collection<int> ColumnWidths { get; private set; }

        public OutgoingInvoiceSettings()
        {
            ColumnOrders = new Collection<int>();
            ColumnWidths = new Collection<int>();
        }

        public static OutgoingInvoiceSettings Create(IOutgoingInvoiceSettings contractObject)
        {
            if (null == contractObject)
                return null;

            var businessObject = contractObject as OutgoingInvoiceSettings;

            if (businessObject != null)
                return businessObject;

            return Mapper.Map<OutgoingInvoiceSettings>(contractObject);
        }

        public bool Equals(OutgoingInvoiceSettings other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!(SecondaryIdVisibility == other.SecondaryIdVisibility &&
                  OrderIdVisibility == other.OrderIdVisibility &&
                  SourcePurseVisibility == other.SourcePurseVisibility &&
                  TargetPurseVisibility == other.TargetPurseVisibility &&
                  DescriptionVisibility == other.DescriptionVisibility &&
                  AddressVisibility == other.AddressVisibility &&
                  ProtectionPeriodVisibility == other.ProtectionPeriodVisibility &&
                  ExpirationPeriodVisibility == other.ExpirationPeriodVisibility &&
                  TransferPrimaryIdVisibility == other.TransferPrimaryIdVisibility &&
                  CreationTimeVisibility == other.CreationTimeVisibility))
                return false;

            if (!ColumnOrders.SequenceEqual(other.ColumnOrders))
                return false;

            if (!ColumnWidths.SequenceEqual(other.ColumnWidths))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            var outgoingInvoiceSettings = obj as OutgoingInvoiceSettings;
            return outgoingInvoiceSettings != null && Equals(outgoingInvoiceSettings);
        }

        public override int GetHashCode()
        {
            return 1;
        }

        public object Clone()
        {
            return Mapper.Map<OutgoingInvoiceSettings>(this);
        }
    }
}
