using System;
using ExtensibilityAssistant;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WMBusinessTools.Extensions.DisplayHelpers.Origins
{
    internal sealed class FilterOrigin
    {
        public ExtensionManager ExtensionManager { get; }
        public string ExtensionId { get; }
        public string MenuItemsTagName { get; set; }
        public string CommandBarTagName { get; set; }
        public IColumnsSettings ColumnsSettings { get; set; }

        public FilterOrigin(ExtensionManager extensionManager, string extensionId)
        {
            ExtensionManager = extensionManager ?? throw new ArgumentNullException(nameof(extensionManager));
            ExtensionId = extensionId ?? throw new ArgumentNullException(nameof(extensionId));

        }
    }
}
