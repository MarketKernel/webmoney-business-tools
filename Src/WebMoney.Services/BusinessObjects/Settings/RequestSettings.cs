using System;
using System.Xml.Serialization;
using AutoMapper;
using WebMoney.Services.BusinessObjects.Annotations;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Utils;

namespace WebMoney.Services.BusinessObjects
{
    public sealed class RequestSettings : IRequestSettings, ICloneable, IEquatable<RequestSettings>
    {
        [LocalizedDisplayName("Maximum transaction request period")]
        [XmlAttribute("transferMaxRequestPeriod")]
        public byte TransferMaxRequestPeriod { get; set; }

        [LocalizedDisplayName("Maximum incoming invoice request period")]
        [XmlAttribute("incomingInvoiceMaxRequestPeriod")]
        public byte IncomingInvoiceMaxRequestPeriod { get; set; }

        [LocalizedDisplayName("Maximum outgoing invoice request period")]
        [XmlAttribute("outgoingInvoiceMaxRequestPeriod")]
        public byte OutgoingInvoiceMaxRequestPeriod { get; set; }

        public static RequestSettings Create(IRequestSettings contractObject)
        {
            if (null == contractObject)
                return null;

            if (contractObject is RequestSettings businessObject)
                return businessObject;

            return Mapper.Map<RequestSettings>(contractObject);
        }

        public bool Equals(RequestSettings other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return TransferMaxRequestPeriod == other.TransferMaxRequestPeriod &&
                   IncomingInvoiceMaxRequestPeriod == other.IncomingInvoiceMaxRequestPeriod &&
                   OutgoingInvoiceMaxRequestPeriod == other.OutgoingInvoiceMaxRequestPeriod;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj is RequestSettings requestSettings && Equals(requestSettings);
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
