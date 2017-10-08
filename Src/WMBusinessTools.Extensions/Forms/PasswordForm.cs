using System.ComponentModel;
using System.Windows.Forms;

namespace WMBusinessTools.Extensions.Forms
{
    internal sealed partial class PasswordForm : Form
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Password => passwordTextBox.Text;

        public PasswordForm()
        {
            InitializeComponent();
        }
    }
}
