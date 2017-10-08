using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using Xml2WinForms.Templates;
using Xml2WinForms.Utils;

namespace Xml2WinForms
{
    public sealed class AtomControlHolder : IControlHolder
    {
        private readonly Color _errorColor = Color.Pink;

        private readonly ReadOnlyCollection<InspectionRule> _inspectionRules;
        private readonly ReadOnlyCollection<BehaviorRule> _behaviorRules;
        private readonly List<Action> _initActions;

        private ErrorProvider _errorProvider;

        public string Name { get; }
        public Label Label { get; set; }
        public Control Control { get; }

        public AtomControlHolder(string name, Label label, Control control, List<InspectionRule> inspectionRules, List<BehaviorRule> behaviorRules)
        {
            if (null == control)
                throw new ArgumentNullException(nameof(control));

            if (!(control is IAtomControl))
                throw new InvalidOperationException("!(control is IAtomControl)");

            Name = name;
            Label = label;
            Control = control;

            if (null != inspectionRules)
                _inspectionRules = new ReadOnlyCollection<InspectionRule>(inspectionRules);

            if (null != behaviorRules)
                _behaviorRules = new ReadOnlyCollection<BehaviorRule>(behaviorRules);

            _initActions = new List<Action>();
        }

        public IEnumerable<IControlHolder> CollectNamedControlHolders()
        {
            if (null != Name)
                return new List<IControlHolder> {this};

            return new List<IControlHolder>();
        }

        public void ApplyValue(object value)
        {
            ((IAtomControl) Control).ApplyValue(value);
        }

        public void ApplyBehavior(IDictionary<string, IControlHolder> namedControlHolders)
        {
            if (null == namedControlHolders)
                throw new ArgumentNullException(nameof(namedControlHolders));

            if (null == _behaviorRules)
                return;

            var atomControl = (IAtomControl) Control;

            foreach (var behaviorRule in _behaviorRules)
            {
                var action = atomControl.ApplyBehavior(namedControlHolders, behaviorRule);

                if (null == action)
                    continue;

                _initActions.Add(action);
            }
        }

        public void InitControl()
        {
            foreach (var initAction in _initActions)
            {
                initAction();
            }
        }

        public void SetErrorProvider(ErrorProvider errorProvider)
        {
            if (null == errorProvider)
                throw new ArgumentNullException(nameof(errorProvider));

            if (null == _inspectionRules || _inspectionRules.Count < 0)
                return;

            if (!ApplicationUtility.IsRunningOnMono)
            {
                var atomControl = (IAtomControl) Control;
                atomControl.SetErrorProvider(errorProvider);

                _errorProvider = errorProvider;
            }

            Control.Enter += (sender, args) =>
            {
                ClearHighlight();
            };
        }

        public IEnumerable<InspectionReport> Inspect(IDictionary<string, IControlHolder> namedControlHolders)
        {
            if (null == namedControlHolders)
                throw new ArgumentNullException(nameof(namedControlHolders));

            if (null == _inspectionRules || _inspectionRules.Count < 0)
                yield break;

            if (!Control.Enabled || !Control.Visible)
                yield break;

            ClearHighlight();

            foreach (var inspectionRule in _inspectionRules)
            {
                var atomControl = (IAtomControl) Control;

                if (null == atomControl)
                    continue;

                var isValid = atomControl.Validate(namedControlHolders, inspectionRule);

                if (isValid)
                    continue;

                var errorMessage = inspectionRule.Message;

                HighlightError(errorMessage);

                yield return new InspectionReport(errorMessage, !ApplicationUtility.IsRunningOnMono);
            }
        }

        public object ObtainValue()
        {
            return ((IAtomControl)Control).ObtainValue();
        }

        public IEnumerable<object> SelectValues()
        {
            if (!Control.Enabled || !Control.Visible)
                yield return null;
            else
                yield return ObtainValue();
        }

        private void HighlightError(string errorMessage)
        {
            if (null == errorMessage)
                throw new ArgumentNullException(nameof(errorMessage));

            if (!ApplicationUtility.IsRunningOnMono)
            {
                if (null == _errorProvider)
                    throw new InvalidOperationException("null == _errorProvider");

                _errorProvider.SetError(Control, errorMessage);
            }
            else
            {
                if (null != Label)
                    Label.BackColor = _errorColor;
            }
        }

        private void ClearHighlight()
        {
            if (!ApplicationUtility.IsRunningOnMono)
            {
                if (null == _errorProvider)
                    throw new InvalidOperationException("null == _errorProvider");

                _errorProvider.SetError(Control, null);
            }
            else
                Label?.ResetBackColor();
        }
    }
}