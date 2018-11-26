using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace WiXBuilder
{
    class Component
    {
        private const string ComponentTemplate = "<Component Id=\"{0}\" Guid=\"{1}\" Win64=\"$(var.Win64)\">\r\n" +
                                                 "<File Id=\"{2}\" Name=\"{3}\" Source=\"$(var.ProductionDir){4}\" KeyPath=\"yes\" />\r\n" +
                                                 "</Component>\r\n";

        private static readonly Dictionary<string, int> ComponentIdDictionary = new Dictionary<string, int>();
        private static readonly Dictionary<string, int> FileIdDictionary = new Dictionary<string, int>();

        public string Id { get; set; }
        public string Guid { get; set; }

        public string FileId { get; set; }
        public string FileName { get; set; }
        public string FileSource { get; set; }

        public static Component Create(string binDirectory, string path)
        {
            if (null == binDirectory)
                throw new ArgumentNullException(nameof(binDirectory));

            if (null == path)
                throw new ArgumentNullException(nameof(path));

            var fileName = Path.GetFileName(path);

            var componentId = string.Format(CultureInfo.InvariantCulture, "Component_{0}", IdUtility.BuildId(fileName));
            var fileId = string.Format(CultureInfo.InvariantCulture, "File_{0}", IdUtility.BuildId(fileName));

            if (ComponentIdDictionary.ContainsKey(componentId.ToLower()))
            {
                int count = ComponentIdDictionary[componentId.ToLower()];
                count++;
                ComponentIdDictionary[componentId.ToLower()] = count;

                componentId = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", componentId, count);
            }
            else
                ComponentIdDictionary.Add(componentId.ToLower(), 0);

            if (FileIdDictionary.ContainsKey(fileId.ToLower()))
            {
                int count = FileIdDictionary[fileId.ToLower()];
                count++;
                FileIdDictionary[fileId.ToLower()] = count;

                fileId = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", fileId, count);
            }
            else
                FileIdDictionary.Add(fileId.ToLower(), 0);

            var component = new Component
            {
                Id = componentId,
                FileId = fileId,
                Guid = System.Guid.NewGuid().ToString().ToLower(),
                FileName = Path.GetFileName(path)
            };

            if (!path.StartsWith(binDirectory))
                throw new InvalidOperationException();

            component.FileSource = path.Remove(0, binDirectory.Length);

            return component;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, ComponentTemplate, Id, Guid, FileId, FileName,
                FileSource);
        }
    }
}
