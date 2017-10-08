namespace Xml2WinForms
{
    partial class TunableTextBoxWithButton
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
            this.mTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.mExtendedTextBox = new Xml2WinForms.TunableTextBox();
            this.mButton = new System.Windows.Forms.Button();
            this.mTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mTableLayoutPanel
            // 
            this.mTableLayoutPanel.ColumnCount = 2;
            this.mTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mTableLayoutPanel.Controls.Add(this.mExtendedTextBox, 0, 0);
            this.mTableLayoutPanel.Controls.Add(this.mButton, 1, 0);
            this.mTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.mTableLayoutPanel.Name = "mTableLayoutPanel";
            this.mTableLayoutPanel.RowCount = 1;
            this.mTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mTableLayoutPanel.Size = new System.Drawing.Size(150, 20);
            this.mTableLayoutPanel.TabIndex = 0;
            // 
            // mExtendedTextBox
            // 
            this.mExtendedTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mExtendedTextBox.DigitsOnly = false;
            this.mExtendedTextBox.Location = new System.Drawing.Point(0, 0);
            this.mExtendedTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.mExtendedTextBox.Name = "mExtendedTextBox";
            this.mExtendedTextBox.Size = new System.Drawing.Size(130, 20);
            this.mExtendedTextBox.TabIndex = 0;
            // 
            // mButton
            // 
            this.mButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.mButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.mButton.Location = new System.Drawing.Point(130, 0);
            this.mButton.Margin = new System.Windows.Forms.Padding(0);
            this.mButton.Name = "mButton";
            this.mButton.Size = new System.Drawing.Size(20, 20);
            this.mButton.TabIndex = 1;
            this.mButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.mButton.UseVisualStyleBackColor = true;
            // 
            // TunableTextBoxWithButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mTableLayoutPanel);
            this.Name = "TunableTextBoxWithButton";
            this.Size = new System.Drawing.Size(150, 20);
            this.mTableLayoutPanel.ResumeLayout(false);
            this.mTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mTableLayoutPanel;
        private TunableTextBox mExtendedTextBox;
        private System.Windows.Forms.Button mButton;
    }
}
