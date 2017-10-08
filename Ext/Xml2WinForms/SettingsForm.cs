using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Xml2WinForms.Utils;

namespace Xml2WinForms
{
    public sealed partial class SettingsForm : Form
    {
        [Category("Behavior"), Description("Sets the currently selected object that the grid will browse.")]
        public object SelectedObject
        {
            get => mPropertyGrid.SelectedObject;
            set => mPropertyGrid.SelectedObject = value;
        }

        public SettingsForm(string caption)
        {
            if (ApplicationUtility.IsRunningOnMono)
                Font = new Font("Arial", 8);

            InitializeComponent();

            if (!string.IsNullOrEmpty(caption))
                Text = caption;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            FormUtils.MoveToCenterParent(this);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}