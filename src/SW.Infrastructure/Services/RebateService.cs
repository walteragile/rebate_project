using SW.Core.Contracts;
using SW.Core.Services;
using SW.Infrastructure.Data;

namespace SW.Infrastructure.Services;

public class RebateService : IRebateService
{
    private readonly IRebateCalculator _rebateCalculator;

    public RebateService(IRebateCalculator rebateCalculator)
    {
        _rebateCalculator = rebateCalculator;
    }

    public CalculateRebateResponse Calculate(CalculateRebateRequest request)
    {
        var rebateDataStore = new RebateDataStore();
        var productDataStore = new ProductDataStore();

        var result = new CalculateRebateResponse();

        var product = productDataStore.GetProduct(request.ProductIdentifier);
        if (product == null)
        {
            result.Success = false;
            return result;
        }

        var rebate = rebateDataStore.GetRebate(request.RebateIdentifier);
        if (rebate == null)
        {
            result.Success = false;
            return result;
        }

        if (!product.SupportedIncentives.HasFlag(_rebateCalculator.SupportedIncentiveType))
        {
            result.Success = false;
            return result;
        }

        var rebateAmount = 0m;
        result = _rebateCalculator.Calculate(request, product, rebate);

        if (result.Success)
        {
            var storeRebateDataStore = new RebateDataStore();
            storeRebateDataStore.StoreCalculationResult(rebate, rebateAmount);
        }

        return result;
    }
}
