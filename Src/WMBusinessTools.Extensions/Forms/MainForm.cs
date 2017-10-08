using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using log4net;
using LocalizationAssistant;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.Properties;
using WMBusinessTools.Extensions.Utils;
using Xml2WinForms;

namespace WMBusinessTools.Extensions.Forms
{
    internal sealed partial class MainForm : Form, IScreenContainer
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MainForm));

        private readonly SessionContext _sessionContext;
        private readonly IIdentifierService _identifierService;
        private readonly IFormattingService _formattingService;

        private int _processCount;

        public MainForm(SessionContext sessionContext)
        {
            _sessionContext = sessionContext ?? throw new ArgumentNullException(nameof(sessionContext));
            InitializeComponent();

            _identifierService = _sessionContext.UnityContainer.Resolve<IIdentifierService>();
            _formattingService = _sessionContext.UnityContainer.Resolve<IFormattingService>();

            UpdateComboBox();
            BuildMainMenu();
            BuildScreens();

            removeIdentifierButton.Enabled = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            WindowState = Settings.Default.MainFormWindowState;

            if (FormWindowState.Normal == WindowState)
            {
                Location = Settings.Default.MainFormLocation;
                Size = Settings.Default.MainFormSize;
            }

            EventBroker.DatabaseChanged += EventBrokerOnDatabaseChanged;
            EventBroker.LanguageChanged += EventBrokerOnLanguageChanged;
        }

        private void UpdateComboBox(long? selectIdentifier = null)
        {
            var identifiers = _identifierService.SelectIdentifiers();

            identifierComboBox.BeginUpdate(); // BeginUpdate

            identifierComboBox.Items.Clear();

            int index = 0;
            int selectedIndex = 0;

            foreach (var identifierInfo in identifiers)
            {
                var text = IdentifierDisplayHelper.FormatIdentifierWithAlias(_formattingService,
                    identifierInfo.Identifier, identifierInfo.IsMaster ? null : identifierInfo.IdentifierAlias);

                identifierComboBox.Items.Add(new ComboBoxItem(text, identifierInfo));

                if (null != selectIdentifier && selectIdentifier.Value == identifierInfo.Identifier)
                    selectedIndex = index;

                index++;
            }

            identifierComboBox.EndUpdate(); // EndUpdate

            if (identifierComboBox.Items.Count > 0)
                identifierComboBox.SelectedIndex = selectedIndex;
        }

        private void identifierComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null == identifierComboBox.SelectedItem)
                return;

            var identifierSummary = (IIdentifierSummary) ((ComboBoxItem) identifierComboBox.SelectedItem).Value;

            if (_sessionContext.Session.CurrentIdentifier == identifierSummary.Identifier)
                return;

            _sessionContext.Session.CurrentIdentifier = identifierSummary.Identifier;

            BuildMainMenu();
            BuildScreens();

            removeIdentifierButton.Enabled = !_sessionContext.Session.IsMaster();
        }

        private void infoButton_Click(object sender, EventArgs e)
        {
            IdentifierDisplayHelper.ShowFindCertificateForm(this, _sessionContext,
                _formattingService.FormatIdentifier(_sessionContext.Session.CurrentIdentifier));
        }

        private void addIdentifierButton_Click(object sender, EventArgs e)
        {
            var formProvider =
                _sessionContext.ExtensionManager.TryGetTopFormProvider(ExtensionCatalog.AddIdentifier);

            if (DialogResult.OK != formProvider?.GetForm(_sessionContext).ShowDialog(this))
                return;

            removeIdentifierButton.Enabled = _sessionContext.Session.IsMaster();

            UpdateComboBox(_sessionContext.Session.CurrentIdentifier);
            BuildMainMenu();
            BuildScreens();
        }

        private void removeIdentifierButton_Click(object sender, EventArgs e)
        {
            var identifierSummary = (IIdentifierSummary)((ComboBoxItem)identifierComboBox.SelectedItem).Value;

            var identifierValue = _formattingService.FormatIdentifier(identifierSummary.Identifier);

            var message = string.Format(CultureInfo.InvariantCulture, "{0} {1} {2}",
                Translator.Instance.Translate(ExtensionCatalog.Main, "WMID"), identifierValue,
                Translator.Instance.Translate(ExtensionCatalog.Main, "will be deleted."));

            if (DialogResult.OK != MessageBox.Show(this, message,
                    Translator.Instance.Translate(ExtensionCatalog.Main, "Сonfirm deletion"), MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
                return;

            _identifierService.RemoveSecondaryIdentifier(identifierSummary.Identifier);
            removeIdentifierButton.Enabled = false;

            UpdateComboBox();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sessionContext.ExtensionManager.TryGetTopActionProvider(ExtensionCatalog.Content)
                ?.RunAction(_sessionContext);
        }

        private void technicalSupportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sessionContext.ExtensionManager.TryGetTopFormProvider(ExtensionCatalog.SendMessageToDeveloper)
                ?.GetForm(_sessionContext)
                .Show(this);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sessionContext.ExtensionManager.TryGetTopFormProvider(ExtensionCatalog.About)
                ?.GetForm(_sessionContext)
                .ShowDialog(this);
        }

        private void BuildMainMenu()
        {
            mMenuStrip.SuspendLayout(); // SuspendLayout

            foreach (ToolStripItem toolStripItem in toolsToolStripMenuItem.DropDownItems.Cast<ToolStripItem>().ToList())
            {
                toolStripItem.Dispose();
            }

            foreach (ToolStripItem toolStripItem in optionsToolStripMenuItem.DropDownItems.Cast<ToolStripItem>()
                .ToList())
            {
                toolStripItem.Dispose();
            }

            toolsToolStripMenuItem.DropDownItems.Clear();
            optionsToolStripMenuItem.DropDownItems.Clear();

            var extensionManager = _sessionContext.ExtensionManager;

            toolsToolStripMenuItem.DropDownItems.AddRange(
                MenuDisplayHelper.BuildToolStripItems(extensionManager, ExtensionCatalog.Tags.TopExtension));
            optionsToolStripMenuItem.DropDownItems.AddRange(
                MenuDisplayHelper.BuildToolStripItems(extensionManager, ExtensionCatalog.Tags.SettingsExtension));

            // Добавляем события
            foreach (ToolStripItem toolStripItem in toolsToolStripMenuItem.DropDownItems)
            {
                AddHandler(toolStripItem);
            }

            foreach (ToolStripItem toolStripItem in optionsToolStripMenuItem.DropDownItems)
            {
                AddHandler(toolStripItem);
            }

            mMenuStrip.ResumeLayout(); // ResumeLayout
        }

        private void AddHandler(ToolStripItem toolStripItem)
        {
            var toolStripMenuItem = toolStripItem as ToolStripMenuItem;

            if (null == toolStripMenuItem)
                return;

            var extensionId = (string) toolStripMenuItem.Tag;

            var formProvider =
                _sessionContext.ExtensionManager.TryGetTopFormProvider(extensionId);

            if (null == formProvider)
            {
                toolStripMenuItem.Enabled = false;
                return;
            }

            switch (extensionId)
            {
                case ExtensionCatalog.GeneralSettings:
                    ChangeSettings(toolStripMenuItem);
                    break;
                default:
                    toolStripMenuItem.Click += (sender, args) =>
                    {
                        formProvider.GetForm(_sessionContext).Show(this);
                    };
                    break;
            }

            if (!formProvider.CheckCompatibility(_sessionContext))
                toolStripMenuItem.Enabled = false;

        }

        private void ChangeSettings(ToolStripMenuItem toolStripMenuItem)
        {
            toolStripMenuItem.Click += (sender, args) =>
            {
                var formProvider =
                    _sessionContext.ExtensionManager.TryGetTopFormProvider(ExtensionCatalog.GeneralSettings);

                if (null == formProvider)
                    return;

                var form = (SettingsForm) formProvider.GetForm(_sessionContext);


                form.Closed += (o, eventArgs) =>
                {
                    if (DialogResult.OK == form.DialogResult)
                    {
                        var settingsCopy = _sessionContext.Session.SettingsService.GetSettings();
                        var settings = (ISettings) form.SelectedObject;

                        _sessionContext.Session.SettingsService.SetSettings(settings);
                        _sessionContext.Session.SettingsService.Save();

                        if (settingsCopy.Language != settings.Language)
                        {
                            LocalizationUtility.ApplyLanguage(settings.Language);

                            var translator = new Translator();
                            translator.Apply();

                            EventBroker.OnLanguageChanged();
                        }
                    }
                };

                form.Show(this);
            };
        }

        private void EventBrokerOnDatabaseChanged(object sender, EventArgs eventArgs)
        {
            UpdateComboBox();
            BuildMainMenu();
            BuildScreens();
        }

        private void EventBrokerOnLanguageChanged(object o, EventArgs eventArgs)
        {
            if (IsDisposed)
                return;

            LocalizationUtility.TranslateForm(this);
            BuildMainMenu();
            BuildScreens();
        }

        private void BuildScreens()
        {
            var extensionInfoList =
                _sessionContext.ExtensionManager.SelectExtensionInfoListByTag(ExtensionCatalog.Tags
                    .TopScreen);

            mTabControl.SuspendLayout(); // SuspendLayout

            foreach (Control control in mTabControl.Controls.Cast<Control>().ToList())
            {
                control.Dispose();
            }

            mTabControl.Controls.Clear();

            foreach (var extensionInfo in extensionInfoList)
            {
                var screenProvider = _sessionContext.ExtensionManager.TryGetTopScreenProvider(extensionInfo.Id);

                if (null == screenProvider)
                    continue;

                var screenContainerContext = new ScreenContainerContext(_sessionContext, this);

                if (!screenProvider.CheckCompatibility(screenContainerContext))
                    continue;

                var control = screenProvider.GetScreen(screenContainerContext);
                control.Dock = DockStyle.Fill;

                var tabPage = new TabPage(Translator.Instance.Translate("Screens", extensionInfo.Name)) { UseVisualStyleBackColor = true };
                tabPage.Controls.Add(control);

                mTabControl.Controls.Add(tabPage);
            }
            mTabControl.ResumeLayout(); // ResumeLayout
        }

        public void OnStartProgress()
        {
            _processCount++;
            UpdateProgressBar();
        }

        public void OnStopProgress()
        {
            _processCount--;
            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            mToolStripProgressBar.Visible = _processCount > 0;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            EventBroker.DatabaseChanged -= EventBrokerOnDatabaseChanged;
            EventBroker.LanguageChanged -= EventBrokerOnLanguageChanged;

            Settings.Default.MainFormWindowState = WindowState;

            if (WindowState == FormWindowState.Normal)
            {
                Settings.Default.MainFormLocation = Location;
                Settings.Default.MainFormSize = Size;
            }

            try
            {
                Settings.Default.Save();
                _sessionContext.Session.SettingsService.Save();
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
            }
        }
    }
}
