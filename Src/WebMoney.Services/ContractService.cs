using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.DataAccess.EF;
using WebMoney.Services.ExternalServices.Contracts;

namespace WebMoney.Services
{
    public sealed class ContractService : SessionBasedService, IContractService
    {
        public int CreateContract(string name, string text, IEnumerable<long> authorizedIdentifiers)
        {
            if (null == name)
                throw new ArgumentNullException(nameof(name));

            if (null == text)
                throw new ArgumentNullException(nameof(text));

            if (null == authorizedIdentifiers)
                authorizedIdentifiers = new List<long>();

            var externalContractService = Container.Resolve<IExternalContractService>();

            var authorizedIdentifierList = authorizedIdentifiers.ToList();

            var contractId = externalContractService.CreateContract(name, text, authorizedIdentifierList);

            if (Session.AuthenticationService.HasConnectionSettings)
                using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
                {
                    var contract = new Contract(name, text, DateTime.UtcNow)
                    {
                        Id = contractId,
                        IsPublic = 0 == authorizedIdentifierList.Count
                    };

                    if (authorizedIdentifierList.Any())
                    {
                        contract.Signatures.AddRange(authorizedIdentifierList.Select(
                            ai => new ContractSignature
                            {
                                ContractId = contract.Id,
                                AcceptorIdentifier = ai,
                                Contract = contract
                            }));
                    }

                    context.Contracts.Add(contract);
                    context.SaveChanges();
                }

            return contractId;
        }

        public void RefreshContract(int id)
        {
            if (!Session.AuthenticationService.HasConnectionSettings)
                throw new InvalidOperationException("!Session.AuthenticationService.HasConnectionSettings");

            var externalContractService = Container.Resolve<IExternalContractService>();
            var contractSignatures = externalContractService.SelectContractSignatures(id);

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                var localContract = context.Contracts.Include("Signatures").FirstOrDefault(e => e.Id == id);

                if (null != localContract)
                {
                    foreach (var contractSignature in contractSignatures)
                    {
                        var localContractSignature =
                            localContract.Signatures.FirstOrDefault(
                                s => s.AcceptorIdentifier == contractSignature.AcceptorIdentifier);

                        if (null == localContractSignature)
                        {
                            localContract.Signatures.Add(new ContractSignature
                            {
                                AcceptorIdentifier = contractSignature.AcceptorIdentifier,
                                AcceptTime = contractSignature.AcceptTime
                            });
                        }
                        else if (null == localContractSignature.AcceptTime)
                            localContractSignature.AcceptTime = contractSignature.AcceptTime;
                    }
                }

                context.SaveChanges();
            }
        }

        public IReadOnlyCollection<IContract> SelectContracts(DateTime fromTime, DateTime toTime, bool fresh = false)
        {
            if (fresh)
                throw new ArgumentOutOfRangeException(nameof(fresh));

            if (!Session.AuthenticationService.HasConnectionSettings)
                throw new InvalidOperationException("!Session.AuthenticationService.HasConnectionSettings");

            List<Contract> contracts;

            using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
            {
                contracts = (from c in context.Contracts.Include("Signatures")
                    where c.CreationTime >= fromTime
                          && c.CreationTime <= toTime
                    orderby c.CreationTime descending
                    select c).ToList();
            }

            return contracts;
        }
    }
}