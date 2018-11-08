using System;
using System.Globalization;
using System.IO;
using System.Text;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Utils;

namespace WebMoney.Services
{
    public sealed class SettingsService :  ISettingsService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SettingsService));
        private static readonly object Anchor = new object();

        private static Settings _settings;

        private readonly long _masterIdentifier;

        public SettingsService(long masterIdentifier)
        {
            _masterIdentifier = masterIdentifier;
        }

        public int AllocateTransferId()
        {
            int transferId;

            lock (Anchor)
            {
                if (null == _settings)
                    _settings = Load(_masterIdentifier);

                _settings.TransferId++;
                transferId = _settings.TransferId;
            }

            return transferId;
        }

        public int AllocateOrderId()
        {
            int orderId;

            lock (Anchor)
            {
                if (null == _settings)
                    _settings = Load(_masterIdentifier);

                _settings.OrderId++;
                orderId = _settings.OrderId;
            }

            return orderId;
        }

        public ISettings GetSettings()
        {
            lock (Anchor)
            {
                if (null == _settings)
                {
                    var settings = Load(_masterIdentifier);
                    _settings = settings;
                }

                return (ISettings) _settings.Clone();
            }
        }

        public void SetSettings(ISettings contractObject)
        {
            if (null == contractObject)
                throw new ArgumentNullException(nameof(contractObject));

            var settings = Settings.Create(contractObject);

            lock (Anchor)
            {
                if (_settings.Equals(settings))
                    return;

                _settings = (Settings) settings.Clone();
            }
        }

        public void Save()
        {
            lock (Anchor)
            {
                var settingsFilePath = SettingsUtility.GetSettingsFilePath(_masterIdentifier);

                var serializer = new JsonSerializer {ContractResolver = new CamelCasePropertyNamesContractResolver()};

                using (var fileStream = new FileStream(settingsFilePath, FileMode.Create, FileAccess.Write))
                using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                using (var jsonTextWriter = new JsonTextWriter(streamWriter) {Formatting = Formatting.Indented})
                {
                    serializer.Serialize(jsonTextWriter, _settings);
                }
            }
        }

        private static Settings Load(long masterIdentifier)
        {
            Settings settings;

            var settingsFilePath = SettingsUtility.GetSettingsFilePath(masterIdentifier);

            if (File.Exists(settingsFilePath))
            {
                var serializer = new JsonSerializer();

                using (var fileStream = new FileStream(settingsFilePath, FileMode.Open, FileAccess.Read))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    try
                    {
                        settings = serializer.Deserialize<Settings>(jsonTextReader);
                    }
                    catch (JsonReaderException exception)
                    {
                        Logger.Error(exception.Message, exception);
                        settings = new Settings { Language = GetDefaultLanguage() };
                    }
                }
            }
            else
                settings = new Settings {Language = GetDefaultLanguage()};

            return settings;
        }

        private static Language GetDefaultLanguage()
        {
            switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower())
            {
                case "ru": // Россия
                case "uk": // Украина
                case "be": // Беларусь
                case "kk": // Казахстан
                    return Language.Russian;
                default:
                    return Language.English;
            }
        }
    }
}
