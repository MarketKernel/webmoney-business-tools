using LocalizationAssistant;
using System;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.DisplayHelpers.Origins;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;
using System.Collections.Generic;
using System.Drawing;
using WebMoney.Services.Contracts.BasicTypes;
using WMBusinessTools.Extensions.StronglyTypedWrappers;

namespace WMBusinessTools.Extensions
{
    public sealed class TransferBundleFilterFormProvider : ITopFormProvider
    {
        class Subscriber : ISubscriber
        {
            private readonly SessionContext _context;
            private readonly FilterForm _filterForm;

            public Subscriber(SessionContext context, FilterForm filterForm)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _filterForm = filterForm ?? throw new ArgumentNullException(nameof(filterForm));
            }

            public bool IsDisposed { get; private set; }

            public void Notify(INotification notification)
            {
                if (null == notification)
                    throw new ArgumentNullException(nameof(notification));

                Notify((ITransferBundleNotification)notification);
            }

            private void Notify(ITransferBundleNotification notification)
            {
                if (IsDisposed)
                    return;

                var transferBundleNotification = notification;
                var transferBundleSettings = _context.Session.SettingsService.GetSettings().TransferBundleSettings;
                var gridRowContent = BuildGridRowContent(transferBundleNotification.TransferBundle, transferBundleSettings);

                _filterForm.UpdateRow(gridRowContent);
            }

            public void Dispose()
            {
                IsDisposed = true;
            }
        }

        private FilterForm _form;

        public bool CheckCompatibility(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public Form GetForm(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var origin = new FilterOrigin(context.ExtensionManager,
                ExtensionCatalog.TransferBundleFilter)
            {
                MenuItemsTagName = ExtensionCatalog.Tags.TransferBundleExtension,
                CommandBarTagName = ExtensionCatalog.Tags.TransferBundleFilterExtension,
                ColumnsSettings = context.Session.SettingsService.GetSettings().TransferBundleSettings
            };

            var template = FilterDisplayHelper.LoadFilterFormTemplate(origin);

            template.Title = Translator.Instance.Translate(ExtensionCatalog.TransferFilter, "Transfer bundles");

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
            _form.ApplyTemplate(template);

            var now = DateTime.Now;

            var incomeValuesWrapper = new TransferBundleFilterFormValuesWrapper
            {
                Control1FromTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Local),
                Control2ToTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, DateTimeKind.Local)
            };

            _form.ApplyShapeValues(incomeValuesWrapper.CollectIncomeValues());

            EventBroker.TransferBundleCreated += EventBrokerOnTransferBundleCreated;

            var eventBroker = context.UnityContainer.Resolve<IEventBroker>();

            var subscriber = new Subscriber(context, _form);

            eventBroker.Subscribe(subscriber, nameof(ITransferBundleNotification));

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
                var bundleSettings = settings.TransferBundleSettings;

                FilterDisplayHelper.UpdateColumnsSettings(bundleSettings, _form.SelectGridSettings());

                context.Session.SettingsService.SetSettings(settings);
                context.Session.SettingsService.Save();
            };

            // Меню
            _form.MenuItemResolver = (entity, command) =>
            {
                var transferBundle = _form.CurrentEntity as ITransferBundle;

                if (null == transferBundle)
                    return false;

                var transferContext = new TransferBundleContext(context, transferBundle);

                if (command.Equals(ExtensionCatalog.StartTransferBundle) ||
                    command.Equals(ExtensionCatalog.StopTransferBundle))
                {
                    var actionProvider = context.ExtensionManager.TryGetTransferBundleActionProvider(command);
                    return actionProvider?.CheckCompatibility(transferContext) ?? false;
                }

                var formProvider = context.ExtensionManager.TryGetTransferBundleFormProvider(command);
                return formProvider?.CheckCompatibility(transferContext) ?? false;
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
                    command = ExtensionCatalog.PreparedTransferFilter;

                var transferBundle = args.Argument as ITransferBundle;

                // Кнопки
                if (null == transferBundle)
                {
                    var formProvider = context.ExtensionManager.TryGetTopFormProvider(command);
                    formProvider?.GetForm(context).Show(_form);
                }
                else
                {
                    var transferBundleContext = new TransferBundleContext(context, transferBundle);

                    if (command.Equals(ExtensionCatalog.StartTransferBundle) ||
                        command.Equals(ExtensionCatalog.StopTransferBundle))
                    {
                        var actionProvider = context.ExtensionManager.TryGetTransferBundleActionProvider(command);
                        actionProvider?.RunAction(transferBundleContext);

                        return;
                    }

                    var formProvider = context.ExtensionManager.TryGetTransferBundleFormProvider(command);
                    formProvider?.GetForm(transferBundleContext).Show(_form);
                }
            };

            // Команда Refresh
            _form.WorkCallback = values =>
            {
                var valuesWrapper = new TransferBundleFilterFormValuesWrapper(values);

                var transferBundleService = context.UnityContainer.Resolve<ITransferBundleService>();
                var bundles = transferBundleService.SelectBundles(valuesWrapper.Control1FromTime.ToUniversalTime(),
                    valuesWrapper.Control2ToTime.ToUniversalTime());

                var gridRowContentList = new List<GridRowContent>();

                var bundleSettings = context.Session.SettingsService.GetSettings().TransferBundleSettings;

                foreach (var bundle in bundles)
                {
                    var gridRowContent = BuildGridRowContent(bundle, bundleSettings);
                    gridRowContentList.Add(gridRowContent);
                }

                var filterScreenContent = new FilterScreenContent();
                filterScreenContent.RowContentList.AddRange(gridRowContentList);
                var filterFormContent = new FilterFormContent(filterScreenContent);

                return filterFormContent;
            };

            _form.Disposed += (sender, args) =>
            {
                EventBroker.TransferBundleCreated -= EventBrokerOnTransferBundleCreated;
                subscriber.Dispose();
            };

            return _form;
        }

        private void EventBrokerOnTransferBundleCreated(object o, EventArgs eventArgs)
        {
            if (null == _form)
                return;

            if (_form.IsDisposed)
                return;

            _form.ShowData();
        }

        private static GridRowContent BuildGridRowContent(ITransferBundle bundle, ITransferBundleSettings bundleSettings)
        {
            var gridRowContent = new GridRowContent(bundle.Id.ToString(), bundle);

            switch (bundle.State)
            {
                case TransferBundleState.Registered:
                    gridRowContent.BackColor = Color.White;
                    gridRowContent.SelectionBackColor = bundleSettings.SelectionColor;
                    break;
                case TransferBundleState.Pended:
                    gridRowContent.BackColor = bundleSettings.PendedColor;
                    gridRowContent.SelectionBackColor = ColorUtility.CalculateSelectionColor(bundleSettings.PendedColor);
                    break;
                case TransferBundleState.Processed:
                    gridRowContent.BackColor = bundleSettings.ProcessedColor;
                    gridRowContent.SelectionBackColor = ColorUtility.CalculateSelectionColor(bundleSettings.ProcessedColor);
                    break;
                case TransferBundleState.Completed:
                    gridRowContent.BackColor = bundleSettings.CompletedColor;
                    gridRowContent.SelectionBackColor = ColorUtility.CalculateSelectionColor(bundleSettings.CompletedColor);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return gridRowContent;
        }
    }
}
