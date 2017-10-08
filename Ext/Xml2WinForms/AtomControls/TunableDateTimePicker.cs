using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Xml2WinForms.Templates;

namespace Xml2WinForms
{
    public sealed class TunableDateTimePicker : DateTimePicker, IAtomControl
    {
        public void ApplyTemplate(DateTimePickerTemplate template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            Format = DateTimePickerFormat.Custom;
            CustomFormat = template.Format;
            Value = template.DefaultValue;
        }

        public void ApplyValue(object value)
        {
            Value = (DateTime?)value ?? throw new ArgumentNullException(nameof(value));
        }

        public Action ApplyBehavior(IDictionary<string, IControlHolder> namedControlHolders, BehaviorRule rule)
        {
            if (null == namedControlHolders)
                throw new ArgumentNullException(nameof(namedControlHolders));

            if (null == rule)
                throw new ArgumentNullException(nameof(rule));

            return null;
        }

        public void SetErrorProvider(ErrorProvider errorProvider)
        {
            if (null == errorProvider)
                throw new ArgumentNullException(nameof(errorProvider));

        }

        public bool Validate(IDictionary<string, IControlHolder> namedControlHolders, InspectionRule rule)
        {
            if (null == namedControlHolders)
                throw new ArgumentNullException(nameof(namedControlHolders));

            if (null == rule)
                throw new ArgumentNullException(nameof(rule));

            return true;
        }

        public object ObtainValue()
        {
            return Value;
        }
    }
}