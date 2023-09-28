using SW.Core.Types;
using SW.Core.Entities;

namespace SW.Infrastructure.Data;

public class RebateDataStore
{
    public Rebate GetRebate(string rebateIdentifier)
    {
        // TODO: Access database to retrieve account (code removed for brevity)
        return new Rebate
        {
            Identifier = rebateIdentifier,
            Incentive = IncentiveType.FixedRateRebate,
            Amount = 1m,
            Percentage = rebateIdentifier == "R1" ? 0.1m : 0m,
        };
    }

    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        // Update account in database, code removed for brevity
    }
}
