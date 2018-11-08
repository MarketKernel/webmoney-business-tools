using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.StronglyTypedWrappers;

namespace WMBusinessTools.Extensions
{
    public sealed class FindCertificateFormProvider : ITopFormProvider, IIdentifierFormProvider, ITransferFormProvider,
        IIncomingInvoiceFormProvider, IOutgoingInvoiceFormProvider, ITrustFormProvider
    {
        public bool CheckCompatibility(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public bool CheckCompatibility(IdentifierContext context)
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

        public bool CheckCompatibility(TrustContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public Form GetForm(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return GetForm(context, null, false);
        }

        public Form GetForm(IdentifierContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return GetForm(context, context.IdentifierValue, true);
        }

        public Form GetForm(TransferContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return GetForm(context, context.Transfer.PartnerIdentifier, true);
        }

        public Form GetForm(IncomingInvoiceContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return GetForm(context, context.Invoice.TargetIdentifier, true);
        }

        public Form GetForm(OutgoingInvoiceContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return GetForm(context, context.Invoice.ClientIdentifier, true);
        }

        public Form GetForm(TrustContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return GetForm(context, context.Trust.MasterIdentifier, true);
        }

        private Form GetForm(SessionContext context, long identifier, bool offline)
        {
            var formattingService = context.UnityContainer.Resolve<IFormattingService>();
            return GetForm(context, formattingService.FormatIdentifier(identifier), offline);
        }

        private Form GetForm(SessionContext context, string identifierValue, bool offline)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var incomeValuesWrapper = new FindCertificateFormValuesWrapper();

            if (!string.IsNullOrEmpty(identifierValue))
                incomeValuesWrapper.Control1Identifier = identifierValue;

            incomeValuesWrapper.Control5OfflineSearch = offline;

            var form = SubmitFormDisplayHelper.LoadSubmitFormByExtensionId(context.ExtensionManager,
                ExtensionCatalog.FindCertificate,
                incomeValuesWrapper.CollectIncomeValues());

            form.FinalAction = values =>
            {
                var certificate = (ICertificate) values["Certificate"];

                var certificateFormProvider =
                    context.ExtensionManager.TryGetCertificateFormProvider(ExtensionCatalog.Certificate);

                if (null == certificateFormProvider)
                    return true;

                var certificateForm = certificateFormProvider.GetForm(new CertificateContext(context, certificate));

                bool closed = false;

                certificateForm.FormClosed += (sender, args) =>
                {
                    if (closed)
                        return;

                    closed = true;

                    if (DialogResult.OK != certificateForm.DialogResult)
                        form.Close();
                };

                certificateForm.Show(form);

                return false;
            };

            form.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new FindCertificateFormValuesWrapper(list);
                var identifierService = context.UnityContainer.Resolve<IIdentifierService>();

                var certificate = identifierService.FindCertificate(long.Parse(valuesWrapper.Control1Identifier),
                    valuesWrapper.Control2GetBlAndTl, valuesWrapper.Control4CheckComplaintsAndComments,
                    valuesWrapper.Control3GetBlAndTlForAttachedWmids, !valuesWrapper.Control5OfflineSearch);

                return new Dictionary<string, object>
                {
                    {"Certificate", certificate}
                };
            };

            if (!string.IsNullOrEmpty(identifierValue))
                form.Load += (sender, args) =>
                {
                    form.Submit();
                };

            return form;
        }
    }
}
