using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Properties;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.Forms
{
    internal sealed partial class ProgressForm : Form
    {
        private readonly IUnityContainer _container;
        private readonly ISession _session;

        public ProgressForm(IUnityContainer container, ISession session)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _session = session ?? throw new ArgumentNullException(nameof(session));
            InitializeComponent();
        }

        private void ConnectionForm_Load(object sender, EventArgs e)
        {
            mBackgroundWorker.RunWorkerAsync();
        }

        private void mBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var entranceService = _container.Resolve<IEntranceService>();
            entranceService.Handshake(_session);
        }

        private void mBackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (IsDisposed)
                return;

            var error = e.Error;

            if (null != error)
            {
                mProgressBar.Style = ProgressBarStyle.Blocks;
                mProgressBar.Value = 100;

                if (!ApplicationUtility.IsRunningOnMono)
                    SetState(mProgressBar, 2);

                string connectionString = string.Empty;
                var connectionSettings = _session.AuthenticationService.GetConnectionSettings();

                if (null != connectionSettings)
                    connectionString = connectionSettings.ConnectionString;

                string caption = Resources.ProgressForm_mBackgroundWorker_RunWorkerCompleted_Connection_error;
                string message =
                    string.Format(Resources.ProgressForm_mBackgroundWorker_RunWorkerCompleted_Could_not_connect_to_the_database_with_connection_string___0___, connectionString);

                var errorFormTemplate = new ErrorFormTemplate(caption, message)
                {
                    Details = error.ToString()
                };

                errorFormTemplate.SetTemplateInternals(ExtensionCatalog.Enter, null);

                ErrorForm.ShowDialog(this, errorFormTemplate);
                DialogResult = DialogResult.Cancel;
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void cancelLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr w, IntPtr l);

        private static void SetState(ProgressBar progressBar, int state)
        {
            SendMessage(progressBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }
    }
}
