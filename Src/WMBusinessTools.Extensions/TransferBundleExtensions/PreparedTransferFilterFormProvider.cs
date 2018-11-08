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
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;

namespace WMBusinessTools.Extensions
{
    public sealed class PreparedTransferFilterFormProvider : ITransferBundleFormProvider
    {
        class Subscriber : ISubscriber
        {
            private readonly TransferBundleContext _context;
            private readonly FilterForm _filterForm;

            public Subscriber(TransferBundleContext context, FilterForm filterForm)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _filterForm = filterForm ?? throw new ArgumentNullException(nameof(filterForm));
            }

            public bool IsDisposed { get; private set; }

            public void Notify(INotification notification)
            {
                if (null == notification)
                    throw new ArgumentNullException(nameof(notification));

                Notify((IPreparedTransferNotification) notification);
            }

            private void Notify(IPreparedTransferNotification notification)
            {
                if (IsDisposed)
                    return;

                var preparedTransferNotification = notification;

                var transfer = preparedTransferNotification.PreparedTransfer;
                var transferSettings = _context.Session.SettingsService.GetSettings().PreparedTransferSettings;

                var gridRowContent = BuildGridRowContent(transfer, transferSettings);

                _filterForm.UpdateRow(gridRowContent);
            }

            public void Dispose()
            {
                IsDisposed = true;
            }
        }

        public bool CheckCompatibility(TransferBundleContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public Form GetForm(TransferBundleContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var origin = new FilterOrigin(context.ExtensionManager,
                ExtensionCatalog.PreparedTransferFilter)
            {
                MenuItemsTagName = ExtensionCatalog.Tags.PreparedTransferExtension,
                CommandBarTagName = ExtensionCatalog.Tags.PreparedTransferFilterExtension,
                ColumnsSettings = context.Session.SettingsService.GetSettings().PreparedTransferSettings
            };

            var template = FilterDisplayHelper.LoadFilterFormTemplate(origin);

            template.Title = string.Format(CultureInfo.InvariantCulture, "{0} <{1}>",
                Translator.Instance.Translate(ExtensionCatalog.TransferFilter, "History by bundle"),
                context.TransferBundle.Name);

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

            form.Load += (sender, args) =>
            {
                form.ShowData();
            };

            form.ApplyTemplate(template);

            var eventBroker = context.UnityContainer.Resolve<IEventBroker>();

            var subscriber = new Subscriber(context, form);

            eventBroker.Subscribe(subscriber, nameof(IPreparedTransferNotification));

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
                var transferSettings = settings.PreparedTransferSettings;

                FilterDisplayHelper.UpdateColumnsSettings(transferSettings, form.SelectGridSettings());

                context.Session.SettingsService.SetSettings(settings);
                context.Session.SettingsService.Save();
            };

            // Меню
            form.MenuItemResolver = (entity, command) =>
            {
                var transfer = form.CurrentEntity as IPreparedTransfer;

                if (null == transfer)
                    return false;

                var transferContext = new PreparedTransferContext(context, transfer);

                var formProvider = context.ExtensionManager.TryGetPreparedTransferFormProvider(command);
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

                var transfer = args.Argument as IPreparedTransfer;

                // Кнопки
                if (null == transfer)
                {
                    var formProvider = context.ExtensionManager.TryGetTransferBundleFormProvider(command);
                    formProvider?.GetForm(context).Show(form);
                }
                else
                {
                    var transferContext = new PreparedTransferContext(context, transfer);

                    var formProvider = context.ExtensionManager.TryGetPreparedTransferFormProvider(command);
                    formProvider?.GetForm(transferContext).Show(form);
                }
            };

            // Команда Refresh
            form.WorkCallback = values =>
            {
                var bundleService = context.UnityContainer.Resolve<ITransferBundleService>();

                var transfers = bundleService.ObtainBundle(context.TransferBundle.Id, true).Transfers;

                var gridRowContentList = new List<GridRowContent>();

                decimal completedAmount = 0;
                decimal uncompletedAmount = 0;

                var transferSettings = context.Session.SettingsService.GetSettings().PreparedTransferSettings;

                foreach (var transfer in transfers)
                {
                    var gridRowContent = BuildGridRowContent(transfer, transferSettings);
                    gridRowContentList.Add(gridRowContent);

                    switch (transfer.State)
                    {
                        case PreparedTransferState.Failed:
                        case PreparedTransferState.Registered:
                        case PreparedTransferState.Pended:
                        case PreparedTransferState.Processed:
                        case PreparedTransferState.Interrupted:
                            uncompletedAmount += transfer.Amount;
                            break;
                        case PreparedTransferState.Completed:
                            completedAmount += transfer.Amount;
                            break;
                        default:
                            throw new InvalidOperationException("transfer.State == " + transfer.State);
                    }
                }

                var filterScreenContent = new FilterScreenContent();
                filterScreenContent.RowContentList.AddRange(gridRowContentList);

                var formattingService = context.UnityContainer.Resolve<IFormattingService>();

                var filterFormContent = new FilterFormContent(filterScreenContent);
                filterFormContent.LabelValues.Add(formattingService.FormatAmount(completedAmount));
                filterFormContent.LabelValues.Add(formattingService.FormatAmount(uncompletedAmount));

                return filterFormContent;
            };

            form.Disposed += (sender, args) =>
            {
                subscriber.Dispose();
            };

            return form;
        }

        private static GridRowContent BuildGridRowContent(IPreparedTransfer transfer, IPreparedTransferSettings transferSettings)
        {
            var gridRowContent = new GridRowContent(transfer.Id.ToString(), transfer);

            switch (transfer.State)
            {
                case PreparedTransferState.Failed:
                    gridRowContent.BackColor = transferSettings.FailedColor;
                    gridRowContent.SelectionBackColor =
                        ColorUtility.CalculateSelectionColor(transferSettings.FailedColor);
                    break;
                case PreparedTransferState.Registered:
                    gridRowContent.BackColor = Color.White;
                    gridRowContent.SelectionBackColor = transferSettings.SelectionColor;
                    break;
                case PreparedTransferState.Pended:
                    gridRowContent.BackColor = transferSettings.PendedColor;
                    gridRowContent.SelectionBackColor =
                        ColorUtility.CalculateSelectionColor(transferSettings.PendedColor);
                    break;
                case PreparedTransferState.Processed:
                    gridRowContent.BackColor = transferSettings.ProcessedColor;
                    gridRowContent.SelectionBackColor =
                        ColorUtility.CalculateSelectionColor(transferSettings.ProcessedColor);
                    break;
                case PreparedTransferState.Interrupted:
                    gridRowContent.BackColor = transferSettings.InterruptedColor;
                    gridRowContent.SelectionBackColor =
                        ColorUtility.CalculateSelectionColor(transferSettings.InterruptedColor);
                    break;
                case PreparedTransferState.Completed:
                    gridRowContent.BackColor = transferSettings.CompletedColor;
                    gridRowContent.SelectionBackColor =
                        ColorUtility.CalculateSelectionColor(transferSettings.CompletedColor);
                    break;
            }

            return gridRowContent;
        }
    }
}