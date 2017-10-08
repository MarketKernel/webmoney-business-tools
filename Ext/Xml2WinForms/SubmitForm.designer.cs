using System.Windows.Forms;

namespace Xml2WinForms
{
    partial class SubmitForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubmitForm));
            this.mStatusStrip = new System.Windows.Forms.StatusStrip();
            this.mToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.mBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.bottomFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.previousButton = new System.Windows.Forms.Button();
            this.mFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.mPanel = new System.Windows.Forms.Panel();
            this.mStatusStrip.SuspendLayout();
            this.bottomFlowLayoutPanel.SuspendLayout();
            this.mPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mStatusStrip
            // 
            resources.ApplyResources(this.mStatusStrip, "mStatusStrip");
            this.mStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mToolStripProgressBar});
            this.mStatusStrip.Name = "mStatusStrip";
            this.mStatusStrip.SizingGrip = false;
            // 
            // mToolStripProgressBar
            // 
            resources.ApplyResources(this.mToolStripProgressBar, "mToolStripProgressBar");
            this.mToolStripProgressBar.Name = "mToolStripProgressBar";
            this.mToolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // mBackgroundWorker
            // 
            this.mBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.mBackgroundWorker_DoWork);
            this.mBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.mBackgroundWorker_RunWorkerCompleted);
            // 
            // bottomFlowLayoutPanel
            // 
            resources.ApplyResources(this.bottomFlowLayoutPanel, "bottomFlowLayoutPanel");
            this.bottomFlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.bottomFlowLayoutPanel.Controls.Add(this.cancelButton);
            this.bottomFlowLayoutPanel.Controls.Add(this.nextButton);
            this.bottomFlowLayoutPanel.Controls.Add(this.previousButton);
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
            // nextButton
            // 
            resources.ApplyResources(this.nextButton, "nextButton");
            this.nextButton.Name = "nextButton";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // previousButton
            // 
            resources.ApplyResources(this.previousButton, "previousButton");
            this.previousButton.Name = "previousButton";
            this.previousButton.UseVisualStyleBackColor = true;
            this.previousButton.Click += new System.EventHandler(this.previousButton_Click);
            // 
            // mFlowLayoutPanel
            // 
            resources.ApplyResources(this.mFlowLayoutPanel, "mFlowLayoutPanel");
            this.mFlowLayoutPanel.Name = "mFlowLayoutPanel";
            // 
            // mPanel
            // 
            resources.ApplyResources(this.mPanel, "mPanel");
            this.mPanel.Controls.Add(this.mFlowLayoutPanel);
            this.mPanel.Name = "mPanel";
            // 
            // SubmitForm
            // 
            this.AcceptButton = this.nextButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.mPanel);
            this.Controls.Add(this.bottomFlowLayoutPanel);
            this.Controls.Add(this.mStatusStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SubmitForm";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.SubmitForm_Load);
            this.mStatusStrip.ResumeLayout(false);
            this.mStatusStrip.PerformLayout();
            this.bottomFlowLayoutPanel.ResumeLayout(false);
            this.bottomFlowLayoutPanel.PerformLayout();
            this.mPanel.ResumeLayout(false);
            this.mPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip mStatusStrip;
        private System.Windows.Forms.ToolStripProgressBar mToolStripProgressBar;
        private System.ComponentModel.BackgroundWorker mBackgroundWorker;
        private System.Windows.Forms.FlowLayoutPanel bottomFlowLayoutPanel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button previousButton;
        private System.Windows.Forms.FlowLayoutPanel mFlowLayoutPanel;
        private Panel mPanel;
    }
}