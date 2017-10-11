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
    internal sealed class ExternalTrustService : SessionBasedService, IExternalTrustService
    {
        public IEnumerable<ITrust> SelectTrusts()
        {
            var request =
                new OutgoingTrustFilter((WmId) Session.CurrentIdentifier)
                {
                    Initializer = Session.AuthenticationService.ObtainInitializer()
                };

            TrustRegister trustRegister;

            try
            {
                trustRegister = request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalServiceException(exception.Message, exception);
            }

            return trustRegister.TrustList.Where(t => null != t.Purse)
                .Select(t => new BusinessObjects.Trust(t.Master, t.InvoiceAllowed, t.TransferAllowed,
                    t.BalanceAllowed,
                    t.HistoryAllowed,
                    t.Purse.ToString(), t.DayLimit, t.WeekLimit, t.MonthLimit, t.DayAmount, t.WeekAmount,
                    t.MonthAmount,
                    t.LastDate.ToUniversalTime()));
        }
    }
}