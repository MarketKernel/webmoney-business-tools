namespace Xml2WinForms
{
    partial class ListScreen
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListScreen));
            this.mTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.commandFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.refreshButton = new System.Windows.Forms.Button();
            this.mTunableListView = new Xml2WinForms.TunableList();
            this.mBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.mTableLayoutPanel.SuspendLayout();
            this.commandFlowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mTableLayoutPanel
            // 
            resources.ApplyResources(this.mTableLayoutPanel, "mTableLayoutPanel");
            this.mTableLayoutPanel.Controls.Add(this.commandFlowLayoutPanel, 0, 1);
            this.mTableLayoutPanel.Controls.Add(this.mTunableListView, 0, 0);
            this.mTableLayoutPanel.Name = "mTableLayoutPanel";
            // 
            // commandFlowLayoutPanel
            // 
            resources.ApplyResources(this.commandFlowLayoutPanel, "commandFlowLayoutPanel");
            this.commandFlowLayoutPanel.Controls.Add(this.refreshButton);
            this.commandFlowLayoutPanel.Name = "commandFlowLayoutPanel";
            // 
            // refreshButton
            // 
            resources.ApplyResources(this.refreshButton, "refreshButton");
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // mTunableListView
            // 
            resources.ApplyResources(this.mTunableListView, "mTunableListView");
            this.mTunableListView.FullRowSelect = true;
            this.mTunableListView.HideSelection = false;
            this.mTunableListView.MultiSelect = false;
            this.mTunableListView.Name = "mTunableListView";
            this.mTunableListView.UseCompatibleStateImageBehavior = false;
            this.mTunableListView.View = System.Windows.Forms.View.Details;
            // 
            // mBackgroundWorker
            // 
            this.mBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.mBackgroundWorker_DoWork);
            this.mBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.mBackgroundWorker_RunWorkerCompleted);
            // 
            // ListScreen
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mTableLayoutPanel);
            this.Name = "ListScreen";
            this.mTableLayoutPanel.ResumeLayout(false);
            this.mTableLayoutPanel.PerformLayout();
            this.commandFlowLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mTableLayoutPanel;
        private System.Windows.Forms.FlowLayoutPanel commandFlowLayoutPanel;
        private System.Windows.Forms.Button refreshButton;
        private TunableList mTunableListView;
        private System.ComponentModel.BackgroundWorker mBackgroundWorker;
    }
}
