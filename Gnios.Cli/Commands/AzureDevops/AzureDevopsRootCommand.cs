using System.CommandLine;
using Gnios.Cli.Settings;

namespace Gnios.Cli.Commands.AzureDevops;

public class AzureDevopsRootCommand : Command
{
    public AzureDevopsRootCommand(AppConfiguration appConfig, RootCommand rootCommand)
        : base("devops", "Azure DevOps CLI")
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        var syncCommand = new SyncCommand(baseDirectory, appConfig, rootCommand);
        var searchCommand = new SearchCommand(baseDirectory, syncCommand);
        AddCommand(syncCommand);
        AddCommand(searchCommand);
    }
}