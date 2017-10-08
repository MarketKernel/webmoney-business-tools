using System;
using WMBusinessTools.Extensions.DisplayHelpers.Origins;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.DisplayHelpers
{
    internal static class ListScreenDisplayHelper
    {
        public static ListScreenTemplate LoadListScreenTemplate(ListScreenOrigin origin)
        {
            if (null == origin)
                throw new ArgumentNullException(nameof(origin));

            var template = TemplateLoader.LoadTemplate<ListScreenTemplate>(origin.ExtensionManager, origin.ExtensionId);

            if (null == template.TunableList)
                throw new BadTemplateException("null == template.TunableList");

            // Меню
            if (null != origin.MenuItemsTagName)
                template.TunableList.CommandMenu =
                    MenuDisplayHelper.BuildCommandMenu(origin.ExtensionManager, origin.MenuItemsTagName, template.BaseDirectory);

            // Кнопки
            if (null != origin.CommandBarTagName)
            {
                template.CommandButtons.Clear();
                template.CommandButtons.AddRange(
                    CommandBarDisplayHelper.BuildCommandButtons(origin.ExtensionManager, origin.CommandBarTagName));
            }

            template.SetTemplateInternals(template.TemplateName, template.BaseDirectory);

            return template;
        }
    }
}