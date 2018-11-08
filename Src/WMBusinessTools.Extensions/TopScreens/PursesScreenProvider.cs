using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.DisplayHelpers.Origins;
using Xml2WinForms;

namespace WMBusinessTools.Extensions
{
    public sealed class PursesScreenProvider : ITopScreenProvider
    {
        private ScreenContainerContext _context;
        private ListScreen _screen;

        public bool CheckCompatibility(ScreenContainerContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public Control GetScreen(ScreenContainerContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            var origin =
                new AccountListScreenOrigin(context, ExtensionCatalog.PursesScreen)
                {
                    MenuItemsTagName = ExtensionCatalog.Tags.PurseExtension,
                    CommandBarTagName = ExtensionCatalog.Tags.PursesScreenExtension
                };

            var template = AccountDisplayHelper.LoadListScreenTemplate(origin);

            _screen = new ListScreen
            {
                Dock = DockStyle.Fill
            };

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, _screen);

            EventBroker.PurseChanged += OnPurseChanged;

            _screen.Disposed += (sender, args) =>
            {
                EventBroker.PurseChanged -= OnPurseChanged;
            };

            _screen.ApplyTemplate(template);

            // Команда Refresh
            _screen.RefreshCallback += () => SelectAccounts(true);

            _screen.MenuItemResolver = (entity, command) =>
            {
                var account = _screen.CurrentEntity as IAccount;

                if (null == account)
                    return false;

                var purseContext = new PurseContext(context, account);

                if (command.Equals(ExtensionCatalog.CopyPurseNumber) ||
                    command.Equals(ExtensionCatalog.ClearMerchantKey) || command.Equals(ExtensionCatalog.RemovePurse))
                {
                    var actionProvider = context.ExtensionManager.TryGetPurseActionProvider(command);
                    return actionProvider?.CheckCompatibility(purseContext) ?? false;
                }

                var formProvider = context.ExtensionManager.TryGetPurseFormProvider(command);
                return formProvider?.CheckCompatibility(purseContext) ?? false;
            };

            // Обработка событий.
            _screen.ServiceCommand += (sender, args) =>
            {
                string command = args.Command;

                if (null == command)
                    throw new InvalidOperationException("null == command");

                switch (command)
                {
                    case "BeginRefresh":
                        context.ScreenContainer.OnStartProgress();
                        break;
                    case "EndRefresh":
                        context.ScreenContainer.OnStopProgress();
                        break;
                    default:
                    {
                        var account = args.Argument as IAccount;

                        // Кнопки
                        if (null == account)
                        {
                            var formProvider = context.ExtensionManager.TryGetTopFormProvider(command);
                            formProvider?.GetForm(context).Show(context.ScreenContainer);
                        }
                        else
                        {
                            if (command.Equals(TunableList.CellMouseDoubleClickCommandName))
                                command = ExtensionCatalog.TransferFilter;

                            var purseContext = new PurseContext(context, account);

                                // Системная
                                if (command.Equals(ExtensionCatalog.CopyPurseNumber)
                                || command.Equals(ExtensionCatalog.ClearMerchantKey)
                                || command.Equals(ExtensionCatalog.RemovePurse))
                            {
                                var actionProvider = context.ExtensionManager.TryGetPurseActionProvider(command);
                                actionProvider?.RunAction(purseContext);
                            }
                            else
                            {
                                var formProvider = context.ExtensionManager.TryGetPurseFormProvider(command);
                                formProvider?.GetForm(purseContext)
                                    .Show(context.ScreenContainer);
                            }
                        }
                    }
                        break;
                }
            };

            _screen.DisplayContent(SelectAccounts(false));

            return _screen;
        }

        private void OnPurseChanged(object sender, DataChangedEventArgs eventArgs)
        {
            if (eventArgs.FreshDataRequired)
                _screen.RefreshContent();
            else
                _screen.DisplayContent(SelectAccounts(eventArgs.FreshDataRequired));
        }

        private List<ListItemContent> SelectAccounts(bool fresh)
        {
            var currencyService = _context.UnityContainer.Resolve<ICurrencyService>();
            var accounts = _context.UnityContainer.Resolve<IPurseService>().SelectAccounts(fresh);

            var listViewItems = new List<ListItemContent>();

            foreach (var account in accounts)
            {
                var currency = currencyService.ObtainCurrencyByAccountNumber(account.Number);

                var listItemContent =
                    new ListItemContent(account)
                    {
                        ImageKey = currency,
                        Group = AccountDisplayHelper.BuildGroupKey(currencyService, currency)
                    };

                listViewItems.Add(listItemContent);
            }

            return listViewItems;
        }
    }
}
