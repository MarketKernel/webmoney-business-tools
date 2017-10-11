using System;
using System.Collections.Generic;
using WebMoney.Services.Contracts.BasicTypes;

namespace WebMoney.Services.Contracts.BusinessObjects
{
    public interface ICertificate
    {
        long Identifier { get; }

        // attestat
        CertificateDegree Degree { get; }

        bool Revoked { get; }
        DateTime CreationTime { get; }
        long IssuerIdentifier { get; }
        string IssuerAlias { get; }
        IEnumerable<IAttachedIdentifierSummary> AttachedIdentifierSummaries { get; }

        // userinfo
        CertificateStatus Status { get; }

        int? Bl { get; set; }
        int? Tl { get; set; }
        int? PositiveRating { get; set; }
        int? NegativeRating { get; set; }

        CertificateAppointment Appointment { get; }

        string Basis { get; }
        string IdentifierAlias { get; }
        string Information { get; }
        string City { get; }
        string Region { get; }
        string Country { get; }
        string ZipCode { get; }
        string Address { get; }
        string Surname { get; }
        string FirstName { get; }
        string Patronymic { get; }
        string PassportNumber { get; }
        DateTime? PassportDate { get; }
        string PassportCountry { get; }
        string PassportCity { get; }
        string PassportIssuer { get; }
        string RegistrationCountry { get; }
        string RegistrationCity { get; }
        string RegistrationAddress { get; }
        string Birthplace { get; }
        DateTime? Birthday { get; }
        string OrganizationName { get; }
        string OrganizationManager { get; }
        string OrganizationAccountant { get; }
        string OrganizationTaxId { get; }
        string OrganizationId { get; }
        string OrganizationActivityId { get; }
        string OrganizationAddress { get; }
        string OrganizationCountry { get; }
        string OrganizationCity { get; }
        string OrganizationZipCode { get; }
        string OrganizationBankName { get; }
        string OrganizationBankId { get; }
        string OrganizationCorrespondentAccount { get; }
        string OrganizationAccount { get; }
        string HomePhone { get; }
        string CellPhone { get; }
        string Icq { get; }
        string Fax { get; }
        string Email { get; }
        string WebAddress { get; }
        string ContactPhone { get; }
        long? CapitallerParent { get; }
        bool PassportInspection { get; }
        bool TaxInspection { get; }

        // check-lock
        CertificateRecordAspects StatusAspects { get; }

        CertificateRecordAspects AppointmentAspects { get; }
        CertificateRecordAspects BasisAspects { get; }
        CertificateRecordAspects AliasAspects { get; }
        CertificateRecordAspects InformationAspects { get; }
        CertificateRecordAspects CityAspects { get; }
        CertificateRecordAspects RegionAspects { get; }
        CertificateRecordAspects CountryAspects { get; }
        CertificateRecordAspects ZipCodeAspects { get; }
        CertificateRecordAspects AddressAspects { get; }
        CertificateRecordAspects SurnameAspects { get; }
        CertificateRecordAspects FirstNameAspects { get; }
        CertificateRecordAspects PatronymicAspects { get; }
        CertificateRecordAspects PassportNumberAspects { get; }
        CertificateRecordAspects PassportDateAspects { get; }
        CertificateRecordAspects PassportCountryAspects { get; }
        CertificateRecordAspects PassportCityAspects { get; }
        CertificateRecordAspects PassportIssuerAspects { get; }
        CertificateRecordAspects RegistrationCountryAspects { get; }
        CertificateRecordAspects RegistrationCityAspects { get; }
        CertificateRecordAspects RegistrationAddressAspects { get; }
        CertificateRecordAspects BirthplaceAspects { get; }
        CertificateRecordAspects BirthdayAspects { get; }
        CertificateRecordAspects OrganizationNameAspects { get; }
        CertificateRecordAspects OrganizationManagerAspects { get; }
        CertificateRecordAspects OrganizationAccountantAspects { get; }
        CertificateRecordAspects OrganizationTaxIdAspects { get; }
        CertificateRecordAspects OrganizationIdAspects { get; }
        CertificateRecordAspects OrganizationActivityIdAspects { get; }
        CertificateRecordAspects OrganizationAddressAspects { get; }
        CertificateRecordAspects OrganizationCountryAspects { get; }
        CertificateRecordAspects OrganizationCityAspects { get; }
        CertificateRecordAspects OrganizationZipCodeAspects { get; }
        CertificateRecordAspects OrganizationBankNameAspects { get; }
        CertificateRecordAspects OrganizationBankIdAspects { get; }
        CertificateRecordAspects OrganizationCorrespondentAccountAspects { get; }
        CertificateRecordAspects OrganizationAccountAspects { get; }
        CertificateRecordAspects HomePhoneAspects { get; }
        CertificateRecordAspects CellPhoneAspects { get; }
        CertificateRecordAspects IcqAspects { get; }
        CertificateRecordAspects FaxAspects { get; }
        CertificateRecordAspects EmailAspects { get; }
        CertificateRecordAspects WebAddressAspects { get; }
        CertificateRecordAspects ContactPhoneAspects { get; }
        CertificateRecordAspects CapitallerParentAspects { get; }
        CertificateRecordAspects PassportInspectionAspects { get; }
        CertificateRecordAspects TaxInspectionAspects { get; }
    }
}
