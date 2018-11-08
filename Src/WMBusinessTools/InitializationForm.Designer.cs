namespace WMBusinessTools
{
    partial class InitializationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitializationForm));
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.mBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.precompileTabPage = new System.Windows.Forms.TabPage();
            this.precompileLabel = new System.Windows.Forms.Label();
            this.precompileProgressBar = new System.Windows.Forms.ProgressBar();
            this.eulaTabPage = new System.Windows.Forms.TabPage();
            this.notAcceptAgreementRadioButton = new System.Windows.Forms.RadioButton();
            this.acceptAgreementRadioButton = new System.Windows.Forms.RadioButton();
            this.eulaRichTextBox = new System.Windows.Forms.RichTextBox();
            this.mTabControl = new System.Windows.Forms.TabControl();
            this.settingsTabPage = new System.Windows.Forms.TabPage();
            this.optionsLabel = new System.Windows.Forms.Label();
            this.notPrecompileRadioButton = new System.Windows.Forms.RadioButton();
            this.precompileRadioButton = new System.Windows.Forms.RadioButton();
            this.precompileTimer = new System.Windows.Forms.Timer(this.components);
            this.bottomPanel.SuspendLayout();
            this.precompileTabPage.SuspendLayout();
            this.eulaTabPage.SuspendLayout();
            this.mTabControl.SuspendLayout();
            this.settingsTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // bottomPanel
            // 
            this.bottomPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.bottomPanel.Controls.Add(this.cancelButton);
            this.bottomPanel.Controls.Add(this.backButton);
            this.bottomPanel.Controls.Add(this.nextButton);
            resources.ApplyResources(this.bottomPanel, "bottomPanel");
            this.bottomPanel.Name = "bottomPanel";
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // backButton
            // 
            resources.ApplyResources(this.backButton, "backButton");
            this.backButton.Name = "backButton";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // nextButton
            // 
            resources.ApplyResources(this.nextButton, "nextButton");
            this.nextButton.Name = "nextButton";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // mBackgroundWorker
            // 
            this.mBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.mBackgroundWorker_DoWork);
            this.mBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.mBackgroundWorker_RunWorkerCompleted);
            // 
            // precompileTabPage
            // 
            this.precompileTabPage.Controls.Add(this.precompileLabel);
            this.precompileTabPage.Controls.Add(this.precompileProgressBar);
            resources.ApplyResources(this.precompileTabPage, "precompileTabPage");
            this.precompileTabPage.Name = "precompileTabPage";
            this.precompileTabPage.UseVisualStyleBackColor = true;
            // 
            // precompileLabel
            // 
            resources.ApplyResources(this.precompileLabel, "precompileLabel");
            this.precompileLabel.Name = "precompileLabel";
            // 
            // precompileProgressBar
            // 
            resources.ApplyResources(this.precompileProgressBar, "precompileProgressBar");
            this.precompileProgressBar.Maximum = 600;
            this.precompileProgressBar.Name = "precompileProgressBar";
            this.precompileProgressBar.Step = 1;
            // 
            // eulaTabPage
            // 
            this.eulaTabPage.Controls.Add(this.notAcceptAgreementRadioButton);
            this.eulaTabPage.Controls.Add(this.acceptAgreementRadioButton);
            this.eulaTabPage.Controls.Add(this.eulaRichTextBox);
            resources.ApplyResources(this.eulaTabPage, "eulaTabPage");
            this.eulaTabPage.Name = "eulaTabPage";
            this.eulaTabPage.UseVisualStyleBackColor = true;
            // 
            // notAcceptAgreementRadioButton
            // 
            resources.ApplyResources(this.notAcceptAgreementRadioButton, "notAcceptAgreementRadioButton");
            this.notAcceptAgreementRadioButton.Name = "notAcceptAgreementRadioButton";
            this.notAcceptAgreementRadioButton.UseVisualStyleBackColor = true;
            this.notAcceptAgreementRadioButton.CheckedChanged += new System.EventHandler(this.notAcceptAgreementRadioButton_CheckedChanged);
            // 
            // acceptAgreementRadioButton
            // 
            resources.ApplyResources(this.acceptAgreementRadioButton, "acceptAgreementRadioButton");
            this.acceptAgreementRadioButton.Checked = true;
            this.acceptAgreementRadioButton.Name = "acceptAgreementRadioButton";
            this.acceptAgreementRadioButton.TabStop = true;
            this.acceptAgreementRadioButton.UseVisualStyleBackColor = true;
            // 
            // eulaRichTextBox
            // 
            resources.ApplyResources(this.eulaRichTextBox, "eulaRichTextBox");
            this.eulaRichTextBox.Name = "eulaRichTextBox";
            // 
            // mTabControl
            // 
            this.mTabControl.Controls.Add(this.eulaTabPage);
            this.mTabControl.Controls.Add(this.settingsTabPage);
            this.mTabControl.Controls.Add(this.precompileTabPage);
            resources.ApplyResources(this.mTabControl, "mTabControl");
            this.mTabControl.Name = "mTabControl";
            this.mTabControl.SelectedIndex = 0;
            this.mTabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.mTabControl.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.mTabControl_Selecting);
            // 
            // settingsTabPage
            // 
            this.settingsTabPage.Controls.Add(this.optionsLabel);
            this.settingsTabPage.Controls.Add(this.notPrecompileRadioButton);
            this.settingsTabPage.Controls.Add(this.precompileRadioButton);
            resources.ApplyResources(this.settingsTabPage, "settingsTabPage");
            this.settingsTabPage.Name = "settingsTabPage";
            this.settingsTabPage.UseVisualStyleBackColor = true;
            // 
            // optionsLabel
            // 
            resources.ApplyResources(this.optionsLabel, "optionsLabel");
            this.optionsLabel.Name = "optionsLabel";
            // 
            // notPrecompileRadioButton
            // 
            resources.ApplyResources(this.notPrecompileRadioButton, "notPrecompileRadioButton");
            this.notPrecompileRadioButton.Name = "notPrecompileRadioButton";
            this.notPrecompileRadioButton.UseVisualStyleBackColor = true;
            // 
            // precompileRadioButton
            // 
            resources.ApplyResources(this.precompileRadioButton, "precompileRadioButton");
            this.precompileRadioButton.Checked = true;
            this.precompileRadioButton.Name = "precompileRadioButton";
            this.precompileRadioButton.TabStop = true;
            this.precompileRadioButton.UseVisualStyleBackColor = true;
            this.precompileRadioButton.CheckedChanged += new System.EventHandler(this.precompileRadioButton_CheckedChanged);
            // 
            // precompileTimer
            // 
            this.precompileTimer.Interval = 600;
            this.precompileTimer.Tick += new System.EventHandler(this.precompileTimer_Tick);
            // 
            // InitializationForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mTabControl);
            this.Controls.Add(this.bottomPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InitializationForm";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.InitializationForm_Load);
            this.bottomPanel.ResumeLayout(false);
            this.precompileTabPage.ResumeLayout(false);
            this.eulaTabPage.ResumeLayout(false);
            this.eulaTabPage.PerformLayout();
            this.mTabControl.ResumeLayout(false);
            this.settingsTabPage.ResumeLayout(false);
            this.settingsTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Button nextButton;
        private System.ComponentModel.BackgroundWorker mBackgroundWorker;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.TabPage precompileTabPage;
        private System.Windows.Forms.TabPage eulaTabPage;
        private System.Windows.Forms.TabControl mTabControl;
        private System.Windows.Forms.TabPage settingsTabPage;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label precompileLabel;
        private System.Windows.Forms.ProgressBar precompileProgressBar;
        private System.Windows.Forms.Label optionsLabel;
        private System.Windows.Forms.RadioButton notPrecompileRadioButton;
        private System.Windows.Forms.RadioButton precompileRadioButton;
        private System.Windows.Forms.Timer precompileTimer;
        private System.Windows.Forms.RadioButton notAcceptAgreementRadioButton;
        private System.Windows.Forms.RadioButton acceptAgreementRadioButton;
        private System.Windows.Forms.RichTextBox eulaRichTextBox;
    }
}