using System;
using System.Windows.Forms;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace SimpleTabExtension
{
    public partial class UserControl1 : UserControl
    {
        private readonly ISession _session;

        public UserControl1(ISession session)
        {
            InitializeComponent();

            _session = session;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Текущий WMID " + _session.CurrentIdentifier.ToString("000000000000"));


            if (_session.AuthenticationService.AuthenticationMethod == AuthenticationMethod.KeeperClassic)
            {
                string sign = _session.AuthenticationService.Sign("test");
                MessageBox.Show("Подпись строки 'test' " + sign);
            }
            else
            {
                MessageBox.Show("Сертификат Thumbprint=" + _session.AuthenticationService.GetCertificate().Thumbprint);
            }

            if (_session.AuthenticationService.HasConnectionSettings)
            {
                MessageBox.Show("Строка подключения=" + _session.AuthenticationService.GetConnectionSettings()
                                    .ConnectionString);
            }
        }
    }
}
