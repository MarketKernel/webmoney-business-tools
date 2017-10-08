namespace Xml2WinForms
{
    sealed partial class AboutBox
    {
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
            this.mTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.copyrightFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.copyrightValueLabel = new System.Windows.Forms.Label();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.okButton = new System.Windows.Forms.Button();
            this.companyNameFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.companyNameLabel = new System.Windows.Forms.Label();
            this.companyNameValueLabel = new System.Windows.Forms.Label();
            this.warningLabel = new System.Windows.Forms.Label();
            this.descriptionRichTextBox = new System.Windows.Forms.RichTextBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.detailsLabel = new System.Windows.Forms.Label();
            this.versionFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.versionLabel = new System.Windows.Forms.Label();
            this.versionValueLabel = new System.Windows.Forms.Label();
            this.licenseFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.licenseLabel = new System.Windows.Forms.Label();
            this.licenseValueLabel = new System.Windows.Forms.Label();
            this.mPanel = new System.Windows.Forms.Panel();
            this.mTableLayoutPanel.SuspendLayout();
            this.copyrightFlowLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.companyNameFlowLayoutPanel.SuspendLayout();
            this.versionFlowLayoutPanel.SuspendLayout();
            this.licenseFlowLayoutPanel.SuspendLayout();
            this.mPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mTableLayoutPanel
            // 
            resources.ApplyResources(this.mTableLayoutPanel, "mTableLayoutPanel");
            this.mTableLayoutPanel.Controls.Add(this.copyrightFlowLayoutPanel, 0, 3);
            this.mTableLayoutPanel.Controls.Add(this.logoPictureBox, 1, 0);
            this.mTableLayoutPanel.Controls.Add(this.okButton, 0, 8);
            this.mTableLayoutPanel.Controls.Add(this.companyNameFlowLayoutPanel, 0, 2);
            this.mTableLayoutPanel.Controls.Add(this.warningLabel, 0, 7);
            this.mTableLayoutPanel.Controls.Add(this.descriptionRichTextBox, 0, 6);
            this.mTableLayoutPanel.Controls.Add(this.titleLabel, 0, 0);
            this.mTableLayoutPanel.Controls.Add(this.detailsLabel, 0, 5);
            this.mTableLayoutPanel.Controls.Add(this.versionFlowLayoutPanel, 0, 1);
            this.mTableLayoutPanel.Controls.Add(this.licenseFlowLayoutPanel, 0, 4);
            this.mTableLayoutPanel.Name = "mTableLayoutPanel";
            // 
            // copyrightFlowLayoutPanel
            // 
            resources.ApplyResources(this.copyrightFlowLayoutPanel, "copyrightFlowLayoutPanel");
            this.copyrightFlowLayoutPanel.Controls.Add(this.copyrightLabel);
            this.copyrightFlowLayoutPanel.Controls.Add(this.copyrightValueLabel);
            this.copyrightFlowLayoutPanel.Name = "copyrightFlowLayoutPanel";
            // 
            // copyrightLabel
            // 
            resources.ApplyResources(this.copyrightLabel, "copyrightLabel");
            this.copyrightLabel.ForeColor = System.Drawing.Color.DimGray;
            this.copyrightLabel.Name = "copyrightLabel";
            // 
            // copyrightValueLabel
            // 
            resources.ApplyResources(this.copyrightValueLabel, "copyrightValueLabel");
            this.copyrightValueLabel.ForeColor = System.Drawing.Color.DimGray;
            this.copyrightValueLabel.Name = "copyrightValueLabel";
            // 
            // logoPictureBox
            // 
            resources.ApplyResources(this.logoPictureBox, "logoPictureBox");
            this.logoPictureBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.logoPictureBox.Name = "logoPictureBox";
            this.mTableLayoutPanel.SetRowSpan(this.logoPictureBox, 6);
            this.logoPictureBox.TabStop = false;
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.mTableLayoutPanel.SetColumnSpan(this.okButton, 2);
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Name = "okButton";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // companyNameFlowLayoutPanel
            // 
            resources.ApplyResources(this.companyNameFlowLayoutPanel, "companyNameFlowLayoutPanel");
            this.companyNameFlowLayoutPanel.Controls.Add(this.companyNameLabel);
            this.companyNameFlowLayoutPanel.Controls.Add(this.companyNameValueLabel);
            this.companyNameFlowLayoutPanel.Name = "companyNameFlowLayoutPanel";
            // 
            // companyNameLabel
            // 
            resources.ApplyResources(this.companyNameLabel, "companyNameLabel");
            this.companyNameLabel.ForeColor = System.Drawing.Color.DimGray;
            this.companyNameLabel.Name = "companyNameLabel";
            // 
            // companyNameValueLabel
            // 
            resources.ApplyResources(this.companyNameValueLabel, "companyNameValueLabel");
            this.companyNameValueLabel.ForeColor = System.Drawing.Color.DimGray;
            this.companyNameValueLabel.Name = "companyNameValueLabel";
            // 
            // warningLabel
            // 
            resources.ApplyResources(this.warningLabel, "warningLabel");
            this.mTableLayoutPanel.SetColumnSpan(this.warningLabel, 2);
            this.warningLabel.ForeColor = System.Drawing.Color.DimGray;
            this.warningLabel.Name = "warningLabel";
            // 
            // descriptionRichTextBox
            // 
            resources.ApplyResources(this.descriptionRichTextBox, "descriptionRichTextBox");
            this.mTableLayoutPanel.SetColumnSpan(this.descriptionRichTextBox, 2);
            this.descriptionRichTextBox.Name = "descriptionRichTextBox";
            this.descriptionRichTextBox.ReadOnly = true;
            // 
            // titleLabel
            // 
            resources.ApplyResources(this.titleLabel, "titleLabel");
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.titleLabel.Name = "titleLabel";
            // 
            // detailsLabel
            // 
            resources.ApplyResources(this.detailsLabel, "detailsLabel");
            this.detailsLabel.ForeColor = System.Drawing.Color.DimGray;
            this.detailsLabel.Name = "detailsLabel";
            // 
            // versionFlowLayoutPanel
            // 
            resources.ApplyResources(this.versionFlowLayoutPanel, "versionFlowLayoutPanel");
            this.versionFlowLayoutPanel.Controls.Add(this.versionLabel);
            this.versionFlowLayoutPanel.Controls.Add(this.versionValueLabel);
            this.versionFlowLayoutPanel.Name = "versionFlowLayoutPanel";
            // 
            // versionLabel
            // 
            resources.ApplyResources(this.versionLabel, "versionLabel");
            this.versionLabel.ForeColor = System.Drawing.Color.DimGray;
            this.versionLabel.Name = "versionLabel";
            // 
            // versionValueLabel
            // 
            resources.ApplyResources(this.versionValueLabel, "versionValueLabel");
            this.versionValueLabel.ForeColor = System.Drawing.Color.DimGray;
            this.versionValueLabel.Name = "versionValueLabel";
            // 
            // licenseFlowLayoutPanel
            // 
            resources.ApplyResources(this.licenseFlowLayoutPanel, "licenseFlowLayoutPanel");
            this.licenseFlowLayoutPanel.Controls.Add(this.licenseLabel);
            this.licenseFlowLayoutPanel.Controls.Add(this.licenseValueLabel);
            this.licenseFlowLayoutPanel.Name = "licenseFlowLayoutPanel";
            // 
            // licenseLabel
            // 
            resources.ApplyResources(this.licenseLabel, "licenseLabel");
            this.licenseLabel.ForeColor = System.Drawing.Color.DimGray;
            this.licenseLabel.Name = "licenseLabel";
            // 
            // licenseValueLabel
            // 
            resources.ApplyResources(this.licenseValueLabel, "licenseValueLabel");
            this.licenseValueLabel.ForeColor = System.Drawing.Color.DimGray;
            this.licenseValueLabel.Name = "licenseValueLabel";
            // 
            // mPanel
            // 
            resources.ApplyResources(this.mPanel, "mPanel");
            this.mPanel.Controls.Add(this.mTableLayoutPanel);
            this.mPanel.Name = "mPanel";
            // 
            // AboutBox
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.mPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.AboutBox_Load);
            this.mTableLayoutPanel.ResumeLayout(false);
            this.mTableLayoutPanel.PerformLayout();
            this.copyrightFlowLayoutPanel.ResumeLayout(false);
            this.copyrightFlowLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.companyNameFlowLayoutPanel.ResumeLayout(false);
            this.companyNameFlowLayoutPanel.PerformLayout();
            this.versionFlowLayoutPanel.ResumeLayout(false);
            this.versionFlowLayoutPanel.PerformLayout();
            this.licenseFlowLayoutPanel.ResumeLayout(false);
            this.licenseFlowLayoutPanel.PerformLayout();
            this.mPanel.ResumeLayout(false);
            this.mPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mTableLayoutPanel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label versionValueLabel;
        private System.Windows.Forms.Label copyrightValueLabel;
        private System.Windows.Forms.Label companyNameValueLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label licenseValueLabel;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.RichTextBox descriptionRichTextBox;
        private System.Windows.Forms.Label detailsLabel;
        private System.Windows.Forms.FlowLayoutPanel versionFlowLayoutPanel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.FlowLayoutPanel copyrightFlowLayoutPanel;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.FlowLayoutPanel companyNameFlowLayoutPanel;
        private System.Windows.Forms.Label companyNameLabel;
        private System.Windows.Forms.FlowLayoutPanel licenseFlowLayoutPanel;
        private System.Windows.Forms.Label licenseLabel;
        private System.Windows.Forms.Panel mPanel;
    }
}
