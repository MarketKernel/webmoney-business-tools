using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Xml2WinForms.Templates;

namespace Xml2WinForms
{
    public interface IAtomControl
    {
        void ApplyValue(object value);
        Action ApplyBehavior(IDictionary<string, IControlHolder> namedControlHolders, BehaviorRule rule);
        void SetErrorProvider(ErrorProvider errorProvider);
        bool Validate(IDictionary<string, IControlHolder> namedControlHolders, InspectionRule rule);
        object ObtainValue();
    }
}
