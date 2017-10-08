using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebMoney.Services.BusinessObjects;

namespace WebMoney.Services.DataAccess.EF
{
    internal static class AccountExtensions
    {
        public static List<Account> Select(this DbSet<Account> accounts, long currentIdentifier)
        {
            return (from a in accounts
                    where a.Identifier == currentIdentifier
                select a).ToList();
        }
    }
}
