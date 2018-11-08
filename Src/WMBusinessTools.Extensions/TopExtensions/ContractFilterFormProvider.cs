using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using LocalizationAssistant;
using Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.DisplayHelpers.Origins;
using WMBusinessTools.Extensions.StronglyTypedWrappers;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;

namespace WMBusinessTools.Extensions
{
    public sealed class ContractFilterFormProvider : ITopFormProvider
    {
        private FilterForm _form;

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
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var origin = new FilterOrigin(context.ExtensionManager,
                ExtensionCatalog.ContractFilter)
            {
                MenuItemsTagName = ExtensionCatalog.Tags.ContractExtension,
                CommandBarTagName = ExtensionCatalog.Tags.ContractFilterExtension,
                ColumnsSettings = context.Session.SettingsService.GetSettings().ContractSettings
            };

            var template = FilterDisplayHelper.LoadFilterFormTemplate(origin);

            template.Title = Translator.Instance.Translate(ExtensionCatalog.TransferFilter, "Contracts");

            // Отключаем кнопки.
            foreach (var buttonTemplate in template.FilterScreen.CommandButtons)
            {
                var formProvider =
                    context.ExtensionManager.TryGetTopFormProvider(buttonTemplate.Command);

                if (!(formProvider?.CheckCompatibility(context) ?? false))
                    buttonTemplate.Enabled = false;
            }

            _form = new FilterForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, _form);

            var now = DateTime.Now;

            var incomeValuesWrapper = new TransferFilterFormValuesWrapper
            {
                Control1FromTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Local),
                Control2ToTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, DateTimeKind.Local)
            };

            _form.ApplyTemplate(template);
            _form.ApplyShapeValues(incomeValuesWrapper.CollectIncomeValues());

            // Подписка на создание/изменение контракта
            EventBroker.ContractChanged += EventBrokerOnContractChanged;

            // Сохранение документа
            _form.SaveItemsCallback = (s, list) =>
            {
                var importExportService = context.UnityContainer.Resolve<IImportExportService>();
                importExportService.Save(list, s);
            };

            // Сохранение настроек
            _form.FormClosing += (sender, args) =>
            {
                var settings = context.Session.SettingsService.GetSettings();
                var contractSettings = settings.ContractSettings;

                FilterDisplayHelper.UpdateColumnsSettings(contractSettings, _form.SelectGridSettings());

                context.Session.SettingsService.SetSettings(settings);
                context.Session.SettingsService.Save();
            };

            // Меню
            _form.MenuItemResolver = (entity, command) =>
            {
                var contract = _form.CurrentEntity as IContract;

                if (null == contract)
                    return false;

                var contractContext = new ContractContext(context, contract);

                if (command.Equals(ExtensionCatalog.RefreshContract))
                {
                    var actionProvider = context.ExtensionManager.TryGetContractActionProvider(command);
                    return actionProvider?.CheckCompatibility(contractContext) ?? false;
                }

                var formProvider = context.ExtensionManager.TryGetContractFormProvider(command);
                return formProvider?.CheckCompatibility(contractContext) ?? false;
            };

            // Обработка событий.
            _form.ServiceCommand += (sender, args) =>
            {
                string command = args.Command;

                if (null == command)
                    throw new InvalidOperationException("null == command");

                if (command.StartsWith("CellContentClick:", StringComparison.Ordinal))
                    return;

                if (command.StartsWith("CellMouseDoubleClick:", StringComparison.Ordinal))
                    command = ExtensionCatalog.ContractDetails;

                var contract = args.Argument as IContract;

                // Кнопки
                if (null == contract)
                {
                    var formProvider = context.ExtensionManager.TryGetTopFormProvider(command);

                    formProvider?.GetForm(context).Show(_form);
                }
                else
                {
                    var contractContext = new ContractContext(context, contract);

                    if (command.Equals(ExtensionCatalog.RefreshContract))
                    {
                        var actionProvider = context.ExtensionManager.TryGetContractActionProvider(command);
                        actionProvider?.RunAction(contractContext);
                    }
                    else
                    {
                        var formProvider = context.ExtensionManager.TryGetContractFormProvider(command);
                        formProvider?.GetForm(contractContext).Show(_form);
                    }
                }
            };

            // Команда Refresh
            _form.WorkCallback = values =>
            {
                var valuesWrapper = new ContractFilterFormValuesWrapper(values);

                var contractService = context.UnityContainer.Resolve<IContractService>();

                var contracts =
                    contractService.SelectContracts(valuesWrapper.Control1FromTime, valuesWrapper.Control2ToTime);

                var gridRowContentList = new List<GridRowContent>();

                int signed = 0;
                int unsigned = 0;

                var contractSettings = context.Session.SettingsService.GetSettings().ContractSettings;

                foreach (var contract in contracts)
                {
                    var gridRowContent = new GridRowContent(contract.Id.ToString(), contract);

                    switch (contract.State)
                    {
                        case ContractState.Created:
                            gridRowContent.BackColor = Color.White;
                            gridRowContent.SelectionBackColor = contractSettings.SelectionColor;
                            unsigned++;
                            break;
                        case ContractState.Signed:
                        case ContractState.Completed:
                            gridRowContent.BackColor = contractSettings.SignedColor;
                            gridRowContent.SelectionBackColor =
                                ColorUtility.CalculateSelectionColor(contractSettings.SignedColor);
                            signed++;
                            break;
                        default:
                            throw new InvalidOperationException("contract.State == " + contract.State);
                    }

                    if (0 == contract.AccessCount)
                    {
                        gridRowContent.ForeColor = contractSettings.PublicForeColor;
                        gridRowContent.SelectionForeColor = contractSettings.PublicForeColor;
                    }

                    gridRowContentList.Add(gridRowContent);
                }

                var filterScreenContent = new FilterScreenContent();
                filterScreenContent.RowContentList.AddRange(gridRowContentList);

                var filterFormContent = new FilterFormContent(filterScreenContent);

                filterFormContent.LabelValues.Add(string.Format(CultureInfo.InvariantCulture, "SIG: {0}", signed));
                filterFormContent.LabelValues.Add(string.Format(CultureInfo.InvariantCulture, "UNSIG: {0}", unsigned));

                return filterFormContent;
            };

            _form.Disposed += (sender, args) =>
            {
                EventBroker.ContractChanged -= EventBrokerOnContractChanged;
            };

            return _form;
        }

        private void EventBrokerOnContractChanged(object o, EventArgs eventArgs)
        {
            if (null == _form)
                return;

            if (_form.IsDisposed)
                return;

            _form.ShowData();
        }
    }
}
