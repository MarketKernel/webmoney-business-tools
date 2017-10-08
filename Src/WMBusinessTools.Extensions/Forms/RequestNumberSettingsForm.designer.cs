namespace WMBusinessTools.Extensions.Forms
{
    partial class RequestNumberSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RequestNumberSettingsForm));
            this.liveViewTextBox = new System.Windows.Forms.TextBox();
            this.mTunableShape = new Xml2WinForms.TunableShape();
            this.mTimer = new System.Windows.Forms.Timer(this.components);
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.bottomFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.mFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.bottomFlowLayoutPanel.SuspendLayout();
            this.mFlowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // liveViewTextBox
            // 
            this.liveViewTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.liveViewTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.liveViewTextBox, "liveViewTextBox");
            this.liveViewTextBox.Name = "liveViewTextBox";
            // 
            // mTunableShape
            // 
            resources.ApplyResources(this.mTunableShape, "mTunableShape");
            this.mTunableShape.BackColor = System.Drawing.SystemColors.Control;
            this.mTunableShape.Name = "mTunableShape";
            // 
            // mTimer
            // 
            this.mTimer.Enabled = true;
            this.mTimer.Interval = 500;
            this.mTimer.Tick += new System.EventHandler(this.mTimer_Tick);
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
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // bottomFlowLayoutPanel
            // 
            resources.ApplyResources(this.bottomFlowLayoutPanel, "bottomFlowLayoutPanel");
            this.bottomFlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.bottomFlowLayoutPanel.Controls.Add(this.cancelButton);
            this.bottomFlowLayoutPanel.Controls.Add(this.okButton);
            this.bottomFlowLayoutPanel.Name = "bottomFlowLayoutPanel";
            // 
            // mFlowLayoutPanel
            // 
            this.mFlowLayoutPanel.Controls.Add(this.liveViewTextBox);
            this.mFlowLayoutPanel.Controls.Add(this.mTunableShape);
            resources.ApplyResources(this.mFlowLayoutPanel, "mFlowLayoutPanel");
            this.mFlowLayoutPanel.Name = "mFlowLayoutPanel";
            // 
            // RequestNumberSettingsForm
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.mFlowLayoutPanel);
            this.Controls.Add(this.bottomFlowLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "RequestNumberSettingsForm";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.RequestNumberSettingsForm_Load);
            this.bottomFlowLayoutPanel.ResumeLayout(false);
            this.mFlowLayoutPanel.ResumeLayout(false);
            this.mFlowLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox liveViewTextBox;
        private Xml2WinForms.TunableShape mTunableShape;
        private System.Windows.Forms.Timer mTimer;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.FlowLayoutPanel bottomFlowLayoutPanel;
        private System.Windows.Forms.FlowLayoutPanel mFlowLayoutPanel;
    }
}