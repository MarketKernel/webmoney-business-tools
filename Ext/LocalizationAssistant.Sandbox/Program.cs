using System;
using System.Globalization;
using System.Threading;

namespace LocalizationAssistant.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Translator.Instance.Translate("cat1", "test1"));
            Console.WriteLine(Translator.Instance.Translate("cat1", "test2"));
            Console.WriteLine(Translator.Instance.Translate("cat2", "test"));

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en");

            var translator = new Translator();
            translator.Apply();

            Console.WriteLine(Translator.Instance.Translate("cat1", "test1"));
            Console.WriteLine(Translator.Instance.Translate("cat1", "test2"));
            Console.WriteLine(Translator.Instance.Translate("cat2", "test"));

            Translator.Instance.Save();
        }
    }
}
