using WMBusinessTools.Extensions.Controls;

namespace WMBusinessTools.Extensions.Forms
{
    partial class TransferRegisterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransferRegisterForm));
            this.bottomFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.mTunableGrid = new Xml2WinForms.TunableGrid();
            this.mMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.bundleNameLabel = new System.Windows.Forms.Label();
            this.bundleNameTextBox = new System.Windows.Forms.TextBox();
            this.sourcePurseLabel = new System.Windows.Forms.Label();
            this.sourcePurseDropDownList = new WMBusinessTools.Extensions.Controls.AccountDropDownList();
            this.mErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.mStatusStrip = new System.Windows.Forms.StatusStrip();
            this.separatorToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.countToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.totalAmountToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.mSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.bottomFlowLayoutPanel.SuspendLayout();
            this.mMenuStrip.SuspendLayout();
            this.topFlowLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mErrorProvider)).BeginInit();
            this.mStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // bottomFlowLayoutPanel
            // 
            resources.ApplyResources(this.bottomFlowLayoutPanel, "bottomFlowLayoutPanel");
            this.bottomFlowLayoutPanel.Controls.Add(this.cancelButton);
            this.bottomFlowLayoutPanel.Controls.Add(this.okButton);
            this.bottomFlowLayoutPanel.Name = "bottomFlowLayoutPanel";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // mTunableGrid
            // 
            resources.ApplyResources(this.mTunableGrid, "mTunableGrid");
            this.mTunableGrid.Name = "mTunableGrid";
            this.mTunableGrid.PageCount = 1;
            this.mTunableGrid.ReadOnly = false;
            this.mTunableGrid.CellValueChanged += new System.EventHandler<Xml2WinForms.ValueChangedEventArgs>(this.mTunableGrid_CellValueChanged);
            // 
            // mMenuStrip
            // 
            this.mMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            resources.ApplyResources(this.mMenuStrip, "mMenuStrip");
            this.mMenuStrip.Name = "mMenuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            resources.ApplyResources(this.saveAsToolStripMenuItem, "saveAsToolStripMenuItem");
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            resources.ApplyResources(this.closeToolStripMenuItem, "closeToolStripMenuItem");
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // topFlowLayoutPanel
            // 
            resources.ApplyResources(this.topFlowLayoutPanel, "topFlowLayoutPanel");
            this.topFlowLayoutPanel.Controls.Add(this.bundleNameLabel);
            this.topFlowLayoutPanel.Controls.Add(this.bundleNameTextBox);
            this.topFlowLayoutPanel.Controls.Add(this.sourcePurseLabel);
            this.topFlowLayoutPanel.Controls.Add(this.sourcePurseDropDownList);
            this.topFlowLayoutPanel.Name = "topFlowLayoutPanel";
            // 
            // bundleNameLabel
            // 
            resources.ApplyResources(this.bundleNameLabel, "bundleNameLabel");
            this.bundleNameLabel.Name = "bundleNameLabel";
            // 
            // bundleNameTextBox
            // 
            resources.ApplyResources(this.bundleNameTextBox, "bundleNameTextBox");
            this.mErrorProvider.SetIconPadding(this.bundleNameTextBox, ((int)(resources.GetObject("bundleNameTextBox.IconPadding"))));
            this.bundleNameTextBox.Name = "bundleNameTextBox";
            // 
            // sourcePurseLabel
            // 
            resources.ApplyResources(this.sourcePurseLabel, "sourcePurseLabel");
            this.sourcePurseLabel.Name = "sourcePurseLabel";
            // 
            // sourcePurseDropDownList
            // 
            resources.ApplyResources(this.sourcePurseDropDownList, "sourcePurseDropDownList");
            this.sourcePurseDropDownList.FormattingEnabled = true;
            this.sourcePurseDropDownList.Name = "sourcePurseDropDownList";
            // 
            // mErrorProvider
            // 
            this.mErrorProvider.ContainerControl = this;
            // 
            // mStatusStrip
            // 
            this.mStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.separatorToolStripStatusLabel,
            this.countToolStripStatusLabel,
            this.totalAmountToolStripStatusLabel});
            resources.ApplyResources(this.mStatusStrip, "mStatusStrip");
            this.mStatusStrip.Name = "mStatusStrip";
            // 
            // separatorToolStripStatusLabel
            // 
            this.separatorToolStripStatusLabel.Name = "separatorToolStripStatusLabel";
            resources.ApplyResources(this.separatorToolStripStatusLabel, "separatorToolStripStatusLabel");
            this.separatorToolStripStatusLabel.Spring = true;
            // 
            // countToolStripStatusLabel
            // 
            resources.ApplyResources(this.countToolStripStatusLabel, "countToolStripStatusLabel");
            this.countToolStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.countToolStripStatusLabel.ForeColor = System.Drawing.Color.DarkBlue;
            this.countToolStripStatusLabel.Name = "countToolStripStatusLabel";
            // 
            // totalAmountToolStripStatusLabel
            // 
            resources.ApplyResources(this.totalAmountToolStripStatusLabel, "totalAmountToolStripStatusLabel");
            this.totalAmountToolStripStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)(((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.totalAmountToolStripStatusLabel.ForeColor = System.Drawing.Color.DarkBlue;
            this.totalAmountToolStripStatusLabel.Name = "totalAmountToolStripStatusLabel";
            // 
            // mOpenFileDialog
            // 
            resources.ApplyResources(this.mOpenFileDialog, "mOpenFileDialog");
            // 
            // mSaveFileDialog
            // 
            resources.ApplyResources(this.mSaveFileDialog, "mSaveFileDialog");
            // 
            // TransferRegisterForm
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.bottomFlowLayoutPanel);
            this.Controls.Add(this.mStatusStrip);
            this.Controls.Add(this.mTunableGrid);
            this.Controls.Add(this.topFlowLayoutPanel);
            this.Controls.Add(this.mMenuStrip);
            this.MainMenuStrip = this.mMenuStrip;
            this.Name = "TransferRegisterForm";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.TransferRegisterForm_Load);
            this.bottomFlowLayoutPanel.ResumeLayout(false);
            this.mMenuStrip.ResumeLayout(false);
            this.mMenuStrip.PerformLayout();
            this.topFlowLayoutPanel.ResumeLayout(false);
            this.topFlowLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mErrorProvider)).EndInit();
            this.mStatusStrip.ResumeLayout(false);
            this.mStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel bottomFlowLayoutPanel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private Xml2WinForms.TunableGrid mTunableGrid;
        private System.Windows.Forms.MenuStrip mMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel topFlowLayoutPanel;
        private System.Windows.Forms.Label sourcePurseLabel;
        private AccountDropDownList sourcePurseDropDownList;
        private System.Windows.Forms.Label bundleNameLabel;
        private System.Windows.Forms.TextBox bundleNameTextBox;
        private System.Windows.Forms.ErrorProvider mErrorProvider;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.StatusStrip mStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel separatorToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel countToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel totalAmountToolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.OpenFileDialog mOpenFileDialog;
        private System.Windows.Forms.SaveFileDialog mSaveFileDialog;
    }
}