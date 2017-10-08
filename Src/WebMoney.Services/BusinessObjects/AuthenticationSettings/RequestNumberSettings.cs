using System;
using System.Xml.Serialization;
using AutoMapper;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    [Serializable]
    public sealed class RequestNumberSettings : IRequestNumberSettings, ICloneable, IEquatable<RequestNumberSettings>
    {
        [XmlAttribute("method")]
        public RequestNumberGenerationMethod Method { get; set; }

        [XmlAttribute("increment")]
        public long Increment { get; set; }

        public static RequestNumberSettings Create(IRequestNumberSettings contractObject)
        {
            if (null == contractObject)
                return null;

            var businessObject = contractObject as RequestNumberSettings;

            if (businessObject != null)
                return businessObject;

            return Mapper.Map<RequestNumberSettings>(contractObject);
        }

        public bool Equals(RequestNumberSettings other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Method == other.Method && Increment == other.Increment;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            var requestNumberSettings = obj as RequestNumberSettings;
            return requestNumberSettings != null && Equals(requestNumberSettings);
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