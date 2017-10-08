namespace WMBusinessTools.Extensions.Controls
{
    partial class AmountNumericUpDown
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
            this.currencyLabel = new System.Windows.Forms.Label();
            this.availableAmountLinkLabel = new System.Windows.Forms.LinkLabel();
            this.mNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.mTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // mTableLayoutPanel
            // 
            this.mTableLayoutPanel.ColumnCount = 3;
            this.mTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mTableLayoutPanel.Controls.Add(this.currencyLabel, 1, 0);
            this.mTableLayoutPanel.Controls.Add(this.availableAmountLinkLabel, 2, 0);
            this.mTableLayoutPanel.Controls.Add(this.mNumericUpDown, 0, 0);
            this.mTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.mTableLayoutPanel.Name = "mTableLayoutPanel";
            this.mTableLayoutPanel.RowCount = 1;
            this.mTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mTableLayoutPanel.Size = new System.Drawing.Size(274, 26);
            this.mTableLayoutPanel.TabIndex = 0;
            // 
            // currencyLabel
            // 
            this.currencyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.currencyLabel.AutoSize = true;
            this.currencyLabel.Location = new System.Drawing.Point(175, 6);
            this.currencyLabel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.currencyLabel.Name = "currencyLabel";
            this.currencyLabel.Size = new System.Drawing.Size(34, 13);
            this.currencyLabel.TabIndex = 1;
            this.currencyLabel.Text = "WMZ";
            // 
            // availableAmountLinkLabel
            // 
            this.availableAmountLinkLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.availableAmountLinkLabel.AutoSize = true;
            this.availableAmountLinkLabel.Location = new System.Drawing.Point(212, 6);
            this.availableAmountLinkLabel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.availableAmountLinkLabel.Name = "availableAmountLinkLabel";
            this.availableAmountLinkLabel.Size = new System.Drawing.Size(62, 13);
            this.availableAmountLinkLabel.TabIndex = 2;
            this.availableAmountLinkLabel.TabStop = true;
            this.availableAmountLinkLabel.Text = "<< max 100";
            this.availableAmountLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.availableAmountLinkLabel_LinkClicked);
            // 
            // mNumericUpDown
            // 
            this.mNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mNumericUpDown.DecimalPlaces = 2;
            this.mNumericUpDown.Location = new System.Drawing.Point(0, 3);
            this.mNumericUpDown.Margin = new System.Windows.Forms.Padding(0);
            this.mNumericUpDown.Name = "mNumericUpDown";
            this.mNumericUpDown.Size = new System.Drawing.Size(172, 20);
            this.mNumericUpDown.TabIndex = 0;
            this.mNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // AmountNumericUpDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mTableLayoutPanel);
            this.Name = "AmountNumericUpDown";
            this.Size = new System.Drawing.Size(274, 26);
            this.mTableLayoutPanel.ResumeLayout(false);
            this.mTableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mTableLayoutPanel;
        private System.Windows.Forms.LinkLabel availableAmountLinkLabel;
        private System.Windows.Forms.Label currencyLabel;
        private System.Windows.Forms.NumericUpDown mNumericUpDown;
    }
}
