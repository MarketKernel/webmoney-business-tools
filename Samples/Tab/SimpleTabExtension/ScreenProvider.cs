using System.Windows.Forms;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;

namespace SimpleTabExtension
{
    public class ScreenProvider : ITopScreenProvider
    {
        public bool CheckCompatibility(ScreenContainerContext context)
        {
            return true;
        }

        public Control GetScreen(ScreenContainerContext context)
        {
            return new UserControl1(context.Session);
        }
    }
}