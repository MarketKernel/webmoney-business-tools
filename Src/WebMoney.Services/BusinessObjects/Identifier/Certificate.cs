using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.BusinessObjects
{
    [Table("Certificate")]
    internal sealed class Certificate : ICertificate
    {
        private string _issuerAlias;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long Identifier { get; set; }

        // attestat
        [Required]
        public CertificateDegree Degree { get; set; }

        [Required]
        public bool Revoked { get; set; }

        [Required]
        public DateTime CreationTime { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = FormattingService.IdentifierTemplate)]
        public long IssuerIdentifier { get; set; }

        public string IssuerAlias
        {
            get => _issuerAlias;
            set => _issuerAlias = value ?? throw new ArgumentNullException(nameof(value));
        }

        [Required]
        [InverseProperty("Certificate")]
        public List<AttachedIdentifierSummary> AttachedIdentifierSummaries { get; }

        [NotMapped]
        IEnumerable<IAttachedIdentifierSummary> ICertificate.AttachedIdentifierSummaries => AttachedIdentifierSummaries;

        // userinfo
        [Required]
        public CertificateStatus Status { get; set; }

        public int? Bl { get; set; }
        public int? Tl { get; set; }
        public int? PositiveRating { get; set; }
        public int? NegativeRating { get; set; }

        [Required]
        public CertificateAppointment Appointment { get; set; }
        public string Basis { get; set; }
        public string IdentifierAlias { get; set; }
        public string Information { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string PassportNumber { get; set; }
        public DateTime? PassportDate { get; set; }
        public string PassportCountry { get; set; }
        public string PassportCity { get; set; }
        public string PassportIssuer { get; set; }
        public string RegistrationCountry { get; set; }
        public string RegistrationCity { get; set; }
        public string RegistrationAddress { get; set; }
        public string Birthplace { get; set; }
        public DateTime? Birthday { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationManager { get; set; }
        public string OrganizationAccountant { get; set; }
        public string OrganizationTaxId { get; set; }
        public string OrganizationId { get; set; }
        public string OrganizationActivityId { get; set; }
        public string OrganizationAddress { get; set; }
        public string OrganizationCountry { get; set; }
        public string OrganizationCity { get; set; }
        public string OrganizationZipCode { get; set; }
        public string OrganizationBankName { get; set; }
        public string OrganizationBankId { get; set; }
        public string OrganizationCorrespondentAccount { get; set; }
        public string OrganizationAccount { get; set; }
        public string HomePhone { get; set; }
        public string CellPhone { get; set; }
        public string Icq { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string WebAddress { get; set; }
        public string ContactPhone { get; set; }
        public long? CapitallerParent { get; set; }
        public bool PassportInspection { get; set; }
        public bool TaxInspection { get; set; }

        // check-lock
        [Required]
        public CertificateRecordAspects StatusAspects { get; set; }

        [Required]
        public CertificateRecordAspects AppointmentAspects { get; set; }

        [Required]
        public CertificateRecordAspects BasisAspects { get; set; }

        [Required]
        public CertificateRecordAspects AliasAspects { get; set; }

        [Required]
        public CertificateRecordAspects InformationAspects { get; set; }

        [Required]
        public CertificateRecordAspects CityAspects { get; set; }

        [Required]
        public CertificateRecordAspects RegionAspects { get; set; }

        [Required]
        public CertificateRecordAspects CountryAspects { get; set; }

        [Required]
        public CertificateRecordAspects ZipCodeAspects { get; set; }

        [Required]
        public CertificateRecordAspects AddressAspects { get; set; }

        [Required]
        public CertificateRecordAspects SurnameAspects { get; set; }

        [Required]
        public CertificateRecordAspects FirstNameAspects { get; set; }

        [Required]
        public CertificateRecordAspects PatronymicAspects { get; set; }

        [Required]
        public CertificateRecordAspects PassportNumberAspects { get; set; }

        [Required]
        public CertificateRecordAspects PassportDateAspects { get; set; }

        [Required]
        public CertificateRecordAspects PassportCountryAspects { get; set; }

        [Required]
        public CertificateRecordAspects PassportCityAspects { get; set; }

        [Required]
        public CertificateRecordAspects PassportIssuerAspects { get; set; }

        [Required]
        public CertificateRecordAspects RegistrationCountryAspects { get; set; }

        [Required]
        public CertificateRecordAspects RegistrationCityAspects { get; set; }

        [Required]
        public CertificateRecordAspects RegistrationAddressAspects { get; set; }

        [Required]
        public CertificateRecordAspects BirthplaceAspects { get; set; }

        [Required]
        public CertificateRecordAspects BirthdayAspects { get; set; }

        [Required]
        public CertificateRecordAspects OrganizationNameAspects { get; set; }

        [Required]
        public CertificateRecordAspects OrganizationManagerAspects { get; set; }

        [Required]
        public CertificateRecordAspects OrganizationAccountantAspects { get; set; }

        [Required]
        public CertificateRecordAspects OrganizationTaxIdAspects { get; set; }

        [Required]
        public CertificateRecordAspects OrganizationIdAspects { get; set; }

        [Required]
        public CertificateRecordAspects OrganizationActivityIdAspects { get; set; }

        [Required]
        public CertificateRecordAspects OrganizationAddressAspects { get; set; }

        [Required]
        public CertificateRecordAspects OrganizationCountryAspects { get; set; }

        [Required]
        public CertificateRecordAspects OrganizationCityAspects { get; set; }

        [Required]
        public CertificateRecordAspects OrganizationZipCodeAspects { get; set; }

        [Required]
        public CertificateRecordAspects OrganizationBankNameAspects { get; set; }

        [Required]
        public CertificateRecordAspects OrganizationBankIdAspects { get; set; }

        [Required]
        public CertificateRecordAspects OrganizationCorrespondentAccountAspects { get; set; }

        [Required]
        public CertificateRecordAspects OrganizationAccountAspects { get; set; }

        [Required]
        public CertificateRecordAspects HomePhoneAspects { get; set; }

        [Required]
        public CertificateRecordAspects CellPhoneAspects { get; set; }

        [Required]
        public CertificateRecordAspects IcqAspects { get; set; }

        [Required]
        public CertificateRecordAspects FaxAspects { get; set; }

        [Required]
        public CertificateRecordAspects EmailAspects { get; set; }

        [Required]
        public CertificateRecordAspects WebAddressAspects { get; set; }

        [Required]
        public CertificateRecordAspects ContactPhoneAspects { get; set; }

        [Required]
        public CertificateRecordAspects CapitallerParentAspects { get; set; }

        [Required]
        public CertificateRecordAspects PassportInspectionAspects { get; set; }

        [Required]
        public CertificateRecordAspects TaxInspectionAspects { get; set; }

        internal Certificate()
        {
            AttachedIdentifierSummaries = new List<AttachedIdentifierSummary>();
        }

        public Certificate(long identifier, CertificateDegree degree, bool revoked, DateTime creationTime,
            long issuerIdentifier, string issuerAlias)
        {
            Identifier = identifier;
            Degree = degree;
            Revoked = revoked;
            CreationTime = creationTime;
            IssuerIdentifier = issuerIdentifier;
            IssuerAlias = issuerAlias ?? throw new ArgumentNullException(nameof(issuerAlias));
            AttachedIdentifierSummaries = new List<AttachedIdentifierSummary>();
        }
    }
}
