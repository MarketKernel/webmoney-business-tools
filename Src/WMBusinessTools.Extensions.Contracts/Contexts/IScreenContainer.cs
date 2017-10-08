using System.Windows.Forms;

namespace WMBusinessTools.Extensions.Contracts.Contexts
{
    public interface IScreenContainer : IWin32Window
    {
        void OnStartProgress();
        void OnStopProgress();
    }
}