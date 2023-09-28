using SW.Core.Types;
using SW.Core.Entities;
using SW.Core.Services;

namespace SW.Core.Contracts;

public interface IRebateCalculator
{
    SupportedIncentiveType SupportedIncentiveType { get; }

    CalculateRebateResponse Calculate(CalculateRebateRequest request, Product product, Rebate rebate);
}
