namespace WMBusinessTools.Extensions.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mMenuStrip = new System.Windows.Forms.MenuStrip();
            this.sessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.technicalSupportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mStatusStrip = new System.Windows.Forms.StatusStrip();
            this.mToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.mTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.mTabControl = new System.Windows.Forms.TabControl();
            this.identifierComboBox = new System.Windows.Forms.ComboBox();
            this.IdentifierCommandFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.addIdentifierButton = new System.Windows.Forms.Button();
            this.removeIdentifierButton = new System.Windows.Forms.Button();
            this.IdentifierServiceFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.infoButton = new System.Windows.Forms.Button();
            this.copyButton = new System.Windows.Forms.Button();
            this.mMenuStrip.SuspendLayout();
            this.mStatusStrip.SuspendLayout();
            this.mTableLayoutPanel.SuspendLayout();
            this.IdentifierCommandFlowLayoutPanel.SuspendLayout();
            this.IdentifierServiceFlowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mMenuStrip
            // 
            this.mMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sessionToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            resources.ApplyResources(this.mMenuStrip, "mMenuStrip");
            this.mMenuStrip.Name = "mMenuStrip";
            // 
            // sessionToolStripMenuItem
            // 
            this.sessionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.sessionToolStripMenuItem.Name = "sessionToolStripMenuItem";
            resources.ApplyResources(this.sessionToolStripMenuItem, "sessionToolStripMenuItem");
            // 
            // exitToolStripMenuItem
            // 
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            resources.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            resources.ApplyResources(this.optionsToolStripMenuItem, "optionsToolStripMenuItem");
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.technicalSupportToolStripMenuItem,
            this.aboutToolStripSeparator,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            // 
            // contentsToolStripMenuItem
            // 
            resources.ApplyResources(this.contentsToolStripMenuItem, "contentsToolStripMenuItem");
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.Click += new System.EventHandler(this.contentsToolStripMenuItem_Click);
            // 
            // technicalSupportToolStripMenuItem
            // 
            resources.ApplyResources(this.technicalSupportToolStripMenuItem, "technicalSupportToolStripMenuItem");
            this.technicalSupportToolStripMenuItem.Name = "technicalSupportToolStripMenuItem";
            this.technicalSupportToolStripMenuItem.Click += new System.EventHandler(this.technicalSupportToolStripMenuItem_Click);
            // 
            // aboutToolStripSeparator
            // 
            this.aboutToolStripSeparator.Name = "aboutToolStripSeparator";
            resources.ApplyResources(this.aboutToolStripSeparator, "aboutToolStripSeparator");
            // 
            // aboutToolStripMenuItem
            // 
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // mStatusStrip
            // 
            this.mStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mToolStripProgressBar});
            resources.ApplyResources(this.mStatusStrip, "mStatusStrip");
            this.mStatusStrip.Name = "mStatusStrip";
            // 
            // mToolStripProgressBar
            // 
            this.mToolStripProgressBar.Name = "mToolStripProgressBar";
            resources.ApplyResources(this.mToolStripProgressBar, "mToolStripProgressBar");
            this.mToolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // mTableLayoutPanel
            // 
            resources.ApplyResources(this.mTableLayoutPanel, "mTableLayoutPanel");
            this.mTableLayoutPanel.Controls.Add(this.mTabControl, 0, 1);
            this.mTableLayoutPanel.Controls.Add(this.identifierComboBox, 1, 0);
            this.mTableLayoutPanel.Controls.Add(this.IdentifierCommandFlowLayoutPanel, 2, 0);
            this.mTableLayoutPanel.Controls.Add(this.IdentifierServiceFlowLayoutPanel, 0, 0);
            this.mTableLayoutPanel.Name = "mTableLayoutPanel";
            // 
            // mTabControl
            // 
            this.mTableLayoutPanel.SetColumnSpan(this.mTabControl, 3);
            resources.ApplyResources(this.mTabControl, "mTabControl");
            this.mTabControl.Name = "mTabControl";
            this.mTabControl.SelectedIndex = 0;
            // 
            // identifierComboBox
            // 
            resources.ApplyResources(this.identifierComboBox, "identifierComboBox");
            this.identifierComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.identifierComboBox.FormattingEnabled = true;
            this.identifierComboBox.Name = "identifierComboBox";
            this.identifierComboBox.SelectedIndexChanged += new System.EventHandler(this.identifierComboBox_SelectedIndexChanged);
            // 
            // IdentifierCommandFlowLayoutPanel
            // 
            this.IdentifierCommandFlowLayoutPanel.Controls.Add(this.addIdentifierButton);
            this.IdentifierCommandFlowLayoutPanel.Controls.Add(this.removeIdentifierButton);
            resources.ApplyResources(this.IdentifierCommandFlowLayoutPanel, "IdentifierCommandFlowLayoutPanel");
            this.IdentifierCommandFlowLayoutPanel.Name = "IdentifierCommandFlowLayoutPanel";
            // 
            // addIdentifierButton
            // 
            resources.ApplyResources(this.addIdentifierButton, "addIdentifierButton");
            this.addIdentifierButton.Name = "addIdentifierButton";
            this.addIdentifierButton.UseVisualStyleBackColor = true;
            this.addIdentifierButton.Click += new System.EventHandler(this.addIdentifierButton_Click);
            // 
            // removeIdentifierButton
            // 
            resources.ApplyResources(this.removeIdentifierButton, "removeIdentifierButton");
            this.removeIdentifierButton.Name = "removeIdentifierButton";
            this.removeIdentifierButton.UseVisualStyleBackColor = true;
            this.removeIdentifierButton.Click += new System.EventHandler(this.removeIdentifierButton_Click);
            // 
            // IdentifierServiceFlowLayoutPanel
            // 
            resources.ApplyResources(this.IdentifierServiceFlowLayoutPanel, "IdentifierServiceFlowLayoutPanel");
            this.IdentifierServiceFlowLayoutPanel.Controls.Add(this.infoButton);
            this.IdentifierServiceFlowLayoutPanel.Controls.Add(this.copyButton);
            this.IdentifierServiceFlowLayoutPanel.Name = "IdentifierServiceFlowLayoutPanel";
            // 
            // infoButton
            // 
            resources.ApplyResources(this.infoButton, "infoButton");
            this.infoButton.FlatAppearance.BorderSize = 0;
            this.infoButton.Name = "infoButton";
            this.infoButton.UseVisualStyleBackColor = true;
            this.infoButton.Click += new System.EventHandler(this.infoButton_Click);
            // 
            // copyButton
            // 
            resources.ApplyResources(this.copyButton, "copyButton");
            this.copyButton.FlatAppearance.BorderSize = 0;
            this.copyButton.Name = "copyButton";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mTableLayoutPanel);
            this.Controls.Add(this.mStatusStrip);
            this.Controls.Add(this.mMenuStrip);
            this.MainMenuStrip = this.mMenuStrip;
            this.Name = "MainForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mMenuStrip.ResumeLayout(false);
            this.mMenuStrip.PerformLayout();
            this.mStatusStrip.ResumeLayout(false);
            this.mStatusStrip.PerformLayout();
            this.mTableLayoutPanel.ResumeLayout(false);
            this.mTableLayoutPanel.PerformLayout();
            this.IdentifierCommandFlowLayoutPanel.ResumeLayout(false);
            this.IdentifierServiceFlowLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem sessionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.StatusStrip mStatusStrip;
        private System.Windows.Forms.ToolStripProgressBar mToolStripProgressBar;
        private System.Windows.Forms.TableLayoutPanel mTableLayoutPanel;
        private System.Windows.Forms.TabControl mTabControl;
        private System.Windows.Forms.ComboBox identifierComboBox;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem technicalSupportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator aboutToolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel IdentifierCommandFlowLayoutPanel;
        private System.Windows.Forms.Button addIdentifierButton;
        private System.Windows.Forms.Button removeIdentifierButton;
        private System.Windows.Forms.Button infoButton;
        private System.Windows.Forms.FlowLayoutPanel IdentifierServiceFlowLayoutPanel;
        private System.Windows.Forms.Button copyButton;
    }
}

