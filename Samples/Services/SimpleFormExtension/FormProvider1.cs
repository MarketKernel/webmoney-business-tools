using System.Linq;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using WebMoney.Services.Contracts;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;

namespace ServicesFormExtension
{
    public class FormProvider1 : ITopFormProvider
    {
        public bool CheckCompatibility(SessionContext context)
        {
            return true;
        }

        public Form GetForm(SessionContext context)
        {
            var purseService = context.UnityContainer.Resolve<IPurseService>();

            // Получаем список кошельков из базы данных
            var accounts = purseService.SelectAccounts();

            var sum = accounts.Sum(a => a.Amount);

            return new Form1("Сумма на всех кошельках: " + sum);
        }
    }
}
