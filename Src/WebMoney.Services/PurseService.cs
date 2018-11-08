using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Contracts.Exceptions;
using WebMoney.Services.DataAccess.EF;
using WebMoney.Services.ExternalServices.Contracts;
using WebMoney.Services.Utils;
using WebMoney.XmlInterfaces;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Exceptions;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.Services
{
    public sealed class PurseService : SessionBasedService, IPurseService
    {
        private static readonly object Anchor = new object();

        private static readonly Dictionary<long, List<Account>> AccountsDictionary =
            new Dictionary<long, List<Account>>();

        public string CreatePurse(string currency, string name)
        {
            if (null == currency)
                throw new ArgumentNullException(nameof(currency));

            if (null == name)
                throw new ArgumentNullException(nameof(name));

            var request = new OriginalPurse((WmId) Session.CurrentIdentifier, Purse.LetterToCurrency(currency[0]),
                (Description) name)
            {
                Initializer = Session.AuthenticationService.ObtainInitializer()
            };

            RecentPurse response;

            try
            {
                response = request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalServiceException(exception.Message, exception);
            }

            return response.Purse.ToString();
        }

        public void AddPurse(string purse, string name)
        {
            if (null == purse)
                throw new ArgumentNullException(nameof(purse));

            if (null == name)
                throw new ArgumentNullException(nameof(name));

            if (!Session.AuthenticationService.HasConnectionSettings)
                throw new InvalidOperationException("!Session.AuthenticationService.HasConnectionSettings");

            var identifierService = Container.Resolve<IIdentifierService>();
            var purseIdentifier = identifierService.FindIdentifier(purse);

            var currentIdentifier = Session.CurrentIdentifier;

            if (null == purseIdentifier || purseIdentifier.Value != currentIdentifier)
                throw new AlienPurseException();

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                if (context.Accounts.Any(entity => entity.Number == purse))
                    return;

                var account = new Account(purse, currentIdentifier, name)
                {
                    IsManuallyAdded = true
                };

                context.Accounts.Add(account);

                context.SaveChanges();
            }
        }

        public void RemovePurse(string purse)
        {
            if (null == purse)
                throw new ArgumentNullException(nameof(purse));

            if (!Session.AuthenticationService.HasConnectionSettings)
                throw new InvalidOperationException("!Session.AuthenticationService.HasConnectionSettings");

            var currentIdentifier = Session.CurrentIdentifier;

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                var account =
                    context.Accounts.FirstOrDefault(
                        entity => entity.Identifier == currentIdentifier && entity.Number == purse);

                if (null == account)
                    return;

                if (!account.IsManuallyAdded)
                    throw new InvalidOperationException("!account.IsManuallyAdded");

                context.Accounts.Remove(account);

                context.SaveChanges();
            }
        }

        public void SetMerchantKey(string purse, string key)
        {
            if (null == purse)
                throw new ArgumentNullException(nameof(purse));

            if (null == key)
                throw new ArgumentNullException(nameof(key));

            if (!Session.AuthenticationService.HasConnectionSettings)
                throw new InvalidOperationException("!Session.AuthenticationService.HasConnectionSettings");

            var currentIdentifier = Session.CurrentIdentifier;

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                var entity = context.Accounts.FirstOrDefault(a => a.Identifier == currentIdentifier && a.Number == purse);

                if (null == entity)
                    throw new InvalidOperationException("null == entity");

                entity.MerchantKey = key;

                context.SaveChanges();
            }
        }

        public void SetSecretKeyX20(string purse, string key)
        {
            if (null == purse)
                throw new ArgumentNullException(nameof(purse));

            if (null == key)
                throw new ArgumentNullException(nameof(key));

            if (!Session.AuthenticationService.HasConnectionSettings)
                throw new InvalidOperationException("!Session.AuthenticationService.HasConnectionSettings");

            var currentIdentifier = Session.CurrentIdentifier;

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                var entity = context.Accounts.FirstOrDefault(a => a.Identifier == currentIdentifier && a.Number == purse);

                if (null == entity)
                    throw new InvalidOperationException("null == entity");

                entity.SecretKeyX20 = key;

                context.SaveChanges();
            }
        }

        public void ClearMerchantKey(string purse)
        {
            if (null == purse)
                throw new ArgumentNullException(nameof(purse));

            if (!Session.AuthenticationService.HasConnectionSettings)
                throw new InvalidOperationException("!Session.AuthenticationService.HasConnectionSettings");

            var currentIdentifier = Session.CurrentIdentifier;

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                var entity = context.Accounts.FirstOrDefault(a => a.Identifier == currentIdentifier && a.Number == purse);

                if (null == entity)
                    throw new InvalidOperationException("null == entity");

                entity.MerchantKey = null;

                context.SaveChanges();
            }
        }

        public void ClearSecretKeyX20(string purse)
        {
            if (null == purse)
                throw new ArgumentNullException(nameof(purse));

            if (!Session.AuthenticationService.HasConnectionSettings)
                throw new InvalidOperationException("!Session.AuthenticationService.HasConnectionSettings");

            var currentIdentifier = Session.CurrentIdentifier;

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                var entity = context.Accounts.FirstOrDefault(a => a.Identifier == currentIdentifier && a.Number == purse);

                if (null == entity)
                    throw new InvalidOperationException("null == entity");

                entity.SecretKeyX20 = null;

                context.SaveChanges();
            }
        }

        public IEnumerable<IAccount> SelectAccounts(bool fresh = false, bool masterAccountsRequested = false)
        {
            List<Account> accounts;

            var identifier = masterAccountsRequested ? Session.AuthenticationService.MasterIdentifier : Session.CurrentIdentifier;

            if (!fresh)
            {
                if (Session.AuthenticationService.HasConnectionSettings)
                    using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
                    {
                        accounts = (from a in context.Accounts
                            where a.Identifier == identifier
                            select a).ToList();
                    }
                else
                {
                    lock (Anchor)
                    {
                        if (AccountsDictionary.ContainsKey(identifier))
                            return AccountsDictionary[identifier];
                    }

                    return new List<Account>();
                }
            }
            else
            {
                var externalPurseService = Container.Resolve<IExternalPurseService>();

                // TODO [L] Применить копирование из IAccount в Account (тип может не совпадать!).
                accounts = externalPurseService.SelectAccounts().Select(a => (Account) a).ToList();

                // Сохраняем
                if (Session.AuthenticationService.HasConnectionSettings)
                    using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
                    {
                        var localAccounts = context.Accounts.Select(identifier);

                        // Удаляем существующие (кроме добавленных вручную).
                        foreach (var localAccount in localAccounts)
                        {
                            var account = accounts.FirstOrDefault(a => a.Number == localAccount.Number);

                            if (null == account && localAccount.IsManuallyAdded)
                                continue; // Не удаляем (добавлен вручную)

                            if (null != account && null != localAccount.MerchantKey)
                                account.MerchantKey = localAccount.MerchantKey; // Копируем MerchantKey

                            context.Accounts.Remove(localAccount);
                        }

                        context.Accounts.AddRange(accounts);

                        context.SaveChanges();

                        accounts = context.Accounts.Select(identifier);
                    }
                else
                {
                    lock (Anchor)
                    {
                        if (AccountsDictionary.ContainsKey(identifier))
                            AccountsDictionary[identifier].Clear();
                        else
                            AccountsDictionary.Add(identifier, new List<Account>());

                        AccountsDictionary[identifier].AddRange(accounts);
                    }
                }
            }

            return accounts;
        }
    }
}
