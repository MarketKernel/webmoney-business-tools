using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebMoney.Services.BusinessObjects;

namespace WebMoney.Services.DataAccess.EF
{
    internal static class TransferExtensions
    {
        public static List<Transfer> Select(this DbSet<Transfer> transfers, long currentIdentifier,
            string purse, DateTime fromTime, DateTime toTime)
        {
            if (null == purse)
                throw new ArgumentNullException(nameof(purse));

            var result = new List<Transfer>();

            result.AddRange(from t in transfers
                where t.Identifier == currentIdentifier &&
                      t.SourcePurse == purse &&
                      t.UpdateTime >= fromTime &&
                      t.UpdateTime <= toTime
                select t);

            result.AddRange(from t in transfers
                where t.Identifier == currentIdentifier &&
                      t.TargetPurse == purse &&
                      t.UpdateTime >= fromTime &&
                      t.UpdateTime <= toTime
                select t);

            return result.OrderByDescending(t => t.UpdateTime).ToList();
        }
    }
}
