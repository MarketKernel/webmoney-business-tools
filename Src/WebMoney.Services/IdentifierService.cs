using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using log4net;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Contracts.Exceptions;
using WebMoney.Services.DataAccess.EF;
using WebMoney.Services.Utils;
using WebMoney.XmlInterfaces;
using WebMoney.XmlInterfaces.BasicObjects;
using WebMoney.XmlInterfaces.Exceptions;
using WebMoney.XmlInterfaces.Responses;

namespace WebMoney.Services
{
    public sealed class IdentifierService : SessionBasedService, IIdentifierService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(IdentifierService));

        private static readonly object Anchor = new object();
        private static readonly List<IIdentifierSummary> IdentifierSummaries;

        static IdentifierService()
        {
            IdentifierSummaries = new List<IIdentifierSummary>();
        }

        public void AddSecondaryIdentifier(IIdentifierSummary identifierSummary)
        {
            if (null == identifierSummary)
                throw new ArgumentNullException(nameof(identifierSummary));

            if (!(identifierSummary is IdentifierSummary))
                identifierSummary = IdentifierSummary.Create(identifierSummary);

            if (identifierSummary.IsMaster)
                throw new InvalidOperationException("identifierSummary.IsMaster");

            var entity = (IdentifierSummary) identifierSummary;

            //var certificate = FindCertificate(identifierSummary.Identifier, false, false, false);

            //if (CertificateDegree.Capitaller == certificate.Degree)
            //    entity.IsCapitaller = true;

            if (Session.AuthenticationService.HasConnectionSettings)
                using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
                {
                    var identifier = identifierSummary.Identifier;

                    if (context.IdentifierSummaries.Any(e => e.Identifier == identifier))
                        return;

                    context.IdentifierSummaries.Add(entity);
                    context.SaveChanges();
                }
            else
            {
                lock (Anchor)
                {
                    if (IdentifierSummaries.Any(e => e.Identifier == identifierSummary.Identifier))
                        return;

                    IdentifierSummaries.Add(identifierSummary);
                }
            }
        }

        public void RemoveSecondaryIdentifier(long identifier)
        {
            var longIdentifier = identifier;

            if (Session.AuthenticationService.HasConnectionSettings)
                using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
                {
                    var entity = context.IdentifierSummaries.FirstOrDefault(e => e.Identifier == longIdentifier);

                    if (null == entity)
                        return;

                    if (entity.IsMaster)
                        throw new InvalidOperationException("entity.IsMaster");

                    context.IdentifierSummaries.Remove(entity);
                    context.SaveChanges();
                }
            else
            {
                lock (Anchor)
                {
                    var entity = IdentifierSummaries.FirstOrDefault(e => e.Identifier == identifier);

                    if (null == entity)
                        return;

                    if (entity.IsMaster)
                        throw new InvalidOperationException("entity.IsMaster");

                    IdentifierSummaries.Remove(entity);
                }
            }
        }

        public bool IsIdentifierExists(long identifier)
        {
            if (Session.AuthenticationService.HasConnectionSettings)
                using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
                {
                    return context.IdentifierSummaries.Any(entity => entity.Identifier == identifier);
                }

            lock (Anchor)
            {
                return IdentifierSummaries.Any(entity => entity.Identifier == identifier);
            }
        }

        public IReadOnlyCollection<IIdentifierSummary> SelectIdentifiers()
        {
            List<IIdentifierSummary> identifierSummaries;

            if (Session.AuthenticationService.HasConnectionSettings)
                using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
                {
                    identifierSummaries = new List<IIdentifierSummary>();
                    identifierSummaries.AddRange(context.IdentifierSummaries.ToList());
                }
            else
            {
                lock (Anchor)
                {
                    identifierSummaries = new List<IIdentifierSummary>(IdentifierSummaries);

                    var masterIdentifier = Session.AuthenticationService.MasterIdentifier;

                    if (identifierSummaries.All(i => i.Identifier != masterIdentifier))
                        identifierSummaries.Add(new IdentifierSummary(masterIdentifier, "Master") {IsMaster = true});
                }
            }

            return identifierSummaries;
        }

        public long? FindIdentifier(string purse)
        {
            if (null == purse)
                throw new ArgumentNullException(nameof(purse));

            if (Session.AuthenticationService.HasConnectionSettings)
                using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
                {
                    var purseSummary = context.PurseSummaries.FirstOrDefault(ps => ps.Purse == purse);

                    if (null != purseSummary)
                        return purseSummary.Identifier;
                }

            var request =
                new WmIdFinder(Purse.Parse(purse)) {Initializer = Session.AuthenticationService.ObtainInitializer()};

            WmIdReport response;

            try
            {
                response = request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalException(exception.Message, exception);
            }

            if (null == response.WmId)
                return null;

            if (Session.AuthenticationService.HasConnectionSettings)
                using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
                {
                    context.PurseSummaries.Add(new PurseSummary(purse, response.WmId.Value));
                    context.SaveChanges();
                }

            return response.WmId;
        }

        public ICertificate FindCertificate(long identifier, bool levelsRequested, bool claimsRequested, bool attachedLevelsRequested, bool fresh = false)
        {
            if (!fresh)
            {
                if (Session.AuthenticationService.HasConnectionSettings)
                    using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
                    {
                        var localCertificate = context.Certificates.Include("AttachedIdentifierSummaries")
                            .FirstOrDefault(c => c.Identifier == identifier);

                        if (null != localCertificate)
                            return localCertificate;
                    }
            }

            var request =
                new PassportFinder((WmId) identifier) {Initializer = Session.AuthenticationService.ObtainInitializer()};

            Passport response;

            try
            {
                response = request.Submit();
            }
            catch (WmException exception)
            {
                throw new ExternalException(exception.Message, exception);
            }

            int? bl = null;
            int? tl = null;

            if (levelsRequested)
            {
                var blObtainer =
                    new BLObtainer((WmId) identifier) {Initializer = Session.AuthenticationService.ObtainInitializer()};
                var tlObtainer =
                    new TLObtainer((WmId) identifier) {Initializer = Session.AuthenticationService.ObtainInitializer()};

                try
                {
                    bl = blObtainer.Submit().Value;
                    tl = tlObtainer.Submit().Value;
                }
                catch (WebException exception)
                {
                    Logger.Error(exception.Message, exception);

                    bl = null;
                    tl = null;
                }
            }

            int? positiveRating = null;
            int? negativeRating = null;

            if (claimsRequested)
            {
                var claimsObtainer =
                    new ClaimsObtainer((WmId)identifier)
                    {
                        Initializer = Session.AuthenticationService.ObtainInitializer()
                    };

                try
                {
                    var claimsReport = claimsObtainer.Submit();

                    positiveRating = claimsReport.PositiveCount;
                    negativeRating = claimsReport.NegativeCount;
                }
                catch (WebException exception)
                {
                    Logger.Error(exception.Message, exception);

                    positiveRating = null;
                    negativeRating = null;
                }
            }

            var certificate = new Certificate(response.WmId, ConvertFrom.ApiTypeToContractType(response.Degree),
                response.Revoked,
                response.CreateTime.ToUniversalTime(), response.IssuerId, response.IssuerAlias)
            {
                Status = ConvertFrom.ApiTypeToContractType(response.Status),
                Bl = bl,
                Tl = tl,
                PositiveRating = positiveRating,
                NegativeRating = negativeRating,
                Appointment = ConvertFrom.ApiTypeToContractType(response.Appointment),
                Basis = response.Basis,
                IdentifierAlias = response.Alias,
                Information = response.Information,
                City = response.City,
                Region = response.Region,
                Country = response.Country,
                ZipCode = response.ZipCode,
                Address = response.Address,
                Surname = response.Surname,
                FirstName = response.FirstName,
                Patronymic = response.Patronymic,
                PassportNumber = response.PassportNumber,
                PassportDate = response.PassportDate?.ToUniversalTime(),
                PassportCountry = response.PassportCountry,
                PassportCity = response.PassportCity,
                PassportIssuer = response.PassportIssuer,
                RegistrationCountry = response.RegistrationCountry,
                RegistrationCity = response.RegistrationCity,
                RegistrationAddress = response.RegistrationAddress,
                Birthplace = response.Birthplace,
                Birthday = response.Birthday?.ToUniversalTime(),
                OrganizationName = response.OrganizationName,
                OrganizationManager = response.OrganizationManager,
                OrganizationAccountant = response.OrganizationAccountant,
                OrganizationTaxId = response.OrganizationTaxId,
                OrganizationId = response.OrganizationId,
                OrganizationActivityId = response.OrganizationActivityId,
                OrganizationAddress = response.OrganizationAddress,
                OrganizationCountry = response.OrganizationCountry,
                OrganizationCity = response.OrganizationCity,
                OrganizationZipCode = response.OrganizationZipCode,
                OrganizationBankName = response.OrganizationBankName,
                OrganizationBankId = response.OrganizationBankId,
                OrganizationCorrespondentAccount = response.OrganizationCorrespondentAccount,
                OrganizationAccount = response.OrganizationAccount,
                HomePhone = response.HomePhone,
                CellPhone = response.CellPhone,
                Icq = response.ICQ,
                Fax = response.Fax,
                Email = response.EMail,
                WebAddress = response.WebAddress,
                ContactPhone = response.ContactPhone,
                CapitallerParent = response.CapitallerParent,
                PassportInspection = response.PassportInspection,
                TaxInspection = response.TaxInspection,
                StatusAspects = (CertificateRecordAspects) response.StatusConfirmation,
                AppointmentAspects = (CertificateRecordAspects) response.AppointmentConfirmation,
                BasisAspects = (CertificateRecordAspects) response.BasisConfirmation,
                AliasAspects = (CertificateRecordAspects) response.AliasConfirmation,
                InformationAspects = (CertificateRecordAspects) response.InformationConfirmation,
                CityAspects = (CertificateRecordAspects) response.CityConfirmation,
                RegionAspects = (CertificateRecordAspects) response.RegionConfirmation,
                CountryAspects = (CertificateRecordAspects) response.CountryConfirmation,
                ZipCodeAspects = (CertificateRecordAspects) response.ZipCodeConfirmation,
                AddressAspects = (CertificateRecordAspects) response.AddressConfirmation,
                SurnameAspects = (CertificateRecordAspects) response.SurnameConfirmation,
                FirstNameAspects = (CertificateRecordAspects) response.FirstNameConfirmation,
                PatronymicAspects = (CertificateRecordAspects) response.PatronymicConfirmation,
                PassportNumberAspects = (CertificateRecordAspects) response.PassportNumberConfirmation,
                PassportDateAspects = (CertificateRecordAspects) response.PassportDateConfirmation,
                PassportCountryAspects = (CertificateRecordAspects) response.PassportCountryConfirmation,
                PassportCityAspects = (CertificateRecordAspects) response.PassportCityConfirmation,
                PassportIssuerAspects = (CertificateRecordAspects) response.PassportIssuerConfirmation,
                RegistrationCountryAspects = (CertificateRecordAspects) response.RegistrationCountryConfirmation,
                RegistrationCityAspects = (CertificateRecordAspects) response.RegistrationCityConfirmation,
                RegistrationAddressAspects = (CertificateRecordAspects) response.RegistrationAddressConfirmation,
                BirthplaceAspects = (CertificateRecordAspects) response.BirthplaceConfirmation,
                BirthdayAspects = (CertificateRecordAspects) response.BirthdayConfirmation,
                OrganizationNameAspects = (CertificateRecordAspects) response.OrganizationNameConfirmation,
                OrganizationManagerAspects = (CertificateRecordAspects) response.OrganizationManagerConfirmation,
                OrganizationAccountantAspects = (CertificateRecordAspects) response.OrganizationAccountantConfirmation,
                OrganizationTaxIdAspects = (CertificateRecordAspects) response.OrganizationTaxIdConfirmation,
                OrganizationIdAspects = (CertificateRecordAspects) response.OrganizationIdConfirmation,
                OrganizationActivityIdAspects = (CertificateRecordAspects) response.OrganizationActivityIdConfirmation,
                OrganizationAddressAspects = (CertificateRecordAspects) response.OrganizationAddressConfirmation,
                OrganizationCountryAspects = (CertificateRecordAspects) response.OrganizationCountryConfirmation,
                OrganizationCityAspects = (CertificateRecordAspects) response.OrganizationCityConfirmation,
                OrganizationZipCodeAspects = (CertificateRecordAspects) response.OrganizationZipCodeConfirmation,
                OrganizationBankNameAspects = (CertificateRecordAspects) response.OrganizationBankNameConfirmation,
                OrganizationBankIdAspects = (CertificateRecordAspects) response.OrganizationBankIdConfirmation,
                OrganizationCorrespondentAccountAspects =
                    (CertificateRecordAspects) response.OrganizationCorrespondentAccountConfirmation,
                OrganizationAccountAspects = (CertificateRecordAspects) response.OrganizationAccountConfirmation,
                HomePhoneAspects = (CertificateRecordAspects) response.HomePhoneConfirmation,
                CellPhoneAspects = (CertificateRecordAspects) response.CellPhoneConfirmation,
                IcqAspects = (CertificateRecordAspects) response.ICQConfirmation,
                FaxAspects = (CertificateRecordAspects) response.FaxConfirmation,
                EmailAspects = (CertificateRecordAspects) response.EMailConfirmation,
                WebAddressAspects = (CertificateRecordAspects) response.WebAddressConfirmation,
                ContactPhoneAspects = (CertificateRecordAspects) response.ContactPhoneConfirmation,
                CapitallerParentAspects = (CertificateRecordAspects) response.CapitallerParentConfirmation,
                PassportInspectionAspects = (CertificateRecordAspects) response.PassportInspectionConfirmation,
                TaxInspectionAspects = (CertificateRecordAspects) response.TaxInspectionConfirmation
            };

            if (null != response.PassportDate)
                certificate.PassportDate = response.PassportDate.Value;

            foreach (var wmIdInfo in response.WmIdInfoList)
            {
                var attachedIdentifier = new AttachedIdentifierSummary(wmIdInfo.WmId, wmIdInfo.Alias,
                    wmIdInfo.Description, wmIdInfo.RegistrationDate) {Certificate = certificate};

                if (attachedIdentifier.Identifier == identifier && null != bl && null != tl)
                {
                    attachedIdentifier.Bl = bl;
                    attachedIdentifier.Tl = tl;
                }

                if (attachedLevelsRequested && null == attachedIdentifier.Bl)
                {
                    var blObtainer =
                        new BLObtainer((WmId)attachedIdentifier.Identifier) { Initializer = Session.AuthenticationService.ObtainInitializer() };
                    var tlObtainer =
                        new TLObtainer((WmId)attachedIdentifier.Identifier) { Initializer = Session.AuthenticationService.ObtainInitializer() };

                    int? attachedBl;
                    int? attachedTl;

                    try
                    {
                        attachedBl = blObtainer.Submit().Value;
                        attachedTl = tlObtainer.Submit().Value;
                    }
                    catch (WebException exception)
                    {
                        Logger.Error(exception.Message, exception);

                        attachedBl = null;
                        attachedTl = null;
                    }

                    attachedIdentifier.Bl = attachedBl;
                    attachedIdentifier.Tl = attachedTl;

                    if (null == bl && identifier == attachedIdentifier.Identifier)
                    {
                        certificate.Bl = attachedBl;
                        certificate.Tl = attachedTl;
                    }
                }

                certificate.AttachedIdentifierSummaries.Add(attachedIdentifier);
            }

            if (Session.AuthenticationService.HasConnectionSettings)
                using (var context = new DataContext(Session.AuthenticationService.GetConnectionSettings()))
                {
                    var localCertificate = context.Certificates.Include("AttachedIdentifierSummaries")
                        .FirstOrDefault(c => c.Identifier == identifier);

                    if (null != localCertificate)
                    {
                        context.Certificates.Remove(localCertificate);

                        if (null != localCertificate.AttachedIdentifierSummaries &&
                            localCertificate.AttachedIdentifierSummaries.Count > 0)
                            context.AttachedIdentifierSummaries.RemoveRange(
                                localCertificate.AttachedIdentifierSummaries);
                    }

                    context.SaveChanges();

                    context.Certificates.Add(certificate);

                    context.SaveChanges();
                }

            return certificate;
        }

        internal static void RegisterMasterIdentifierIfNeeded(IAuthenticationService authenticationService)
        {
            if (null == authenticationService)
                throw new ArgumentNullException(nameof(authenticationService));

            var masterIdentifier = authenticationService.MasterIdentifier;

            using (var context = new DataContext(authenticationService.GetConnectionSettings()))
            {
                if (!context.IdentifierSummaries.Any(entity => entity.Identifier == masterIdentifier))
                {
                    context.IdentifierSummaries.Add(new IdentifierSummary(masterIdentifier, "Master")
                    {
                        IsMaster = true
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
