using System;
using System.Collections.Generic;
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
    public sealed class IncomingInvoiceFilterFormProvider : ITopFormProvider
    {
        public bool CheckCompatibility(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (!context.Session.IsMaster())
                return false;

            return true;
        }

        public Form GetForm(SessionContext context)
        {
            var origin = new FilterOrigin(context.ExtensionManager,
                ExtensionCatalog.IncomingInvoiceFilter)
            {
                MenuItemsTagName = ExtensionCatalog.Tags.IncomingInvoiceExtension,
                CommandBarTagName = ExtensionCatalog.Tags.IncomingInvoiceFilterExtension,
                ColumnsSettings = context.Session.SettingsService.GetSettings().IncomingInvoiceSettings
            };

            var template = FilterDisplayHelper.LoadFilterFormTemplate(origin);

            template.Title = Translator.Instance.Translate(ExtensionCatalog.TransferFilter, "Incoming invoices");

            // Отключаем кнопки.
            foreach (var buttonTemplate in template.FilterScreen.CommandButtons)
            {
                var formProvider =
                    context.ExtensionManager.TryGetTopFormProvider(buttonTemplate.Command);

                if (!(formProvider?.CheckCompatibility(context) ?? false))
                    buttonTemplate.Enabled = false;
            }

            var form = new FilterForm();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, form);

            var now = DateTime.Now;

            var incomeValuesWrapper = new IncomingInvoiceFilterFormValuesWrapper
            {
                Control1FromTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Local),
                Control2ToTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, DateTimeKind.Local)
            };

            form.ApplyTemplate(template);
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
                var incomingInvoiceSettings = settings.IncomingInvoiceSettings;

                FilterDisplayHelper.UpdateColumnsSettings(incomingInvoiceSettings, form.SelectGridSettings());

                context.Session.SettingsService.SetSettings(settings);
                context.Session.SettingsService.Save();
            };

            // Меню
            form.MenuItemResolver = (entity, command) =>
            {
                var invoice = form.CurrentEntity as IIncomingInvoice;

                if (null == invoice)
                    return false;

                var invoiceContext = new IncomingInvoiceContext(context, invoice);

                var formProvider = context.ExtensionManager.TryGetIncomingInvoiceFormProvider(command);
                return formProvider?.CheckCompatibility(invoiceContext) ?? false;
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

                var invoice = args.Argument as IIncomingInvoice;

                // Кнопки
                if (null == invoice)
                {
                    var formProvider = context.ExtensionManager.TryGetTopFormProvider(command);
                    formProvider?.GetForm(context).Show(form);
                }
                else
                {
                    // Системная
                    var formProvider = context.ExtensionManager.TryGetIncomingInvoiceFormProvider(command);
                    formProvider?.GetForm(new IncomingInvoiceContext(context, invoice))
                        .Show(form);
                }
            };

            // Команда Refresh
            form.WorkCallback = list =>
            {
                var valuesWrapper = new IncomingInvoiceFilterFormValuesWrapper(list);

                var invoiceService = context.UnityContainer.Resolve<IInvoiceService>();

                var invoices = invoiceService.SelectIncomingInvoices(valuesWrapper.Control1FromTime.ToUniversalTime(),
                    valuesWrapper.Control2ToTime.ToUniversalTime(), true);

                var gridRowContentList = new List<GridRowContent>();
                var operationSettings = context.Session.SettingsService.GetSettings().OperationSettings;

                decimal paid = 0;
                decimal notPaid = 0;
                decimal refused = 0;

                foreach (var invoice in invoices)
                {
                    var gridRowContent = new GridRowContent(invoice.PrimaryId.ToString(), invoice);

                    switch (invoice.State)
                    {
                        case InvoiceState.Paid:
                            Paid:
                            gridRowContent.ForeColor = operationSettings.OutcomeForeColor;
                            gridRowContent.SelectionForeColor = operationSettings.OutcomeForeColor;
                            break;
                        case InvoiceState.PaidWithProtection:
                            gridRowContent.BackColor = operationSettings.ProtectedColor;
                            gridRowContent.SelectionBackColor = ColorUtility.CalculateSelectionColor(operationSettings.ProtectedColor);
                            goto Paid;
                        case InvoiceState.Refusal:
                            gridRowContent.Strikeout = true;
                            break;
                    }

                    switch (invoice.State)
                    {
                        case InvoiceState.NotPaid:
                            notPaid += invoice.Amount;
                            break;
                        case InvoiceState.PaidWithProtection:
                        case InvoiceState.Paid:
                            paid += invoice.Amount;
                            break;
                        case InvoiceState.Refusal:
                            refused += invoice.Amount;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    gridRowContentList.Add(gridRowContent);
                }

                var chartPoint1 = new ChartPoint(
                    Translator.Instance.Translate(ExtensionCatalog.IncomingInvoiceFilter, "Paid"), (double) paid);
                var chartPoint2 = new ChartPoint(
                    Translator.Instance.Translate(ExtensionCatalog.IncomingInvoiceFilter, "Not paid"),
                    (double) notPaid);
                var chartPoint3 = new ChartPoint(
                    Translator.Instance.Translate(ExtensionCatalog.IncomingInvoiceFilter, "Refused"), (double) refused);

                var filterScreenContent = new FilterScreenContent();
                filterScreenContent.RowContentList.AddRange(gridRowContentList);

                if (paid > 0)
                    filterScreenContent.ChartPoints.Add(chartPoint1);

                if (notPaid > 0)
                    filterScreenContent.ChartPoints.Add(chartPoint2);

                if (refused > 0)
                    filterScreenContent.ChartPoints.Add(chartPoint3);

                var formattingService = context.UnityContainer.Resolve<IFormattingService>();

                var filterFormContent = new FilterFormContent(filterScreenContent);
                filterFormContent.LabelValues.Add(
                    $"{Translator.Instance.Translate(ExtensionCatalog.TransferFilter, "PAID")}: {formattingService.FormatAmount(paid)}");
                filterFormContent.LabelValues.Add(
                    $"{Translator.Instance.Translate(ExtensionCatalog.TransferFilter, "UNP")}: {formattingService.FormatAmount(notPaid)}");
                filterFormContent.LabelValues.Add(
                    $"{Translator.Instance.Translate(ExtensionCatalog.TransferFilter, "REF")}: {formattingService.FormatAmount(refused)}");

                return filterFormContent;
            };

            return form;
        }
    }
}
