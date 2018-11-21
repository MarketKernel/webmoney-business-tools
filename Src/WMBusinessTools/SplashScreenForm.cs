using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace WMBusinessTools
{
    public partial class SplashScreenForm : Form
    {
        public SplashScreenForm()
        {
            InitializeComponent();

            var fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            var version = Version.Parse(fileVersionInfo.FileVersion);

            wmbt2Label.Text = string.Format(CultureInfo.InvariantCulture, wmbt2Label.Text,
                $"{version.Major}.{version.Minor}");
        }

        private void SplashScreenForm_DoubleClick(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        public void SafeClose()
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(SafeClose));
            else
                Application.ExitThread();
        }

        private void mTimer_Tick(object sender, EventArgs e)
        {
            var opacity = Opacity;

            if (opacity < 1)
                opacity += 0.05;
            else
                return;

            if (opacity > 1)
                opacity = 1;

            Opacity = opacity;
        }

        private void SplashScreenForm_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void copyrightLinkLabel_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.marketkernel.com/");
        }
    }
}
