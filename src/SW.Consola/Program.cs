using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Test application is running!");
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