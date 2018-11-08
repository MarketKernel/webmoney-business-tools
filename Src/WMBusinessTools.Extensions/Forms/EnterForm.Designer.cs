namespace WMBusinessTools.Extensions.Forms
{
    partial class EnterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnterForm));
            this.mTunableList = new Xml2WinForms.TunableList();
            this.bottomFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.exitButton = new System.Windows.Forms.Button();
            this.enterButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.registerButton = new System.Windows.Forms.Button();
            this.mTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.mPanel = new System.Windows.Forms.Panel();
            this.topTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.topPictureBox = new System.Windows.Forms.PictureBox();
            this.captionLabel = new System.Windows.Forms.Label();
            this.bottomFlowLayoutPanel.SuspendLayout();
            this.mTableLayoutPanel.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.mPanel.SuspendLayout();
            this.topTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.topPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mTunableList
            // 
            resources.ApplyResources(this.mTunableList, "mTunableList");
            this.mTunableList.FullRowSelect = true;
            this.mTunableList.HideSelection = false;
            this.mTunableList.MultiSelect = false;
            this.mTunableList.Name = "mTunableList";
            this.mTunableList.UseCompatibleStateImageBehavior = false;
            this.mTunableList.View = System.Windows.Forms.View.Details;
            this.mTunableList.SelectedIndexChanged += new System.EventHandler(this.mTunableList_SelectedIndexChanged);
            // 
            // bottomFlowLayoutPanel
            // 
            resources.ApplyResources(this.bottomFlowLayoutPanel, "bottomFlowLayoutPanel");
            this.bottomFlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.bottomFlowLayoutPanel.Controls.Add(this.exitButton);
            this.bottomFlowLayoutPanel.Controls.Add(this.enterButton);
            this.bottomFlowLayoutPanel.Name = "bottomFlowLayoutPanel";
            // 
            // exitButton
            // 
            this.exitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.exitButton, "exitButton");
            this.exitButton.Name = "exitButton";
            this.exitButton.UseVisualStyleBackColor = true;
            // 
            // enterButton
            // 
            resources.ApplyResources(this.enterButton, "enterButton");
            this.enterButton.Name = "enterButton";
            this.enterButton.UseVisualStyleBackColor = true;
            this.enterButton.Click += new System.EventHandler(this.enterButton_Click);
            // 
            // removeButton
            // 
            resources.ApplyResources(this.removeButton, "removeButton");
            this.removeButton.Name = "removeButton";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // registerButton
            // 
            resources.ApplyResources(this.registerButton, "registerButton");
            this.registerButton.Name = "registerButton";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // mTableLayoutPanel
            // 
            resources.ApplyResources(this.mTableLayoutPanel, "mTableLayoutPanel");
            this.mTableLayoutPanel.Controls.Add(this.mTunableList, 0, 0);
            this.mTableLayoutPanel.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.mTableLayoutPanel.Name = "mTableLayoutPanel";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.registerButton);
            this.flowLayoutPanel1.Controls.Add(this.removeButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // mPanel
            // 
            this.mPanel.Controls.Add(this.mTableLayoutPanel);
            resources.ApplyResources(this.mPanel, "mPanel");
            this.mPanel.Name = "mPanel";
            // 
            // topTableLayoutPanel
            // 
            resources.ApplyResources(this.topTableLayoutPanel, "topTableLayoutPanel");
            this.topTableLayoutPanel.BackColor = System.Drawing.Color.White;
            this.topTableLayoutPanel.Controls.Add(this.topPictureBox, 0, 0);
            this.topTableLayoutPanel.Controls.Add(this.captionLabel, 1, 0);
            this.topTableLayoutPanel.Name = "topTableLayoutPanel";
            // 
            // topPictureBox
            // 
            resources.ApplyResources(this.topPictureBox, "topPictureBox");
            this.topPictureBox.Name = "topPictureBox";
            this.topPictureBox.TabStop = false;
            // 
            // captionLabel
            // 
            resources.ApplyResources(this.captionLabel, "captionLabel");
            this.captionLabel.Name = "captionLabel";
            // 
            // EnterForm
            // 
            this.AcceptButton = this.enterButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.exitButton;
            this.Controls.Add(this.mPanel);
            this.Controls.Add(this.topTableLayoutPanel);
            this.Controls.Add(this.bottomFlowLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EnterForm";
            this.Load += new System.EventHandler(this.EnterForm_Load);
            this.bottomFlowLayoutPanel.ResumeLayout(false);
            this.mTableLayoutPanel.ResumeLayout(false);
            this.mTableLayoutPanel.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.mPanel.ResumeLayout(false);
            this.topTableLayoutPanel.ResumeLayout(false);
            this.topTableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.topPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Xml2WinForms.TunableList mTunableList;
        private System.Windows.Forms.FlowLayoutPanel bottomFlowLayoutPanel;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button enterButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button registerButton;
        private System.Windows.Forms.TableLayoutPanel mTableLayoutPanel;
        private System.Windows.Forms.Panel mPanel;
        private System.Windows.Forms.TableLayoutPanel topTableLayoutPanel;
        private System.Windows.Forms.Label captionLabel;
        private System.Windows.Forms.PictureBox topPictureBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}