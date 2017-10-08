using System;
using System.Collections.Generic;
using System.Linq;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Contracts.Exceptions;
using WebMoney.Services.Utils;
using WebMoney.XmlInterfaces;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Exceptions;
using WebMoney.XmlInterfaces.Responses;
using IncomingInvoice = WebMoney.Services.BusinessObjects.IncomingInvoice;
using OutgoingInvoice = WebMoney.Services.BusinessObjects.OutgoingInvoice;

namespace WebMoney.Services
{
    public sealed class InvoiceService : SessionBasedService, IInvoiceService
    {
        public void CreateOutgoingInvoice(IOriginalOutgoingInvoice outgoingInvoice)
        {
            if (null == outgoingInvoice)
                throw new ArgumentNullException(nameof(outgoingInvoice));

            var request = new OriginalInvoice((uint) outgoingInvoice.OrderId, (WmId) outgoingInvoice.ClientIdentifier,
                Purse.Parse(outgoingInvoice.StorePurse), (Amount) outgoingInvoice.Amount)
            {
                Initializer = Session.AuthenticationService.ObtainInitializer()
            };

            if (null != outgoingInvoice.Description)
                request.Description = (Description) outgoingInvoice.Description;

            if (null != outgoingInvoice.Address)
                request.Address = (Description) outgoingInvoice.Address;

            if (null != outgoingInvoice.ProtectionPeriod)
                request.Period = outgoingInvoice.ProtectionPeriod.Value;

            request.Expiration = outgoingInvoice.ExpirationPeriod;

            try
            {
                request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalException(exception.Message, exception);
            }
        }

        public void RejectInvoice(long id)
        {
            var request =
                new InvoiceRefusal((WmId) Session.CurrentIdentifier, (uint) id)
                {
                    Initializer = Session.AuthenticationService.ObtainInitializer()
                };

            try
            {
                request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalException(exception.Message, exception);
            }
        }

        public IReadOnlyCollection<IIncomingInvoice> SelectIncomingInvoices(DateTime fromTime, DateTime toTime, bool fresh = false)
        {
            var request = new IncomingInvoiceFilter((WmId) Session.CurrentIdentifier, fromTime, toTime)
            {
                Initializer = Session.AuthenticationService.ObtainInitializer()
            };

            IncomingInvoiceRegister response;

            try
            {
                response = request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalException(exception.Message, exception);
            }

            return response.IncomingInvoiceList.Select(ii =>
                {
                    var incomingInvoice = new IncomingInvoice(ii.Id, ii.Ts, (int) ii.OperationId, ii.TargetWmId,
                        ii.TargetPurse.ToString(),
                        ii.Amount, ii.Description, ii.Expiration, ConvertFrom.ApiTypeToContractType(ii.InvoiceState),
                        ii.CreateTime,
                        ii.UpdateTime) {Address = ii.Address};


                    if (ii.Period > 0)
                        incomingInvoice.ProtectionPeriod = ii.Period;

                    if (ii.OperationId > 0)
                        incomingInvoice.TransferPrimaryId = ii.OperationId;

                    return (IIncomingInvoice) incomingInvoice;
                })
                .OrderByDescending(ii => ii.UpdateTime)
                .ToList();
        }

        public IReadOnlyCollection<IOutgoingInvoice> SelectOutgoingInvoices(string purse, DateTime fromTime, DateTime toTime,
            bool fresh = false)
        {
            var request =
                new OutgoingInvoiceFilter(Purse.Parse(purse), fromTime, toTime)
                {
                    Initializer = Session.AuthenticationService.ObtainInitializer()
                };

            OutgoingInvoiceRegister response;

            try
            {
                response = request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalException(exception.Message, exception);
            }

            return response.OutgoingInvoiceList.Select(oi =>
                {
                    var outgoingInvoice = new OutgoingInvoice(oi.Id, oi.Ts, (int) oi.OperationId, oi.SourceWmId,
                        oi.TargetPurse.ToString(), oi.Amount, oi.Description, oi.Expiration,
                        ConvertFrom.ApiTypeToContractType(oi.InvoiceState), oi.CreateTime, oi.UpdateTime)
                    {
                        Address = oi.Address
                    };

                    if (oi.Period > 0)
                        outgoingInvoice.ProtectionPeriod = oi.Period;

                    if (oi.OperationId > 0)
                        outgoingInvoice.TransferPrimaryId = oi.OperationId;

                    return (IOutgoingInvoice) outgoingInvoice;
                })
                .OrderByDescending(oi => oi.UpdateTime)
                .ToList();
        }
    }
}
