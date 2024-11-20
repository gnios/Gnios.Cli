using System;
using System.CommandLine;
using System.Threading.Tasks;
using Gnios.AzureDevops.Cli.Commands;
using Gnios.AzureDevops.Cli.Settings;

namespace Gnios.AzureDevops.Cli;

public class ConsoleApplication(AppConfiguration appConfig)
{
    public async Task<int> Run(string[] args)
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        var syncCommand = new SyncCommand(baseDirectory, appConfig);
        var searchCommand = new SearchCommand(baseDirectory, appConfig, syncCommand);

        var rootCommand = new RootCommand
        {
            syncCommand,
            searchCommand
        };

        string[] debugArgs = ["search", "Funding"];
        return await rootCommand.InvokeAsync(debugArgs);
        // return await rootCommand.InvokeAsync(args);
    }
}