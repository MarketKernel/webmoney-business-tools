using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Xml.Serialization;
using AutoMapper;
using WebMoney.Services.BusinessObjects.Annotations;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Utils;

namespace WebMoney.Services.BusinessObjects
{
    public sealed class ContractSettings : IContractSettings, ICloneable, IEquatable<ContractSettings>
    {
        [LocalizedDisplayName("Public contract font color")]
        [XmlElement("publicContractForeColor")]
        public Color PublicForeColor { get; set; } = Color.FromArgb(0, 0, 255);

        [LocalizedDisplayName("Selection color")]
        [XmlElement("selectionColor")]
        public Color SelectionColor { get; set; } = Color.FromArgb(204, 232, 255);

        [LocalizedDisplayName("Signed contract color")]
        [XmlElement("signedColor")]
        public Color SignedColor { get; set; } = Color.FromArgb(151, 255, 187);

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display text")]
        [XmlElement("textVisibility")]
        public bool TextVisibility { get; set; }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display accepted count")]
        [XmlElement("acceptedCountVisibility")]
        public bool AcceptedCountVisibility { get; set; }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Display access count")]
        [XmlElement("accessCountVisibility")]
        public bool AccessCountVisibility { get; set; }

        [Browsable(false)]
        [XmlArray(ElementName = "columnOrders")]
        [XmlArrayItem("columnOrder")]
        public Collection<int> ColumnOrders { get; private set; }

        [Browsable(false)]
        [XmlArray(ElementName = "columnWidths")]
        [XmlArrayItem("columnWidth")]
        public Collection<int> ColumnWidths { get; private set; }

        public ContractSettings()
        {
            ColumnOrders = new Collection<int>();
            ColumnWidths = new Collection<int>();
        }

        public static ContractSettings Create(IContractSettings contractObject)
        {
            if (null == contractObject)
                return null;

            if (contractObject is ContractSettings businessObject)
                return businessObject;

            return Mapper.Map<ContractSettings>(contractObject);
        }

        public bool Equals(ContractSettings other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!(PublicForeColor.Equals(other.PublicForeColor) && SelectionColor.Equals(other.SelectionColor) &&
                  SignedColor.Equals(other.SignedColor) && TextVisibility == other.TextVisibility &&
                  AcceptedCountVisibility == other.AcceptedCountVisibility &&
                  AccessCountVisibility == other.AccessCountVisibility))
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
            return obj is ContractSettings contractSettings && Equals(contractSettings);
        }

        public override int GetHashCode()
        {
            return 1;
        }

        public object Clone()
        {
            var o = MemberwiseClone();
            CloneUtility.CloneProperties(o);

            return o;
        }
    }
}
