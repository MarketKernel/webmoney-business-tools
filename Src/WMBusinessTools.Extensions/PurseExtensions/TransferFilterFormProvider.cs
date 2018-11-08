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
    public sealed class TransferFilterFormProvider : IPurseFormProvider
    {
        private static readonly Color ChartFontColor = Color.FromArgb(4, 100, 150);

        public bool CheckCompatibility(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public Form GetForm(PurseContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var origin = new FilterOrigin(context.ExtensionManager,
                ExtensionCatalog.TransferFilter)
            {
                MenuItemsTagName = ExtensionCatalog.Tags.TransferExtension,
                CommandBarTagName = ExtensionCatalog.Tags.TransferFilterExtension,
                ColumnsSettings = context.Session.SettingsService.GetSettings().TransferSettings
            };

            var template = FilterDisplayHelper.LoadFilterFormTemplate(origin);

            template.Title = string.Format(CultureInfo.InvariantCulture, "{0} {1}",
                Translator.Instance.Translate(ExtensionCatalog.TransferFilter, "History by purse"),
                context.Account.Number);

            // Отключаем кнопки.
            foreach (var buttonTemplate in template.FilterScreen.CommandButtons)
            {
                var formProvider =
                    context.ExtensionManager.TryGetPurseFormProvider(buttonTemplate.Command);

                if (!(formProvider?.CheckCompatibility(context) ?? false))
                    buttonTemplate.Enabled = false;
            }

            var form = new FilterForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, form);

            form.ApplyTemplate(template);

            var now = DateTime.Now;

            var incomeValuesWrapper = new TransferFilterFormValuesWrapper
            {
                Control1FromTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Local),
                Control2ToTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, DateTimeKind.Local)
            };

            form.ApplyShapeValues(incomeValuesWrapper.CollectIncomeValues());

            // Сохранение документа
            form.SaveItemsCallback = (s, list) =>
            {
                var importExportService = context.UnityContainer.Resolve<IImportExportService>();
                importExportService.Save(list, s);
            };

            // Сохранение настроек
            form.FormClosing += (sender, args) =>
            {
                var settings = context.Session.SettingsService.GetSettings();
                var transferSettings = settings.TransferSettings;

                FilterDisplayHelper.UpdateColumnsSettings(transferSettings, form.SelectGridSettings());

                context.Session.SettingsService.SetSettings(settings);
                context.Session.SettingsService.Save();
            };

            // Меню
            form.MenuItemResolver = (entity, command) =>
            {
                var transfer = form.CurrentEntity as ITransfer;

                if (null == transfer)
                    return false;

                var transferContext = new TransferContext(context, transfer);

                var formProvider = context.ExtensionManager.TryGetTransferFormProvider(command);
                return formProvider?.CheckCompatibility(transferContext) ?? false;
            };

            // Обработка событий.
            form.ServiceCommand += (sender, args) =>
            {
                string command = args.Command;

                if (null == command)
                    throw new InvalidOperationException("null == command");

                if (command.StartsWith("CellContentClick:", StringComparison.Ordinal))
                    return;

                if (command.StartsWith("CellMouseDoubleClick:", StringComparison.Ordinal))
                    command = ExtensionCatalog.Details;

                var transfer = args.Argument as ITransfer;

                // Кнопки
                if (null == transfer)
                {
                    var formProvider = context.ExtensionManager.TryGetPurseFormProvider(command);
                    formProvider?.GetForm(context).Show(form);
                }
                else
                {
                    // Системная
                    var formProvider = context.ExtensionManager.TryGetTransferFormProvider(command);
                    formProvider?.GetForm(new TransferContext(context, transfer))
                        .Show(form);
                }
            };

            // Команда Refresh
            form.WorkCallback = values =>
            {
                var valuesWrapper = new TransferFilterFormValuesWrapper(values);


                var tansferService = context.UnityContainer.Resolve<ITransferService>();

                var transfers = tansferService.SelectTransfers(context.Account.Number,
                    valuesWrapper.Control1FromTime.ToUniversalTime(), valuesWrapper.Control2ToTime.ToUniversalTime(),
                    !valuesWrapper.Control3OfflineSearch);

                var gridRowContentList = new List<GridRowContent>();

                decimal income = 0;
                decimal outcome = 0;

                var operationSettings = context.Session.SettingsService.GetSettings().OperationSettings;

                foreach (var transfer in transfers)
                {
                    var gridRowContent = new GridRowContent(transfer.PrimaryId.ToString(), transfer);

                    switch (transfer.Type)
                    {
                        case TransferType.Regular:
                            gridRowContent.SelectionBackColor = operationSettings.SelectionColor;
                            break;
                        case TransferType.Protected:
                            Protected:
                            gridRowContent.BackColor = operationSettings.ProtectedColor;
                            gridRowContent.SelectionBackColor = ColorUtility.CalculateSelectionColor(operationSettings.ProtectedColor);
                            break;
                        case TransferType.Canceled:
                            gridRowContent.Strikeout = true;
                            goto Protected;
                    }

                    var foreColor = transfer.SourcePurse.Equals(context.Account.Number)
                        ? operationSettings.OutcomeForeColor
                        : operationSettings.IncomeForeColor;

                    gridRowContent.ForeColor = foreColor;
                    gridRowContent.SelectionForeColor = foreColor;

                    if (TransferType.Canceled != transfer.Type
                        && TransferType.Protected != transfer.Type)
                    {
                        if (transfer.SourcePurse.Equals(context.Account.Number))
                            outcome += transfer.Amount;
                        else
                            income += transfer.Amount;
                    }

                    gridRowContentList.Add(gridRowContent);
                }

                var chartPoint1 = new ChartPoint(
                    Translator.Instance.Translate(ExtensionCatalog.TransferFilter, "Income"), (double) income)
                {
                    Color = operationSettings.IncomeChartColor,
                    FontColor = ChartFontColor
                };
                var chartPoint2 = new ChartPoint(
                    Translator.Instance.Translate(ExtensionCatalog.TransferFilter, "Outcome"), (double) outcome)
                {
                    Color = operationSettings.OutcomeChartColor,
                    FontColor = ChartFontColor
                };

                var filterScreenContent = new FilterScreenContent();
                filterScreenContent.RowContentList.AddRange(gridRowContentList);

                if (income > 0)
                    filterScreenContent.ChartPoints.Add(chartPoint1);

                if (outcome > 0)
                    filterScreenContent.ChartPoints.Add(chartPoint2);

                var formattingService = context.UnityContainer.Resolve<IFormattingService>();

                var filterFormContent = new FilterFormContent(filterScreenContent);
                filterFormContent.LabelValues.Add(
                    $"{Translator.Instance.Translate(ExtensionCatalog.TransferFilter, "IN")}: {formattingService.FormatAmount(income)}");
                filterFormContent.LabelValues.Add(
                    $"{Translator.Instance.Translate(ExtensionCatalog.TransferFilter, "OUT")}: {formattingService.FormatAmount(outcome)}");

                return filterFormContent;
            };

            return form;
        }
    }
}
