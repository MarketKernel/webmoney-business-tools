using System;
using System.ComponentModel;
using LocalizationAssistant;

namespace WebMoney.Services.BusinessObjects.Annotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event)]
    internal sealed class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        public LocalizedDisplayNameAttribute(string displayName)
            : base(Translator.Instance.Translate("Settings", displayName))
        {
        }
    }
}
