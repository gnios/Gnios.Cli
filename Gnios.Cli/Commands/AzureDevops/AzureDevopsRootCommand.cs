using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ConsoleTables;
using Gnios.AzureDevops.Cli.Settings;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Newtonsoft.Json;

namespace Gnios.AzureDevops.Cli.Commands;

public class AzureDevopsRootCommand : Command
{
    public AzureDevopsRootCommand(AppConfiguration appConfig)
        : base("devops", "Azure DevOps CLI")
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        var syncCommand = new SyncCommand(baseDirectory, appConfig);
        var searchCommand = new SearchCommand(baseDirectory, appConfig, syncCommand);
        AddCommand(syncCommand);
        AddCommand(searchCommand);
    }
}