using Microsoft.Extensions.Logging;
using SW.Core.Services;

public class App
{
    private readonly IRebateService _rebateService;
    private readonly ILogger<App> _logger;

    private const string INCENTIVE_TYPE_PARAM_NAME = "incentivetype";
    private const string PRODUCT_ID_PARAM_NAME = "productid";
    private const string REBATE_ID_PARAM_NAME = "rebateid";
    private const string VOLUME_PARAM_NAME = "volume";

    public App(IRebateService rebateService, ILogger<App> logger)
    {
        _rebateService = rebateService;
        _logger = logger;
    }

    public void Run(string[] args)
    {
        _logger.LogInformation("Rebate service has started!");
        var request = new CalculateRebateRequest();
        ReadParameters(args, request);

        var result = _rebateService.Calculate(request);

        if (result.Success)
        {
            _logger.LogInformation($"Operation was successfull. New rebate: {result.RebateAmount:C}");
        }
        else
        {
            _logger.LogCritical("Operation failed");
        }
    }

    private void ReadParameters(string[] args, CalculateRebateRequest request)
    {
        for (int argsIndex = 0; argsIndex < args.Length; argsIndex++)
        {
            if (args[argsIndex].ToLower().StartsWith($"{INCENTIVE_TYPE_PARAM_NAME}=")) 
            {
                _rebateService.SetRebateCalculator(args[argsIndex].Substring(INCENTIVE_TYPE_PARAM_NAME.Length + 1));
            }
            else if (args[argsIndex].ToLower().StartsWith($"{PRODUCT_ID_PARAM_NAME}="))
            {
                request.ProductIdentifier = args[argsIndex].Substring(PRODUCT_ID_PARAM_NAME.Length + 1);
            }
            else if (args[argsIndex].ToLower().StartsWith($"{REBATE_ID_PARAM_NAME}="))
            {
                request.RebateIdentifier = args[argsIndex].Substring(REBATE_ID_PARAM_NAME.Length + 1);
            }
            else if (args[argsIndex].ToLower().StartsWith($"{VOLUME_PARAM_NAME}="))
            {
                request.Volume = Convert.ToDecimal(args[argsIndex].Substring(VOLUME_PARAM_NAME.Length + 1));
            }
        }
    }
}