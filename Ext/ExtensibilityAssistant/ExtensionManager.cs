using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Xml.Serialization;
using log4net;
using Newtonsoft.Json;

namespace ExtensibilityAssistant
{
    public sealed class ExtensionManager
    {
        private enum ManifestFileFormat
        {
            Json = 1,
            Xml = 2
        }

        private static readonly ILog Logger = LogManager.GetLogger(typeof(ExtensionManager));

        private const string JsonManifestFileName = "Extensions.json";
        private const string XmlManifestFileName = "Extensions.xml";

        private readonly Dictionary<string, List<ExtensionConfiguration>> _extensionConfigurationsByIdRegister =
            new Dictionary<string, List<ExtensionConfiguration>>();

        private readonly Dictionary<string, SortedSet<string>> _extensionIdListByTagRegister =
            new Dictionary<string, SortedSet<string>>();

        private readonly Dictionary<string, string> _assemblyByFullNameRegister = new Dictionary<string, string>();

        public ExtensionManager(string basePath)
        {
            if (null == basePath)
                throw new ArgumentNullException(nameof(basePath));

            foreach (var directory in Directory.GetDirectories(basePath))
            {
                var register = TryLoadExtensionInfoRegister(directory);

                if (null == register)
                    continue;

                foreach (var extensionConfiguration in register.ExtensionConfigurations)
                {
                    extensionConfiguration.BaseDirectory = directory;
                    extensionConfiguration.AssemblyBrief =
                        new AssemblyBrief(extensionConfiguration.AssemblyFullName, null);

                    // By name.
                    if (!_extensionConfigurationsByIdRegister.ContainsKey(extensionConfiguration.Id))
                        _extensionConfigurationsByIdRegister.Add(extensionConfiguration.Id, new List<ExtensionConfiguration>());

                    _extensionConfigurationsByIdRegister[extensionConfiguration.Id].Add(extensionConfiguration);

                    // By tags.
                    foreach (var tag in extensionConfiguration.Tags)
                    {
                        if (!_extensionIdListByTagRegister.ContainsKey(tag.TagName))
                            _extensionIdListByTagRegister.Add(tag.TagName, new SortedSet<string>());

                        var extensionIdList = _extensionIdListByTagRegister[tag.TagName];

                        if (!extensionIdList.Contains(extensionConfiguration.Id))
                            extensionIdList.Add(extensionConfiguration.Id);
                    }
                }
            }

            foreach (var assemblyFilePath in Directory.GetFiles(basePath, "*.dll", SearchOption.AllDirectories))
            {
                AssemblyName assemblyName;

                try
                {
                    assemblyName = AssemblyName.GetAssemblyName(assemblyFilePath);
                }
                catch (BadImageFormatException)
                {
                    continue;
                }

                if (!_assemblyByFullNameRegister.ContainsKey(assemblyName.FullName))
                    _assemblyByFullNameRegister.Add(assemblyName.FullName, assemblyFilePath);
            }
        }

        [SecurityCritical]
        public void ApplyCustomAssemblyResolving()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }

        public ExtensionConfiguration TryObtainExtensionConfiguration(string extensionId)
        {
            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            if (!_extensionConfigurationsByIdRegister.ContainsKey(extensionId))
                return null;

            return _extensionConfigurationsByIdRegister[extensionId].OrderBy(e => e.Priority).First();
        }

        public Collection<ExtensionInfo> SelectExtensionInfoListByTag(string tagName)
        {
            if (null == tagName)
                throw new ArgumentNullException(nameof(tagName));

            var extensionInfoList = new Collection<ExtensionInfo>();

            if (!_extensionIdListByTagRegister.ContainsKey(tagName))
                return extensionInfoList;

            var extensionIdList = _extensionIdListByTagRegister[tagName];

            foreach (var extensionId in extensionIdList)
            {
                var extensionConfiguration = TryObtainExtensionConfiguration(extensionId);

                if (null == extensionConfiguration)
                    throw new InvalidOperationException("null == extensionConfiguration");

                var tagConfiguration = extensionConfiguration.Tags.First(t => t.TagName == tagName);

                if (null == tagConfiguration)
                    throw new InvalidOperationException("null == tagConfiguration");

                extensionInfoList.Add(new ExtensionInfo(extensionConfiguration, tagConfiguration));
            }

            return new Collection<ExtensionInfo>((from ei in extensionInfoList
                orderby ei.GroupOrder, ei.Order
                select ei).ToList());
        }

        public TExtension TryCreateExtension<TExtension>()
            where TExtension : class
        {
            var extensionType = typeof(TExtension);

            var extensionId = extensionType.Name;

            if (extensionType.IsInterface && extensionId.StartsWith("I", StringComparison.Ordinal))
                extensionId = extensionId.Remove(0, 1);

            return (TExtension) TryCreateExtension(extensionId);
        }

        public object TryCreateExtension(string extensionId)
        {
            if (null == extensionId)
                throw new ArgumentNullException(nameof(extensionId));

            var extensionConfiguration = TryObtainExtensionConfiguration(extensionId);

            if (null == extensionConfiguration)
                return null;

            object result;

            try
            {
                var objectHandle = Activator.CreateInstance(extensionConfiguration.AssemblyBrief.AssemblyFullName,
                    extensionConfiguration.ExtensionType);

                result = objectHandle.Unwrap();
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);
                result = null;
            }

            return result;
        }

        public TExtension CreateExtension<TExtension>()
            where TExtension : class
        {
            var extensionType = typeof(TExtension);

            var extensionId = extensionType.Name;

            if (extensionType.IsInterface && extensionId.StartsWith("I", StringComparison.Ordinal))
                extensionId = extensionId.Remove(0, 1);

            return CreateExtension<TExtension>(extensionId);
        }

        public TExtension CreateExtension<TExtension>(string extensionId)
            where TExtension : class
        {
            var extension = TryCreateExtension(extensionId);

            if (null == extension)
                throw new ExtensionNotFoundException($"Extension \"{extensionId}\" not found!");

            var t = extension as TExtension;

            if (null == t)
                throw new WrongExtensionTypeException(
                    $"Wrong extension type (extension=\"{extensionId}\"; expected=\"{typeof(TExtension).Name}\"; actual=\"{extension.GetType().Name}\")!");

            return t;
        }

        private static ConfigurationRegister TryLoadExtensionInfoRegister(string directory)
        {
            var manifestFilePath = Path.Combine(directory, JsonManifestFileName);
            var manifestFileFormat = ManifestFileFormat.Json;

            if (!File.Exists(manifestFilePath))
            {
                manifestFilePath = Path.Combine(directory, XmlManifestFileName);
                manifestFileFormat = ManifestFileFormat.Xml;

                if (!File.Exists(manifestFilePath))
                    return null;
            }

            ConfigurationRegister register;

            switch (manifestFileFormat)
            {
                case ManifestFileFormat.Json:
                    var serializer = new JsonSerializer();

                    using (var fileStream = new FileStream(manifestFilePath, FileMode.Open, FileAccess.Read))
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        try
                        {
                            register = serializer.Deserialize<ConfigurationRegister>(jsonTextReader);
                        }
                        catch (JsonException exception)
                        {
                            Logger.Error(exception.Message, exception);

                            register = null;
                        }
                    }

                    break;
                case ManifestFileFormat.Xml:
                    var xmlSerializer = new XmlSerializer(typeof(ConfigurationRegister));
                    var xmlSerializerNamespaces = new XmlSerializerNamespaces();
                    xmlSerializerNamespaces.Add("", "");

                    using (var fileStream = new FileStream(manifestFilePath, FileMode.Open, FileAccess.Read,
                        FileShare.Read))
                    {
                        // TODO [L] Добавить обработку исключений по аналогии с JSON
                        register = (ConfigurationRegister) xmlSerializer.Deserialize(fileStream);
                    }

                    break;
                default:
                    throw new InvalidOperationException("manifestFileFormat == " + manifestFileFormat);
            }

            return register;
        }

        private Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            if (null == args)
                throw new ArgumentNullException(nameof(args));

            string assemblyFilePath;

            if (_assemblyByFullNameRegister.TryGetValue(args.Name, out assemblyFilePath))
            {
                Logger.DebugFormat("Assembly found [name={0}; file={1}]", args.Name, assemblyFilePath);
                return Assembly.LoadFile(assemblyFilePath);
            }

            Logger.WarnFormat("Assembly not found [name={0}]", args.Name);

            return null;
        }
    }
}
