using System;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.Contracts.Contexts
{
    public class ContractContext : SessionContext
    {
        public IContract Contract { get; }

        public ContractContext(ContractContext origin)
            : base(origin)
        {
            Contract = origin.Contract;
        }

        public ContractContext(SessionContext baseContext, IContract contract)
            : base(baseContext)
        {
            Contract = contract ?? throw new ArgumentNullException(nameof(contract));
        }
    }
}
