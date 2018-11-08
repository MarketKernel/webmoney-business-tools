using System;
using System.ComponentModel;
using System.Xml.Serialization;
using AutoMapper;
using WebMoney.Services.BusinessObjects.Annotations;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Utils;

namespace WebMoney.Services.BusinessObjects
{
    [XmlRoot("settings")]
    public sealed class Settings : ISettings, ICloneable, IEquatable<Settings>
    {
        private RequestSettings _requestSettings;
        private TransferSettings _transferSettings;
        private IOperationSettings _operationSettings;
        private IncomingInvoiceSettings _incomingInvoiceSettings;
        private OutgoingInvoiceSettings _outgoingInvoiceSettings;
        private ContractSettings _contractSettings;
        private TransferBundleSettings _transferBundleSettings;
        private PreparedTransferSettings _preparedTransferSettings;

        [LocalizedCategory("Payment"), LocalizedDisplayName("Current transfer ID")]
        [XmlAttribute("transferId")]
        public int TransferId { get; set; }

        [LocalizedCategory("Payment"), LocalizedDisplayName("Current order ID")]
        [XmlAttribute("orderId")]
        public int OrderId { get; set; }

        [LocalizedCategory("Request"), LocalizedDisplayName("Request settings")]
        [XmlElement("requestSettings")]
        public IRequestSettings RequestSettings
        {
            get => _requestSettings;
            set
            {
                if (!(value is RequestSettings))
                    throw new InvalidOperationException("!(value is RequestSettings)");

                _requestSettings = BusinessObjects.RequestSettings.Create(value);
            }
        }

        [TypeConverter(typeof(LocalizedBooleanConverter))]
        [LocalizedDisplayName("Allow unauthorized extensions")]
        [XmlAttribute("allowUnauthorizedExtensions")]
        public bool AllowUnauthorizedExtensions { get; set; }

        [LocalizedDisplayName("Язык интерфейса")]
        [XmlAttribute("language")]
        [TypeConverter(typeof(LocalizedEnumConverter))]
        public Language Language { get; set; }

        [LocalizedCategory("Appearance"), LocalizedDisplayName("Transfer display settings")]
        [XmlElement("transferSettings")]
        public ITransferSettings TransferSettings
        {
            get => _transferSettings;
            set => _transferSettings = BusinessObjects.TransferSettings.Create(value);
        }

        [LocalizedCategory("Appearance"), LocalizedDisplayName("Operation display settings")]
        [XmlElement("operationSettings")]
        public IOperationSettings OperationSettings
        {
            get => _operationSettings;
            set => _operationSettings = BusinessObjects.OperationSettings.Create(value);
        }

        [LocalizedCategory("Appearance"), LocalizedDisplayName("Incoming invoice display settings")]
        [XmlElement("incomingInvoiceSettings")]
        public IIncomingInvoiceSettings IncomingInvoiceSettings
        {
            get => _incomingInvoiceSettings;
            set => _incomingInvoiceSettings = BusinessObjects.IncomingInvoiceSettings.Create(value);
        }

        [LocalizedCategory("Appearance"), LocalizedDisplayName("Outgoing invoice display settings")]
        [XmlElement("outgoingInvoiceSettings")]
        public IOutgoingInvoiceSettings OutgoingInvoiceSettings
        {
            get => _outgoingInvoiceSettings;
            set => _outgoingInvoiceSettings = BusinessObjects.OutgoingInvoiceSettings.Create(value);
        }

        [LocalizedCategory("Appearance"), LocalizedDisplayName("Contract display settings")]
        [XmlElement("contractSettings")]
        public IContractSettings ContractSettings
        {
            get => _contractSettings;
            set => _contractSettings = BusinessObjects.ContractSettings.Create(value);
        }

        [LocalizedCategory("Appearance"), LocalizedDisplayName("Transfer bundle display settings")]
        [XmlElement("transferBundleSettings")]
        public ITransferBundleSettings TransferBundleSettings
        {
            get => _transferBundleSettings;
            set => _transferBundleSettings = BusinessObjects.TransferBundleSettings.Create(value);
        }

        [LocalizedCategory("Appearance"), LocalizedDisplayName("Prepared transfer display settings")]
        [XmlElement("preparedTransferSettings")]
        public IPreparedTransferSettings PreparedTransferSettings
        {
            get => _preparedTransferSettings;
            set => _preparedTransferSettings = BusinessObjects.PreparedTransferSettings.Create(value);
        }

        public Settings()
        {
            _transferSettings = new TransferSettings();
            _operationSettings = new OperationSettings();
            _incomingInvoiceSettings = new IncomingInvoiceSettings();
            _outgoingInvoiceSettings = new OutgoingInvoiceSettings();
            _contractSettings = new ContractSettings();
            _transferBundleSettings = new TransferBundleSettings();
            _preparedTransferSettings = new PreparedTransferSettings();
            _requestSettings = new RequestSettings();
        }

        public static Settings Create(ISettings contractObject)
        {
            if (null == contractObject)
                return null;

            if (contractObject is Settings businessObject)
                return businessObject;

            return Mapper.Map<Settings>(contractObject);
        }

        public bool Equals(Settings other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return Equals(_requestSettings, other._requestSettings) &&
                   Equals(_transferSettings, other._transferSettings) &&
                   Equals(_operationSettings, other._operationSettings) &&
                   Equals(_incomingInvoiceSettings, other._incomingInvoiceSettings) &&
                   Equals(_outgoingInvoiceSettings, other._outgoingInvoiceSettings) &&
                   Equals(_contractSettings, other._contractSettings) &&
                   Equals(_transferBundleSettings, other._transferBundleSettings) &&
                   Equals(_preparedTransferSettings, other._preparedTransferSettings) &&
                   TransferId == other.TransferId && OrderId == other.OrderId &&
                   AllowUnauthorizedExtensions == other.AllowUnauthorizedExtensions && Language == other.Language;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Settings settings && Equals(settings);
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