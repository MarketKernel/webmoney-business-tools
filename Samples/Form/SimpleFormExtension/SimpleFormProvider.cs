using System.Windows.Forms;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;

namespace SimpleFormExtension
{
    public class SimpleFormProvider : ITopFormProvider, IPurseFormProvider
    {
        public bool CheckCompatibility(SessionContext context)
        {
            return true;
        }

        public Form GetForm(SessionContext context)
        {
            return new Form1("Текущий WMID: " + context.Session.CurrentIdentifier.ToString("00000000000000"));
        }

        public bool CheckCompatibility(PurseContext context)
        {
            if (context.Account.Amount > 0)
                return true;

            return false;
        }

        public Form GetForm(PurseContext context)
        {
            return new Form1("Выбран кошелек: " + context.Account.Number);
        }
    }
}
