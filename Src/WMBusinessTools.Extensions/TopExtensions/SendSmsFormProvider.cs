using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.DisplayHelpers.Origins;
using WMBusinessTools.Extensions.StronglyTypedWrappers;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;

namespace WMBusinessTools.Extensions
{
    public sealed class SendSmsFormProvider : ITopFormProvider, ICertificateFormProvider
    {
        public bool CheckCompatibility(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (AuthenticationMethod.KeeperClassic != context.Session.AuthenticationService.AuthenticationMethod)
                return false;

            return true;
        }

        public bool CheckCompatibility(CertificateContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var certificate = context.Certificate;

            if (string.IsNullOrEmpty(certificate.ContactPhone) && string.IsNullOrEmpty(certificate.CellPhone))
                return false;

            if (AuthenticationMethod.KeeperClassic != context.Session.AuthenticationService.AuthenticationMethod)
                return false;

            return true;
        }

        public Form GetForm(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return GetForm(context, null);
        }

        public Form GetForm(CertificateContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            string phone = context.Certificate.ContactPhone;

            if (string.IsNullOrEmpty(phone))
                phone = context.Certificate.CellPhone;

            if (string.IsNullOrEmpty(phone))
                throw new InvalidOperationException("string.IsNullOrEmpty(phone)");

            return GetForm(context, phone);
        }

        private Form GetForm(SessionContext context, string phone)
        {
            var template =
                TemplateLoader.LoadSubmitFormTemplate(context.ExtensionManager, ExtensionCatalog.SendSms);

            var templateWrapper = new SendSmsFormTemplateWrapper(template);

            if (null != phone)
                templateWrapper.Control1PhoneNumber.DefaultValue = phone;

            var origin = new AccountDropDownListOrigin(context.UnityContainer);
            origin.FilterCriteria.HasMoney = true;
            origin.FilterCriteria.CurrencyCapabilities = CurrencyCapabilities.Transfer;

            var form = new SubmitForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, form);

            form.ApplyTemplate(template);

            form.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new SendSmsFormValuesWrapper(list);

                var smsService = context.UnityContainer.Resolve<ISmsService>();

                smsService.SendSms(null, valuesWrapper.Control1PhoneNumber,
                    valuesWrapper.Control2Message, valuesWrapper.Control3UseTransliteration);

                return new Dictionary<string, object>();
            };

            return form;
        }
    }
}