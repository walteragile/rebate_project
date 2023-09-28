using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SW.Core.Entities;
using SW.Core.Services;
using SW.Core.Contracts;
using SW.Infrastructure.Data;
using SW.Infrastructure.RebateCalculators;
using SW.Infrastructure.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Test application is running!");
    using IHost host = CreateHostBuilder(args).Build();
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;
    services.GetRequiredService<App>().Run(args);
}
catch (Exception exception)
{
    Log.Error(exception, "A fatal error has ocurred.");
}
finally
{
    Log.Information("Test application has ended.");
    Log.CloseAndFlush();
}

IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<App>();
            services.AddSingleton<IRebateService, RebateService>();
            services.AddScoped<IRebateCalculator, FixedRateRebateCalculator>();
            services.AddScoped<IRepository<Product>, ProductDataStore>();
            services.AddScoped<IRepository<Rebate>, RebateDataStore>();
        });
}