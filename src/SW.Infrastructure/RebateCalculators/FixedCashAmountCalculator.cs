using SW.Core.Types;
using SW.Core.Entities;
using SW.Core.Services;
using SW.Core.Contracts;

namespace SW.Infrastructure.RebateCalculators;

public class FixedCashAmountCalculator : IRebateCalculator
{
    public SupportedIncentiveType SupportedIncentiveType => SupportedIncentiveType.FixedCashAmount;

    public CalculateRebateResponse Calculate(CalculateRebateRequest request, Product product, Rebate rebate)
    {
        var result = new CalculateRebateResponse();

        if (rebate.Amount == 0)
        {
            result.Success = false;
        }
        else
        {
            result.RebateAmount = rebate.Amount;
            result.Success = true;
        }

        return result;
    }
}
