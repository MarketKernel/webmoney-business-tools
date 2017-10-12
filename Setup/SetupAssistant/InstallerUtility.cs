using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace SetupAssistant
{
    [RunInstaller(true)]
    public partial class InstallerUtility : Installer
    {
        public InstallerUtility()
        {
            InitializeComponent();
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);

            string runtimeDirectory = RuntimeEnvironment.GetRuntimeDirectory();
            string ngenPath = Path.Combine(runtimeDirectory, "ngen.exe");

            var process = new Process
            {
                StartInfo =
                {
                    FileName = ngenPath
                }
            };

            string assemblyPath = Context.Parameters["assemblypath"];
            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);
            var dllPath = Path.Combine(assemblyDirectory, "WebMoney.Services.dll");

            process.StartInfo.Arguments = "install \"" + dllPath + "\"";

            process.Start();
            process.WaitForExit();
        }
    }
}
