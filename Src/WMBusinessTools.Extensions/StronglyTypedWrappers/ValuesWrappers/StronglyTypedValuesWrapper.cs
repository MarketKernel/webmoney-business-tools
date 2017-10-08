using System;
using System.Collections.Generic;
using Xml2WinForms;

namespace WMBusinessTools.Extensions.StronglyTypedWrappers
{
    internal abstract class StronglyTypedValuesWrapper
    {
        private Dictionary<string, object> _incomeValues;
        private List<object> _outcomeValues;

        public void CopyIncomeValuesTo(SubmitForm submitForm)
        {
            if (null == submitForm)
                throw new ArgumentNullException(nameof(submitForm));

            submitForm.ApplyValues(_incomeValues);
        }

        public Dictionary<string, object> CollectIncomeValues()
        {
            return _incomeValues;
        }

        public void ApplyOutcomeValues(List<object> outcomeValues)
        {
            _outcomeValues = outcomeValues ?? throw new ArgumentNullException(nameof(outcomeValues));
        }

        protected object GetValue(int index)
        {
            if (null == _outcomeValues)
                throw new InvalidOperationException("null == _outcomeValues");

            if (index < 0 || index > _outcomeValues.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            return _outcomeValues[index];
        }

        protected void SetValue(string name, object value)
        {
            if (null == name)
                throw new ArgumentNullException(nameof(name));

            if (null == _incomeValues)
                _incomeValues = new Dictionary<string, object>();

            _incomeValues[name] = value;
        }
    }
}
