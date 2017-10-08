using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Xml2WinForms
{
    public sealed class GroupBoxControlHolder : IControlHolder
    {
        private readonly TunableGroupBox _control;

        public string Name { get; }

        public Label Label => null;

        public Control Control => _control;

        public GroupBoxControlHolder(string name, TunableGroupBox control)
        {
            Name = name;
            _control = control ?? throw new ArgumentNullException(nameof(control));
        }

        public IEnumerable<IControlHolder> CollectNamedControlHolders()
        {
            var result = new List<IControlHolder>();

            if (null != Name)
                result.Add(this);

            foreach (var controlHolder in _control.ControlHolders)
            {
                if (null != controlHolder.Name)
                    result.Add(controlHolder);

                var groupBoxControlHolder = controlHolder as GroupBoxControlHolder;

                if (null != groupBoxControlHolder)
                    result.AddRange(groupBoxControlHolder.CollectNamedControlHolders());
            }

            return result;
        }

        public void ApplyValue(object value)
        {
            throw new NotSupportedException();
        }

        public void ApplyBehavior(IDictionary<string, IControlHolder> namedControlHolders)
        {
            if (null == namedControlHolders)
                throw new ArgumentNullException(nameof(namedControlHolders));

            foreach (var controlControlHolder in _control.ControlHolders)
            {
                controlControlHolder.ApplyBehavior(namedControlHolders);
            }
        }

        public void InitControl()
        {
            foreach (var controlControlHolder in _control.ControlHolders)
            {
                controlControlHolder.InitControl();
            }
        }

        public void SetErrorProvider(ErrorProvider errorProvider)
        {
            if (null == errorProvider)
                throw new ArgumentNullException(nameof(errorProvider));

            foreach (var controlControlHolder in _control.ControlHolders)
            {
                controlControlHolder.SetErrorProvider(errorProvider);
            }
        }

        public IEnumerable<InspectionReport> Inspect(IDictionary<string, IControlHolder> namedControlHolders)
        {
            if (null == namedControlHolders)
                throw new ArgumentNullException(nameof(namedControlHolders));

            if (!Control.Enabled || !Control.Visible)
                return new List<InspectionReport>();

            var inspectionReports = new List<InspectionReport>();

            foreach (var controlHolder in _control.ControlHolders)
            {
                inspectionReports.AddRange(controlHolder.Inspect(namedControlHolders));
            }

            return inspectionReports;
        }

        public object ObtainValue()
        {
            var value = new List<object>();

            foreach (var controlHolder in _control.ControlHolders)
            {
                value.Add(controlHolder.ObtainValue());
            }

            return value;
        }

        public IEnumerable<object> SelectValues()
        {
            var values = new List<object>();

            foreach (var controlHolder in _control.ControlHolders)
            {
                values.AddRange(controlHolder.SelectValues());
            }

            return values;
        }
    }
}