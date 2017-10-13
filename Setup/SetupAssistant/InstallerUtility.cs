using System;
using Microsoft.Build.Utilities;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;

namespace SetupAssistant
{
    [RunInstaller(true)]
    public partial class InstallerUtility : Installer
    {
        private const string NGenFileName = "ngen.exe";

        public InstallerUtility()
        {
            InitializeComponent();
        }
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            string frameworkDirectory = ToolLocationHelper.GetPathToDotNetFramework(
                TargetDotNetFrameworkVersion.Version40,
                Environment.Is64BitOperatingSystem
                    ? DotNetFrameworkArchitecture.Bitness64
                    : DotNetFrameworkArchitecture.Bitness32);


            string ngenPath = Path.Combine(frameworkDirectory, NGenFileName);

            if (!File.Exists(ngenPath))
                return;

            var process = new Process
            {
                StartInfo =
                {
                    FileName = ngenPath,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                }
            };

            string assemblyPath = Context.Parameters["assemblypath"];

            if (string.IsNullOrEmpty(assemblyPath))
                return;

            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);

            if (string.IsNullOrEmpty(assemblyDirectory))
                return;

            var dllPath = Path.Combine(assemblyDirectory, "WebMoney.Services.dll");

            if (!File.Exists(dllPath))
                return;

            if (Context.Parameters.ContainsKey("Precompile"))
            {
                var precompile = Context.Parameters["Precompile"];

                if (precompile.Equals("2"))
                    return;
            }

            process.StartInfo.Arguments = "install \"" + dllPath + "\"";

            process.Start();
            process.WaitForExit();
        }
    }
}
