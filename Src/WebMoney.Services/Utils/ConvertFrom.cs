using System;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.XmlInterfaces.BasicObjects;

namespace WebMoney.Services.Utils
{
    internal static class ConvertFrom
    {
        #region ConfirmationType

        public static Contracts.BasicTypes.ConfirmationType ApiTypeToContractType(
            XmlInterfaces.BasicObjects.ConfirmationType apiType)
        {
            switch (apiType)
            {
                case XmlInterfaces.BasicObjects.ConfirmationType.SMS:
                    return Contracts.BasicTypes.ConfirmationType.Sms;
                case XmlInterfaces.BasicObjects.ConfirmationType.USSD:
                    return Contracts.BasicTypes.ConfirmationType.Ussd;
                case XmlInterfaces.BasicObjects.ConfirmationType.Auto:
                    return Contracts.BasicTypes.ConfirmationType.Auto;
                case XmlInterfaces.BasicObjects.ConfirmationType.Invoice:
                    return Contracts.BasicTypes.ConfirmationType.Invoice;
                case XmlInterfaces.BasicObjects.ConfirmationType.SmsWithoutInvoice:
                    return Contracts.BasicTypes.ConfirmationType.SmsWithoutInvoice;
                default:
                    throw new ArgumentOutOfRangeException(nameof(apiType));
            }
        }

        public static XmlInterfaces.BasicObjects.ConfirmationType ContractTypeToApiType(
            Contracts.BasicTypes.ConfirmationType contractType)
        {
            switch (contractType)
            {
                case Contracts.BasicTypes.ConfirmationType.Sms:
                    return XmlInterfaces.BasicObjects.ConfirmationType.SMS;
                case Contracts.BasicTypes.ConfirmationType.Ussd:
                    return XmlInterfaces.BasicObjects.ConfirmationType.USSD;
                case Contracts.BasicTypes.ConfirmationType.Auto:
                    return XmlInterfaces.BasicObjects.ConfirmationType.Auto;
                case Contracts.BasicTypes.ConfirmationType.Invoice:
                    return XmlInterfaces.BasicObjects.ConfirmationType.Invoice;
                case Contracts.BasicTypes.ConfirmationType.SmsWithoutInvoice:
                    return XmlInterfaces.BasicObjects.ConfirmationType.SmsWithoutInvoice;
                default:
                    throw new ArgumentOutOfRangeException(nameof(contractType));
            }
        }

        #endregion

        #region  TransferType

        public static Contracts.BasicTypes.TransferType ApiTypeToContractType(
            XmlInterfaces.BasicObjects.TransferType apiType)
        {
            switch (apiType)
            {
                case XmlInterfaces.BasicObjects.TransferType.Normal:
                    return Contracts.BasicTypes.TransferType.Regular;
                case XmlInterfaces.BasicObjects.TransferType.Protection:
                    return Contracts.BasicTypes.TransferType.Protected;
                case XmlInterfaces.BasicObjects.TransferType.ProtectionCancel:
                    return Contracts.BasicTypes.TransferType.Canceled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(apiType));
            }
        }

        public static XmlInterfaces.BasicObjects.TransferType ContractTypeToApiType(
            Contracts.BasicTypes.TransferType contractType)
        {
            switch (contractType)
            {
                case Contracts.BasicTypes.TransferType.Regular:
                    return XmlInterfaces.BasicObjects.TransferType.Normal;
                case Contracts.BasicTypes.TransferType.Protected:
                    return XmlInterfaces.BasicObjects.TransferType.Protection;
                case Contracts.BasicTypes.TransferType.Canceled:
                    return XmlInterfaces.BasicObjects.TransferType.ProtectionCancel;
                default:
                    throw new ArgumentOutOfRangeException(nameof(contractType));
            }
        }

        #endregion

        #region  PaymentNumberKind

        public static Contracts.BasicTypes.PaymentNumberKind ApiTypeToContractType(
            XmlInterfaces.BasicObjects.PaymentNumberKind apiType)
        {
            switch (apiType)
            {
                case XmlInterfaces.BasicObjects.PaymentNumberKind.Unknown:
                    return Contracts.BasicTypes.PaymentNumberKind.Auto;
                case XmlInterfaces.BasicObjects.PaymentNumberKind.OrderId:
                    return Contracts.BasicTypes.PaymentNumberKind.OrderId;
                case XmlInterfaces.BasicObjects.PaymentNumberKind.InvoicePrimaryId:
                    return Contracts.BasicTypes.PaymentNumberKind.InvoicePrimaryId;
                case XmlInterfaces.BasicObjects.PaymentNumberKind.TransferPrimaryId:
                    return Contracts.BasicTypes.PaymentNumberKind.TransferPrimaryId;
                default:
                    throw new ArgumentOutOfRangeException(nameof(apiType));
            }
        }

        public static XmlInterfaces.BasicObjects.PaymentNumberKind ContractTypeToApiType(
            Contracts.BasicTypes.PaymentNumberKind contractType)
        {
            switch (contractType)
            {
                case Contracts.BasicTypes.PaymentNumberKind.Auto:
                    return XmlInterfaces.BasicObjects.PaymentNumberKind.Unknown;
                case Contracts.BasicTypes.PaymentNumberKind.OrderId:
                    return XmlInterfaces.BasicObjects.PaymentNumberKind.OrderId;
                case Contracts.BasicTypes.PaymentNumberKind.InvoicePrimaryId:
                    return XmlInterfaces.BasicObjects.PaymentNumberKind.InvoicePrimaryId;
                case Contracts.BasicTypes.PaymentNumberKind.TransferPrimaryId:
                    return XmlInterfaces.BasicObjects.PaymentNumberKind.TransferPrimaryId;
                default:
                    throw new ArgumentOutOfRangeException(nameof(contractType));
            }
        }

        #endregion

        #region  CertificateDegree

        public static CertificateDegree ApiTypeToContractType(
            PassportDegree apiType)
        {
            switch (apiType)
            {
                case PassportDegree.Alias:
                    return CertificateDegree.Alias;
                case PassportDegree.Formal:
                    return CertificateDegree.Formal;
                case PassportDegree.Initial:
                    return CertificateDegree.Initial;
                case PassportDegree.Personal:
                    return CertificateDegree.Personal;
                case PassportDegree.Merchant:
                    return CertificateDegree.Merchant;
                case PassportDegree.Capitaller:
                    return CertificateDegree.Capitaller;
                case PassportDegree.CapitallerLegalEntity:
                    return CertificateDegree.CapitallerLegalEntity;
                case PassportDegree.Developer:
                    return CertificateDegree.Developer;
                case PassportDegree.Cashier:
                    return CertificateDegree.Cashier;
                case PassportDegree.Registrar:
                    return CertificateDegree.Registrar;
                case PassportDegree.Guarantor:
                    return CertificateDegree.Guarantor;
                case PassportDegree.Service1:
                case PassportDegree.Service2:
                    return CertificateDegree.Service;
                case PassportDegree.Operator:
                    return CertificateDegree.Operator;
                default:
                    throw new ArgumentOutOfRangeException(nameof(apiType), apiType, null);
            }
        }

        public static PassportDegree ContractTypeToApiType(
            CertificateDegree contractType)
        {
            switch (contractType)
            {
                case CertificateDegree.Alias:
                    return PassportDegree.Alias;
                case CertificateDegree.Formal:
                    return PassportDegree.Formal;
                case CertificateDegree.Initial:
                    return PassportDegree.Initial;
                case CertificateDegree.Personal:
                    return PassportDegree.Personal;
                case CertificateDegree.Merchant:
                    return PassportDegree.Merchant;
                case CertificateDegree.Capitaller:
                    return PassportDegree.Capitaller;
                case CertificateDegree.CapitallerLegalEntity:
                    return PassportDegree.CapitallerLegalEntity;
                case CertificateDegree.Cashier:
                    return PassportDegree.Cashier;
                case CertificateDegree.Developer:
                    return PassportDegree.Developer;
                case CertificateDegree.Registrar:
                    return PassportDegree.Registrar;
                case CertificateDegree.Guarantor:
                    return PassportDegree.Guarantor;
                case CertificateDegree.Service:
                    return PassportDegree.Service1;
                case CertificateDegree.Operator:
                    return PassportDegree.Operator;
                default:
                    throw new ArgumentOutOfRangeException(nameof(contractType), contractType, null);
            }
        }

        #endregion

        #region  CertificateStatus

        public static CertificateStatus ApiTypeToContractType(PassportStatus apiType)
        {
            switch (apiType)
            {
                case PassportStatus.PrivatePerson:
                    return CertificateStatus.PrivatePerson;
                case PassportStatus.Entity:
                    return CertificateStatus.Entity;
                default:
                    throw new ArgumentOutOfRangeException(nameof(apiType), apiType, null);
            }
        }

        public static PassportStatus ContractTypeToApiType(CertificateStatus contractType)
        {
            switch (contractType)
            {
                case CertificateStatus.PrivatePerson:
                    return PassportStatus.PrivatePerson;
                case CertificateStatus.Entity:
                    return PassportStatus.Entity;
                default:
                    throw new ArgumentOutOfRangeException(nameof(contractType), contractType, null);
            }
        }

        #endregion

        #region  CertificateAppointment

        public static CertificateAppointment ApiTypeToContractType(PassportAppointment apiType)
        {
            switch (apiType)
            {
                case PassportAppointment.PrivatePerson:
                    return CertificateAppointment.PrivatePerson;
                case PassportAppointment.Director:
                    return CertificateAppointment.Director;
                case PassportAppointment.Accountant:
                    return CertificateAppointment.Accountant;
                case PassportAppointment.Representative:
                    return CertificateAppointment.Representative;
                case PassportAppointment.PrivateEntrepreneur:
                    return CertificateAppointment.PrivateEntrepreneur;
                default:
                    throw new ArgumentOutOfRangeException(nameof(apiType), apiType, null);
            }
        }

        public static PassportAppointment ContractTypeToApiType(CertificateAppointment contractType)
        {
            switch (contractType)
            {
                case CertificateAppointment.PrivatePerson:
                    return PassportAppointment.PrivatePerson;
                case CertificateAppointment.Director:
                    return PassportAppointment.Director;
                case CertificateAppointment.Accountant:
                    return PassportAppointment.Accountant;
                case CertificateAppointment.Representative:
                    return PassportAppointment.Representative;
                case CertificateAppointment.PrivateEntrepreneur:
                    return PassportAppointment.PrivateEntrepreneur;
                default:
                    throw new ArgumentOutOfRangeException(nameof(contractType), contractType, null);
            }
        }

        #endregion

        #region  TransferType

        public static Contracts.BasicTypes.InvoiceState ApiTypeToContractType(
            XmlInterfaces.BasicObjects.InvoiceState apiType)
        {
            switch (apiType)
            {
                case XmlInterfaces.BasicObjects.InvoiceState.NotPaid:
                    return Contracts.BasicTypes.InvoiceState.NotPaid;
                case XmlInterfaces.BasicObjects.InvoiceState.PaidProtection:
                    return Contracts.BasicTypes.InvoiceState.PaidWithProtection;
                case XmlInterfaces.BasicObjects.InvoiceState.Paid:
                    return Contracts.BasicTypes.InvoiceState.Paid;
                case XmlInterfaces.BasicObjects.InvoiceState.Refusal:
                    return Contracts.BasicTypes.InvoiceState.Refusal;
                default:
                    throw new ArgumentOutOfRangeException(nameof(apiType));
            }
        }

        public static XmlInterfaces.BasicObjects.InvoiceState ContractTypeToApiType(
            Contracts.BasicTypes.InvoiceState contractType)
        {
            switch (contractType)
            {
                case Contracts.BasicTypes.InvoiceState.NotPaid:
                    return XmlInterfaces.BasicObjects.InvoiceState.NotPaid;
                case Contracts.BasicTypes.InvoiceState.PaidWithProtection:
                    return XmlInterfaces.BasicObjects.InvoiceState.PaidProtection;
                case Contracts.BasicTypes.InvoiceState.Paid:
                    return XmlInterfaces.BasicObjects.InvoiceState.Paid;
                case Contracts.BasicTypes.InvoiceState.Refusal:
                    return XmlInterfaces.BasicObjects.InvoiceState.Refusal;
                default:
                    throw new ArgumentOutOfRangeException(nameof(contractType));
            }
        }

        #endregion

        #region TelepatMethod

        public static Contracts.BasicTypes.TelepatMethod ApiTypeToContractType(
            XmlInterfaces.BasicObjects.TelepatMethod apiType)
        {
            switch (apiType)
            {
                case XmlInterfaces.BasicObjects.TelepatMethod.KeeperMobile:
                    return Contracts.BasicTypes.TelepatMethod.KeeperMobile;
                case XmlInterfaces.BasicObjects.TelepatMethod.SmsX20:
                    return Contracts.BasicTypes.TelepatMethod.SmsX20;
                case XmlInterfaces.BasicObjects.TelepatMethod.MerchantWebMoneyWithSms:
                    return Contracts.BasicTypes.TelepatMethod.MerchantWebMoneyWithSms;
                default:
                    throw new ArgumentOutOfRangeException(nameof(apiType));
            }
        }

        public static XmlInterfaces.BasicObjects.TelepatMethod ContractTypeToApiType(
            Contracts.BasicTypes.TelepatMethod contractType)
        {
            switch (contractType)
            {
                case Contracts.BasicTypes.TelepatMethod.KeeperMobile:
                    return XmlInterfaces.BasicObjects.TelepatMethod.KeeperMobile;
                case Contracts.BasicTypes.TelepatMethod.SmsX20:
                    return XmlInterfaces.BasicObjects.TelepatMethod.SmsX20;
                case Contracts.BasicTypes.TelepatMethod.MerchantWebMoneyWithSms:
                    return XmlInterfaces.BasicObjects.TelepatMethod.MerchantWebMoneyWithSms;
                default:
                    throw new ArgumentOutOfRangeException(nameof(contractType));
            }
        }

        #endregion

        #region PaymerType

        public static Contracts.BasicTypes.PaymerType ApiTypeToContractType(
            XmlInterfaces.BasicObjects.PaymerType apiType)
        {
            switch (apiType)
            {
                case XmlInterfaces.BasicObjects.PaymerType.Paymer:
                    return Contracts.BasicTypes.PaymerType.Paymer;
                case XmlInterfaces.BasicObjects.PaymerType.Note:
                    return Contracts.BasicTypes.PaymerType.Note;
                case XmlInterfaces.BasicObjects.PaymerType.Check:
                    return Contracts.BasicTypes.PaymerType.Check;
                default:
                    throw new ArgumentOutOfRangeException(nameof(apiType));
            }
        }

        public static XmlInterfaces.BasicObjects.PaymerType ContractTypeToApiType(
            Contracts.BasicTypes.PaymerType contractType)
        {
            switch (contractType)
            {
                case Contracts.BasicTypes.PaymerType.Paymer:
                    return XmlInterfaces.BasicObjects.PaymerType.Paymer;
                case Contracts.BasicTypes.PaymerType.Note:
                    return XmlInterfaces.BasicObjects.PaymerType.Note;
                case Contracts.BasicTypes.PaymerType.Check:
                    return XmlInterfaces.BasicObjects.PaymerType.Check;
                default:
                    throw new ArgumentOutOfRangeException(nameof(contractType));
            }
        }

        #endregion

        #region SmsState

        public static Contracts.BasicTypes.SmsState ApiTypeToContractType(
            XmlInterfaces.BasicObjects.SmsState apiType)
        {
            switch (apiType)
            {
                case XmlInterfaces.BasicObjects.SmsState.BUFFERED:
                    return Contracts.BasicTypes.SmsState.Buffered;
                case XmlInterfaces.BasicObjects.SmsState.SENDING:
                    return Contracts.BasicTypes.SmsState.Sending;
                case XmlInterfaces.BasicObjects.SmsState.SENDED:
                    return Contracts.BasicTypes.SmsState.Sended;
                case XmlInterfaces.BasicObjects.SmsState.DELIVERED:
                    return Contracts.BasicTypes.SmsState.Delivered;
                case XmlInterfaces.BasicObjects.SmsState.NON_DELIVERED:
                    return Contracts.BasicTypes.SmsState.NonDelivered;
                case XmlInterfaces.BasicObjects.SmsState.SUSPENDED:
                    return Contracts.BasicTypes.SmsState.Suspended;
                case XmlInterfaces.BasicObjects.SmsState.HLRPENDING:
                    return Contracts.BasicTypes.SmsState.HlrPending;
                case XmlInterfaces.BasicObjects.SmsState.HLRMISMATCH:
                    return Contracts.BasicTypes.SmsState.HlrMismatch;
                default:
                    throw new ArgumentOutOfRangeException(nameof(apiType));
            }
        }

        public static XmlInterfaces.BasicObjects.SmsState ContractTypeToApiType(
            Contracts.BasicTypes.SmsState contractType)
        {
            switch (contractType)
            {
                case Contracts.BasicTypes.SmsState.Buffered:
                    return XmlInterfaces.BasicObjects.SmsState.BUFFERED;
                case Contracts.BasicTypes.SmsState.Sending:
                    return XmlInterfaces.BasicObjects.SmsState.SENDING;
                case Contracts.BasicTypes.SmsState.Sended:
                    return XmlInterfaces.BasicObjects.SmsState.SENDED;
                case Contracts.BasicTypes.SmsState.Delivered:
                    return XmlInterfaces.BasicObjects.SmsState.DELIVERED;
                case Contracts.BasicTypes.SmsState.NonDelivered:
                    return XmlInterfaces.BasicObjects.SmsState.NON_DELIVERED;
                case Contracts.BasicTypes.SmsState.Suspended:
                    return XmlInterfaces.BasicObjects.SmsState.SUSPENDED;
                case Contracts.BasicTypes.SmsState.HlrPending:
                    return XmlInterfaces.BasicObjects.SmsState.HLRPENDING;
                case Contracts.BasicTypes.SmsState.HlrMismatch:
                    return XmlInterfaces.BasicObjects.SmsState.HLRMISMATCH;
                default:
                    throw new ArgumentOutOfRangeException(nameof(contractType));
            }
        }

        #endregion
    }
}
