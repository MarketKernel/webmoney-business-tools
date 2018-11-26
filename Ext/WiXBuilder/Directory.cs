using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace WiXBuilder
{
    internal class Directory
    {
        private const string DirectoryTemplate = "<Directory Id=\"{0}\" Name=\"{1}\">\r\n" +
                                                 "{2}" +
                                                 "</Directory>\r\n";

        private const string ComponentRefTemplate = "<ComponentRef Id=\"{0}\" />\r\n";

        private static readonly Dictionary<string, int> DirectoryIdDictionary = new Dictionary<string, int>();

        public string Id { get; set; }
        public string Name { get; set; }

        public List<Directory> Directories { get; } = new List<Directory>();
        public List<Component> Components { get; } = new List<Component>();

        public static Directory Create(string binDirectory, string path)
        {
            if (null == binDirectory)
                throw new ArgumentNullException(nameof(binDirectory));

            if (null == path)
                throw new ArgumentNullException(nameof(path));

            if (!path.EndsWith("\\"))
                path += "\\";

            var name = Path.GetFileName(Path.GetDirectoryName(path));

            var id = string.Format(CultureInfo.InvariantCulture, "Directory_{0}", IdUtility.BuildId(name));

            if (DirectoryIdDictionary.ContainsKey(id.ToLower()))
            {
                int count = DirectoryIdDictionary[id.ToLower()];
                count++;
                DirectoryIdDictionary[id.ToLower()] = count;

                id = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", id, count);
            }
            else
                DirectoryIdDictionary.Add(id.ToLower(), 0);

            var directory = new Directory
            {
                Id = id,
                Name = name
            };

            foreach (var file in System.IO.Directory.GetFiles(path))
            {
                directory.Components.Add(Component.Create(binDirectory, file));
            }

            foreach (var subDirectory in System.IO.Directory.GetDirectories(path))
            {
                directory.Directories.Add(Create(binDirectory, subDirectory));
            }

            return directory;
        }

        public XDocument BuildComponentRefsXDocument()
        {
            var components = SelectComponents();

            var stringBuilder = new StringBuilder();

            foreach (var component in components)
            {
                stringBuilder.AppendFormat(ComponentRefTemplate, component.Id);
            }

            return XDocument.Parse($"<ComponentRefs xmlns=\"http://schemas.microsoft.com/wix/2006/wi\">${stringBuilder}</ComponentRefs>");
        }

        public XDocument BuildDirectoryXDocument()
        {
            return XDocument.Parse($"<Directory xmlns=\"http://schemas.microsoft.com/wix/2006/wi\">${ToString()}</Directory>");
        }

        private List<Component> SelectComponents()
        {
            var components = new List<Component>();

            components.AddRange(Components);

            foreach (var directory in Directories)
            {
                components.AddRange(directory.SelectComponents());
            }

            return components;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var directory in Directories)
            {
                stringBuilder.Append(directory);
            }

            foreach (var component in Components)
            {
                stringBuilder.Append(component);
            }

            return string.Format(CultureInfo.InvariantCulture, DirectoryTemplate, Id, Name,
                stringBuilder.ToString());
        }
    }
}
