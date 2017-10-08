using System.Collections.Generic;
using System.Windows.Forms;

namespace Xml2WinForms
{
    public interface IControlHolder
    {
        string Name { get; }
        Label Label { get; }
        Control Control { get; }

        IEnumerable<IControlHolder> CollectNamedControlHolders();
        void ApplyValue(object value);
        void ApplyBehavior(IDictionary<string, IControlHolder> namedControlHolders);
        void InitControl();
        void SetErrorProvider(ErrorProvider errorProvider);
        IEnumerable<InspectionReport> Inspect(IDictionary<string, IControlHolder> namedControlHolders);
        object ObtainValue();
        IEnumerable<object> SelectValues();
    }
}