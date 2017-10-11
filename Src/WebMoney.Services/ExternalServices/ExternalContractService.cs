using System.Collections.Generic;
using System.Linq;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Contracts.Exceptions;
using WebMoney.Services.ExternalServices.Contracts;
using WebMoney.Services.Utils;
using WebMoney.XmlInterfaces;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Exceptions;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.Services.ExternalServices
{
    internal sealed class ExternalContractService : SessionBasedService, IExternalContractService
    {
        public int CreateContract(string name, string text, IEnumerable<long> authorizedIdentifiers)
        {
            var request =
                new OriginalContract((Description) name, text)
                {
                    Initializer = Session.AuthenticationService.ObtainInitializer()
                };

            if (null != authorizedIdentifiers)
            {
                var authorizedIdentifierList = authorizedIdentifiers.ToList();

                if (authorizedIdentifierList.Any())
                    request.AcceptorList = authorizedIdentifierList.Select(entity => (WmId) entity).ToList();
            }

            RecentContract response;

            try
            {
                response = request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalServiceException(exception.Message, exception);
            }

            return (int)response.ContractId;
        }

        public IEnumerable<IContractSignature> SelectContractSignatures(int contractId)
        {
            var request = new AcceptorFilter((uint) contractId)
            {
                Initializer = Session.AuthenticationService.ObtainInitializer()
            };

            AcceptorRegister response;

            try
            {
                response = request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalServiceException(exception.Message, exception);
            }

            return response.AcceptorList
                .Select(acceptor => new ContractSignature(acceptor.WmId)
                {
                    ContractId = (int) acceptor.ContractId,
                    AcceptTime = acceptor.AcceptTime?.ToUniversalTime()
                });
        }
    }
}