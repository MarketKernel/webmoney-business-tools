using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LocalizationAssistant;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;
using WMBusinessTools.Extensions.BusinessObjects;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.Templates;
using Xml2WinForms;
using Xml2WinForms.Templates;
using Xml2WinForms.Utils;

namespace WMBusinessTools.Extensions.Forms
{
    internal sealed partial class CertificateForm : Form
    {
        private const string GeneralInformationGroupName = "GeneralInformation";
        private const string CertificateGroupName = "Certificate";
        private const string PersonalDataGroupName = "PersonalData";
        private const string ActualLocationGroupName = "ActualLocation";
        private const string PassportDataGroupName = "PassportData";
        private const string ContactDataGroupName = "ContactData";
        private const string OrganizationGroupName = "Organization";

        private const string VerifiedProtectedImageKey = "VerifiedProtected";
        private const string VerifiedImageKey = "Verified";
        private const string UnverifiedProtectedImageKey = "UnverifiedProtected";
        private const string UnverifiedImageKey = "Unverified";

        private readonly IFormattingService _formattingService;

        [Category("Action"), Description("Service command.")]
        public event EventHandler<CommandEventArgs> ServiceCommand;

        public CertificateForm(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            _formattingService = context.UnityContainer.Resolve<IFormattingService>();

            InitializeComponent();

            certificateTunableList.ServiceCommand += (sender, args) =>
            {
                ServiceCommand?.Invoke(sender, args);
            };

            attachedIdentifierTunableList.ServiceCommand += (sender, args) =>
            {
                ServiceCommand?.Invoke(sender, args);
            };
        }

        private void CertificateForm_Load(object sender, EventArgs e)
        {
            FormUtils.MoveToCenterParent(this);
        }

        public void ApplyTemplate(CertificateFormTemplate template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            if (null == template.CertificateRecordList)
                throw new BadTemplateException("null == template.CertificateRecordList");

            if (null == template.AttachedIdentifierList)
                throw new BadTemplateException("null == template.AttachedIdentifierList");

            certificateTunableList.ApplyTemplate(template.CertificateRecordList);
            attachedIdentifierTunableList.ApplyTemplate(template.AttachedIdentifierList);

            // Command buttons
            if (template.CommandButtons.Count > 0)
            {
                commandBarFlowLayoutPanel.SuspendLayout(); // SuspendLayout

                foreach (var buttonTemplate in template.CommandButtons)
                {
                    var button = new TunableButton
                    {
                        Width = 139
                    };

                    button.ApplyTemplate(buttonTemplate);
                    button.ServiceCommand += (sender, args) => ServiceCommand?.Invoke(sender, args);

                    commandBarFlowLayoutPanel.Controls.Add(button);
                }

                commandBarFlowLayoutPanel.ResumeLayout(); // ResumeLayout

                commandBarGroupBox.Visible = true;
            }
            else
                commandBarGroupBox.Visible = false;
        }

        public void DisplayValue(ICertificate certificate)
        {
            if (null == certificate)
                throw new ArgumentNullException(nameof(certificate));

            var imageKey = certificate.Degree.ToString();

            if (certificate.Revoked)
                imageKey = $"{imageKey}Revoked";

            mPictureBox.Image = certificateImageList.Images[imageKey];

            // 1. Общая информация
            wmIdTextBox.Text = IdentifierDisplayHelper.FormatIdentifierWithAlias(_formattingService,
                certificate.Identifier, certificate.IdentifierAlias);

            certificateTextBox.Text =
                Translator.Instance.Translate(ExtensionCatalog.Certificate, certificate.Degree.ToString());

            if (certificate.Revoked)
                certificateTextBox.Font = new Font(certificateTextBox.Font, FontStyle.Strikeout);

            if (null != certificate.Bl)
            {
                levelsTextBox.Text = $@"BL {certificate.Bl.Value}";

                if (null != certificate.Tl)
                    levelsTextBox.Text += $@"; TL {certificate.Tl.Value}";
            }

            nameTextBox.Text =
                string.Format(CultureInfo.InvariantCulture, "{0} {1} {2}", certificate.FirstName, certificate.Surname,
                    certificate.Patronymic);
            addressTextBox.Text = Combine(certificate.Country, certificate.Region, certificate.City,
                certificate.Address);
            contactsTextBox.Text = Combine(certificate.ContactPhone, certificate.Email, certificate.Icq,
                certificate.WebAddress);

            if (null != certificate.PositiveRating && null != certificate.NegativeRating)
            {
                var pos = Translator.Instance.Translate(ExtensionCatalog.Certificate, "POS");
                var neg = Translator.Instance.Translate(ExtensionCatalog.Certificate, "NEG");
                claimsTextBox.Text =
                    $@"{pos} {certificate.PositiveRating ?? -1}; {neg} {certificate.NegativeRating ?? -1}";
            }

            var certificateListItems = new List<ListItemContent>
            {
                //  WMID
                ToListItemContent(Translator.Instance.Translate(ExtensionCatalog.Certificate, "WMID"),
                    _formattingService.FormatIdentifier(certificate.Identifier), CertificateRecordAspects.Verified,
                    GeneralInformationGroupName),
                // Название проекта, имя в сети
                ToListItemContent(Translator.Instance.Translate(ExtensionCatalog.Certificate, "Nickname"),
                    certificate.IdentifierAlias, certificate.AliasAspects, GeneralInformationGroupName)
            };

            // Дата регистрации в системе

            foreach (IAttachedIdentifierSummary identifierSummary in certificate.AttachedIdentifierSummaries)
                if (identifierSummary.Identifier == certificate.Identifier)
                {
                    certificateListItems.Add(ToListItemContent(
                        Translator.Instance.Translate(ExtensionCatalog.Certificate, "Registration date"),
                        _formattingService.FormatDateTime(identifierSummary.RegistrationDate),
                        CertificateRecordAspects.Verified,
                        GeneralInformationGroupName));
                    break;
                }

            // 2. Аттестат

            // Тип аттестата
            var verificationStatus = certificate.Revoked
                ? Translator.Instance.Translate(ExtensionCatalog.Certificate, "Revoked")
                : Translator.Instance.Translate(ExtensionCatalog.Certificate, certificate.Degree.ToString());

            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Verification status"), verificationStatus,
                CertificateRecordAspects.Verified, CertificateGroupName));

            // Дата получения аттестата
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Date of issue"),
                _formattingService.FormatDateTime(certificate.CreationTime), CertificateRecordAspects.Verified,
                CertificateGroupName));
            // Кем выдан аттестат
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Issued by"),
                Combine(certificate.IssuerIdentifier.ToString("000000000000"), certificate.IssuerAlias),
                CertificateRecordAspects.Verified, CertificateGroupName));

            // 3. Персональные данные владельца аттестата

            // Статус владельца
            var status = Translator.Instance.Translate(ExtensionCatalog.Certificate, certificate.Status.ToString());
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Status"),
                status, CertificateRecordAspects.Verified, PersonalDataGroupName));

            if (CertificateStatus.Entity == certificate.Status)
            {
                // На основании чего действует
                certificateListItems.Add(ToListItemContent(
                    Translator.Instance.Translate(ExtensionCatalog.Certificate, "Based on"),
                    certificate.Basis, certificate.BasisAspects,
                    PersonalDataGroupName));
            }

            // Фамилия
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Last name"),
                certificate.Surname, certificate.SurnameAspects, PersonalDataGroupName));
            // Имя
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "First name"),
                certificate.FirstName, certificate.FirstNameAspects, PersonalDataGroupName));
            // Отчество
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Middle name"),
                certificate.Patronymic, certificate.PatronymicAspects, PersonalDataGroupName));
            // ИНН
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Tax ID"),
                certificate.OrganizationTaxId, certificate.OrganizationTaxIdAspects, PersonalDataGroupName));

            // 4. Фактическое местонахождение владельца аттестата

            // Город/Страна
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Country"),
                certificate.Country, certificate.CountryAspects, ActualLocationGroupName));
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Region"),
                certificate.Region, certificate.RegionAspects, ActualLocationGroupName));
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "City"),
                certificate.City, certificate.CityAspects, ActualLocationGroupName));
            // Почтовый индекс
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Postal code"), certificate.ZipCode,
                certificate.ZipCodeAspects, ActualLocationGroupName));
            // Адрес
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Address"),
                certificate.Address, certificate.AddressAspects, ActualLocationGroupName));

            // 5. Паспортные данные владельца аттестата

            // Номер паспорта
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Passport number"),
                certificate.PassportNumber, certificate.PassportNumberAspects, PassportDataGroupName));
            // Когда выдан
            string dateOfIssue = null;

            if (null != certificate.PassportDate)
                dateOfIssue = _formattingService.FormatDateTime(certificate.PassportDate.Value);

            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Date of issue"),
                dateOfIssue, certificate.PassportDateAspects,
                PassportDataGroupName));
            // Где выдан - Город/Страна
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Issuing country"),
                certificate.PassportCountry, certificate.PassportCountryAspects,
                PassportDataGroupName));
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Issuing city"),
                certificate.PassportCity, certificate.PassportCityAspects, PassportDataGroupName));
            // Кем выдан
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Issued by (issuing authority)"),
                certificate.PassportIssuer, certificate.PassportIssuerAspects, PassportDataGroupName));
            // Дата рождения
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Date of birth"),
                certificate.Birthday.ToString(), certificate.BirthdayAspects, PassportDataGroupName));
            // Место рождения - Город/Страна
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Place of Birth"), certificate.Birthplace,
                certificate.BirthplaceAspects, PassportDataGroupName));
            // Место постоянной регистрации - Город/Страна
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Country"),
                certificate.RegistrationCountry, certificate.RegistrationCountryAspects,
                PassportDataGroupName));
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "City"),
                certificate.RegistrationCity, certificate.RegistrationCityAspects,
                PassportDataGroupName));
            // Место постоянной регистрации - Адрес
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Address"),
                certificate.RegistrationAddress, certificate.RegistrationAddressAspects,
                PassportDataGroupName));

            // 6. Контактная информация владельца аттестата

            // Контактный телефон
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Contact phone."),
                certificate.ContactPhone, certificate.ContactPhoneAspects, ContactDataGroupName));
            // Факс
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Fax"),
                certificate.Fax, certificate.FaxAspects, ContactDataGroupName));
            // Мобильный телефон
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Mobile phone"), certificate.CellPhone,
                certificate.CellPhoneAspects, ContactDataGroupName));
            // Домашний телефон
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Home phone"), certificate.HomePhone,
                certificate.HomePhoneAspects, ContactDataGroupName));
            // E-mail
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Email"),
                certificate.Email, certificate.EmailAspects, ContactDataGroupName));
            // ICQ
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "ICQ UIN"),
                certificate.Icq, certificate.IcqAspects, ContactDataGroupName));
            // Web-сайт
            certificateListItems.Add(ToListItemContent(
                Translator.Instance.Translate(ExtensionCatalog.Certificate, "Web site"),
                certificate.WebAddress, certificate.WebAddressAspects, ContactDataGroupName));

            if (CertificateStatus.Entity == certificate.Status)
            {
                // 7. Информация об организации
                var appointment =
                    Translator.Instance.Translate(ExtensionCatalog.Certificate, certificate.Appointment.ToString());

                certificateListItems.Add(ToListItemContent(
                    Translator.Instance.Translate(ExtensionCatalog.Certificate, "Appointment"), appointment,
                    certificate.AppointmentAspects, OrganizationGroupName));

                // Название организации
                certificateListItems.Add(ToListItemContent(
                    Translator.Instance.Translate(ExtensionCatalog.Certificate, "Organization name"),
                    certificate.OrganizationName, certificate.OrganizationNameAspects, OrganizationGroupName));
                // ИНН
                certificateListItems.Add(ToListItemContent(
                    Translator.Instance.Translate(ExtensionCatalog.Certificate, "Tax ID"),
                    certificate.OrganizationTaxId, certificate.OrganizationTaxIdAspects, OrganizationGroupName));
                // ОКПО
                certificateListItems.Add(ToListItemContent(
                    Translator.Instance.Translate(ExtensionCatalog.Certificate, "Organization ID"),
                    certificate.OrganizationId, certificate.OrganizationIdAspects, OrganizationGroupName));
                // ОКВЭД
                certificateListItems.Add(ToListItemContent(
                    Translator.Instance.Translate(ExtensionCatalog.Certificate, "Activity Number"),
                    certificate.OrganizationActivityId, certificate.OrganizationActivityIdAspects,
                    OrganizationGroupName));
                // Директор (ФИО)
                certificateListItems.Add(ToListItemContent(
                    Translator.Instance.Translate(ExtensionCatalog.Certificate, "Director"),
                    certificate.OrganizationManager, certificate.OrganizationManagerAspects, OrganizationGroupName));
                // Гл. бухгалтер (ФИО)
                certificateListItems.Add(ToListItemContent(
                    Translator.Instance.Translate(ExtensionCatalog.Certificate, "Accountant"),
                    certificate.OrganizationAccountant, certificate.OrganizationAccountantAspects,
                    OrganizationGroupName));
                // Юридический адрес - Город/Страна
                certificateListItems.Add(ToListItemContent(
                    Translator.Instance.Translate(ExtensionCatalog.Certificate, "Country"),
                    certificate.OrganizationCountry, certificate.OrganizationCountryAspects, OrganizationGroupName));
                certificateListItems.Add(ToListItemContent(
                    Translator.Instance.Translate(ExtensionCatalog.Certificate, "City"),
                    certificate.OrganizationCity, certificate.OrganizationCityAspects, OrganizationGroupName));
                // Юридический адрес - Индекс
                certificateListItems.Add(ToListItemContent(
                    Translator.Instance.Translate(ExtensionCatalog.Certificate, "Postal code"),
                    certificate.OrganizationZipCode, certificate.OrganizationZipCodeAspects, OrganizationGroupName));
                // Юридический адрес - Улица, дом
                certificateListItems.Add(ToListItemContent(
                    Translator.Instance.Translate(ExtensionCatalog.Certificate, "Address"),
                    certificate.OrganizationAddress, certificate.OrganizationAddressAspects, OrganizationGroupName));
                // Банк
                certificateListItems.Add(ToListItemContent(
                    Translator.Instance.Translate(ExtensionCatalog.Certificate, "Bank"),
                    certificate.OrganizationBankName, certificate.OrganizationBankNameAspects, OrganizationGroupName));
                // БИК
                certificateListItems.Add(ToListItemContent(
                    Translator.Instance.Translate(ExtensionCatalog.Certificate, "Bank ID"),
                    certificate.OrganizationBankId, certificate.OrganizationBankIdAspects, OrganizationGroupName));
                // Корреспондентский счет
                certificateListItems.Add(ToListItemContent(
                    Translator.Instance.Translate(ExtensionCatalog.Certificate, "Correspondent account"),
                    certificate.OrganizationCorrespondentAccount, certificate.OrganizationCorrespondentAccountAspects,
                    OrganizationGroupName));
                // Расчетный счет
                certificateListItems.Add(ToListItemContent(
                    Translator.Instance.Translate(ExtensionCatalog.Certificate, "Checking account"),
                    certificate.OrganizationAccount, certificate.OrganizationAccountAspects, OrganizationGroupName));
            }

            certificateTunableList.DisplayContent(certificateListItems);
            attachedIdentifierTunableList.DisplayContent(certificate.AttachedIdentifierSummaries
                .Select(ais => new ListItemContent(new AttachedIdentifierRecord(ais))
                {
                    ImageKey = imageKey
                })
                .ToList());
        }

        private static string Combine(params string[] parametrs)
        {
            StringBuilder stringBuilder = new StringBuilder();

            bool flag = true;
            foreach (string s in parametrs)
            {
                if (string.IsNullOrEmpty(s))
                    continue;

                string value = s.TrimEnd(' ');

                if (string.IsNullOrEmpty(value))
                    continue;

                if (flag)
                {
                    stringBuilder.Append(value);
                    flag = false;
                    continue;
                }

                stringBuilder.Append(", ");
                stringBuilder.Append(value);
            }

            return stringBuilder.ToString();
        }

        private ListItemContent ToListItemContent(string name, string value, CertificateRecordAspects aspects,
            string group)
        {
            if (string.IsNullOrEmpty(value) && aspects.HasFlag(CertificateRecordAspects.Protected))
                value = Translator.Instance.Translate(ExtensionCatalog.Certificate, "[hidden by the owner]");
            else if (string.IsNullOrEmpty(value))
                value = string.Empty;

            string imageKey;

            if (aspects.HasFlag(CertificateRecordAspects.Verified))
                imageKey = aspects.HasFlag(CertificateRecordAspects.Protected)
                    ? VerifiedProtectedImageKey
                    : VerifiedImageKey;
            else
                imageKey = aspects.HasFlag(CertificateRecordAspects.Protected) ? UnverifiedProtectedImageKey : UnverifiedImageKey;

            return new ListItemContent(new CertificateRecord(name, value))
            {
                Group = group,
                ImageKey = imageKey
            };
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
