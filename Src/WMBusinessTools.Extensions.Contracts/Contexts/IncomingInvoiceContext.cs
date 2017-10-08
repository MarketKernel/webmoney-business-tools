using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.Contracts.Contexts
{
    public class IncomingInvoiceContext : SessionContext
    {
        public IIncomingInvoice Invoice { get; }

        public IncomingInvoiceContext(IncomingInvoiceContext origin)
            : base(origin)
        {
            Invoice = origin.Invoice;
        }

        public IncomingInvoiceContext(SessionContext baseContext, IIncomingInvoice invoice)
            : base(baseContext)
        {
            Invoice = invoice ?? throw new ArgumentNullException(nameof(invoice));
        }
    }
}
