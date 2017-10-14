using System.Windows.Forms;

namespace SimpleFormExtension
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
