using System;
using System.IO;
using System.Text;
using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms.Templates;

namespace Xml2WinForms.WrappersBuilder
{
    class Program
    {
        enum WrapperKind
        {
            TemplateWrapper,
            ValuesWrapper
        }

        static int Main(string[] args)
        {
            if (args.Length != 2)
                return 1;

            string sourceDirectory = args[0];
            string targetDirectory = args[1];

            var templateWrappersDirectory = Path.Combine(targetDirectory, "TemplateWrappers");
            var valuesWrappersDirectory = Path.Combine(targetDirectory, "ValuesWrappers");

            if (!Directory.Exists(templateWrappersDirectory))
                Directory.CreateDirectory(templateWrappersDirectory);

            if (!Directory.Exists(valuesWrappersDirectory))
                Directory.CreateDirectory(valuesWrappersDirectory);

            foreach (var filePath in Directory.GetFiles(sourceDirectory))
            {
                try
                {
                    var template = TemplateLoader.LoadSubmitFormTemplate(filePath);

                    if (null == template)
                    {
                        Console.WriteLine(@"Error loading -> " + Path.GetFileName(filePath));
                        continue;
                    }

                    // Template Wrapper
                    WriteTemplate(filePath, template, WrapperKind.TemplateWrapper,
                        templateWrappersDirectory);

                    // Values Wrapper
                    WriteTemplate(filePath, template, WrapperKind.ValuesWrapper,
                        valuesWrappersDirectory);
                }
                catch (Exception)
                {
                    Console.WriteLine(@"Error parsing -> " + Path.GetFileName(filePath));
                }
            }

            Console.WriteLine(@"Done");

            return 0;
        }

        private static void WriteTemplate(string sourceFilePath, SubmitFormTemplate<WMColumnTemplate> template, WrapperKind wrapperKind, string targetDirectory)
        {
            var className = Path.GetFileNameWithoutExtension(sourceFilePath);
            string wrapperCode;

            switch (wrapperKind)
            {
                case WrapperKind.TemplateWrapper:
                    className += "TemplateWrapper";
                    wrapperCode = TemplateWrapperBuilder.BuildClass(className, template);
                    break;
                case WrapperKind.ValuesWrapper:
                    className += "ValuesWrapper";
                    wrapperCode = ValuesWrapperBuilder.BuildClass(className, template);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(wrapperKind), wrapperKind, null);
            }

            File.WriteAllText(
                Path.Combine(targetDirectory, Path.ChangeExtension(className, "cs")),
                wrapperCode, Encoding.UTF8);
        }
    }
}
