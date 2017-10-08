using System;
using System.Collections.Generic;
using System.Reflection;
using WebMoney.Services.Contracts.BusinessObjects;
using WMBusinessTools.Extensions.DisplayHelpers.Origins;
using WMBusinessTools.Extensions.Templates.Controls;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.DisplayHelpers
{
    internal static class FilterDisplayHelper
    {
        public static FilterScreenTemplate<WMColumnTemplate> LoadFilterScreenTemplate(FilterOrigin origin)
        {
            if (null == origin)
                throw new ArgumentNullException(nameof(origin));

            var template =
                TemplateLoader.LoadTemplate<FilterScreenTemplate<WMColumnTemplate>>(origin.ExtensionManager,
                    origin.ExtensionId);

            if (null == template.Grid)
                throw new BadTemplateException("null == template.Grid");

            if (null != origin.ColumnsSettings)
                SetupColumns(template.Grid.Columns, origin.ColumnsSettings);

            // Меню
            if (null != origin.MenuItemsTagName)
                template.Grid.CommandMenu =
                    MenuDisplayHelper.BuildCommandMenu(origin.ExtensionManager, origin.MenuItemsTagName,
                        template.BaseDirectory);

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

        public static FilterFormTemplate<WMColumnTemplate> LoadFilterFormTemplate(FilterOrigin origin)
        {
            if (null == origin)
                throw new ArgumentNullException(nameof(origin));

            var template =
                TemplateLoader.LoadTemplate<FilterFormTemplate<WMColumnTemplate>>(origin.ExtensionManager,
                    origin.ExtensionId);

            if (null == template.FilterScreen)
                throw new BadTemplateException("null == template.FilterScreen");

            if (null == template.FilterScreen.Grid)
                throw new BadTemplateException("null == template.FilterScreen.Grid");

            if (null != origin.ColumnsSettings)
                SetupColumns(template.FilterScreen.Grid.Columns, origin.ColumnsSettings);

            // Меню
            if (null != origin.MenuItemsTagName)
                template.FilterScreen.Grid.CommandMenu =
                    MenuDisplayHelper.BuildCommandMenu(origin.ExtensionManager, origin.MenuItemsTagName,
                        template.BaseDirectory);

            // Кнопки
            if (null != origin.CommandBarTagName)
            {
                template.FilterScreen.CommandButtons.Clear();
                template.FilterScreen.CommandButtons.AddRange(
                    CommandBarDisplayHelper.BuildCommandButtons(origin.ExtensionManager, origin.CommandBarTagName));
            }

            template.SetTemplateInternals(template.TemplateName, template.BaseDirectory);

            return template;
        }

        public static void UpdateColumnsSettings(IColumnsSettings columnsSettings, List<GridColumnSettings> sourceGridColumnSettings)
        {
            if (null == columnsSettings)
                throw new ArgumentNullException(nameof(columnsSettings));

            if (null == sourceGridColumnSettings)
                throw new ArgumentNullException(nameof(sourceGridColumnSettings));

            int index = 0;

            foreach (var gridColumnSettings in sourceGridColumnSettings)
            {
                if (columnsSettings.ColumnOrders.Count <= index)
                    columnsSettings.ColumnOrders.Add(gridColumnSettings.DisplayIndex);
                else
                    columnsSettings.ColumnOrders[index] = gridColumnSettings.DisplayIndex;

                if (columnsSettings.ColumnWidths.Count <= index)
                    columnsSettings.ColumnWidths.Add(gridColumnSettings.Width);
                else
                    columnsSettings.ColumnWidths[index] = gridColumnSettings.Width;

                index++;
            }
        }

        private static void SetupColumns(List<GridColumnTemplate> columns, IColumnsSettings columnsSettings)
        {
            int index = 0;

            foreach (var columnTemplate in columns)
            {
                var propertyInfo = columnsSettings.GetType()
                    .GetProperty(columnTemplate.Name + "Visibility", BindingFlags.Instance | BindingFlags.Public);

                if (null != propertyInfo)
                {
                    var visibility = (bool) propertyInfo.GetValue(columnsSettings, null);
                    columnTemplate.Visible = visibility;
                }

                if (columnsSettings.ColumnOrders.Count > index)
                    columnTemplate.Order = columnsSettings.ColumnOrders[index];

                if (columnsSettings.ColumnWidths.Count > index)
                    columnTemplate.Width = columnsSettings.ColumnWidths[index];

                index++;
            }
        }
    }
}
