using System;
using System.Drawing;
using System.Windows.Forms;
using Xml2WinForms.Utils;

namespace Xml2WinForms.Sandbox
{
    public sealed partial class DbSettingsForm : Form
    {
        public DbSettingsForm()
        {
            if (ApplicationUtility.IsRunningOnMono)
                Font = new Font("Arial", 8);

            InitializeComponent();
        }

        private void usePasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            passwordTextBox.Enabled = usePasswordCheckBox.Checked;
            confirmPasswordTextBox.Enabled = usePasswordCheckBox.Checked;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {

        }

        private void okButton_Click(object sender, EventArgs e)
        {

        }
    }
}
