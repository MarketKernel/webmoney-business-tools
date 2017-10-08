using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Windows.Forms;
using LocalizationAssistant;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Contracts.Exceptions;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using Xml2WinForms;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.Forms
{
    internal sealed partial class EnterForm : Form
    {
        private readonly EntranceContext _context;
        private readonly IEntranceService _entranceService;
        private readonly IFormattingService _formattingService;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISession Session { get; private set; }

        public EnterForm(EntranceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _entranceService = _context.UnityContainer.Resolve<IEntranceService>();
            _formattingService = _context.UnityContainer.Resolve<IFormattingService>();

            InitializeComponent();

            mTunableList.ServiceCommand += (sender, args) =>
            {
                switch (args.Command)
                {
                    case "Remove":
                        removeButton_Click(this, null);
                        break;
                    case "CellMouseDoubleClick":
                        enterButton_Click(this, null);
                        break;
                }
            };
        }

        private void EnterForm_Load(object sender, EventArgs e)
        {
            // Форма EnterForm более красивая, пусть пользователь начнет с нее.
            //if (0 == mTunableList.Items.Count)
            //    registerButton_Click(this, null);
        }

        public void ApplyTemplate(TunableListTemplate template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            mTunableList.ApplyTemplate(template);
        }

        public void DisplayItems()
        {
            var registrations = _entranceService.SelectRegistrations();

            mTunableList.DisplayContent(registrations.Select(r => new ListItemContent(r)
                {
                    ImageKey = "Ant"
                })
                .ToList());

            mTunableList_SelectedIndexChanged(this, null);
        }

        private void mTunableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isItemSelected = null != mTunableList.CurrentEntity;

            enterButton.Enabled = isItemSelected;
            removeButton.Enabled = isItemSelected;
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            var formProvider =
                _context.ExtensionManager.CreateExtension<IRegistrationFormProvider>(ExtensionCatalog.Registration);

            if (DialogResult.OK == formProvider.GetForm(_context).ShowDialog(this))
                DisplayItems();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            var registration = (IRegistration) mTunableList.CurrentEntity;

            var identifierValue = _formattingService.FormatIdentifier(registration.Identifier);
            var message = string.Format(CultureInfo.InvariantCulture, "{0} {1} {2}",
                Translator.Instance.Translate(ExtensionCatalog.Enter, "WMID"), identifierValue,
                Translator.Instance.Translate(ExtensionCatalog.Enter, "will be deleted permanently."));

            if (DialogResult.OK != MessageBox.Show(this, message,
                    Translator.Instance.Translate(ExtensionCatalog.Enter, "Сonfirm deletion"), MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
                return;

            var entranceService = _context.UnityContainer.Resolve<IEntranceService>();
            entranceService.RemoveRegistration(registration.Identifier);

            DisplayItems();
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            var registration = (IRegistration) mTunableList.CurrentEntity;

            ISession session;

            // Сначала пытаемся открыть без пароля.
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                session = _entranceService.CreateSession(registration.Identifier);
            }
            catch (WrongPasswordException)
            {
                session = null;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            if (null == session)
            {
                while (true)
                {
                    var passwordForm = new PasswordForm();

                    if (DialogResult.OK != passwordForm.ShowDialog(this))
                        return;

                    var securePassword = new SecureString();

                    foreach (var ch in passwordForm.Password)
                    {
                        securePassword.AppendChar(ch);
                    }

                    securePassword.MakeReadOnly();

                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        session = _entranceService.CreateSession(registration.Identifier, securePassword);
                    }
                    catch (WrongPasswordException)
                    {
                        session = null;
                    }
                    finally
                    {
                       Cursor.Current = Cursors.Default;
                    }

                    if (null == session)
                    {
                        var caption = Translator.Instance.Translate(ExtensionCatalog.Enter, "Invalid password");
                        var errorMessage = Translator.Instance.Translate(ExtensionCatalog.Enter,
                            "Could not authenticate user. Verify the username and password, and try again.");

                        if (DialogResult.Cancel == MessageBox.Show(this, errorMessage, caption,
                                MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1))
                            return;

                        continue;
                    }

                    break;
                }
            }

            using (var progressForm = new ProgressForm(_context.UnityContainer, session))
            {
                if (DialogResult.OK != progressForm.ShowDialog(this))
                    return;
            }

            Session = session;
            DialogResult = DialogResult.OK;
        }
    }
}
