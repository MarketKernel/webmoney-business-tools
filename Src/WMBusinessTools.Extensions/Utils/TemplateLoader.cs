using System;
using System.IO;
using ExtensibilityAssistant;
using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.Utils
{
    internal static class TemplateLoader
    {
        static TemplateLoader()
        {
            ControlTemplateJsonConverter.Logics = new WMConverterLogics();
        }

        public static SubmitFormTemplate<WMColumnTemplate> LoadSubmitFormTemplate(ExtensionManager extensionManager, string extensionId)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            var templateFilePath = ObtainTemplateFilePath(extensionManager, extensionId);

            return LoadTemplate<SubmitFormTemplate<WMColumnTemplate>>(templateFilePath);
        }

        public static TTemplate LoadTemplate<TTemplate>(ExtensionManager extensionManager, string extensionId)
            where TTemplate : class, ITemplate
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            var templateFilePath = ObtainTemplateFilePath(extensionManager, extensionId);

            return Xml2WinForms.Utils.TemplateLoader.LoadTemplateFromJsonFile<TTemplate>(templateFilePath);
        }

        public static TTemplate LoadTemplate<TTemplate>(string templateFilePath)
            where TTemplate : class, ITemplate
        {
            if (null == templateFilePath)
                throw new ArgumentNullException(nameof(templateFilePath));

            return Xml2WinForms.Utils.TemplateLoader.LoadTemplateFromJsonFile<TTemplate>(templateFilePath);
        }

        private static string ObtainTemplateFilePath(ExtensionManager extensionManager, string extensionId)
        {
            var extensionConfiguration = extensionManager.TryObtainExtensionConfiguration(extensionId);

            if (null == extensionConfiguration)
                throw new InvalidOperationException("null == extensionConfiguration");

            if (string.IsNullOrEmpty(extensionConfiguration.ConfigurationString))
                throw new BadTemplateException("ConfigurationString is null or empty!");

            return Path.Combine(extensionConfiguration.BaseDirectory, extensionConfiguration.ConfigurationString);
        }
    }
}
