using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.Contracts.Contexts
{
    public class OutgoingInvoiceContext : PurseContext
    {
        public IOutgoingInvoice Invoice { get; }

        public OutgoingInvoiceContext(OutgoingInvoiceContext origin)
            : base(origin)
        {
            Invoice = origin.Invoice;
        }

        public OutgoingInvoiceContext(PurseContext baseContext, IOutgoingInvoice invoice)
            : base(baseContext)
        {
            Invoice = invoice ?? throw new ArgumentNullException(nameof(invoice));
        }
    }
}
