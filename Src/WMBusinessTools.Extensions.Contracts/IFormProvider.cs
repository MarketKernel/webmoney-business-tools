using System.Windows.Forms;

namespace WMBusinessTools.Extensions.Contracts
{
    public interface IFormProvider<in TContext>
        where TContext : class
    {
        bool CheckCompatibility(TContext context);
        Form GetForm(TContext context);
    }
}