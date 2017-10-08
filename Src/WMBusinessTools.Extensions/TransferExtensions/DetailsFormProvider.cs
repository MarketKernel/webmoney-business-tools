using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using LocalizationAssistant;
using WMBusinessTools.Extensions.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using Xml2WinForms;
using Xml2WinForms.Utils;

namespace WMBusinessTools.Extensions
{
    public sealed class DetailsFormProvider : ITransferFormProvider, IIncomingInvoiceFormProvider, IOutgoingInvoiceFormProvider, IPreparedTransferFormProvider, IContractFormProvider
    {
        private SessionContext _context;

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

        public bool CheckCompatibility(PreparedTransferContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public bool CheckCompatibility(ContractContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public Form GetForm(TransferContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            return GetForm(context.Transfer);
        }

        public Form GetForm(IncomingInvoiceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            return GetForm(context.Invoice);
        }

        public Form GetForm(OutgoingInvoiceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            return GetForm(context.Invoice);
        }

        public Form GetForm(PreparedTransferContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            return GetForm(context.PreparedTransfer);
        }

        public Form GetForm(ContractContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            return GetForm(context.Contract);
        }

        private Form GetForm(object entity)
        {
            var items = new List<ListItemContent>();

            foreach (var propertyInfo in entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var value = propertyInfo.GetValue(entity, null);

                if (!(value is IFormattable) && !(value is string))
                    continue;

                var name = Translator.Instance.Translate("Details", propertyInfo.Name);
                var valueText = DisplayContentHelper.GetText(propertyInfo, value);

                items.Add(new ListItemContent(new ResultRecord(name, valueText ?? string.Empty)));
            }

            var form = SubmitFormDisplayHelper.LoadSubmitFormByExtensionId(_context.ExtensionManager, ExtensionCatalog.Details,
                new Dictionary<string, object>
                {
                    {"Details", items}
                });

            form.ServiceCommand += (sender, args) =>
            {
                if (!"Copy".Equals(args.Command))
                    return;

                Clipboard.SetText(((ResultRecord) args.Argument).Value, TextDataFormat.UnicodeText);
            };

            return form;
        }
    }
}
