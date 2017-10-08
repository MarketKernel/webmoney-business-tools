namespace WMBusinessTools.Extensions.Forms
{
    partial class CertificateForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CertificateForm));
            this.mTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.commandBarGroupBox = new System.Windows.Forms.GroupBox();
            this.commandBarFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.mTabControl = new System.Windows.Forms.TabControl();
            this.generalInfoTabPage = new System.Windows.Forms.TabPage();
            this.generalInfoTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.aboutLabel = new System.Windows.Forms.Label();
            this.wmIdTextBox = new System.Windows.Forms.TextBox();
            this.certificateTextBox = new System.Windows.Forms.TextBox();
            this.levelsTextBox = new System.Windows.Forms.TextBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.contactsTextBox = new System.Windows.Forms.TextBox();
            this.claimsTextBox = new System.Windows.Forms.TextBox();
            this.aboutTextBox = new System.Windows.Forms.TextBox();
            this.wmidLabel = new System.Windows.Forms.Label();
            this.certificateLabel = new System.Windows.Forms.Label();
            this.levelsLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.addressLabel = new System.Windows.Forms.Label();
            this.contactsLabel = new System.Windows.Forms.Label();
            this.claimsLabel = new System.Windows.Forms.Label();
            this.certificateDataTabPage = new System.Windows.Forms.TabPage();
            this.certificateTunableList = new Xml2WinForms.TunableList();
            this.wmIdsTabPage = new System.Windows.Forms.TabPage();
            this.attachedIdentifierTunableList = new Xml2WinForms.TunableList();
            this.bottomFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.mPictureBox = new System.Windows.Forms.PictureBox();
            this.certificateImageList = new System.Windows.Forms.ImageList(this.components);
            this.mTableLayoutPanel.SuspendLayout();
            this.commandBarGroupBox.SuspendLayout();
            this.mTabControl.SuspendLayout();
            this.generalInfoTabPage.SuspendLayout();
            this.generalInfoTableLayoutPanel.SuspendLayout();
            this.certificateDataTabPage.SuspendLayout();
            this.wmIdsTabPage.SuspendLayout();
            this.bottomFlowLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mTableLayoutPanel
            // 
            resources.ApplyResources(this.mTableLayoutPanel, "mTableLayoutPanel");
            this.mTableLayoutPanel.Controls.Add(this.commandBarGroupBox, 2, 0);
            this.mTableLayoutPanel.Controls.Add(this.mTabControl, 1, 0);
            this.mTableLayoutPanel.Controls.Add(this.bottomFlowLayoutPanel, 0, 1);
            this.mTableLayoutPanel.Controls.Add(this.mPictureBox, 0, 0);
            this.mTableLayoutPanel.Name = "mTableLayoutPanel";
            // 
            // commandBarGroupBox
            // 
            resources.ApplyResources(this.commandBarGroupBox, "commandBarGroupBox");
            this.commandBarGroupBox.Controls.Add(this.commandBarFlowLayoutPanel);
            this.commandBarGroupBox.Name = "commandBarGroupBox";
            this.commandBarGroupBox.TabStop = false;
            // 
            // commandBarFlowLayoutPanel
            // 
            resources.ApplyResources(this.commandBarFlowLayoutPanel, "commandBarFlowLayoutPanel");
            this.commandBarFlowLayoutPanel.Name = "commandBarFlowLayoutPanel";
            // 
            // mTabControl
            // 
            resources.ApplyResources(this.mTabControl, "mTabControl");
            this.mTabControl.Controls.Add(this.generalInfoTabPage);
            this.mTabControl.Controls.Add(this.certificateDataTabPage);
            this.mTabControl.Controls.Add(this.wmIdsTabPage);
            this.mTabControl.Name = "mTabControl";
            this.mTabControl.SelectedIndex = 0;
            // 
            // generalInfoTabPage
            // 
            resources.ApplyResources(this.generalInfoTabPage, "generalInfoTabPage");
            this.generalInfoTabPage.Controls.Add(this.generalInfoTableLayoutPanel);
            this.generalInfoTabPage.Name = "generalInfoTabPage";
            this.generalInfoTabPage.UseVisualStyleBackColor = true;
            // 
            // generalInfoTableLayoutPanel
            // 
            resources.ApplyResources(this.generalInfoTableLayoutPanel, "generalInfoTableLayoutPanel");
            this.generalInfoTableLayoutPanel.Controls.Add(this.aboutLabel, 0, 7);
            this.generalInfoTableLayoutPanel.Controls.Add(this.wmIdTextBox, 1, 0);
            this.generalInfoTableLayoutPanel.Controls.Add(this.certificateTextBox, 1, 1);
            this.generalInfoTableLayoutPanel.Controls.Add(this.levelsTextBox, 1, 2);
            this.generalInfoTableLayoutPanel.Controls.Add(this.nameTextBox, 1, 3);
            this.generalInfoTableLayoutPanel.Controls.Add(this.addressTextBox, 1, 4);
            this.generalInfoTableLayoutPanel.Controls.Add(this.contactsTextBox, 1, 5);
            this.generalInfoTableLayoutPanel.Controls.Add(this.claimsTextBox, 1, 6);
            this.generalInfoTableLayoutPanel.Controls.Add(this.aboutTextBox, 1, 7);
            this.generalInfoTableLayoutPanel.Controls.Add(this.wmidLabel, 0, 0);
            this.generalInfoTableLayoutPanel.Controls.Add(this.certificateLabel, 0, 1);
            this.generalInfoTableLayoutPanel.Controls.Add(this.levelsLabel, 0, 2);
            this.generalInfoTableLayoutPanel.Controls.Add(this.nameLabel, 0, 3);
            this.generalInfoTableLayoutPanel.Controls.Add(this.addressLabel, 0, 4);
            this.generalInfoTableLayoutPanel.Controls.Add(this.contactsLabel, 0, 5);
            this.generalInfoTableLayoutPanel.Controls.Add(this.claimsLabel, 0, 6);
            this.generalInfoTableLayoutPanel.Name = "generalInfoTableLayoutPanel";
            // 
            // aboutLabel
            // 
            resources.ApplyResources(this.aboutLabel, "aboutLabel");
            this.aboutLabel.Name = "aboutLabel";
            // 
            // wmIdTextBox
            // 
            resources.ApplyResources(this.wmIdTextBox, "wmIdTextBox");
            this.wmIdTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.wmIdTextBox.Name = "wmIdTextBox";
            this.wmIdTextBox.ReadOnly = true;
            // 
            // certificateTextBox
            // 
            resources.ApplyResources(this.certificateTextBox, "certificateTextBox");
            this.certificateTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.certificateTextBox.Name = "certificateTextBox";
            this.certificateTextBox.ReadOnly = true;
            // 
            // levelsTextBox
            // 
            resources.ApplyResources(this.levelsTextBox, "levelsTextBox");
            this.levelsTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.levelsTextBox.Name = "levelsTextBox";
            this.levelsTextBox.ReadOnly = true;
            // 
            // nameTextBox
            // 
            resources.ApplyResources(this.nameTextBox, "nameTextBox");
            this.nameTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.ReadOnly = true;
            // 
            // addressTextBox
            // 
            resources.ApplyResources(this.addressTextBox, "addressTextBox");
            this.addressTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.ReadOnly = true;
            // 
            // contactsTextBox
            // 
            resources.ApplyResources(this.contactsTextBox, "contactsTextBox");
            this.contactsTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.contactsTextBox.Name = "contactsTextBox";
            this.contactsTextBox.ReadOnly = true;
            // 
            // claimsTextBox
            // 
            resources.ApplyResources(this.claimsTextBox, "claimsTextBox");
            this.claimsTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.claimsTextBox.Name = "claimsTextBox";
            this.claimsTextBox.ReadOnly = true;
            // 
            // aboutTextBox
            // 
            resources.ApplyResources(this.aboutTextBox, "aboutTextBox");
            this.aboutTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.aboutTextBox.Name = "aboutTextBox";
            this.aboutTextBox.ReadOnly = true;
            // 
            // wmidLabel
            // 
            resources.ApplyResources(this.wmidLabel, "wmidLabel");
            this.wmidLabel.Name = "wmidLabel";
            // 
            // certificateLabel
            // 
            resources.ApplyResources(this.certificateLabel, "certificateLabel");
            this.certificateLabel.Name = "certificateLabel";
            // 
            // levelsLabel
            // 
            resources.ApplyResources(this.levelsLabel, "levelsLabel");
            this.levelsLabel.Name = "levelsLabel";
            // 
            // nameLabel
            // 
            resources.ApplyResources(this.nameLabel, "nameLabel");
            this.nameLabel.Name = "nameLabel";
            // 
            // addressLabel
            // 
            resources.ApplyResources(this.addressLabel, "addressLabel");
            this.addressLabel.Name = "addressLabel";
            // 
            // contactsLabel
            // 
            resources.ApplyResources(this.contactsLabel, "contactsLabel");
            this.contactsLabel.Name = "contactsLabel";
            // 
            // claimsLabel
            // 
            resources.ApplyResources(this.claimsLabel, "claimsLabel");
            this.claimsLabel.Name = "claimsLabel";
            // 
            // certificateDataTabPage
            // 
            resources.ApplyResources(this.certificateDataTabPage, "certificateDataTabPage");
            this.certificateDataTabPage.Controls.Add(this.certificateTunableList);
            this.certificateDataTabPage.Name = "certificateDataTabPage";
            this.certificateDataTabPage.UseVisualStyleBackColor = true;
            // 
            // certificateTunableList
            // 
            resources.ApplyResources(this.certificateTunableList, "certificateTunableList");
            this.certificateTunableList.FullRowSelect = true;
            this.certificateTunableList.HideSelection = false;
            this.certificateTunableList.MultiSelect = false;
            this.certificateTunableList.Name = "certificateTunableList";
            this.certificateTunableList.UseCompatibleStateImageBehavior = false;
            this.certificateTunableList.View = System.Windows.Forms.View.Details;
            // 
            // wmIdsTabPage
            // 
            resources.ApplyResources(this.wmIdsTabPage, "wmIdsTabPage");
            this.wmIdsTabPage.Controls.Add(this.attachedIdentifierTunableList);
            this.wmIdsTabPage.Name = "wmIdsTabPage";
            this.wmIdsTabPage.UseVisualStyleBackColor = true;
            // 
            // attachedIdentifierTunableList
            // 
            resources.ApplyResources(this.attachedIdentifierTunableList, "attachedIdentifierTunableList");
            this.attachedIdentifierTunableList.FullRowSelect = true;
            this.attachedIdentifierTunableList.HideSelection = false;
            this.attachedIdentifierTunableList.MultiSelect = false;
            this.attachedIdentifierTunableList.Name = "attachedIdentifierTunableList";
            this.attachedIdentifierTunableList.UseCompatibleStateImageBehavior = false;
            this.attachedIdentifierTunableList.View = System.Windows.Forms.View.Details;
            // 
            // bottomFlowLayoutPanel
            // 
            resources.ApplyResources(this.bottomFlowLayoutPanel, "bottomFlowLayoutPanel");
            this.bottomFlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mTableLayoutPanel.SetColumnSpan(this.bottomFlowLayoutPanel, 3);
            this.bottomFlowLayoutPanel.Controls.Add(this.cancelButton);
            this.bottomFlowLayoutPanel.Controls.Add(this.okButton);
            this.bottomFlowLayoutPanel.Name = "bottomFlowLayoutPanel";
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // mPictureBox
            // 
            resources.ApplyResources(this.mPictureBox, "mPictureBox");
            this.mPictureBox.Name = "mPictureBox";
            this.mPictureBox.TabStop = false;
            // 
            // certificateImageList
            // 
            this.certificateImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("certificateImageList.ImageStream")));
            this.certificateImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.certificateImageList.Images.SetKeyName(0, "Alias");
            this.certificateImageList.Images.SetKeyName(1, "Formal");
            this.certificateImageList.Images.SetKeyName(2, "Initial");
            this.certificateImageList.Images.SetKeyName(3, "Personal");
            this.certificateImageList.Images.SetKeyName(4, "Merchant");
            this.certificateImageList.Images.SetKeyName(5, "Capitaller");
            this.certificateImageList.Images.SetKeyName(6, "Developer");
            this.certificateImageList.Images.SetKeyName(7, "Registrar");
            this.certificateImageList.Images.SetKeyName(8, "Guarantor");
            this.certificateImageList.Images.SetKeyName(9, "Service");
            this.certificateImageList.Images.SetKeyName(10, "Operator");
            this.certificateImageList.Images.SetKeyName(11, "Cashier");
            this.certificateImageList.Images.SetKeyName(12, "AliasRevoked");
            this.certificateImageList.Images.SetKeyName(13, "FormalRevoked");
            this.certificateImageList.Images.SetKeyName(14, "InitialRevoked");
            this.certificateImageList.Images.SetKeyName(15, "PersonalRevoked");
            this.certificateImageList.Images.SetKeyName(16, "MerchantRevoked");
            this.certificateImageList.Images.SetKeyName(17, "CapitallerRevoked");
            this.certificateImageList.Images.SetKeyName(18, "DeveloperRevoked");
            this.certificateImageList.Images.SetKeyName(19, "RegistrarRevoked");
            this.certificateImageList.Images.SetKeyName(20, "GuarantorRevoked");
            this.certificateImageList.Images.SetKeyName(21, "ServiceRevoked");
            this.certificateImageList.Images.SetKeyName(22, "OperatorRevoked");
            this.certificateImageList.Images.SetKeyName(23, "CashierRevoked");
            // 
            // CertificateForm
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.mTableLayoutPanel);
            this.Name = "CertificateForm";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.CertificateForm_Load);
            this.mTableLayoutPanel.ResumeLayout(false);
            this.mTableLayoutPanel.PerformLayout();
            this.commandBarGroupBox.ResumeLayout(false);
            this.commandBarGroupBox.PerformLayout();
            this.mTabControl.ResumeLayout(false);
            this.generalInfoTabPage.ResumeLayout(false);
            this.generalInfoTableLayoutPanel.ResumeLayout(false);
            this.generalInfoTableLayoutPanel.PerformLayout();
            this.certificateDataTabPage.ResumeLayout(false);
            this.wmIdsTabPage.ResumeLayout(false);
            this.bottomFlowLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mTableLayoutPanel;
        private System.Windows.Forms.PictureBox mPictureBox;
        private System.Windows.Forms.FlowLayoutPanel bottomFlowLayoutPanel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TabControl mTabControl;
        private System.Windows.Forms.TabPage generalInfoTabPage;
        private System.Windows.Forms.TableLayoutPanel generalInfoTableLayoutPanel;
        private System.Windows.Forms.Label aboutLabel;
        private System.Windows.Forms.TextBox wmIdTextBox;
        private System.Windows.Forms.TextBox certificateTextBox;
        private System.Windows.Forms.TextBox levelsTextBox;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox addressTextBox;
        private System.Windows.Forms.TextBox contactsTextBox;
        private System.Windows.Forms.TextBox claimsTextBox;
        private System.Windows.Forms.TextBox aboutTextBox;
        private System.Windows.Forms.Label wmidLabel;
        private System.Windows.Forms.Label certificateLabel;
        private System.Windows.Forms.Label levelsLabel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label addressLabel;
        private System.Windows.Forms.Label contactsLabel;
        private System.Windows.Forms.Label claimsLabel;
        private System.Windows.Forms.TabPage certificateDataTabPage;
        private Xml2WinForms.TunableList certificateTunableList;
        private System.Windows.Forms.TabPage wmIdsTabPage;
        private Xml2WinForms.TunableList attachedIdentifierTunableList;
        private System.Windows.Forms.GroupBox commandBarGroupBox;
        private System.Windows.Forms.FlowLayoutPanel commandBarFlowLayoutPanel;
        private System.Windows.Forms.ImageList certificateImageList;
    }
}