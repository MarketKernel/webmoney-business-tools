namespace WMBusinessTools.Extensions
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
            this.liveViewTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.liveViewTextBox.Location = new System.Drawing.Point(11, 11);
            this.liveViewTextBox.Name = "liveViewTextBox";
            this.liveViewTextBox.Size = new System.Drawing.Size(300, 44);
            this.liveViewTextBox.TabIndex = 0;
            // 
            // mTunableShape
            // 
            this.mTunableShape.AutoSize = true;
            this.mTunableShape.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mTunableShape.BackColor = System.Drawing.SystemColors.Control;
            this.mTunableShape.Location = new System.Drawing.Point(8, 58);
            this.mTunableShape.Margin = new System.Windows.Forms.Padding(0);
            this.mTunableShape.MinimumSize = new System.Drawing.Size(20, 20);
            this.mTunableShape.Name = "mTunableShape";
            this.mTunableShape.Size = new System.Drawing.Size(20, 20);
            this.mTunableShape.TabIndex = 1;
            // 
            // mTimer
            // 
            this.mTimer.Enabled = true;
            this.mTimer.Interval = 500;
            this.mTimer.Tick += new System.EventHandler(this.mTimer_Tick);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(237, 8);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(8);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(146, 8);
            this.okButton.Margin = new System.Windows.Forms.Padding(8);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // bottomFlowLayoutPanel
            // 
            this.bottomFlowLayoutPanel.AutoSize = true;
            this.bottomFlowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bottomFlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.bottomFlowLayoutPanel.Controls.Add(this.cancelButton);
            this.bottomFlowLayoutPanel.Controls.Add(this.okButton);
            this.bottomFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.bottomFlowLayoutPanel.Location = new System.Drawing.Point(0, 152);
            this.bottomFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.bottomFlowLayoutPanel.Name = "bottomFlowLayoutPanel";
            this.bottomFlowLayoutPanel.Size = new System.Drawing.Size(324, 43);
            this.bottomFlowLayoutPanel.TabIndex = 3;
            this.bottomFlowLayoutPanel.WrapContents = false;
            // 
            // mFlowLayoutPanel
            // 
            this.mFlowLayoutPanel.Controls.Add(this.liveViewTextBox);
            this.mFlowLayoutPanel.Controls.Add(this.mTunableShape);
            this.mFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mFlowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mFlowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mFlowLayoutPanel.Name = "mFlowLayoutPanel";
            this.mFlowLayoutPanel.Padding = new System.Windows.Forms.Padding(8);
            this.mFlowLayoutPanel.Size = new System.Drawing.Size(324, 152);
            this.mFlowLayoutPanel.TabIndex = 4;
            this.mFlowLayoutPanel.WrapContents = false;
            // 
            // RequestNumberSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 195);
            this.Controls.Add(this.mFlowLayoutPanel);
            this.Controls.Add(this.bottomFlowLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RequestNumberSettingsForm";
            this.ShowIcon = false;
            this.Text = "Request Number Settings";
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