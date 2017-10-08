using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WMBusinessTools.Extensions.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.StronglyTypedWrappers;
using Xml2WinForms;

namespace WMBusinessTools.Extensions
{
    public sealed class ContractDetailsFormProvider : IContractFormProvider
    {
        public bool CheckCompatibility(ContractContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public Form GetForm(ContractContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var signatures = context.Contract.Signatures.Select(s =>
                {
                    var formattingService = context.UnityContainer.Resolve<IFormattingService>();

                    var identifierValue = formattingService.FormatIdentifier(s.AcceptorIdentifier);
                    var acceptTimeValue = null == s.AcceptTime
                        ? string.Empty
                        : formattingService.FormatDateTime(s.AcceptTime.Value);
                    return new ListItemContent(new ResultRecord(identifierValue, acceptTimeValue))
                    {
                        ImageKey = "Ant"
                    };
                })
                .ToList();

            var valuesWrapper = new ContractDetailsFormValuesWrapper
            {
                Control1Name = context.Contract.Name,
                Control2Text = context.Contract.Text,
                Control3HasLimitedAccess = !context.Contract.IsPublic,
                Control4Details = signatures
            };

            var form = SubmitFormDisplayHelper.LoadSubmitFormByExtensionId(context.ExtensionManager,
                ExtensionCatalog.ContractDetails, valuesWrapper.CollectIncomeValues());

            form.ServiceCommand += (sender, args) =>
            {
                if (!"Copy".Equals(args.Command))
                    return;

                var resultRecord = args.Argument as ResultRecord;

                if (null != resultRecord)
                {
                    string text = string.Format(CultureInfo.InvariantCulture, "{0} {1}", resultRecord.Name,
                        resultRecord.Value);
                    Clipboard.SetText(text, TextDataFormat.UnicodeText);
                }
            };

            return form;
        }
    }
}
