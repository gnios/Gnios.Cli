using System;
using System.CommandLine;
using System.Threading.Tasks;
using Gnios.AzureDevops.Cli.Commands;
using Gnios.AzureDevops.Cli.Settings;
using Gnios.Cli.Commands;
using Microsoft.Extensions.Configuration;

namespace Gnios.AzureDevops.Cli;

public class ConsoleApplication
{
    private readonly AppConfiguration _appConfig;

    public ConsoleApplication(AppConfiguration appConfig)
    {
        _appConfig = appConfig;
    }
    public async Task<int> Run(string[] args)
    {
        var rootCommand = new RootCommand
        {
            new ConfigureCommand(),
            new AzureDevopsRootCommand(_appConfig)
        };

        // string[] debugArgs = {"configure"};
        // return await rootCommand.InvokeAsync(debugArgs);
        return await rootCommand.InvokeAsync(args);
    }
}