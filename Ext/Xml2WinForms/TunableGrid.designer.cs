namespace Xml2WinForms
{
    partial class TunableGrid
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TunableGrid));
            this.mDataGridView = new System.Windows.Forms.DataGridView();
            this.navigatorToolStrip = new System.Windows.Forms.ToolStrip();
            this.moveFirstButton = new System.Windows.Forms.ToolStripButton();
            this.navigatorLeftSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.movePreviousButton = new System.Windows.Forms.ToolStripButton();
            this.positionComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.countLabel = new System.Windows.Forms.ToolStripLabel();
            this.moveNextButton = new System.Windows.Forms.ToolStripButton();
            this.navigatorRightSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.moveLastButton = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.mDataGridView)).BeginInit();
            this.navigatorToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mDataGridView
            // 
            this.mDataGridView.AllowUserToOrderColumns = true;
            this.mDataGridView.AllowUserToResizeRows = false;
            this.mDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.mDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.mDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mDataGridView.EnableHeadersVisualStyles = false;
            this.mDataGridView.Location = new System.Drawing.Point(0, 0);
            this.mDataGridView.Name = "mDataGridView";
            this.mDataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.mDataGridView.RowHeadersWidth = 25;
            this.mDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.mDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.mDataGridView.Size = new System.Drawing.Size(505, 264);
            this.mDataGridView.TabIndex = 0;
            this.mDataGridView.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.mDataGridView_CellBeginEdit);
            this.mDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.mDataGridView_CellContentClick);
            this.mDataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.mDataGridView_CellEndEdit);
            this.mDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.mDataGridView_CellMouseDoubleClick);
            this.mDataGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.mDataGridView_ColumnHeaderMouseClick);
            this.mDataGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.mDataGridView_RowsRemoved);
            this.mDataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mDataGridView_MouseDown);
            // 
            // navigatorToolStrip
            // 
            this.navigatorToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.navigatorToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveFirstButton,
            this.navigatorLeftSeparator,
            this.movePreviousButton,
            this.positionComboBox,
            this.countLabel,
            this.moveNextButton,
            this.navigatorRightSeparator,
            this.moveLastButton});
            this.navigatorToolStrip.Location = new System.Drawing.Point(0, 239);
            this.navigatorToolStrip.Name = "navigatorToolStrip";
            this.navigatorToolStrip.Size = new System.Drawing.Size(505, 25);
            this.navigatorToolStrip.TabIndex = 1;
            this.navigatorToolStrip.Visible = false;
            // 
            // moveFirstButton
            // 
            this.moveFirstButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.moveFirstButton.Enabled = false;
            this.moveFirstButton.Image = ((System.Drawing.Image)(resources.GetObject("moveFirstButton.Image")));
            this.moveFirstButton.Name = "moveFirstButton";
            this.moveFirstButton.RightToLeftAutoMirrorImage = true;
            this.moveFirstButton.Size = new System.Drawing.Size(23, 22);
            this.moveFirstButton.Text = "Первая страница";
            this.moveFirstButton.Click += new System.EventHandler(this.moveFirstButton_Click);
            // 
            // navigatorLeftSeparator
            // 
            this.navigatorLeftSeparator.Name = "navigatorLeftSeparator";
            this.navigatorLeftSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // movePreviousButton
            // 
            this.movePreviousButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.movePreviousButton.Enabled = false;
            this.movePreviousButton.Image = ((System.Drawing.Image)(resources.GetObject("movePreviousButton.Image")));
            this.movePreviousButton.Name = "movePreviousButton";
            this.movePreviousButton.RightToLeftAutoMirrorImage = true;
            this.movePreviousButton.Size = new System.Drawing.Size(23, 22);
            this.movePreviousButton.Text = "Назад";
            this.movePreviousButton.Click += new System.EventHandler(this.movePreviousButton_Click);
            // 
            // positionComboBox
            // 
            this.positionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.positionComboBox.Name = "positionComboBox";
            this.positionComboBox.Size = new System.Drawing.Size(75, 25);
            this.positionComboBox.ToolTipText = "Current position";
            this.positionComboBox.SelectedIndexChanged += new System.EventHandler(this.positionComboBox_SelectedIndexChanged);
            // 
            // countLabel
            // 
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(28, 22);
            this.countLabel.Text = "из 1";
            this.countLabel.ToolTipText = "Количество страниц";
            // 
            // moveNextButton
            // 
            this.moveNextButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.moveNextButton.Enabled = false;
            this.moveNextButton.Image = ((System.Drawing.Image)(resources.GetObject("moveNextButton.Image")));
            this.moveNextButton.Name = "moveNextButton";
            this.moveNextButton.RightToLeftAutoMirrorImage = true;
            this.moveNextButton.Size = new System.Drawing.Size(23, 22);
            this.moveNextButton.Text = "Далее";
            this.moveNextButton.Click += new System.EventHandler(this.moveNextButton_Click);
            // 
            // navigatorRightSeparator
            // 
            this.navigatorRightSeparator.Name = "navigatorRightSeparator";
            this.navigatorRightSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // moveLastButton
            // 
            this.moveLastButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.moveLastButton.Enabled = false;
            this.moveLastButton.Image = ((System.Drawing.Image)(resources.GetObject("moveLastButton.Image")));
            this.moveLastButton.Name = "moveLastButton";
            this.moveLastButton.RightToLeftAutoMirrorImage = true;
            this.moveLastButton.Size = new System.Drawing.Size(23, 22);
            this.moveLastButton.Text = "Последняя страница";
            this.moveLastButton.Click += new System.EventHandler(this.moveLastButton_Click);
            // 
            // TunableGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mDataGridView);
            this.Controls.Add(this.navigatorToolStrip);
            this.Name = "TunableGrid";
            this.Size = new System.Drawing.Size(505, 264);
            ((System.ComponentModel.ISupportInitialize)(this.mDataGridView)).EndInit();
            this.navigatorToolStrip.ResumeLayout(false);
            this.navigatorToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView mDataGridView;
        private System.Windows.Forms.ToolStrip navigatorToolStrip;
        private System.Windows.Forms.ToolStripButton moveFirstButton;
        private System.Windows.Forms.ToolStripSeparator navigatorLeftSeparator;
        private System.Windows.Forms.ToolStripButton movePreviousButton;
        private System.Windows.Forms.ToolStripLabel countLabel;
        private System.Windows.Forms.ToolStripButton moveNextButton;
        private System.Windows.Forms.ToolStripSeparator navigatorRightSeparator;
        private System.Windows.Forms.ToolStripButton moveLastButton;
        private System.Windows.Forms.ToolStripComboBox positionComboBox;


    }
}
