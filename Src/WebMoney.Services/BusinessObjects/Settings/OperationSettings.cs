using System;
using System.Drawing;
using System.Xml.Serialization;
using AutoMapper;
using WebMoney.Services.BusinessObjects.Annotations;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    public sealed class OperationSettings : IOperationSettings, ICloneable, IEquatable<OperationSettings>
    {
        [LocalizedDisplayName("Income operation font color")]
        [XmlElement("incomeForeColor")]
        public Color IncomeForeColor { get; set; } = Color.FromArgb(40, 127, 0);

        [LocalizedDisplayName("Outcome operation font color")]
        [XmlElement("outcomeForeColor")]
        public Color OutcomeForeColor { get; set; } = Color.FromArgb(167, 0, 0);

        [LocalizedDisplayName("Income color for chart")]
        [XmlElement("incomeChartColor")]
        public Color IncomeChartColor { get; set; } = Color.FromArgb(66, 179, 113);

        [LocalizedDisplayName("Outcome color for chart")]
        [XmlElement("outcomeChartColor")]
        public Color OutcomeChartColor { get; set; } = Color.FromArgb(255, 67, 67);

        [LocalizedDisplayName("Selection color")]
        [XmlElement("selectionColor")]
        public Color SelectionColor { get; set; } = Color.FromArgb(204, 232, 255);

        [LocalizedDisplayName("Paid with protection color")]
        [XmlElement("protectedColor")]
        public Color ProtectedColor { get; set; } = Color.FromArgb(255, 246, 151);

        public static OperationSettings Create(IOperationSettings contractObject)
        {
            if (null == contractObject)
                return null;

            var businessObject = contractObject as OperationSettings;

            if (businessObject != null)
                return businessObject;

            return Mapper.Map<OperationSettings>(contractObject);
        }

        public bool Equals(OperationSettings other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return IncomeForeColor.Equals(other.IncomeForeColor) && OutcomeForeColor.Equals(other.OutcomeForeColor) &&
                   IncomeChartColor.Equals(other.IncomeChartColor) &&
                   OutcomeChartColor.Equals(other.OutcomeChartColor) && SelectionColor.Equals(other.SelectionColor) &&
                   ProtectedColor.Equals(other.ProtectedColor);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var operationSettings = obj as OperationSettings;
            return operationSettings != null && Equals(operationSettings);
        }

        public override int GetHashCode()
        {
            return 1;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}