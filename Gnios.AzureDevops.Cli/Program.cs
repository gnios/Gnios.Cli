using System;
using System.IO;
using System.Threading.Tasks;
using Gnios.AzureDevops.Cli.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gnios.AzureDevops.Cli;

internal class Program
{
    private static ServiceProvider _serviceProvider;

    private static async Task<int> Main(string[] args)
    {
        var appSettingPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(appSettingPath, optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();

        RegisterServices(configuration);

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var consoleApplication = scope.ServiceProvider.GetRequiredService<ConsoleApplication>();
            return await consoleApplication.Run(args);
        }
        finally
        {
            DisposeServices();
        }
    }

    private static void RegisterServices(IConfiguration configuration)
    {
        var services = new ServiceCollection();
        var appConfiguration = new AppConfiguration();
        configuration.Bind("AppConfiguration", appConfiguration);
        services.AddSingleton(appConfiguration);
        services.AddSingleton<ConsoleApplication>();
        _serviceProvider = services.BuildServiceProvider(true);
    }

    private static void DisposeServices()
    {
        _serviceProvider?.Dispose();
    }
}