using System;
using System.Collections.Generic;
using System.Linq;
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
    internal sealed class ExternalTransferService : SessionBasedService, IExternalTransferService
    {
        public long CreateTransfer(IOriginalTransfer originalTransfer)
        {
            if (null == originalTransfer)
                throw new ArgumentNullException(nameof(originalTransfer));

            var request = new OriginalTransfer((uint)originalTransfer.TransferId,
                Purse.Parse(originalTransfer.SourcePurse), Purse.Parse(originalTransfer.TargetPurse),
                (Amount)originalTransfer.Amount)
            {
                Description = (Description)originalTransfer.Description,
                Period = originalTransfer.ProtectionPeriod,
                Code = (Description)originalTransfer.ProtectionCode,
                Force = originalTransfer.Force,
                Initializer = Session.AuthenticationService.ObtainInitializer()
            };

            if (null != originalTransfer.InvoiceId)
                request.InvoiceId = (uint)originalTransfer.InvoiceId.Value;

            TransferEnvelope response;

            try
            {
                response = request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalException(exception.Message, exception);
            }

            return response.Transfer.Id;
        }

        public IReadOnlyCollection<ITransfer> SelectTransfers(string purse, DateTime fromTime, DateTime toTime)
        {
            if (null == purse)
                throw new ArgumentNullException(nameof(purse));

            var request = new TransferFilter(Purse.Parse(purse), fromTime, toTime)
            {
                Initializer = Session.AuthenticationService.ObtainInitializer()
            };

            TransferRegister response;

            try
            {
                response = request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalException(exception.Message, exception);
            }

            return response.TransferList.Select(t =>
                {
                    var transfer = new BusinessObjects.Transfer(Session.CurrentIdentifier, t.Id, t.Ts,
                        t.SourcePurse.ToString(), t.TargetPurse.ToString(), t.Amount,
                        t.Commission, ConvertFrom.ApiTypeToContractType(t.TransferType), t.Description, t.Partner,
                        t.Rest,
                        t.CreateTime.ToUniversalTime(), t.UpdateTime.ToUniversalTime())
                    {
                        InvoiceId = t.InvoiceId,
                        OrderId = (int) t.OrderId,
                        TransferId = (int) t.TransferId,
                        ProtectionPeriod = t.Period,
                        Locked = t.IsLocked,
                        Description = !string.IsNullOrEmpty(t.Description) ? t.Description : "[empty]"
                    };


                    return (ITransfer) transfer;
                })
                .ToList();
        }
    }
}
