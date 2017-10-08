using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using LocalizationAssistant;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.DisplayHelpers.Origins;
using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;
using WMBusinessTools.Extensions.BusinessObjects;

namespace WMBusinessTools.Extensions.DisplayHelpers
{
    internal static class AccountDisplayHelper
    {
        private const string IconFolder = "../Pictures/Purses";
        private const string IconFileExtension = ".gif";

        public static ListScreenTemplate LoadListScreenTemplate(AccountListScreenOrigin origin)
        {
            if (null == origin)
                throw new ArgumentNullException(nameof(origin));

            var template = ListScreenDisplayHelper.LoadListScreenTemplate(origin);

            // Группы и иконки.
            template.TunableList.Groups.Clear();
            template.TunableList.Icons.Clear();

            var currencyService = origin.Context.UnityContainer.Resolve<ICurrencyService>();

            foreach (var currency in currencyService.SelectCurrencies(CurrencyCapabilities.None))
            {
                // Group
                string groupHeaderText = string.Format(CultureInfo.InvariantCulture, "{0}{1}", currency,
                    Translator.Instance.Translate(ExtensionCatalog.PursesScreen, "-purses"));

                var groupTemplate = new ListGroupTemplate(currency, groupHeaderText, true);

                // Icon
                var currencyWithPrefix = currencyService.AddPrefix(currency);

                var iconFileName = Path.ChangeExtension(currencyWithPrefix, IconFileExtension);

                if (null == iconFileName)
                    throw new InvalidOperationException("null == iconFileName");

                string iconPath = Path.Combine(IconFolder, iconFileName);

                var iconTemplate = new ListIconTemplate(currency) {IconPath = iconPath};

                groupTemplate.SetTemplateInternals(template.TemplateName, template.BaseDirectory);
                iconTemplate.SetTemplateInternals(template.TemplateName, template.BaseDirectory);

                template.TunableList.Groups.Add(groupTemplate);
                template.TunableList.Icons.Add(iconTemplate);
            }

            // Отключаем кнопки.
            foreach (var templateCommandButton in template.CommandButtons)
            {
                var formProvider =
                    origin.Context.ExtensionManager.TryGetTopFormProvider(templateCommandButton.Command);

                templateCommandButton.Enabled = formProvider?.CheckCompatibility(origin.Context) ?? false;
            }

            return template;
        }

        public static List<AccountDropDownListItemTemplate> BuildAccountDropDownListItemTemplates(
            AccountDropDownListOrigin origin)
        {
            if (null == origin)
                throw new ArgumentNullException(nameof(origin));

            var purseService = origin.Container.Resolve<IPurseService>();
            var currencyService = origin.Container.Resolve<ICurrencyService>();
            var transferService = origin.Container.Resolve<ITransferService>();

            List<IAccount> accounts;

            switch (origin.Source)
            {
                case AccountSource.CurrentIdentifier:
                    accounts = purseService.SelectAccounts().ToList();
                    break;
                case AccountSource.MasterIdentifier:
                    accounts = purseService.SelectAccounts(false, true).ToList();
                    break;
                case AccountSource.Trusts:
                {
                    var fastAccounts = new List<Account>();

                    var trustService = origin.Container.Resolve<ITrustService>();
                    var trusts = trustService.SelectTrusts();

                    foreach (var trust in trusts)
                    {
                        fastAccounts.Add(new Account(trust.Purse, null, null));
                    }

                    foreach (var account in purseService.SelectAccounts().ToList())
                    {
                        var fastAccount = fastAccounts.FirstOrDefault(a => a.Number == account.Number);

                        if (null != fastAccount)
                        {
                            fastAccount.Name = account.Name;
                            fastAccount.Amount = account.Amount;
                        }
                    }

                    accounts = new List<IAccount>();
                    accounts.AddRange(fastAccounts);
                }
                    break;
                default:
                    throw new ArgumentException("origin.Source == " + origin.Source);
            }

            var result = new List<AccountDropDownListItemTemplate>();

            foreach (var account in accounts)
            {
                var currency = currencyService.ObtainCurrencyByAccountNumber(account.Number);

                if (null != origin.FilterCriteria)
                {
                    if (null != origin.FilterCriteria.Currency && !currency.Equals(origin.FilterCriteria.Currency))
                        continue;

                    if (origin.FilterCriteria.HasMoney && null != account.Amount && 0 == account.Amount.Value)
                        continue;

                    if (!currencyService.CheckCapabilities(currency, origin.FilterCriteria.CurrencyCapabilities))
                        continue;
                }

                decimal? amount = account.Amount;
                decimal? recommendedAmount = null;

                if (null != amount)
                {
                    decimal commission = 0.01M;

                    if (amount >= 0.01M)
                        commission = transferService.CalculateCommission(amount.Value, currency);

                    if (commission > amount.Value)
                        recommendedAmount = 0;
                    else
                        recommendedAmount = amount - commission;
                }

                result.Add(new AccountDropDownListItemTemplate(account.Number)
                {
                    Amount = amount,
                    AvailableAmount = amount,
                    RecommendedAmount = recommendedAmount,
                    Currency = currencyService.AddPrefix(currency),
                    Name = account.Name
                });
            }

            if (null != origin.SelectedAccountNumber)
            {
                foreach (var itemTemplate in result)
                {
                    if (itemTemplate.Number.Equals(origin.SelectedAccountNumber))
                    {
                        itemTemplate.Selected = true;
                        break;
                    }
                }
            }

            return result;
        }

        public static List<AccountDropDownListItemTemplate> BuildCurrencyDropDownListItemTemplates(
            IUnityContainer container, CurrencyCapabilities currencyCapabilities)
        {
            if (null == container)
                throw new ArgumentNullException(nameof(container));

            var currencyService = container.Resolve<ICurrencyService>();

            var currencies = currencyService.SelectCurrencies(currencyCapabilities);

            var result = new List<AccountDropDownListItemTemplate>();

            foreach (var currency in currencies)
            {
                var currencyWithPrefix = currencyService.AddPrefix(currency);
                result.Add(new AccountDropDownListItemTemplate(currencyWithPrefix));
            }

            return result;
        }
    }
}