using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Xml2WinForms.Templates;

namespace Xml2WinForms.Utils
{
    public static class TemplateLoader
    {
        public static TTemplate LoadTemplateFromXmlFile<TTemplate>(string templatePath)
            where TTemplate : class, ITemplate
        {
            if (null == templatePath)
                throw new ArgumentNullException(nameof(templatePath));

            var xmlSerializer = new XmlSerializer(typeof(TTemplate));
            var xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "");

            TTemplate template;

            using (var fileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                template = (TTemplate) xmlSerializer.Deserialize(fileStream);
            }

            var baseDirectory = Path.GetDirectoryName(templatePath);
            var templateFileName = Path.GetFileNameWithoutExtension(templatePath);

            // Set Internals
            template.SetTemplateInternals(templateFileName, baseDirectory);

            return template;
        }

        public static void SaveTemplateToXmlFile<TTemplate>(TTemplate template, string templatePath)
            where TTemplate : class, ITemplate
        {

            if (null == templatePath)
                throw new ArgumentNullException(nameof(templatePath));

            var xmlSerializer = new XmlSerializer(typeof(TTemplate));
            var xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "");

            using (var fileStream = new FileStream(templatePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                xmlSerializer.Serialize(fileStream, template, xmlSerializerNamespaces);
                fileStream.Flush();
            }
        }

        public static TTemplate LoadTemplateFromJsonFile<TTemplate>(string templatePath)
            where TTemplate: class, ITemplate
        {
            if (null == templatePath)
                throw new ArgumentNullException(nameof(templatePath));

            var baseDirectory = Path.GetDirectoryName(templatePath);
            var templateFileName = Path.GetFileNameWithoutExtension(templatePath);

            var templateJToken = LoadJTokenFromFile(templatePath);

            // Template Library
            Dictionary<string, JToken> templateLibrary = null;

            var templateLibraryPathJToken = templateJToken["templateLibrary"];

            if (null != templateLibraryPathJToken)
            {
                if (null == baseDirectory)
                    throw new InvalidOperationException("null == baseDirectory");

                var templateLibraryPath = Path.Combine(baseDirectory, (string) templateLibraryPathJToken);

                templateLibrary = LoadTemplateLibrary(templateLibraryPath);
            }

            if (null != templateLibrary)
            {
                foreach (var stepJToken in templateJToken["steps"])
                {
                    foreach (var columnJToken in stepJToken["tunableShape"]["columns"])
                    {
                        ApplyTemplates(templateLibrary, columnJToken);
                    }
                }
            }

            TTemplate template;

            var serializer = new JsonSerializer();

            using (var reader = templateJToken.CreateReader())
            {
                template = serializer.Deserialize<TTemplate>(reader);
            }

            // Set Internals
            template.SetTemplateInternals(templateFileName, baseDirectory);

            return template;
        }

        public static void SaveTemplateToJsonFile<TTemplate>(TTemplate template, string templatePath)
            where TTemplate : class, ITemplate
        {
            if (null == templatePath)
                throw new ArgumentNullException(nameof(templatePath));

            var serializer = new JsonSerializer {ContractResolver = new CamelCasePropertyNamesContractResolver()};

            using (var fileStream = new FileStream(templatePath, FileMode.Create, FileAccess.Write))
            using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            using (var jsonTextWriter = new JsonTextWriter(streamWriter) { Formatting = Formatting.Indented })
            {
                serializer.Serialize(jsonTextWriter, template);

                jsonTextWriter.Flush();
                streamWriter.Flush();
                fileStream.Flush(true);
            }
        }

        private static Dictionary<string, JToken> LoadTemplateLibrary(string templateLibraryPath)
        {
            var templateLibrary = new Dictionary<string, JToken>();

            var templateLibraryJToken = LoadJTokenFromFile(templateLibraryPath);

            foreach (var template in templateLibraryJToken["templates"])
            {
                string templateName = (string) template["templateName"];
                templateLibrary.Add(templateName, template);
            }

            return templateLibrary;
        }

        private static JToken LoadJTokenFromFile(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                return JToken.ReadFrom(jsonTextReader);
            }
        }

        private static void ApplyTemplates(Dictionary<string, JToken> templateLibrary, JToken columnJToken)
        {
            foreach (var controlJToken in columnJToken["controls"])
            {
                var innerColumnJToken = controlJToken["column"];

                if (null != innerColumnJToken)
                    ApplyTemplates(templateLibrary, innerColumnJToken);

                var templateIdJToken = controlJToken["prototype"];

                if (null == templateIdJToken)
                    continue;

                var templateId = (string)templateIdJToken;

                JToken template;

                if (!templateLibrary.TryGetValue(templateId, out template))
                    throw new BadTemplateException($"Template \"{templateId}\" not found!");

                var jProperties = ((JObject) template).Properties();

                foreach (var jProperty in jProperties)
                {
                    if (jProperty.Name.Equals("templateName", StringComparison.Ordinal))
                        continue;

                    if (null != controlJToken[jProperty.Name])
                        continue;

                    controlJToken.Last.AddAfterSelf(jProperty);
                }
            }
        }
    }
}
