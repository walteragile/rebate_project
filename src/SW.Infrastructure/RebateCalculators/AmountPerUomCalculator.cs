using SW.Core.Types;
using SW.Core.Entities;
using SW.Core.Services;
using SW.Core.Contracts;

namespace SW.Infrastructure.RebateCalculators;

public class AmountPerUomCalculator : IRebateCalculator
{
    public SupportedIncentiveType SupportedIncentiveType => SupportedIncentiveType.AmountPerUom;

    public CalculateRebateResponse Calculate(CalculateRebateRequest request, Product product, Rebate rebate)
    {
        var result = new CalculateRebateResponse();

        if (rebate.Amount == 0 || request.Volume == 0)
        {
            result.Success = false;
        }
        else
        {
            result.RebateAmount = rebate.Amount * request.Volume;
            result.Success = true;
        }

        return result;
    }
}
