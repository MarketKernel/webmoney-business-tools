using System;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using Xml2WinForms.Templates;
using LocalizationAssistant;
using Xml2WinForms.Utils;

namespace Xml2WinForms
{
    public sealed partial class ErrorForm : Form
    {
        private readonly string _originalCaption;
        private readonly ErrorLevel _errorLevel;
        private readonly int _heightDifference;

        private bool _detailsVisible;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static Action<string, string, string> SupportAction { get; set; }

        public ErrorForm(ErrorFormTemplate errorFormTemplate)
        {
            if (null == errorFormTemplate)
                throw new ArgumentNullException(nameof(errorFormTemplate));

            if (ApplicationUtility.IsRunningOnMono)
                Font = new Font("Arial", 8);

            InitializeComponent();

            _originalCaption = errorFormTemplate.OriginalCaption ??
                               throw new BadTemplateException("null == errorFormTemplate.Caption");
            Text = errorFormTemplate.Caption;
            captionLabel.Text = errorFormTemplate.Caption;
            messageRichTextBox.Text = errorFormTemplate.Message ??
                                      throw new BadTemplateException("null == errorFormTemplate.Message");

            if (null != errorFormTemplate.Details)
                detailsRichTextBox.Text = errorFormTemplate.Details;

            _errorLevel = errorFormTemplate.Level;

            ((ISupportInitialize) iconPictureBox).BeginInit();
            switch (errorFormTemplate.Level)
            {
                case ErrorLevel.Error:
                    iconPictureBox.Image = iconImageList.Images[0];
                    break;
                case ErrorLevel.Warning:
                    iconPictureBox.Image = iconImageList.Images[1];
                    break;
            }
            ((ISupportInitialize) iconPictureBox).EndInit();

            _heightDifference = detailsRichTextBox.Height +
                                detailsRichTextBox.Margin.Top +
                                detailsRichTextBox.Margin.Bottom +
                                separatorLabel.Height +
                                separatorLabel.Margin.Top +
                                separatorLabel.Margin.Bottom +
                                reportButton.Height +
                                reportButton.Margin.Top +
                                reportButton.Margin.Bottom;

            HideDetails();
            _detailsVisible = false;

            if (null == errorFormTemplate.Details)
                detailsButton.Enabled = false;

            reportButton.Enabled = null != SupportAction;
        }

        public static void ShowDialog(IWin32Window window, ErrorFormTemplate errorFormTemplate)
        {
            if (null == errorFormTemplate)
                throw new ArgumentNullException(nameof(errorFormTemplate));

            var errorForm = new ErrorForm(errorFormTemplate);
            errorForm.ShowDialog(window);
        }

        private void errorForm_Load(object sender, EventArgs e)
        {
            FormUtils.MoveToCenterParent(this);

            switch (_errorLevel)
            {
                case ErrorLevel.Error:
                    SystemSounds.Hand.Play();
                    break;
                case ErrorLevel.Warning:
                    SystemSounds.Exclamation.Play();
                    break;
            }
        }

        private void detailsButton_Click(object sender, EventArgs e)
        {
            if (_detailsVisible)
            {
                HideDetails();
                _detailsVisible = false;
            }
            else
            {
                ShowDetails();
                _detailsVisible = true;
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            var dataObject = new DataObject();
            dataObject.SetText(messageRichTextBox.Text, TextDataFormat.UnicodeText);

            Clipboard.Clear();
            Clipboard.SetDataObject(dataObject);
        }

        private void reportButton_Click(object sender, EventArgs e)
        {
            detailsRichTextBox.Enabled = false;
            reportButton.Enabled = false;

            mToolStripProgressBar.Visible = true;

            mBackgroundWorker.RunWorkerAsync(new object[]
                {_originalCaption, messageRichTextBox.Text, detailsRichTextBox.Text});
        }

        private void mBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] parameters = (object[]) e.Argument;

            SupportAction?.Invoke(parameters[0] as string, parameters[1] as string, parameters[2] as string);
        }

        private void mBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IsDisposed)
                return;

            detailsRichTextBox.Enabled = true;
            reportButton.Enabled = true;

            mToolStripProgressBar.Visible = false;

            if (null != e.Error)
            {
                MessageBox.Show(this, e.Error.Message, Translator.Instance.Translate(nameof(ErrorForm), "Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            Close();
        }

        private void ShowDetails()
        {
            mTableLayoutPanel.SuspendLayout();

            detailsButton.Image = arrowImageList.Images[1];

            mTableLayoutPanel.Controls.Add(detailsRichTextBox, 0, 4);
            mTableLayoutPanel.SetColumnSpan(detailsRichTextBox, 4);

            mTableLayoutPanel.Controls.Add(separatorLabel, 0, 3);
            mTableLayoutPanel.SetColumnSpan(separatorLabel, 4);

            mTableLayoutPanel.Controls.Add(reportButton, 3, 5);

            Height += _heightDifference;

            mTableLayoutPanel.ResumeLayout();
        }

        private void HideDetails()
        {
            mTableLayoutPanel.SuspendLayout();

            detailsButton.Image = arrowImageList.Images[0];
            mTableLayoutPanel.Controls.Remove(detailsRichTextBox);
            mTableLayoutPanel.Controls.Remove(separatorLabel);
            mTableLayoutPanel.Controls.Remove(reportButton);

            Height -= _heightDifference;

            mTableLayoutPanel.ResumeLayout();
        }
    }
}