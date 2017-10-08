using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;
using AutoMapper;
using WebMoney.Services.BusinessObjects.Annotations;
using WebMoney.Services.Contracts.BusinessObjects;
using System.Linq;

namespace WebMoney.Services.BusinessObjects
{
    public sealed class TransferBundleSettings : ITransferBundleSettings, ICloneable, IEquatable<TransferBundleSettings>
    {
        [LocalizedDisplayName("Selection color")]
        [XmlElement("selectionColor")]
        public Color SelectionColor { get; set; } = Color.FromArgb(204, 232, 255);

        [LocalizedDisplayName("Pended bundle color")]
        [XmlElement("pendedColor")]
        public Color PendedColor { get; set; } = Color.FromArgb(255, 246, 151);

        [LocalizedDisplayName("Processed bundle color")]
        [XmlElement("processed")]
        public Color ProcessedColor { get; set; } = Color.FromArgb(255, 246, 151);

        [LocalizedDisplayName("Interrupted bundle color")]
        [XmlElement("interrupted")]
        public Color InterruptedColor { get; set; } = Color.FromArgb(255, 188, 170);

        [LocalizedDisplayName("Completed bundle color")]
        [XmlElement("interrupted")]
        public Color CompletedColor { get; set; } = Color.FromArgb(151, 255, 187);

        [Browsable(false)]
        [XmlArray(ElementName = "columnOrders")]
        [XmlArrayItem("columnOrder")]
        public Collection<int> ColumnOrders { get; private set; }

        [Browsable(false)]
        [XmlArray(ElementName = "columnWidths")]
        [XmlArrayItem("columnWidth")]
        public Collection<int> ColumnWidths { get; private set; }

        public TransferBundleSettings()
        {
            ColumnOrders = new Collection<int>();
            ColumnWidths = new Collection<int>();
        }

        public static TransferBundleSettings Create(ITransferBundleSettings contractObject)
        {
            if (null == contractObject)
                return null;

            var businessObject = contractObject as TransferBundleSettings;

            if (businessObject != null)
                return businessObject;

            return Mapper.Map<TransferBundleSettings>(contractObject);
        }

        public bool Equals(TransferBundleSettings other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (!(SelectionColor.Equals(other.SelectionColor) &&
                  PendedColor.Equals(other.PendedColor) &&
                  ProcessedColor.Equals(other.ProcessedColor) &&
                  InterruptedColor.Equals(other.InterruptedColor) &&
                  CompletedColor.Equals(other.CompletedColor)))
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
            var bundleSettings = obj as TransferBundleSettings;
            return bundleSettings != null && Equals(bundleSettings);
        }

        public override int GetHashCode()
        {
            return 1;
        }

        public object Clone()
        {
            return Mapper.Map<TransferBundleSettings>(this);
        }
    }
}
