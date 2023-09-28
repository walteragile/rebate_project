namespace SW.Core.Services;

public interface IRebateService
{
    CalculateRebateResponse Calculate(CalculateRebateRequest request);
}
