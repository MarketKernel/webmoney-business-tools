using System.Windows.Forms;

namespace ServicesFormExtension
{
    public partial class Form1 : Form
    {
        public Form1(string info)
        {
            InitializeComponent();

            richTextBox1.Text = info;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
