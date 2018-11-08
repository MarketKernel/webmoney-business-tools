using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.Properties;
using WMBusinessTools.Extensions.StronglyTypedWrappers;
using WMBusinessTools.Extensions.Utils;

namespace WMBusinessTools.Extensions
{
    public sealed class CreateContractFormProvider : ITopFormProvider
    {
        public bool CheckCompatibility(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (AuthenticationMethod.KeeperClassic != context.Session.AuthenticationService.AuthenticationMethod)
                return false;

            if (!context.Session.IsMaster())
                return false;

            return true;
        }

        public Form GetForm(SessionContext context)
        {
            var submitForm = SubmitFormDisplayHelper.LoadSubmitFormByExtensionId(context.ExtensionManager,
                ExtensionCatalog.CreateContract);

            submitForm.VerificationCallback = (step, list) =>
            {
                var valuesWrapper = new CreateContractFormValuesWrapper(list);

                try
                {
                    if (valuesWrapper.Control3LimitedAccess)
                        ParseIdentifiers(valuesWrapper.Control4AccessList);
                }
                catch (FormatException)
                {
                    MessageBox.Show(submitForm, Resources.CreateContractFormProvider_GetForm_List_of_identifiers_is_in_the_wrong_format, Resources.CreateContractFormProvider_GetForm_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                return true;
            };

            submitForm.WorkCallback = (step, list) =>
            {
                var valuesWrapper = new CreateContractFormValuesWrapper(list);

                List<long> identifiers = new List<long>();

                if (valuesWrapper.Control3LimitedAccess)
                    identifiers = ParseIdentifiers(valuesWrapper.Control4AccessList);

                var contractService = context.UnityContainer.Resolve<IContractService>();

                contractService.CreateContract(valuesWrapper.Control1Name, valuesWrapper.Control2Text, identifiers);

                return new Dictionary<string, object>();
            };

            return submitForm;
        }

        private static List<long> ParseIdentifiers(string value)
        {
            var identifiers = new List<long>();

            foreach (var identifierValue in value.Split(
                new[] {Environment.NewLine, ",", ";", " "}, StringSplitOptions.RemoveEmptyEntries))
            {
                var trimmedValue = identifierValue.Trim();

                if (trimmedValue.Length != 12)
                    throw new FormatException();

                long identifier;

                if (!long.TryParse(trimmedValue, out identifier))
                    throw new FormatException();

                identifiers.Add(identifier);
            }

            return identifiers;
        }
    }
}