using System.Windows.Forms;

namespace WMBusinessTools.Extensions.Contracts
{
    public interface IScreenProvider<in TContext>
        where TContext : class
    {
        bool CheckCompatibility(TContext context);
        Control GetScreen(TContext context);
    }
}
