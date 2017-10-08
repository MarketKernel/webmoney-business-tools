namespace WMBusinessTools.Extensions.Contracts.Internal
{
    partial class ErrorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorForm));
            this.closeButton = new System.Windows.Forms.Button();
            this.iconPictureBox = new System.Windows.Forms.PictureBox();
            this.detailsButton = new System.Windows.Forms.Button();
            this.arrowImageList = new System.Windows.Forms.ImageList(this.components);
            this.captionLabel = new System.Windows.Forms.Label();
            this.mTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.detailsRichTextBox = new System.Windows.Forms.RichTextBox();
            this.copyButton = new System.Windows.Forms.Button();
            this.messageRichTextBox = new System.Windows.Forms.RichTextBox();
            this.separatorLabel = new System.Windows.Forms.Label();
            this.mPanel = new System.Windows.Forms.Panel();
            this.iconImageList = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.mTableLayoutPanel.SuspendLayout();
            this.mPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            resources.ApplyResources(this.closeButton, "closeButton");
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.closeButton.Name = "closeButton";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // iconPictureBox
            // 
            resources.ApplyResources(this.iconPictureBox, "iconPictureBox");
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.TabStop = false;
            // 
            // detailsButton
            // 
            resources.ApplyResources(this.detailsButton, "detailsButton");
            this.mTableLayoutPanel.SetColumnSpan(this.detailsButton, 2);
            this.detailsButton.ImageList = this.arrowImageList;
            this.detailsButton.Name = "detailsButton";
            this.detailsButton.UseVisualStyleBackColor = true;
            this.detailsButton.Click += new System.EventHandler(this.detailsButton_Click);
            // 
            // arrowImageList
            // 
            this.arrowImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("arrowImageList.ImageStream")));
            this.arrowImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.arrowImageList.Images.SetKeyName(0, "ExpandArrow.png");
            this.arrowImageList.Images.SetKeyName(1, "CollapseArrow.png");
            // 
            // captionLabel
            // 
            resources.ApplyResources(this.captionLabel, "captionLabel");
            this.mTableLayoutPanel.SetColumnSpan(this.captionLabel, 3);
            this.captionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.captionLabel.Name = "captionLabel";
            // 
            // mTableLayoutPanel
            // 
            resources.ApplyResources(this.mTableLayoutPanel, "mTableLayoutPanel");
            this.mTableLayoutPanel.Controls.Add(this.detailsRichTextBox, 0, 4);
            this.mTableLayoutPanel.Controls.Add(this.copyButton, 2, 2);
            this.mTableLayoutPanel.Controls.Add(this.captionLabel, 1, 0);
            this.mTableLayoutPanel.Controls.Add(this.messageRichTextBox, 1, 1);
            this.mTableLayoutPanel.Controls.Add(this.detailsButton, 0, 2);
            this.mTableLayoutPanel.Controls.Add(this.iconPictureBox, 0, 1);
            this.mTableLayoutPanel.Controls.Add(this.separatorLabel, 0, 3);
            this.mTableLayoutPanel.Controls.Add(this.closeButton, 3, 2);
            this.mTableLayoutPanel.Name = "mTableLayoutPanel";
            // 
            // detailsRichTextBox
            // 
            this.mTableLayoutPanel.SetColumnSpan(this.detailsRichTextBox, 4);
            resources.ApplyResources(this.detailsRichTextBox, "detailsRichTextBox");
            this.detailsRichTextBox.Name = "detailsRichTextBox";
            // 
            // copyButton
            // 
            resources.ApplyResources(this.copyButton, "copyButton");
            this.copyButton.Name = "copyButton";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // messageRichTextBox
            // 
            this.messageRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mTableLayoutPanel.SetColumnSpan(this.messageRichTextBox, 3);
            this.messageRichTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            resources.ApplyResources(this.messageRichTextBox, "messageRichTextBox");
            this.messageRichTextBox.ForeColor = System.Drawing.Color.DimGray;
            this.messageRichTextBox.Name = "messageRichTextBox";
            this.messageRichTextBox.ReadOnly = true;
            // 
            // separatorLabel
            // 
            resources.ApplyResources(this.separatorLabel, "separatorLabel");
            this.separatorLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mTableLayoutPanel.SetColumnSpan(this.separatorLabel, 4);
            this.separatorLabel.Name = "separatorLabel";
            // 
            // mPanel
            // 
            this.mPanel.Controls.Add(this.mTableLayoutPanel);
            resources.ApplyResources(this.mPanel, "mPanel");
            this.mPanel.Name = "mPanel";
            // 
            // iconImageList
            // 
            this.iconImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconImageList.ImageStream")));
            this.iconImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.iconImageList.Images.SetKeyName(0, "Error.png");
            this.iconImageList.Images.SetKeyName(1, "Warning.png");
            // 
            // ErrorForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.Controls.Add(this.mPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.errorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.mTableLayoutPanel.ResumeLayout(false);
            this.mTableLayoutPanel.PerformLayout();
            this.mPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.PictureBox iconPictureBox;
        private System.Windows.Forms.Button detailsButton;
        private System.Windows.Forms.Label captionLabel;
        private System.Windows.Forms.TableLayoutPanel mTableLayoutPanel;
        private System.Windows.Forms.Panel mPanel;
        private System.Windows.Forms.RichTextBox messageRichTextBox;
        private System.Windows.Forms.ImageList iconImageList;
        private System.Windows.Forms.ImageList arrowImageList;
        private System.Windows.Forms.RichTextBox detailsRichTextBox;
        private System.Windows.Forms.Button copyButton;
        private System.Windows.Forms.Label separatorLabel;
    }
}