namespace Xml2WinForms
{
    partial class TunableShape
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
            this.components = new System.ComponentModel.Container();
            this.mErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.bodyFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.mErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // mErrorProvider
            // 
            this.mErrorProvider.ContainerControl = this;
            // 
            // bodyFlowLayoutPanel
            // 
            this.bodyFlowLayoutPanel.AutoSize = true;
            this.bodyFlowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bodyFlowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.bodyFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.bodyFlowLayoutPanel.MinimumSize = new System.Drawing.Size(10, 10);
            this.bodyFlowLayoutPanel.Name = "bodyFlowLayoutPanel";
            this.bodyFlowLayoutPanel.Size = new System.Drawing.Size(10, 10);
            this.bodyFlowLayoutPanel.TabIndex = 0;
            this.bodyFlowLayoutPanel.WrapContents = false;
            // 
            // TunableShape
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.bodyFlowLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(15, 15);
            this.Name = "TunableShape";
            this.Size = new System.Drawing.Size(15, 15);
            ((System.ComponentModel.ISupportInitialize)(this.mErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ErrorProvider mErrorProvider;
        private System.Windows.Forms.FlowLayoutPanel bodyFlowLayoutPanel;
    }
}
