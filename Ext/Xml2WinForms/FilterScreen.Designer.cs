using System.Windows.Forms;
using Xml2WinForms.Utils;

namespace Xml2WinForms
{
    partial class FilterScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterScreen));
            this.mTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.mTunableGrid = new Xml2WinForms.TunableGrid();
            this.rightFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.commandBarGroupBox = new System.Windows.Forms.GroupBox();
            this.commandBarFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.filterFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.mTunableShape = new Xml2WinForms.TunableShape();
            this.filterButton = new System.Windows.Forms.Button();
            this.mBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.mTableLayoutPanel.SuspendLayout();
            this.rightFlowLayoutPanel.SuspendLayout();
            this.commandBarGroupBox.SuspendLayout();
            this.filterFlowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mTableLayoutPanel
            // 
            resources.ApplyResources(this.mTableLayoutPanel, "mTableLayoutPanel");
            this.mTableLayoutPanel.Controls.Add(this.mTunableGrid, 0, 0);
            this.mTableLayoutPanel.Controls.Add(this.rightFlowLayoutPanel, 1, 0);
            this.mTableLayoutPanel.Name = "mTableLayoutPanel";
            // 
            // mTunableGrid
            // 
            resources.ApplyResources(this.mTunableGrid, "mTunableGrid");
            this.mTunableGrid.Name = "mTunableGrid";
            this.mTunableGrid.PageCount = 1;
            this.mTunableGrid.ReadOnly = false;
            // 
            // rightFlowLayoutPanel
            // 
            resources.ApplyResources(this.rightFlowLayoutPanel, "rightFlowLayoutPanel");
            this.rightFlowLayoutPanel.Controls.Add(this.commandBarGroupBox);
            this.rightFlowLayoutPanel.Controls.Add(this.filterFlowLayoutPanel);
            this.rightFlowLayoutPanel.Name = "rightFlowLayoutPanel";
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
            // filterFlowLayoutPanel
            // 
            resources.ApplyResources(this.filterFlowLayoutPanel, "filterFlowLayoutPanel");
            this.filterFlowLayoutPanel.Controls.Add(this.mTunableShape);
            this.filterFlowLayoutPanel.Controls.Add(this.filterButton);
            this.filterFlowLayoutPanel.Name = "filterFlowLayoutPanel";
            // 
            // mTunableShape
            // 
            resources.ApplyResources(this.mTunableShape, "mTunableShape");
            this.mTunableShape.BackColor = System.Drawing.SystemColors.Control;
            this.mTunableShape.Name = "mTunableShape";
            // 
            // filterButton
            // 
            resources.ApplyResources(this.filterButton, "filterButton");
            this.filterButton.Name = "filterButton";
            this.filterButton.UseVisualStyleBackColor = true;
            this.filterButton.Click += new System.EventHandler(this.filterButton_Click);
            // 
            // mBackgroundWorker
            // 
            this.mBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.mBackgroundWorker_DoWork);
            this.mBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.mBackgroundWorker_RunWorkerCompleted);
            // 
            // FilterScreen
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mTableLayoutPanel);
            this.Name = "FilterScreen";
            this.Load += new System.EventHandler(this.FilterScreen_Load);
            this.mTableLayoutPanel.ResumeLayout(false);
            this.mTableLayoutPanel.PerformLayout();
            this.rightFlowLayoutPanel.ResumeLayout(false);
            this.rightFlowLayoutPanel.PerformLayout();
            this.commandBarGroupBox.ResumeLayout(false);
            this.commandBarGroupBox.PerformLayout();
            this.filterFlowLayoutPanel.ResumeLayout(false);
            this.filterFlowLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mTableLayoutPanel;
        private TunableGrid mTunableGrid;
        private System.Windows.Forms.FlowLayoutPanel rightFlowLayoutPanel;
        private TunableShape mTunableShape;
        private System.Windows.Forms.Button filterButton;
        private System.Windows.Forms.FlowLayoutPanel filterFlowLayoutPanel;
        private System.Windows.Forms.GroupBox commandBarGroupBox;
        private System.Windows.Forms.FlowLayoutPanel commandBarFlowLayoutPanel;
        private System.ComponentModel.BackgroundWorker mBackgroundWorker;
    }
}
