using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Xml2WinForms.Templates;
using Xml2WinForms.Utils;

namespace Xml2WinForms
{
    public sealed partial class AboutBox : Form
    {
        #region Assembly Attribute Accessors

        private static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

                if (attributes.Length > 0)
                {
                    var titleAttribute = (AssemblyTitleAttribute)attributes[0];

                    if (!string.IsNullOrEmpty(titleAttribute.Title))
                        return titleAttribute.Title;
                }

                return Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().CodeBase);
            }
        }

        private static string AssemblyVersion
        {
            get
            {
                var fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
                return fileVersionInfo.FileVersion;
            }
        }

        private static string AssemblyDescription
        {
            get
            {
                var attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                return attributes.Length == 0 ? string.Empty : ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        private static string AssemblyCopyright
        {
            get
            {
                var attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                return attributes.Length == 0 ? string.Empty : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        private static string AssemblyCompany
        {
            get
            {
                var attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                return attributes.Length == 0 ? string.Empty : ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        #endregion

        public AboutBox()
        {
            if (ApplicationUtility.IsRunningOnMono)
                Font = new Font("Arial", 8);

            InitializeComponent();

            Text = String.Format(CultureInfo.InvariantCulture, Text, AssemblyTitle);
            titleLabel.Text = AssemblyTitle;
            versionValueLabel.Text = String.Format(CultureInfo.InvariantCulture, "{0}", AssemblyVersion);
            copyrightValueLabel.Text = AssemblyCopyright;
            companyNameValueLabel.Text = AssemblyCompany;
            descriptionRichTextBox.Text = AssemblyDescription;

            licenseValueLabel.Click += (sender, args) =>
            {
                var url = licenseValueLabel.Tag as string;

                if (null != url)
                    Process.Start(url);
            };

            logoPictureBox.Click += (sender, args) =>
            {
                var url = logoPictureBox.Tag as string;

                if (null != url)
                    Process.Start(url);
            };
        }

        public void ApplyTemplate(AboutBoxTemplate template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            Reset();

            if (null == template.License)
                throw new BadTemplateException("null == template.License");

            if (null == template.License.LicenseName)
                throw new BadTemplateException("null == template.License.LicenseName");

            if (null != template.Description)
                descriptionRichTextBox.Rtf = template.Description;

            SetLicense(template.License);

            if (null != template.Logo)
            {
                if (null == template.Logo.ImagePath || null == template.BaseDirectory)
                    throw new BadTemplateException("null == template.Logo.ImagePath || null == template.BaseDirectory");

                SetLogo(Image.FromFile(Path.Combine(template.BaseDirectory, template.Logo.ImagePath)));

                if (null != template.Logo.Url && CheckUrl(template.Logo.Url))
                {
                    logoPictureBox.Cursor = Cursors.Hand;
                    logoPictureBox.Tag = template.Logo.Url;
                }
            }
        }

        public void Reset()
        {
            descriptionRichTextBox.Rtf = string.Empty;

            // License
            licenseValueLabel.Text = string.Empty;
            licenseValueLabel.ResetForeColor();
            licenseValueLabel.ResetFont();
            licenseValueLabel.ResetCursor();
            licenseValueLabel.Tag = null;

            // Logo
            SetLogo(null);
            logoPictureBox.ResetCursor();
            logoPictureBox.Tag = null;
        }

        private void AboutBox_Load(object sender, EventArgs e)
        {
            // For Mono.
            var heightCorrection = mTableLayoutPanel.PreferredSize.Height + mPanel.Padding.Top + mPanel.Padding.Bottom -
                                   ClientSize.Height;

            if (heightCorrection > 0)
                descriptionRichTextBox.Height = descriptionRichTextBox.Height - heightCorrection;

            FormUtils.MoveToCenterParent(this);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SetLicense(LicenseTemplate license)
        {
            if (null == license)
                throw new ArgumentNullException(nameof(license));

            licenseValueLabel.Text = license.LicenseName;

            if (null != license.LicenseUrl && CheckUrl(license.LicenseUrl))
            {
                licenseValueLabel.ForeColor = Color.Blue;
                licenseValueLabel.Font = new Font(licenseValueLabel.Font, FontStyle.Underline);
                licenseValueLabel.Cursor = Cursors.Hand;
                licenseValueLabel.Tag = license.LicenseUrl;
            }
        }

        private void SetLogo(Image image)
        {
            ((ISupportInitialize)logoPictureBox).BeginInit();
            logoPictureBox.Image = image;
            ((ISupportInitialize)logoPictureBox).EndInit();
        }

        private bool CheckUrl(string value)
        {
            Uri uri;

            return Uri.TryCreate(value, UriKind.Absolute, out uri)
                   && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
