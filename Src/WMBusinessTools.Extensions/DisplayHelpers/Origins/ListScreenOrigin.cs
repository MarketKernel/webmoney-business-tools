using System;
using ExtensibilityAssistant;

namespace WMBusinessTools.Extensions.DisplayHelpers.Origins
{
    internal class ListScreenOrigin
    {
        public ExtensionManager ExtensionManager { get; }
        public string ExtensionId { get; }
        public string MenuItemsTagName { get; set; }
        public string CommandBarTagName { get; set; }

        public ListScreenOrigin(ExtensionManager extensionManager, string extensionId)
        {
            ExtensionManager = extensionManager ?? throw new ArgumentNullException(nameof(extensionManager));
            ExtensionId = extensionId ?? throw new ArgumentNullException(nameof(extensionId));
        }
    }
}
