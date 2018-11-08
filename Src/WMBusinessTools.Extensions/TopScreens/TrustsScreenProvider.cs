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
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;

namespace WMBusinessTools.Extensions
{
    public sealed class TrustsScreenProvider : ITopScreenProvider
    {
        private ScreenContainerContext _context;
        private ListScreen _screen;

        public bool CheckCompatibility(ScreenContainerContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return context.Session.IsMaster();
        }

        public Control GetScreen(ScreenContainerContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            var origin =
                new AccountListScreenOrigin(context, ExtensionCatalog.TrustsScreen)
                {
                    MenuItemsTagName = ExtensionCatalog.Tags.TrustExtension,
                    CommandBarTagName = ExtensionCatalog.Tags.TrustsScreenExtension
                };

            var template = AccountDisplayHelper.LoadListScreenTemplate(origin);

            _screen = new ListScreen
            {
                Dock = DockStyle.Fill
            };

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, _screen);

            EventBroker.TrustChanged += EventBrokerOnTrustChanged;

            _screen.Disposed += (sender, args) =>
            {
                EventBroker.TrustChanged -= EventBrokerOnTrustChanged;
            };

            _screen.ApplyTemplate(template);

            // Команда Refresh
            _screen.RefreshCallback += () => SelectTrusts(true);

            _screen.MenuItemResolver = (entity, command) =>
            {
                var trust = _screen.CurrentEntity as ITrust;

                if (null == trust)
                    return false;

                var trustContext = new TrustContext(context, trust);

                if (command.Equals(ExtensionCatalog.CopyPurseNumber))
                    return true;

                var formProvider = context.ExtensionManager.TryGetTrustFormProvider(command);
                return formProvider?.CheckCompatibility(trustContext) ?? false;
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
                        var trust = args.Argument as ITrust;

                        // Кнопки
                        if (null == trust)
                        {
                            var formProvider =
                                context.ExtensionManager.TryGetTopFormProvider(command);
                            formProvider?.GetForm(context).Show(context.ScreenContainer);
                        }
                        else
                        {
                            if (command.Equals(TunableList.CellMouseDoubleClickCommandName))
                                command = ExtensionCatalog.UpdateTrust;

                            var trustContext = new TrustContext(context, trust);

                            // Системная
                            if (command.Equals(ExtensionCatalog.CopyPurseNumber))
                            {
                                var actionProvider = context.ExtensionManager.TryGetTrustActionProvider(command);
                                actionProvider?.RunAction(trustContext);
                            }
                            else
                            {
                                var formProvider = context.ExtensionManager.TryGetTrustFormProvider(command);
                                formProvider?.GetForm(trustContext).Show(context.ScreenContainer);
                            }
                        }
                    }
                        break;
                }
            };

            _screen.DisplayContent(SelectTrusts(false));

            return _screen;
        }

        private void EventBrokerOnTrustChanged(object o, DataChangedEventArgs eventArgs)
        {
            _screen.DisplayContent(SelectTrusts(eventArgs.FreshDataRequired));
        }

        private List<ListItemContent> SelectTrusts(bool fresh)
        {
            var currencyService = _context.UnityContainer.Resolve<ICurrencyService>();
            var trusts = _context.UnityContainer.Resolve<ITrustService>().SelectTrusts(fresh);

            var listViewItems = new List<ListItemContent>();

            foreach (var trust in trusts)
            {
                var currency = currencyService.ObtainCurrencyByAccountNumber(trust.Purse);

                var listItemContent =
                    new ListItemContent(trust)
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
