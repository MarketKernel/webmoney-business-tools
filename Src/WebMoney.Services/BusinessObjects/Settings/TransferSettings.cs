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
    public sealed class TransferSettings : ITransferSettings, ICloneable, IEquatable<TransferSettings>
    {
        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display secondary system ID")]
        [XmlAttribute("secondaryIdVisibility")]
        public bool SecondaryIdVisibility { get; set; }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display source purse")]
        [XmlAttribute("sourcePurseVisibility")]
        public bool SourcePurseVisibility { get; set; } = true;

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display target purse")]
        [XmlAttribute("targetPurseVisibility")]
        public bool TargetPurseVisibility { get; set; } = true;

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display commission")]
        [XmlAttribute("commissionVisibility")]
        public bool CommissionVisibility { get; set; }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display description")]
        [XmlAttribute("descriptionVisibility")]
        public bool DescriptionVisibility { get; set; } = true;

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display transfer type")]
        [XmlAttribute("typeVisibility")]
        public bool TypeVisibility { get; set; } = true;

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display invoice ID")]
        [XmlAttribute("invoiceIdVisibility")]
        public bool InvoiceIdVisibility { get; set; }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display order ID")]
        [XmlAttribute("orderIdVisibility")]
        public bool OrderIdVisibility { get; set; }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display transfer ID")]
        [XmlAttribute("transferIdVisibility")]
        public bool TransferIdVisibility { get; set; }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display protection period")]
        [XmlAttribute("protectionPeriodVisibility")]
        public bool ProtectionPeriodVisibility { get; set; }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display partner WMID")]
        [XmlAttribute("partnerIdentifierVisibility")]
        public bool PartnerIdentifierVisibility { get; set; } = true;

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display balance")]
        [XmlAttribute("balanceVisibility")]
        public bool BalanceVisibility { get; set; }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display locked flag")]
        [XmlAttribute("lockedVisibility")]
        public bool LockedVisibility { get; set; }

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

        public TransferSettings()
        {
            ColumnOrders = new Collection<int>();
            ColumnWidths = new Collection<int>();
        }

        public static TransferSettings Create(ITransferSettings contractObject)
        {
            if (null == contractObject)
                return null;

            var businessObject = contractObject as TransferSettings;

            if (businessObject != null)
                return businessObject;

            return Mapper.Map<TransferSettings>(contractObject);
        }

        public bool Equals(TransferSettings other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!(SecondaryIdVisibility == other.SecondaryIdVisibility &&
                  SourcePurseVisibility == other.SourcePurseVisibility &&
                  TargetPurseVisibility == other.TargetPurseVisibility &&
                  CommissionVisibility == other.CommissionVisibility &&
                  DescriptionVisibility == other.DescriptionVisibility && TypeVisibility == other.TypeVisibility &&
                  InvoiceIdVisibility == other.InvoiceIdVisibility && OrderIdVisibility == other.OrderIdVisibility &&
                  TransferIdVisibility == other.TransferIdVisibility &&
                  ProtectionPeriodVisibility == other.ProtectionPeriodVisibility &&
                  PartnerIdentifierVisibility == other.PartnerIdentifierVisibility &&
                  BalanceVisibility == other.BalanceVisibility && LockedVisibility == other.LockedVisibility &&
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

            var transferSettings = obj as TransferSettings;
            return transferSettings != null && Equals(transferSettings);
        }

        public override int GetHashCode()
        {
            return 1;
        }

        public object Clone()
        {
            return Mapper.Map<TransferSettings>(this);
        }
    }
}
