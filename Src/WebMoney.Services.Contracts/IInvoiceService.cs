using System;
using System.Collections.Generic;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Contracts
{
    public interface IInvoiceService
    {
        void CreateOutgoingInvoice(IOriginalOutgoingInvoice outgoingInvoice);
        void RejectInvoice(long id);
        IEnumerable<IIncomingInvoice> SelectIncomingInvoices(DateTime fromTime, DateTime toTime, bool fresh = false);
        IEnumerable<IOutgoingInvoice> SelectOutgoingInvoices(string purse, DateTime fromTime, DateTime toTime, bool fresh = false);
    }
}
