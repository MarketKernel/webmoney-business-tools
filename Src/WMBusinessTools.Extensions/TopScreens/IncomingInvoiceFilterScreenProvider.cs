using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LocalizationAssistant;
using Microsoft.Practices.Unity;
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
    public sealed class IncomingInvoiceFilterScreenProvider : ITopScreenProvider
    {
        public bool CheckCompatibility(ScreenContainerContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return context.Session.IsMaster();
        }

        public Control GetScreen(ScreenContainerContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var origin = new FilterOrigin(context.ExtensionManager,
                ExtensionCatalog.IncomingInvoiceFilterScreen)
            {
                MenuItemsTagName = ExtensionCatalog.Tags.IncomingInvoiceExtension,
                CommandBarTagName = ExtensionCatalog.Tags.IncomingInvoiceFilterExtension,
                ColumnsSettings = context.Session.SettingsService.GetSettings().IncomingInvoiceSettings
            };

            var template = FilterDisplayHelper.LoadFilterScreenTemplate(origin);

            // Отключаем кнопки.
            foreach (var buttonTemplate in template.CommandButtons)
            {
                var formProvider =
                    context.ExtensionManager.TryGetTopFormProvider(buttonTemplate.Command);

                if (!(formProvider?.CheckCompatibility(context) ?? false))
                    buttonTemplate.Enabled = false;
            }

            var screen = new FilterScreen();

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, screen);

            var now = DateTime.Now;

            var incomeValuesWrapper = new IncomingInvoiceFilterFormValuesWrapper
            {
                Control1FromTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Local),
                Control2ToTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, DateTimeKind.Local)
            };

            screen.ApplyTemplate(template);
            screen.ApplyShapeValues(incomeValuesWrapper.CollectIncomeValues());

            // Меню
            screen.MenuItemResolver = (entity, command) =>
            {
                var invoice = screen.CurrentEntity as IIncomingInvoice;

                if (null == invoice)
                    return false;

                var invoiceContext = new IncomingInvoiceContext(context, invoice);

                var formProvider = context.ExtensionManager.TryGetIncomingInvoiceFormProvider(command);
                return formProvider?.CheckCompatibility(invoiceContext) ?? false;
            };

            // Обработка событий.
            screen.ServiceCommand += (sender, args) =>
            {
                string command = args.Command;

                if (null == command)
                    throw new InvalidOperationException("null == command");

                if (command.StartsWith("CellContentClick:", StringComparison.Ordinal))
                    return;

                if (FilterScreen.BeginUpdateServiceCommand.Equals(args.Command))
                {
                    context.ScreenContainer.OnStartProgress();
                    return;
                }

                if (FilterScreen.EndUpdateServiceCommand.Equals(args.Command))
                {
                    context.ScreenContainer.OnStopProgress();
                    return;
                }

                if (FilterScreen.DisplayContentServiceCommand.Equals(args.Command))
                    return;

                if (command.StartsWith("CellMouseDoubleClick:", StringComparison.Ordinal))
                    command = ExtensionCatalog.Details;

                var form = screen.FindForm();

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
            screen.WorkCallback = list =>
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
                    Translator.Instance.Translate(ExtensionCatalog.IncomingInvoiceFilterScreen, "Paid"), (double)paid);
                var chartPoint2 = new ChartPoint(
                    Translator.Instance.Translate(ExtensionCatalog.IncomingInvoiceFilterScreen, "Not paid"),
                    (double)notPaid);
                var chartPoint3 = new ChartPoint(
                    Translator.Instance.Translate(ExtensionCatalog.IncomingInvoiceFilterScreen, "Refused"), (double)refused);

                var filterScreenContent = new FilterScreenContent();

                filterScreenContent.RowContentList.AddRange(gridRowContentList);

                if (paid > 0)
                    filterScreenContent.ChartPoints.Add(chartPoint1);

                if (notPaid > 0)
                    filterScreenContent.ChartPoints.Add(chartPoint2);

                if (refused > 0)
                    filterScreenContent.ChartPoints.Add(chartPoint3);

                return filterScreenContent;
            };

            return screen;
        }
    }
}