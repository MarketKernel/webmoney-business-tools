using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ExtensibilityAssistant;
using LocalizationAssistant;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms.Templates;
using Xml2WinForms.Utils;

namespace WMBusinessTools.Extensions.DisplayHelpers
{
    public static class MenuDisplayHelper
    {
        public static TunableMenuTemplate BuildCommandMenu(ExtensionManager extensionManager, string tagName, string templateBaseDirectory)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == tagName)
                throw new ArgumentNullException(nameof(tagName));

            if (null == templateBaseDirectory)
                throw new ArgumentNullException(nameof(templateBaseDirectory));

            var extensionInfoList = extensionManager.SelectExtensionInfoListByTag(tagName);
            var itemTemplates = new List<MenuItemTemplate>();

            decimal? currentGroupOrder = null;

            foreach (var extensionInfo in extensionInfoList)
            {
                if (null == currentGroupOrder)
                    currentGroupOrder = extensionInfo.GroupOrder;

                if (currentGroupOrder != extensionInfo.GroupOrder)
                {
                    itemTemplates.Add(new MenuItemTemplate("-"));
                    currentGroupOrder = extensionInfo.GroupOrder;
                }

                string iconPath = null;

                if (null != extensionInfo.IconPath)
                {
                    var iconAbsolutePath = Path.Combine(extensionInfo.BaseDirectory, extensionInfo.IconPath);
                    iconPath = FileUtility.MakeRelativePath(iconAbsolutePath, templateBaseDirectory);
                }

                var menuItemTemplate = new MenuItemTemplate(extensionInfo.Name)
                {
                    Command = extensionInfo.Id,
                    IconPath = iconPath
                };

                itemTemplates.Add(menuItemTemplate);
            }

            TunableMenuTemplate tunableMenuTemplate;

            if (0 == itemTemplates.Count)
                tunableMenuTemplate = null;
            else
            {
                tunableMenuTemplate = new TunableMenuTemplate();

                tunableMenuTemplate.Items.Clear();
                tunableMenuTemplate.Items.AddRange(itemTemplates);
            }

            return tunableMenuTemplate;
        }

        public static ToolStripItem[] BuildToolStripItems(ExtensionManager extensionManager, string tagName)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == tagName)
                throw new ArgumentNullException(nameof(tagName));

            var extensionInfoList = extensionManager.SelectExtensionInfoListByTag(tagName);

            var toolStripItems = new List<ToolStripItem>();

            decimal? currentGroupOrder = null;

            foreach (var extensionInfo in extensionInfoList)
            {
                if (null == currentGroupOrder)
                    currentGroupOrder = extensionInfo.GroupOrder;

                if (currentGroupOrder.Value != extensionInfo.GroupOrder)
                {
                    toolStripItems.Add(new ToolStripSeparator());
                    currentGroupOrder = extensionInfo.GroupOrder;
                }

                var toolStripItem = new ToolStripMenuItem(Translator.Instance.Translate("Menu", extensionInfo.Name))
                {
                    Tag = extensionInfo.Id,
                };

                var iconHolder = null != extensionInfo.IconPath
                    ? new IconHolder(extensionInfo.BaseDirectory, extensionInfo.IconPath)
                    : null;

                var image = iconHolder?.TryBuildImage();

                if (image != null)
                    toolStripItem.Image = image;

                toolStripItems.Add(toolStripItem);
            }

            return toolStripItems.ToArray();
        }
    }
}