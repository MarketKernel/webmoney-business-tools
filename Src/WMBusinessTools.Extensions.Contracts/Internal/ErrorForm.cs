using System;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using WMBusinessTools.Extensions.Contracts.Contexts;

namespace WMBusinessTools.Extensions.Contracts.Internal
{
    internal sealed partial class ErrorForm : Form
    {
        private readonly ErrorContext.ErrorLevel _errorLevel;
        private readonly int _heightDifference;

        private bool _detailsVisible;

        public ErrorForm(ErrorContext errorContext)
        {
            if (null == errorContext)
                throw new ArgumentNullException(nameof(errorContext));

            if (null != Type.GetType("Mono.Runtime"))
                Font = new Font("Arial", 8);

            InitializeComponent();

            Text = errorContext.Caption ?? string.Empty;
            captionLabel.Text = errorContext.Caption ?? string.Empty;
            messageRichTextBox.Text = errorContext.Message ?? string.Empty;

            if (null != errorContext.Details)
                detailsRichTextBox.Text = errorContext.Details;

            _errorLevel = errorContext.Level;

            ((ISupportInitialize)iconPictureBox).BeginInit();
            switch (errorContext.Level)
            {
                case ErrorContext.ErrorLevel.Error:
                    iconPictureBox.Image = iconImageList.Images[0];
                    break;
                case ErrorContext.ErrorLevel.Warning:
                    iconPictureBox.Image = iconImageList.Images[1];
                    break;
            }
            ((ISupportInitialize)iconPictureBox).EndInit();

            _heightDifference = detailsRichTextBox.Height +
                                detailsRichTextBox.Margin.Top +
                                detailsRichTextBox.Margin.Bottom +
                                separatorLabel.Height +
                                separatorLabel.Margin.Top +
                                separatorLabel.Margin.Bottom;

            HideDetails();
            _detailsVisible = false;

            if (null == errorContext.Details)
                detailsButton.Enabled = false;
        }
        
        private void errorForm_Load(object sender, EventArgs e)
        {
            switch (_errorLevel)
            {
                case ErrorContext.ErrorLevel.Error:
                    SystemSounds.Hand.Play();
                    break;
                case ErrorContext.ErrorLevel.Warning:
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

        private void ShowDetails()
        {
            mTableLayoutPanel.SuspendLayout();

            detailsButton.Image = arrowImageList.Images[1];

            mTableLayoutPanel.Controls.Add(detailsRichTextBox, 0, 4);
            mTableLayoutPanel.SetColumnSpan(detailsRichTextBox, 4);

            mTableLayoutPanel.Controls.Add(separatorLabel, 0, 3);
            mTableLayoutPanel.SetColumnSpan(separatorLabel, 4);

            Height += _heightDifference;

            mTableLayoutPanel.ResumeLayout();
        }

        private void HideDetails()
        {
            mTableLayoutPanel.SuspendLayout();

            detailsButton.Image = arrowImageList.Images[0];
            mTableLayoutPanel.Controls.Remove(detailsRichTextBox);
            mTableLayoutPanel.Controls.Remove(separatorLabel);

            Height -= _heightDifference;

            mTableLayoutPanel.ResumeLayout();
        }
    }
}