using System;
using Unity;
using WMBusinessTools.Extensions.BusinessObjects;

namespace WMBusinessTools.Extensions.DisplayHelpers.Origins
{
    internal sealed class AccountDropDownListOrigin
    {
        public IUnityContainer Container { get; }
        public AccountFilterCriteria FilterCriteria { get; }
        public string SelectedAccountNumber { get; set; }
        public AccountSource Source { get; set; }

        public AccountDropDownListOrigin(IUnityContainer container)
        {
            Container = container ?? throw new ArgumentNullException(nameof(container));
            FilterCriteria = new AccountFilterCriteria();
        }
    }
}