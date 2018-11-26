using System;
using System.IO;
using System.Xml.Linq;

namespace WiXBuilder
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: templatesDirectory binDirectory outputDirectory");
                return 1;
            }

            string templatesDirectory = args[0];
            string binDirectory = args[1];
            string outputDirectory = args[2];

            var directory = Directory.Create(binDirectory, binDirectory);

            var componentRefsXDocument = directory.BuildComponentRefsXDocument();
            var directoryXDocument = directory.BuildDirectoryXDocument();

            var productXDocument = XDocument.Load(Path.Combine(templatesDirectory, "Product.wxs"));
            var componentsXDocument = XDocument.Load(Path.Combine(templatesDirectory, "Components.wxs"));

            XNamespace wi = "http://schemas.microsoft.com/wix/2006/wi";

            var installDirectoryElement = componentsXDocument.Root?.Element(wi + "Fragment")?.Element(wi + "Directory")
                ?.Element(wi + "Directory")?.Element(wi + "Directory")?.Element(wi + "Directory");

            if (null == installDirectoryElement)
                throw new InvalidOperationException("null == installDirectoryElement");

            var featureElement = productXDocument.Root?.Element(wi + "Product")?.Element(wi + "Feature");

            if (null == featureElement)
                throw new InvalidOperationException("null == featureElement");

            installDirectoryElement.Add(directoryXDocument.Root?.Element(wi + "Directory")?.Elements());
            featureElement.Add(componentRefsXDocument.Root?.Elements());

            productXDocument.Save(Path.Combine(outputDirectory, "Product.wxs"));
            componentsXDocument.Save(Path.Combine(outputDirectory, "Components.wxs"));

            return 0;
        }
    }
}
