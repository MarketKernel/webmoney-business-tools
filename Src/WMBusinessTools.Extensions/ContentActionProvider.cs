using System;
using System.Diagnostics;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions
{
    public sealed class ContentActionProvider : ITopActionProvider
    {
        public bool CheckCompatibility(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return true;
        }

        public void RunAction(SessionContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            var extensionConfiguration =
                context.ExtensionManager.TryObtainExtensionConfiguration(ExtensionCatalog.Content);

            if (null == extensionConfiguration)
                throw new InvalidOperationException("null == extensionConfiguration");

            if (!CheckUrl(extensionConfiguration.ConfigurationString))
                throw new BadTemplateException(
                    $"Wrong url (url=\"{extensionConfiguration.ConfigurationString}\")!");

            Process.Start(extensionConfiguration.ConfigurationString);
        }

        private bool CheckUrl(string value)
        {
            Uri uri;

            return Uri.TryCreate(value, UriKind.Absolute, out uri)
                   && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
