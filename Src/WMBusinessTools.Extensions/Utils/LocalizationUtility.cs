using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using WebMoney.Services.Contracts.BasicTypes;

namespace WMBusinessTools.Extensions.Utils
{
    internal static class LocalizationUtility
    {
        private const string EnCultureName = "en-US";
        private const string RuCultureName = "ru-RU";

        public static void ApplyLanguage(Language language)
        {
            switch (language)
            {
                case Language.Russian:
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(RuCultureName);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(RuCultureName);
                    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(RuCultureName);
                    CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(RuCultureName);
                    break;
                default:
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(EnCultureName);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(EnCultureName);
                    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(EnCultureName);
                    CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(EnCultureName);
                    break;
            }
        }

        public static void TranslateForm(Form form)
        {
            if (null == form)
                throw new ArgumentNullException(nameof(form));

            var formType = form.GetType();
            var resourceManager = new ResourceManager(formType);

            form.Text = (string)resourceManager.GetObject("$this.Text");

            var formFields = formType.GetFields(BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);

            foreach (var fieldInfo in formFields)
            {
                object field = fieldInfo.GetValue(form);

                var menu = field as Menu;

                if (menu != null)
                {
                    foreach (MenuItem item in menu.MenuItems)
                    {
                        string resourceText = (string)resourceManager.GetObject(item.Name + ".Text");
                        item.Text = resourceText;
                    }
                }
                else if (field is DataGridView)
                {
                    var dataGridView = (DataGridView)field;

                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        string hText = (string)resourceManager.GetObject(column.Name + ".HeaderText");
                        string tText = (string)resourceManager.GetObject(column.Name + ".ToolTipText");

                        column.HeaderText = hText;
                        column.ToolTipText = tText;
                    }
                }
                else
                {
                    var propertyInfo = fieldInfo.FieldType.GetProperty("Text");

                    if (null == propertyInfo)
                        continue;

                    string textProperty = (string)resourceManager.GetObject(fieldInfo.Name + '.' + propertyInfo.Name);

                    if (string.IsNullOrEmpty(textProperty))
                        continue;

                    if (field != null)
                        propertyInfo.SetValue(field, textProperty, null);
                }
            }
        }
    }
}
