using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.StronglyTypedWrappers;

namespace WMBusinessTools.Extensions
{
    public sealed class SendMessageFormProvider : ITopFormProvider, ICertificateFormProvider, ITransferFormProvider, IIncomingInvoiceFormProvider, IOutgoingInvoiceFormProvider
    {
        public bool CheckCompatibility(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public bool CheckCompatibility(CertificateContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public bool CheckCompatibility(TransferContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public bool CheckCompatibility(IncomingInvoiceContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public bool CheckCompatibility(OutgoingInvoiceContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

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

            return GetForm(context, context.Certificate.Identifier);
        }

        public Form GetForm(TransferContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return GetForm(context, context.Transfer.PartnerIdentifier);
        }

        public Form GetForm(IncomingInvoiceContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return GetForm(context, context.Invoice.TargetIdentifier);
        }

        public Form GetForm(OutgoingInvoiceContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return GetForm(context, context.Invoice.ClientIdentifier);
        }

        public Form GetForm(SessionContext context, long? identifier)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var incomeValuesWrapper = new SendMessageFormValuesWrapper();

            if (null != identifier)
            {
                var formattingService = context.UnityContainer.Resolve<IFormattingService>();
                incomeValuesWrapper.Control1Identifier = formattingService.FormatIdentifier(identifier.Value);
            }

            var form = SubmitFormDisplayHelper.LoadSubmitFormByExtensionId(context.ExtensionManager, ExtensionCatalog.SendMessage,
                incomeValuesWrapper.CollectIncomeValues());

            form.ServiceCommand += (sender, args) =>
            {
                if (!SendMessageFormValuesWrapper.Control1IdentifierCommandFindPassport
                    .Equals(args.Command))
                    return;

                IdentifierDisplayHelper.ShowFindCertificateForm(form, context, (string)args.Argument);
            };

            form.WorkCallback = (step, list) =>
            {
                if (0 != step)
                    throw new InvalidOperationException("0 != step");

                var valuesWrapper = new SendMessageFormValuesWrapper(list);

                var messageService = context.UnityContainer.Resolve<IMessageService>();
                messageService.SendMessage(long.Parse(valuesWrapper.Control1Identifier), valuesWrapper.Control2Subject,
                    valuesWrapper.Control3Message, false);

                return new Dictionary<string, object>();
            };

            return form;
        }
    }
}
