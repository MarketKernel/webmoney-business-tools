namespace WMBusinessTools.Extensions.Forms
{
    partial class ProgressForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressForm));
            this.mProgressBar = new System.Windows.Forms.ProgressBar();
            this.cancelLinkLabel = new System.Windows.Forms.LinkLabel();
            this.infoLabel = new System.Windows.Forms.Label();
            this.mBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.mTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.mPanel = new System.Windows.Forms.Panel();
            this.mTableLayoutPanel.SuspendLayout();
            this.mPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mProgressBar
            // 
            resources.ApplyResources(this.mProgressBar, "mProgressBar");
            this.mProgressBar.Name = "mProgressBar";
            this.mProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // cancelLinkLabel
            // 
            resources.ApplyResources(this.cancelLinkLabel, "cancelLinkLabel");
            this.cancelLinkLabel.Name = "cancelLinkLabel";
            this.cancelLinkLabel.TabStop = true;
            this.cancelLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.cancelLinkLabel_LinkClicked);
            // 
            // infoLabel
            // 
            resources.ApplyResources(this.infoLabel, "infoLabel");
            this.infoLabel.Name = "infoLabel";
            // 
            // mBackgroundWorker
            // 
            this.mBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.mBackgroundWorker_DoWork);
            this.mBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.mBackgroundWorker_RunWorkerCompleted);
            // 
            // mTableLayoutPanel
            // 
            resources.ApplyResources(this.mTableLayoutPanel, "mTableLayoutPanel");
            this.mTableLayoutPanel.Controls.Add(this.mProgressBar, 0, 0);
            this.mTableLayoutPanel.Controls.Add(this.cancelLinkLabel, 0, 2);
            this.mTableLayoutPanel.Controls.Add(this.infoLabel, 0, 1);
            this.mTableLayoutPanel.Name = "mTableLayoutPanel";
            // 
            // mPanel
            // 
            resources.ApplyResources(this.mPanel, "mPanel");
            this.mPanel.Controls.Add(this.mTableLayoutPanel);
            this.mPanel.Name = "mPanel";
            // 
            // ProgressForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.ConnectionForm_Load);
            this.mTableLayoutPanel.ResumeLayout(false);
            this.mTableLayoutPanel.PerformLayout();
            this.mPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar mProgressBar;
        private System.Windows.Forms.LinkLabel cancelLinkLabel;
        private System.Windows.Forms.Label infoLabel;
        private System.ComponentModel.BackgroundWorker mBackgroundWorker;
        private System.Windows.Forms.TableLayoutPanel mTableLayoutPanel;
        private System.Windows.Forms.Panel mPanel;
    }
}