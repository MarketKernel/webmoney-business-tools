using System;
using System.Collections.Generic;
using ExtensibilityAssistant;
using WMBusinessTools.Extensions.Templates.Controls;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.DisplayHelpers
{
    internal static class SubmitFormDisplayHelper
    {
        public static SubmitForm LoadSubmitFormByExtensionId(ExtensionManager extensionManager, string extensionId,
            Dictionary<string, object> values = null)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            var template =
                TemplateLoader.LoadTemplate<SubmitFormTemplate<WMColumnTemplate>>(extensionManager,
                    extensionId);

            return BuildSubmitForm(extensionManager, template, values);
        }

        public static SubmitForm LoadSubmitFormByTemplatePath(ExtensionManager extensionManager, string templatePath,
            Dictionary<string, object> values = null)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == templatePath)
                throw new ArgumentNullException(nameof(templatePath));

            var template = TemplateLoader.LoadTemplate<SubmitFormTemplate<WMColumnTemplate>>(templatePath);
            return BuildSubmitForm(extensionManager, template, values);
        }

        public static SubmitForm BuildSubmitForm(ExtensionManager extensionManager, SubmitFormTemplate<WMColumnTemplate> template,
            Dictionary<string, object> values = null)
        {
            if (null == extensionManager)
                throw new ArgumentNullException(nameof(extensionManager));

            if (null == template)
                throw new ArgumentNullException(nameof(template));

            var submitForm = new SubmitForm();

            ErrorFormDisplayHelper.ApplyErrorAction(extensionManager, submitForm);

            submitForm.ApplyTemplate(template, values);

            return submitForm;
        }
    }
}