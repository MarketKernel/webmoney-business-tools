using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace Xml2WinForms.WrappersBuilder
{
    internal static class TemplateLoader
    {
        public static SubmitFormTemplate<WMColumnTemplate> LoadSubmitFormTemplate(string filePath)
        {
            if (null == filePath)
                throw new ArgumentNullException(nameof(filePath));

            // SubmitFormTemplate
            var result = TryLoadSubmitFormTemplate(() =>
                Utils.TemplateLoader.LoadTemplateFromJsonFile<SubmitFormTemplate<WMColumnTemplate>>(filePath));

            if (null != result)
                return result;

            // TunableShapeTemplate
            result = TryLoadSubmitFormTemplate(() =>
            {
                var tunableShapeTemplate = Utils.TemplateLoader
                    .LoadTemplateFromJsonFile<TunableShapeTemplate<WMColumnTemplate>>(filePath);

                var stepTemplates = new List<StepTemplate<WMColumnTemplate>>
                {
                    new StepTemplate<WMColumnTemplate>(tunableShapeTemplate, "action")
                };

                var submitFormTemplate = new SubmitFormTemplate<WMColumnTemplate>("text");
                submitFormTemplate.Steps.AddRange(stepTemplates);

                return submitFormTemplate;
            });

            if (null != result)
                return result;

            // FilterFormTemplate
            result = TryLoadSubmitFormTemplate(() =>
            {
                var filterFormTemplate = Utils.TemplateLoader
                    .LoadTemplateFromJsonFile<FilterFormTemplate<WMColumnTemplate>>(filePath);

                var tunableShapeTemplate =
                    new TunableShapeTemplate<WMColumnTemplate>();
                tunableShapeTemplate.Columns.Add(filterFormTemplate.FilterScreen.Column);

                var stepTemplates = new List<StepTemplate<WMColumnTemplate>>
                {
                    new StepTemplate<WMColumnTemplate>(tunableShapeTemplate, "action")
                };

                var submitFormTemplate = new SubmitFormTemplate<WMColumnTemplate>("text");
                submitFormTemplate.Steps.AddRange(stepTemplates);

                return submitFormTemplate;
            });

            return result;
        }

        private static SubmitFormTemplate<WMColumnTemplate> TryLoadSubmitFormTemplate(
            Func<SubmitFormTemplate<WMColumnTemplate>> loadTemplate)
        {
            try
            {
                return loadTemplate();
            }
            catch (Exception exception) when (exception is JsonSerializationException || exception is ArgumentException)
            {
                return null;
            }
        }
    }
}
