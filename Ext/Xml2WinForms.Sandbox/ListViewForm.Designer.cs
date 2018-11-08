namespace Xml2WinForms.Sandbox
{
    partial class ListViewForm
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
            this.listViewWithCommandBar1 = new Xml2WinForms.ListScreen();
            this.SuspendLayout();
            // 
            // listViewWithCommandBar1
            // 
            this.listViewWithCommandBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewWithCommandBar1.Location = new System.Drawing.Point(0, 0);
            this.listViewWithCommandBar1.Name = "listViewWithCommandBar1";
            this.listViewWithCommandBar1.RefreshCallback = null;
            this.listViewWithCommandBar1.Size = new System.Drawing.Size(871, 463);
            this.listViewWithCommandBar1.TabIndex = 0;
            // 
            // ListViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 463);
            this.Controls.Add(this.listViewWithCommandBar1);
            this.Name = "ListViewForm";
            this.Text = "ListViewForm";
            this.Load += new System.EventHandler(this.ListViewForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ListScreen listViewWithCommandBar1;
    }
}