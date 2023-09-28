using SW.Core.Types;
using SW.Core.Entities;
using SW.Core.Contracts;

namespace SW.Infrastructure.Data;

public class RebateDataStore : IRepository<Rebate>
{
    public Rebate Get(string id)
    {
        // TODO: Access database to retrieve account (code removed for brevity)
        return new Rebate
        {
            Identifier = id,
            Incentive = IncentiveType.FixedRateRebate,
            Amount = 1m,
            Percentage = id == "R1" ? 0.1m : 0m,
        };
    }

    public Rebate Save(Rebate rebate)
    {
        // TODO: Update account in database (code removed for brevity)
        return rebate;
    }
}
