using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WMBusinessTools.Properties;
using WMBusinessTools.Utils;

namespace WMBusinessTools
{
    public partial class InitializationForm : Form
    {
        private readonly bool _omitPrecompilation;
        private int _currentStep = 1;
        private bool _completed;

        public InitializationForm(bool omitPrecompilation = false)
        {
            InitializeComponent();

            _omitPrecompilation = omitPrecompilation;
        }

        private void InitializationForm_Load(object sender, EventArgs e)
        {
            SwitchTabIndex();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            _currentStep--;
            SwitchTabIndex();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            _currentStep++;
            SwitchTabIndex();
        }

        private void mTabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (_currentStep != mTabControl.SelectedIndex + 1)
                e.Cancel = true;
        }

        private void notAcceptAgreementRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            nextButton.Enabled = !notAcceptAgreementRadioButton.Checked;
        }

        private void precompileRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            mTabControl.SuspendLayout();

            if (precompileRadioButton.Checked)
            {
                nextButton.Text = Resources.InitializationForm_precompileRadioButton_CheckedChanged__Next__;
                nextButton.Image = Resources.RunAsAdministrator;

                if (!mTabControl.TabPages.Contains(precompileTabPage))
                    mTabControl.TabPages.Add(precompileTabPage);
            }
            else
            {
                nextButton.Text = Resources.InitializationForm_precompileRadioButton_CheckedChanged__Finish;
                nextButton.Image = null;

                if (mTabControl.TabPages.Contains(precompileTabPage))
                    mTabControl.TabPages.Remove(precompileTabPage);
            }

            mTabControl.ResumeLayout();
        }

        private void precompileTimer_Tick(object sender, EventArgs e)
        {
            if (_completed)
            {
                if (precompileProgressBar.Value == precompileProgressBar.Maximum)
                {
                    precompileTimer.Enabled = false;
                    Success();
                }
                else
                    precompileProgressBar.Value = precompileProgressBar.Maximum;

                return;
            }

            if (precompileProgressBar.Value > precompileProgressBar.Maximum - precompileProgressBar.Maximum / 10)
                return;

            precompileProgressBar.Value++;
        }

        private void mBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var tempBatchFilePath = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetRandomFileName(), "cmd"));

            string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebMoney.Services.dll");

            var batchScript = string.Format(CultureInfo.InvariantCulture, Resources.PrecompileBatchScriptTemplate,
                dllPath);

            File.WriteAllText(tempBatchFilePath, batchScript, new UTF8Encoding(false));

            var startInfo = new ProcessStartInfo("cmd.exe")
            {
                CreateNoWindow = true,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                Verb = "runas",
                Arguments = "/c call \"" + tempBatchFilePath + "\""
            };

            Process process = null;

            int exitCode;

            try
            {
                process = Process.Start(startInfo);

                if (null == process)
                    throw new InvalidOperationException("null == process");

                process.WaitForExit();
                exitCode = process.ExitCode;
            }
            finally
            {
                process?.Dispose();
                File.Delete(tempBatchFilePath);
            }
            
            if (0 != exitCode)
                throw new PrecompilationException(exitCode);
        }

        private void mBackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (IsDisposed)
                return;

            var exception = e.Error;

            if (null != exception)
            {
                precompileTimer.Enabled = false;
                precompileProgressBar.Value = 0;

                string message;

                if (exception is PrecompilationException precompilationException)
                    message = $"{exception.Message} Exit code {precompilationException.ErrorNumber}.";
                else
                    message = exception.Message;

                MessageBox.Show(this, message, Resources.InitializationForm_mBackgroundWorker_RunWorkerCompleted_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);

                backButton_Click(this, null);
                return;
            }

            _completed = true;
        }

        private void SwitchTabIndex()
        {
            switch (_currentStep)
            {
                case 1:
                    if (_omitPrecompilation)
                    {
                        if (mTabControl.TabPages.Contains(settingsTabPage))
                            mTabControl.TabPages.Remove(settingsTabPage);

                        if (mTabControl.TabPages.Contains(precompileTabPage))
                            mTabControl.TabPages.Remove(precompileTabPage);
                    }

                    backButton.Visible = false;
                    nextButton.Text = _omitPrecompilation
                        ? Resources.InitializationForm_precompileRadioButton_CheckedChanged__Finish
                        : Resources.InitializationForm_precompileRadioButton_CheckedChanged__Next__;
                    nextButton.Image = null;
                    break;
                case 2:
                    if (_omitPrecompilation)
                    {
                        Success();
                        return;
                    }

                    backButton.Visible = true;
                    nextButton.Enabled = true;
                    precompileRadioButton_CheckedChanged(this, null);
                    break;
                case 3:
                    if (precompileRadioButton.Checked)
                    {
                        backButton.Visible = false;
                        nextButton.Enabled = false;
                        nextButton.Text = Resources.InitializationForm_precompileRadioButton_CheckedChanged__Finish;
                        nextButton.Image = null;

                        precompileTimer.Enabled = true;
                        mBackgroundWorker.RunWorkerAsync();
                    }
                    else
                    {
                        Success();
                        return;
                    }
                    break;
            }

            int tabPageIndex = _currentStep - 1;
            mTabControl.SelectedIndex = tabPageIndex;
        }

        private void Success()
        {
            var installationReference = InitializationUtility.BuildInstallationReference();
            string successUrl = string.Format(CultureInfo.InvariantCulture, Resources.SuccessUrlTemplate,
                installationReference);

            try
            {
                Process.Start(successUrl);
            }
            catch (Exception exception)
            {
                Debug.Write(exception);
            }

            Settings.Default.InstallationReference = installationReference;
            Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }
    }
}
