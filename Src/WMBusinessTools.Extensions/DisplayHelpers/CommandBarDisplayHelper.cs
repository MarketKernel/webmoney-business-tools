using System;
using System.Collections.Generic;
using ExtensibilityAssistant;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.DisplayHelpers
{
    internal static class CommandBarDisplayHelper
    {
        public static List<TunableButtonTemplate> BuildCommandButtons(ExtensionManager extensionManager,
            string commandBarTagName)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == commandBarTagName)
                throw new ArgumentNullException(nameof(commandBarTagName));

            var commandButtons = new List<TunableButtonTemplate>();

            var commandExtensions = extensionManager.SelectExtensionInfoListByTag(commandBarTagName);

            commandButtons.Clear();

            foreach (var commandExtension in commandExtensions)
            {
                var commandButtonTemplate = new TunableButtonTemplate(commandExtension.Name, commandExtension.Id);
                commandButtons.Add(commandButtonTemplate);
            }

            return commandButtons;
        }
    }
}
