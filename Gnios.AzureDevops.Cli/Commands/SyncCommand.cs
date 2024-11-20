using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.IO;
using System.Threading.Tasks;
using Gnios.AzureDevops.Cli.Extensions;
using Gnios.AzureDevops.Cli.Settings;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Newtonsoft.Json;

namespace Gnios.AzureDevops.Cli.Commands;

public class SyncCommand : Command
{
    public SyncCommand(string baseDirectory, AppConfiguration appConfig) : base("sync", "Synchronize variable groups")
    {
        Handler = CommandHandler.Create(async () => await SyncVars(baseDirectory, appConfig));
    }

    private async Task SyncVars(string baseDirectory, AppConfiguration configuration)
    {
        var variableGroups = await FetchVariableGroups(configuration);
        var directoryPath = EnsureDirectoryExists(baseDirectory);
        
        foreach (var group in variableGroups)
        {
            SaveVariableGroupToFile(directoryPath, group);
        }
        
        Console.WriteLine("Variable groups synchronized successfully.");
    }

    private static async Task<List<VariableGroup>> FetchVariableGroups(AppConfiguration configuration)
    {
        var creds = new VssBasicCredential(string.Empty, configuration.Pat);
        var connection = new VssConnection(new Uri(configuration.CollectionUri), creds);
        var client = connection.GetClient<TaskAgentHttpClient>();
        try
        {
            return await client.GetVariableGroupsAsync(configuration.ProjectName);
        }
        finally
        {
            client?.Dispose();
        }
    }

    private static string EnsureDirectoryExists(string baseDirectory)
    {
        var path = Path.Combine(baseDirectory, "VariableGroupsJsons");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        return path;
    }

    private static void SaveVariableGroupToFile(string directoryPath, VariableGroup group)
    {
        var contents = JsonConvert.SerializeObject(group, Formatting.Indented);
        var fileName = $"{group.Name.ToCamelCase()}-{group.Id}.json";
        File.WriteAllText(Path.Combine(directoryPath, fileName), contents);
    }
}