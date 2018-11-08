namespace Xml2WinForms.Sandbox
{
    partial class DbSettingsForm
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
            this.mTabControl = new System.Windows.Forms.TabControl();
            this.ceTabPage = new System.Windows.Forms.TabPage();
            this.ceTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.confirmPasswordTextBox = new System.Windows.Forms.TextBox();
            this.confirmPasswordLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.usePasswordCheckBox = new System.Windows.Forms.CheckBox();
            this.pathTextBoxWithButton = new Xml2WinForms.TunableTextBoxWithButton();
            this.pathLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.mPanel = new System.Windows.Forms.Panel();
            this.connectButton = new System.Windows.Forms.Button();
            this.bottomFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.mTabControl.SuspendLayout();
            this.ceTabPage.SuspendLayout();
            this.ceTableLayoutPanel.SuspendLayout();
            this.mPanel.SuspendLayout();
            this.bottomFlowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mTabControl
            // 
            this.mTabControl.Controls.Add(this.ceTabPage);
            this.mTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mTabControl.Location = new System.Drawing.Point(8, 8);
            this.mTabControl.Name = "mTabControl";
            this.mTabControl.Padding = new System.Drawing.Point(3, 0);
            this.mTabControl.SelectedIndex = 0;
            this.mTabControl.Size = new System.Drawing.Size(318, 172);
            this.mTabControl.TabIndex = 0;
            // 
            // ceTabPage
            // 
            this.ceTabPage.Controls.Add(this.ceTableLayoutPanel);
            this.ceTabPage.Location = new System.Drawing.Point(4, 20);
            this.ceTabPage.Name = "ceTabPage";
            this.ceTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ceTabPage.Size = new System.Drawing.Size(310, 148);
            this.ceTabPage.TabIndex = 0;
            this.ceTabPage.Text = "SQL Server CE ";
            this.ceTabPage.UseVisualStyleBackColor = true;
            // 
            // ceTableLayoutPanel
            // 
            this.ceTableLayoutPanel.ColumnCount = 1;
            this.ceTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ceTableLayoutPanel.Controls.Add(this.confirmPasswordTextBox, 0, 6);
            this.ceTableLayoutPanel.Controls.Add(this.confirmPasswordLabel, 0, 5);
            this.ceTableLayoutPanel.Controls.Add(this.passwordTextBox, 0, 4);
            this.ceTableLayoutPanel.Controls.Add(this.passwordLabel, 0, 3);
            this.ceTableLayoutPanel.Controls.Add(this.usePasswordCheckBox, 0, 2);
            this.ceTableLayoutPanel.Controls.Add(this.pathTextBoxWithButton, 0, 1);
            this.ceTableLayoutPanel.Controls.Add(this.pathLabel, 0, 0);
            this.ceTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ceTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.ceTableLayoutPanel.Name = "ceTableLayoutPanel";
            this.ceTableLayoutPanel.RowCount = 8;
            this.ceTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ceTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ceTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ceTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ceTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ceTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ceTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ceTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ceTableLayoutPanel.Size = new System.Drawing.Size(304, 142);
            this.ceTableLayoutPanel.TabIndex = 0;
            // 
            // confirmPasswordTextBox
            // 
            this.confirmPasswordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.confirmPasswordTextBox.Enabled = false;
            this.confirmPasswordTextBox.Location = new System.Drawing.Point(3, 117);
            this.confirmPasswordTextBox.Name = "confirmPasswordTextBox";
            this.confirmPasswordTextBox.Size = new System.Drawing.Size(298, 20);
            this.confirmPasswordTextBox.TabIndex = 6;
            this.confirmPasswordTextBox.UseSystemPasswordChar = true;
            // 
            // confirmPasswordLabel
            // 
            this.confirmPasswordLabel.AutoSize = true;
            this.confirmPasswordLabel.Location = new System.Drawing.Point(3, 101);
            this.confirmPasswordLabel.Name = "confirmPasswordLabel";
            this.confirmPasswordLabel.Size = new System.Drawing.Size(93, 13);
            this.confirmPasswordLabel.TabIndex = 5;
            this.confirmPasswordLabel.Text = "Confirm password:";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextBox.Enabled = false;
            this.passwordTextBox.Location = new System.Drawing.Point(3, 78);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(298, 20);
            this.passwordTextBox.TabIndex = 4;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(3, 62);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(56, 13);
            this.passwordLabel.TabIndex = 3;
            this.passwordLabel.Text = "Password:";
            // 
            // usePasswordCheckBox
            // 
            this.usePasswordCheckBox.AutoSize = true;
            this.usePasswordCheckBox.Location = new System.Drawing.Point(3, 42);
            this.usePasswordCheckBox.Name = "usePasswordCheckBox";
            this.usePasswordCheckBox.Size = new System.Drawing.Size(93, 17);
            this.usePasswordCheckBox.TabIndex = 2;
            this.usePasswordCheckBox.Text = "Use password";
            this.usePasswordCheckBox.UseVisualStyleBackColor = true;
            this.usePasswordCheckBox.CheckedChanged += new System.EventHandler(this.usePasswordCheckBox_CheckedChanged);
            // 
            // pathTextBoxWithButton
            // 
            this.pathTextBoxWithButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pathTextBoxWithButton.ButtonIcon = null;
            this.pathTextBoxWithButton.DigitsOnly = false;
            this.pathTextBoxWithButton.Location = new System.Drawing.Point(3, 16);
            this.pathTextBoxWithButton.MaxLength = 32767;
            this.pathTextBoxWithButton.Name = "pathTextBoxWithButton";
            this.pathTextBoxWithButton.ReadOnly = false;
            this.pathTextBoxWithButton.Size = new System.Drawing.Size(298, 20);
            this.pathTextBoxWithButton.TabIndex = 1;
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Location = new System.Drawing.Point(3, 0);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(91, 13);
            this.pathLabel.TabIndex = 0;
            this.pathLabel.Text = "Path to database:";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(156, 8);
            this.okButton.Margin = new System.Windows.Forms.Padding(8);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(247, 8);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(8);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // mPanel
            // 
            this.mPanel.Controls.Add(this.mTabControl);
            this.mPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mPanel.Location = new System.Drawing.Point(0, 0);
            this.mPanel.Margin = new System.Windows.Forms.Padding(0);
            this.mPanel.Name = "mPanel";
            this.mPanel.Padding = new System.Windows.Forms.Padding(8);
            this.mPanel.Size = new System.Drawing.Size(334, 188);
            this.mPanel.TabIndex = 0;
            // 
            // connectButton
            // 
            this.connectButton.BackColor = System.Drawing.Color.Pink;
            this.connectButton.Location = new System.Drawing.Point(9, 8);
            this.connectButton.Margin = new System.Windows.Forms.Padding(8, 8, 64, 8);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 0;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = false;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // bottomFlowLayoutPanel
            // 
            this.bottomFlowLayoutPanel.AutoSize = true;
            this.bottomFlowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bottomFlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.bottomFlowLayoutPanel.Controls.Add(this.cancelButton);
            this.bottomFlowLayoutPanel.Controls.Add(this.okButton);
            this.bottomFlowLayoutPanel.Controls.Add(this.connectButton);
            this.bottomFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.bottomFlowLayoutPanel.Location = new System.Drawing.Point(0, 188);
            this.bottomFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.bottomFlowLayoutPanel.Name = "bottomFlowLayoutPanel";
            this.bottomFlowLayoutPanel.Size = new System.Drawing.Size(334, 43);
            this.bottomFlowLayoutPanel.TabIndex = 3;
            this.bottomFlowLayoutPanel.WrapContents = false;
            // 
            // DbSettingsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(334, 231);
            this.Controls.Add(this.mPanel);
            this.Controls.Add(this.bottomFlowLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "DbSettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Build connection string";
            this.mTabControl.ResumeLayout(false);
            this.ceTabPage.ResumeLayout(false);
            this.ceTableLayoutPanel.ResumeLayout(false);
            this.ceTableLayoutPanel.PerformLayout();
            this.mPanel.ResumeLayout(false);
            this.bottomFlowLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl mTabControl;
        private System.Windows.Forms.TabPage ceTabPage;
        private System.Windows.Forms.TableLayoutPanel ceTableLayoutPanel;
        private System.Windows.Forms.TextBox confirmPasswordTextBox;
        private System.Windows.Forms.Label confirmPasswordLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.CheckBox usePasswordCheckBox;
        private TunableTextBoxWithButton pathTextBoxWithButton;
        private System.Windows.Forms.Label pathLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Panel mPanel;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.FlowLayoutPanel bottomFlowLayoutPanel;
    }
}