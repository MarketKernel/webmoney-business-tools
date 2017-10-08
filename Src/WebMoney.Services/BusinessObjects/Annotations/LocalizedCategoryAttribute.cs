using System;
using System.ComponentModel;
using LocalizationAssistant;

namespace WebMoney.Services.BusinessObjects.Annotations
{
    [AttributeUsage(AttributeTargets.All)]
    internal sealed class LocalizedCategoryAttribute : CategoryAttribute
    {
        public LocalizedCategoryAttribute(string category)
            : base(Translator.Instance.Translate("Settings", category))
        {
        }
    }
}
