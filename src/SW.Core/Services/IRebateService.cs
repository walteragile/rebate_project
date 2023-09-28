namespace SW.Core.Services;

public interface IRebateService
{
    void SetRebateCalculator(string rebateCalculatorName);
    CalculateRebateResponse Calculate(CalculateRebateRequest request);
}
