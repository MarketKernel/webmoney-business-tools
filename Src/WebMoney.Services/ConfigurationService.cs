using System;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using log4net;
using Microsoft.Practices.Unity;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.DataAccess.EF;
using WebMoney.XmlInterfaces.Core;

namespace WebMoney.Services
{
    public sealed class ConfigurationService : IConfigurationService
    {
        private const string TrustedCertificatesFolder = "TrustedCertificates";

        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConfigurationService));

        public void RegisterServices(IUnityContainer unityContainer)
        {
            if (null == unityContainer)
                throw new ArgumentNullException(nameof(unityContainer));

            // Первичная конфигурация.
            Configure();

            // Регистрация сервисов
            unityContainer.RegisterType<IContractService, ContractService>();
            unityContainer.RegisterType<ICurrencyService, CurrencyService>();
            unityContainer.RegisterType<IEntranceService, EntranceService>();
            unityContainer.RegisterType<IIdentifierService, IdentifierService>();
            unityContainer.RegisterType<IImportExportService, ImportExportService>();
            unityContainer.RegisterType<IInvoiceService, InvoiceService>();
            unityContainer.RegisterType<IMessageService, MessageService>();
            unityContainer.RegisterType<IPaymentService, PaymentService>();
            unityContainer.RegisterType<IPurseService, PurseService>();
            unityContainer.RegisterType<ISmsService, SmsService>();
            unityContainer.RegisterType<ITransferBundleService, TransferBundleService>();
            unityContainer.RegisterType<ITransferService, TransferService>();
            unityContainer.RegisterType<ITrustService, TrustService>();
            unityContainer.RegisterType<IVerificationService, VerificationService>();
            unityContainer.RegisterType<IFormattingService, FormattingService>();

            unityContainer.RegisterType<ITransferBundleProcessor, TransferBundleProcessor>(
                new ContainerControlledLifetimeManager());

            unityContainer.RegisterType<IEventBroker, EventBroker>(new ContainerControlledLifetimeManager());

            // ExternalServices
            ExternalServices.Configurator.ConfigureUnityContainer(unityContainer);
        }

        private static void Configure()
        {
            if (PlatformID.Unix == Environment.OSVersion.Platform)
            {
                CertificateValidator.DisableValidation = true;
            }
            else
            {
                string assemblyFile = new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath;
                string assemblyDirectory = Path.GetDirectoryName(assemblyFile);

                if (null == assemblyDirectory)
                    throw new InvalidOperationException("null == assemblyDirectory");

                var trustedRootCertificatesDirectory =
                    Path.Combine(assemblyDirectory, TrustedCertificatesFolder);

                foreach (var file in Directory.GetFiles(trustedRootCertificatesDirectory, "*.cer"))
                {
                    X509Certificate2 x509Certificate2;

                    try
                    {
                        x509Certificate2 = new X509Certificate2(file);
                    }
                    catch (Exception exception)
                    {
                        Logger.Error(exception.Message, exception);
                        continue;
                    }

                    CertificateValidator.RegisterTrustedCertificate(x509Certificate2);
                    Logger.DebugFormat("Trusted certificate registered [thumbprint={0}].", x509Certificate2.Thumbprint);
                }
            }

            ServicePointManager.ServerCertificateValidationCallback =
                CertificateValidator.RemoteCertificateValidationCallback;

            // DbConfiguration
            DbConfiguration.SetConfiguration(new DataConfiguration());

            // Mapper
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<IAuthenticationSettings, AuthenticationSettings>();
                cfg.CreateMap<IConnectionSettings, ConnectionSettings>();
                cfg.CreateMap<IProxyCredential, ProxyCredential>();
                cfg.CreateMap<IProxySettings, ProxySettings>();
                cfg.CreateMap<IRequestNumberSettings, RequestNumberSettings>();

                cfg.CreateMap<IContractSettings, ContractSettings>();
                cfg.CreateMap<IIncomingInvoiceSettings, IncomingInvoiceSettings>();
                cfg.CreateMap<IOperationSettings, OperationSettings>();
                cfg.CreateMap<IOutgoingInvoiceSettings, OutgoingInvoiceSettings>();
                cfg.CreateMap<IPreparedTransferSettings, PreparedTransferSettings>();
                cfg.CreateMap<IRequestSettings, RequestSettings>();
                cfg.CreateMap<ISettings, Settings>();
                cfg.CreateMap<ITransferBundleSettings, TransferBundleSettings>();
                cfg.CreateMap<ITransferSettings, TransferSettings>();
            });
        }
    }
}
