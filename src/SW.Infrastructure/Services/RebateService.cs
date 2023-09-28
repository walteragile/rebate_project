using SW.Core.Contracts;
using SW.Core.Entities;
using SW.Core.Services;
using SW.Infrastructure.Data;

namespace SW.Infrastructure.Services;

public class RebateService : IRebateService
{
    private readonly IRebateCalculator _rebateCalculator;
    private readonly IRepository<Product> _productDataStore;
    private readonly IRepository<Rebate> _rebateDataStore;

    public RebateService(IRebateCalculator rebateCalculator, IRepository<Product> productDataStore, IRepository<Rebate> rebateDataStore)
    {
        _rebateCalculator = rebateCalculator;
        _productDataStore = productDataStore;
        _rebateDataStore = rebateDataStore;
    }

    public CalculateRebateResponse Calculate(CalculateRebateRequest request)
    {
        var result = new CalculateRebateResponse();

        var product = _productDataStore.Get(request.ProductIdentifier);
        if (product == null)
        {
            result.Success = false;
            return result;
        }

        var rebate = _rebateDataStore.Get(request.RebateIdentifier);
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

        result = _rebateCalculator.Calculate(request, product, rebate);

        if (result.Success)
        {
            rebate.Amount = result.RebateAmount;
            _rebateDataStore.Save(rebate);
        }

        return result;
    }
}
