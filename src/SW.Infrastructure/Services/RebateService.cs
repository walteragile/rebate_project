using Microsoft.Extensions.Logging;
using SW.Core.Types;
using SW.Core.Entities;
using SW.Core.Services;
using SW.Core.Contracts;
using SW.Infrastructure.RebateCalculators;

namespace SW.Infrastructure.Services;

public class RebateService : IRebateService
{
    private IRebateCalculator _rebateCalculator;
    private readonly IRepository<Product> _productDataStore;
    private readonly IRepository<Rebate> _rebateDataStore;
    private readonly ILogger<RebateService> _logger;

    public RebateService(
        IRebateCalculator rebateCalculator, 
        IRepository<Product> productDataStore, 
        IRepository<Rebate> rebateDataStore, 
        ILogger<RebateService> logger)
    {
        _rebateCalculator = rebateCalculator;
        _productDataStore = productDataStore;
        _rebateDataStore = rebateDataStore;
        _logger = logger;
    }

    public void SetRebateCalculator(string rebateCalculatorName)
    {
        try
        {
            var incentiveType = (IncentiveType)Enum.Parse(typeof(IncentiveType), rebateCalculatorName, ignoreCase: true);
            _logger.LogInformation($"Recognized calculator: {incentiveType}");

            _rebateCalculator = incentiveType switch
            {
                IncentiveType.FixedRateRebate => new FixedRateRebateCalculator(),
                IncentiveType.AmountPerUom => new AmountPerUomCalculator(),
                IncentiveType.FixedCashAmount => new FixedCashAmountCalculator(),
                _ => throw new ArgumentException()
            };
        }
        catch (ArgumentException argumentException)
        {
            throw new ArgumentException($"{rebateCalculatorName} is not supported yet. Details: {argumentException.Message}");
        }
        catch (Exception)
        {
            throw;
        }
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
