using System;

namespace ExtensibilityAssistant
{
    public sealed class ExtensionInfo
    {
        private readonly ExtensionConfiguration _extensionConfiguration;
        private readonly TagConfiguration _tagConfiguration;

        public string Id => _extensionConfiguration.Id;

        public string Name => _tagConfiguration.ContextSpecificExtensionName ?? _extensionConfiguration.Name;

        public string Description => _extensionConfiguration.Description;

        public bool LinuxCompatible => _extensionConfiguration.LinuxCompatible;

        public string IconPath => _extensionConfiguration.IconPath;

        public string BaseDirectory => _extensionConfiguration.BaseDirectory;

        public decimal GroupOrder => _tagConfiguration.GroupOrder;

        public decimal Order => _tagConfiguration.Order;

        public ExtensionInfo(ExtensionConfiguration extensionConfiguration, TagConfiguration tagConfiguration)
        {
            _extensionConfiguration = extensionConfiguration ?? throw new ArgumentNullException(nameof(extensionConfiguration));
            _tagConfiguration = tagConfiguration ?? throw new ArgumentNullException(nameof(tagConfiguration));
        }
    }
}