namespace Xml2WinForms
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.okButton = new System.Windows.Forms.Button();
            this.cButton = new System.Windows.Forms.Button();
            this.mPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.bottomFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.mPanel = new System.Windows.Forms.Panel();
            this.bottomFlowLayoutPanel.SuspendLayout();
            this.mPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cButton
            // 
            this.cButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cButton, "cButton");
            this.cButton.Name = "cButton";
            this.cButton.UseVisualStyleBackColor = true;
            this.cButton.Click += new System.EventHandler(this.cButton_Click);
            // 
            // mPropertyGrid
            // 
            this.mPropertyGrid.CommandsBackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.mPropertyGrid, "mPropertyGrid");
            this.mPropertyGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            this.mPropertyGrid.Name = "mPropertyGrid";
            // 
            // bottomFlowLayoutPanel
            // 
            this.bottomFlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.bottomFlowLayoutPanel.Controls.Add(this.cButton);
            this.bottomFlowLayoutPanel.Controls.Add(this.okButton);
            resources.ApplyResources(this.bottomFlowLayoutPanel, "bottomFlowLayoutPanel");
            this.bottomFlowLayoutPanel.Name = "bottomFlowLayoutPanel";
            // 
            // mPanel
            // 
            this.mPanel.Controls.Add(this.mPropertyGrid);
            resources.ApplyResources(this.mPanel, "mPanel");
            this.mPanel.Name = "mPanel";
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cButton;
            this.Controls.Add(this.mPanel);
            this.Controls.Add(this.bottomFlowLayoutPanel);
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.bottomFlowLayoutPanel.ResumeLayout(false);
            this.mPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button cButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.PropertyGrid mPropertyGrid;
        private System.Windows.Forms.FlowLayoutPanel bottomFlowLayoutPanel;
        private System.Windows.Forms.Panel mPanel;
    }
}