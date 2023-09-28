using SW.Core.Types;
using SW.Core.Entities;
using SW.Core.Services;
using SW.Core.Contracts;

namespace SW.Infrastructure.RebateCalculators;

public class FixedRateRebateCalculator : IRebateCalculator
{
    public SupportedIncentiveType SupportedIncentiveType => SupportedIncentiveType.FixedRateRebate;

    public CalculateRebateResponse Calculate(CalculateRebateRequest request, Product product, Rebate rebate)
    {
        var result = new CalculateRebateResponse();

        if (rebate.Percentage == 0 || product.Price == 0 || request.Volume == 0)
        {
            result.Success = false;
        }
        else
        {
            result.RebateAmount = product.Price * rebate.Percentage * request.Volume;
            result.Success = true;
        }

        return result;
    }
}
