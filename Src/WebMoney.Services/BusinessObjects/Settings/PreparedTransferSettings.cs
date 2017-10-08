using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Xml.Serialization;
using AutoMapper;
using WebMoney.Services.BusinessObjects.Annotations;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    public sealed class PreparedTransferSettings : IPreparedTransferSettings, ICloneable, IEquatable<PreparedTransferSettings>
    {
        [LocalizedDisplayName("Failed transfer color")]
        [XmlElement("failedColor")]
        public Color FailedColor { get; set; } = Color.FromArgb(255, 188, 170);

        [LocalizedDisplayName("Selection transfer color")]
        [XmlElement("selectionColor")]
        public Color SelectionColor { get; set; } = Color.FromArgb(204, 232, 255);

        [LocalizedDisplayName("Pended transfer color")]
        [XmlElement("pendedColor")]
        public Color PendedColor { get; set; } = Color.FromArgb(255, 246, 151);

        [LocalizedDisplayName("Processed transfer color")]
        [XmlElement("processedColor")]
        public Color ProcessedColor { get; set; } = Color.FromArgb(255, 246, 151);

        [LocalizedDisplayName("Interrupted transfer color")]
        [XmlElement("interruptedColor")]
        public Color InterruptedColor { get; set; } = Color.FromArgb(255, 188, 170);

        [LocalizedDisplayName("Completed transfer color")]
        [XmlElement("completedColor")]
        public Color CompletedColor { get; set; } = Color.FromArgb(151, 255, 187);

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display system ID")]
        [XmlAttribute("primaryIdVisibility")]
        public bool PrimaryIdVisibility { get; set; }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display secondary system ID")]
        [XmlAttribute("secondaryIdVisibility")]
        public bool SecondaryIdVisibility { get; set; }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display transfer ID")]
        [XmlAttribute("transferIdVisibility")]
        public bool TransferIdVisibility { get; set; }

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
        [LocalizedDisplayName("Display force flag")]
        [XmlAttribute("forceVisibility")]
        public bool ForceVisibility { get; set; }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display creation time")]
        [XmlAttribute("creationTimeVisibility")]
        public bool CreationTimeVisibility { get; set; } = true;

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display transfer creation time")]
        [XmlAttribute("transferCreationTimeVisibility")]
        public bool TransferCreationTimeVisibility { get; set; }

        [Browsable(false)]
        [XmlArray(ElementName = "columnOrders")]
        [XmlArrayItem("columnOrder")]
        public Collection<int> ColumnOrders { get; private set; }

        [Browsable(false)]
        [XmlArray(ElementName = "columnWidths")]
        [XmlArrayItem("columnWidth")]
        public Collection<int> ColumnWidths { get; private set; }

        public PreparedTransferSettings()
        {
            ColumnOrders = new Collection<int>();
            ColumnWidths = new Collection<int>();
        }

        public static PreparedTransferSettings Create(IPreparedTransferSettings contractObject)
        {
            if (null == contractObject)
                return null;

            var businessObject = contractObject as PreparedTransferSettings;

            if (businessObject != null)
                return businessObject;

            return Mapper.Map<PreparedTransferSettings>(contractObject);
        }

        public bool Equals(PreparedTransferSettings other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (!(FailedColor.Equals(other.FailedColor) &&
                  SelectionColor.Equals(other.SelectionColor) &&
                  PendedColor.Equals(other.PendedColor) &&
                  ProcessedColor.Equals(other.ProcessedColor) &&
                  InterruptedColor.Equals(other.InterruptedColor) &&
                  CompletedColor.Equals(other.CompletedColor) &&
                  PrimaryIdVisibility == other.PrimaryIdVisibility &&
                  SecondaryIdVisibility == other.SecondaryIdVisibility &&
                  TransferIdVisibility == other.TransferIdVisibility &&
                  SourcePurseVisibility == other.SourcePurseVisibility &&
                  TargetPurseVisibility == other.TargetPurseVisibility &&
                  DescriptionVisibility == other.DescriptionVisibility &&
                  ForceVisibility == other.ForceVisibility &&
                  CreationTimeVisibility == other.CreationTimeVisibility &&
                  TransferCreationTimeVisibility == other.TransferCreationTimeVisibility))
                return false;

            if (!ColumnOrders.SequenceEqual(other.ColumnOrders))
                return false;

            if (!ColumnWidths.SequenceEqual(other.ColumnWidths))
                return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is PreparedTransferSettings && Equals((PreparedTransferSettings)obj);
        }

        public override int GetHashCode()
        {
            return 1;
        }

        public object Clone()
        {
            return Mapper.Map<PreparedTransferSettings>(this);
        }
    }
}
