using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LocalizationAssistant;
using Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;
using WMBusinessTools.Extensions.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.DisplayHelpers.Origins;
using WMBusinessTools.Extensions.Templates.Controls;
using Xml2WinForms;
using Xml2WinForms.Templates;
using Xml2WinForms.Utils;

namespace WMBusinessTools.Extensions.Forms
{
    internal sealed partial class TransferRegisterForm : Form
    {
        private const string XmlExtension = "xml";
        private const string XlsxExtension = "xlsx";
        private const int XmlExtensionFilterIndex = 1;
        private const int XlsxExtensionFilterIndex = 2;

        private readonly SessionContext _context;
        private readonly ICurrencyService _currencyService;
        private readonly IFormattingService _formattingService;

        private string _fileName;

        public TransferRegisterForm(SessionContext context, string accountNumber = null)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            _currencyService = context.UnityContainer.Resolve<ICurrencyService>();
            _formattingService = context.UnityContainer.Resolve<IFormattingService>();

            InitializeComponent();

            var origin = new AccountDropDownListOrigin(context.UnityContainer);
            origin.FilterCriteria.CurrencyCapabilities = CurrencyCapabilities.Actual | CurrencyCapabilities.Transfer;

            if (null != accountNumber)
                origin.SelectedAccountNumber = accountNumber;

            var itemTemplates = AccountDisplayHelper.BuildAccountDropDownListItemTemplates(origin);

            var accountDropDownListTemplate = new AccountDropDownListTemplate("[empty]");
            accountDropDownListTemplate.Items.AddRange(itemTemplates);

            sourcePurseDropDownList.BeginUpdate();
            sourcePurseDropDownList.Items.Clear();
            sourcePurseDropDownList.ApplyTemplate(accountDropDownListTemplate);
            sourcePurseDropDownList.EndUpdate();

            mTunableGrid.ParseCellValue += ParseCellValue;

            bundleNameTextBox.Click += (sender, args) =>
            {
                mErrorProvider.SetError(bundleNameTextBox, null);
            };
        }

        public void ApplyTemlate(TunableGridTemplate template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            mTunableGrid.ApplyTemplate(template);
        }

        // События элементов формы
        private void TransferRegisterForm_Load(object sender, EventArgs e)
        {
            FormUtils.MoveToCenterParent(this);
            UpdateTotal();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK != mOpenFileDialog.ShowDialog())
                return;

            var fileName = mOpenFileDialog.FileName;

            if (null == fileName)
                throw new InvalidOperationException("null == fileName");

            if (XmlExtensionFilterIndex == mOpenFileDialog.FilterIndex)
                fileName = Path.ChangeExtension(fileName, XmlExtension);
            else if (XlsxExtensionFilterIndex == mOpenFileDialog.FilterIndex)
                fileName = Path.ChangeExtension(fileName, XlsxExtension);
            else
                throw new InvalidOperationException("mOpenFileDialog.FilterIndex == " + mOpenFileDialog.FilterIndex);

            var fileExtension = Path.GetExtension(fileName).ToLower();

            if (fileExtension.StartsWith("."))
                fileExtension = fileExtension.Remove(0, 1);

            List<OriginalPayment> originalPayments;

            switch (fileExtension)
            {
                case XmlExtension:
                {
                    List<IOriginalTransfer> originalTransfers;

                    try
                    {
                        originalTransfers = _context.UnityContainer.Resolve<IImportExportService>()
                            .LoadExportableTransfers(fileName)
                            .ToList();
                    }
                    catch (Exception exception)
                    {
                        ShowWrongFileFormatError(fileName, exception);
                        return;
                    }

                    originalPayments = new List<OriginalPayment>();

                    foreach (var transfer in originalTransfers)
                    {
                        var payment = new OriginalPayment
                        {
                            PaymentId = transfer.PaymentId,
                            TargetPurse = transfer.TargetPurse,
                            Amount = transfer.Amount,
                            Description = transfer.Description
                        };

                        originalPayments.Add(payment);
                    }
                }
                    break;
                case XlsxExtension:
                {
                    try
                    {
                        originalPayments = _context.UnityContainer.Resolve<IImportExportService>()
                            .Load<OriginalPayment>(fileName)
                            .ToList();
                        }
                    catch (Exception exception)
                    {
                        ShowWrongFileFormatError(fileName, exception);
                        return;
                    }
                }
                    break;
                default:
                    throw new InvalidOperationException("fileExtension == " + fileExtension);
            }

            var contentList = originalPayments.Select(op => new GridRowContent(op.PaymentId.ToString(), op))
                .ToList();
            mTunableGrid.DisplayContent(contentList);

            UpdateTotal();

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == _fileName)
                saveAsToolStripMenuItem_Click(this, null);
            else
                Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK != mSaveFileDialog.ShowDialog(this))
                return;

            var fileName = mSaveFileDialog.FileName;

            if (XmlExtensionFilterIndex == mSaveFileDialog.FilterIndex)
                fileName = Path.ChangeExtension(fileName, XmlExtension);
            else if (XlsxExtensionFilterIndex == mSaveFileDialog.FilterIndex)
                fileName = Path.ChangeExtension(fileName, XlsxExtension);

            _fileName = fileName;

            Save();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private Tuple<string, object> ParseCellValue(string value, string columnName)
        {
            if (null == value)
                return null;

            value = value.Trim();

            switch (columnName)
            {
                case "PaymentId":
                {
                    if (!int.TryParse(value, out var paymentId))
                        paymentId = _context.Session.SettingsService.AllocateTransferId();

                    return new Tuple<string, object>(paymentId.ToString(), paymentId);
                }
                case "TargetPurse":
                {
                    if (string.IsNullOrEmpty(value))
                        return null;

                    value = value.ToUpper();

                    string currency;

                    try
                    {
                        currency = _currencyService.ObtainCurrencyByAccountNumber(value);
                    }
                    catch (FormatException)
                    {
                        return null;
                    }

                    var purseCurrency = _currencyService.ObtainCurrencyByAccountNumber(GetAccountNumber());

                    if (!purseCurrency.Equals(currency))
                        return null;

                    if (!_currencyService.CheckCapabilities(currency, CurrencyCapabilities.Transfer))
                        return null;

                    sourcePurseDropDownList.Enabled = false;

                    return new Tuple<string, object>(value, value);
                }
                case "Amount":
                {
                    value = value.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    value = value.Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

                    if (!decimal.TryParse(value, out var amount))
                        return null;

                    return new Tuple<string, object>(_formattingService.FormatAmount(amount), amount);
                }
                case "Description":
                {
                    if (string.IsNullOrEmpty(value))
                        return null;

                    return new Tuple<string, object>(value, value);
                }
            }

            return null;
        }

        private void mTunableGrid_CellValueChanged(object sender, ValueChangedEventArgs e)
        {
            UpdateTotal();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(bundleNameTextBox.Text))
            {
                mErrorProvider.SetError(bundleNameTextBox,
                    Translator.Instance.Translate(ExtensionCatalog.Registration, "Bundle name can not be empty."));

                return;
            }

            var payments = mTunableGrid.MapRows<OriginalPayment>().Where(CheckOriginalPayment).ToList();

            if (0 == payments.Count)
            {
                var caption = Translator.Instance.Translate(ExtensionCatalog.TransferRegister, "Error");
                var message = Translator.Instance.Translate(ExtensionCatalog.TransferRegister,
                    "The list of payments is empty!");

                MessageBox.Show(this, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var settingsService = _context.Session.SettingsService;

            var originalTransfers = payments.Select(p =>
            {
                var transferId = settingsService.AllocateTransferId();

                return new OriginalTransfer(transferId, GetAccountNumber(), p.TargetPurse, p.Amount,
                    p.Description);
            });

            var transferBundleService = _context.UnityContainer.Resolve<ITransferBundleService>();
            transferBundleService.RegisterBundle(originalTransfers, bundleNameTextBox.Text);

            EventBroker.OnTransferBundleCreated();

            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Вспомогательные методы
        private string GetAccountNumber()
        {
            return ((AccountDropDownListItem) sourcePurseDropDownList.SelectedItem).Number;
        }

        private void UpdateTotal()
        {
            var payments = mTunableGrid.MapRows<OriginalPayment>();

            int count = 0;
            decimal total = 0M;

            foreach (var payment in payments)
            {
                if (!CheckOriginalPayment(payment))
                    continue;

                count++;
                total += payment.Amount;
            }

            countToolStripStatusLabel.Text = string.Format(CultureInfo.CurrentUICulture, "CNT={0}", count);
            totalAmountToolStripStatusLabel.Text = string.Format(CultureInfo.CurrentUICulture, "SUM={0}",
                _formattingService.FormatAmount(total));

            if (count > 0)
            {
                openToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
            }
            else
            {
                openToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = false;
            }
        }

        private void Save()
        {
            if (null == _fileName)
                throw new InvalidOperationException("null == _fileName");

            var originalPayments = mTunableGrid.MapRows<OriginalPayment>().Where(CheckOriginalPayment).ToList();

            var fileExtension = Path.GetExtension(_fileName).ToLower();

            if (fileExtension.StartsWith("."))
                fileExtension = fileExtension.Remove(0, 1);

            switch (fileExtension)
            {
                case XmlExtension:
                {
                    var originalTransfers = new List<IOriginalTransfer>();

                    foreach (var originalPayment in originalPayments)
                    {
                        originalTransfers.Add(new OriginalTransfer(originalPayment.PaymentId, string.Empty,
                            originalPayment.TargetPurse,
                            originalPayment.Amount, originalPayment.Description));
                    }

                    _context.UnityContainer.Resolve<IImportExportService>()
                        .SaveExportableTransfers(originalTransfers, _fileName);
                }
                    break;
                case XlsxExtension:
                {
                    _context.UnityContainer.Resolve<IImportExportService>()
                        .Save(originalPayments, _fileName);
                }
                    break;
                default:
                    throw new InvalidOperationException("fileExtension == " + fileExtension);
            }
        }

        private static bool CheckOriginalPayment(OriginalPayment payment)
        {
            if (null == payment)
                return false;

            if (null == payment.TargetPurse)
                return false;

            if (0 == payment.Amount)
                return false;

            if (null == payment.Description)
                return false;

            return true;
        }

        private void ShowWrongFileFormatError(string fileName, Exception exception)
        {
            var caption = "Wrong file format";
            var message = $"File '{fileName}' has a wrong format!";

            var errorContext = new ErrorContext(caption, message)
            {
                Details = exception.ToString()
            };

            _context.ExtensionManager.GetErrorFormProvider().GetForm(errorContext).ShowDialog(this);
        }
    }
}