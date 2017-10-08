using System;
using System.ComponentModel;
using LocalizationAssistant;

namespace WebMoney.Services.BusinessObjects.Annotations
{
    [AttributeUsage(AttributeTargets.All)]
    internal sealed class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        public LocalizedDescriptionAttribute(string description)
            : base(Translator.Instance.Translate("Settings", description))
        {
        }
    }
}