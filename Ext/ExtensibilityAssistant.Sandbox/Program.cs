using System;

namespace ExtensibilityAssistant.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var extensionManager = new ExtensionManager(AppDomain.CurrentDomain.BaseDirectory);

            var res = extensionManager.TryObtainExtensionConfiguration("Test");
        }
    }
}
