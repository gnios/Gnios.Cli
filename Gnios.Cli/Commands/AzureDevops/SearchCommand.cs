using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Text.RegularExpressions;
using ConsoleTables;
using Gnios.Cli.Settings;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Newtonsoft.Json;

namespace Gnios.Cli.Commands.AzureDevops;

public class SearchCommand : Command
{
    private readonly SyncCommand _syncCommand;

    public SearchCommand(string baseDirectory, AppConfiguration appConfig, SyncCommand syncCommand)
        : base("search", "Search for a variable group and variables")
    {
        _syncCommand = syncCommand;
        AddArgument(new Argument<string>("variableGroupName", "The name of the variable group to search for"));
        AddArgument(new Argument<string>("variableName", () => string.Empty, "The name of the variable to search for"));
        Handler = CommandHandler.Create<string, string>(async (variableGroupName, variableName) => await SearchVariableGroupAndVariables(variableGroupName, variableName, baseDirectory));
    }

    private async Task SearchVariableGroupAndVariables(
        string variableGroupName,
        string variableName,
        string baseDirectory)
    {
        var directoryPath = Path.Combine(baseDirectory, "VariableGroupsJsons");
        if (!Directory.Exists(directoryPath))
        {
            string[] args = {"sync"};
            await _syncCommand.InvokeAsync(args);
        }

        var loadedGroups = LoadVariableGroupsFromDirectory(directoryPath);
        var matchedGroups = FindMatchingGroups(loadedGroups, variableGroupName);
        DisplayMatchedGroups(matchedGroups, variableName);
    }

    private static List<VariableGroup> LoadVariableGroupsFromDirectory(string directoryPath)
    {
        var variableGroups = new List<VariableGroup>();
        foreach (var file in Directory.GetFiles(directoryPath, "*.json"))
        {
            var variableGroup = JsonConvert.DeserializeObject<VariableGroup>(File.ReadAllText(file));
            variableGroups.Add(variableGroup);
        }

        return variableGroups;
    }

    private static List<VariableGroup> FindMatchingGroups(List<VariableGroup> loadedGroups, string variableGroupName)
    {
        var regex = new Regex($".*{Regex.Escape(variableGroupName)}.*", RegexOptions.IgnoreCase);
        var matchingGroups = new List<VariableGroup>();
        foreach (var group in loadedGroups)
        {
            if (!string.IsNullOrEmpty(group.Name) && regex.IsMatch(group.Name))
                matchingGroups.Add(group);
        }

        return matchingGroups;
    }

    private static void DisplayMatchedGroups(List<VariableGroup> matchedGroups, string variableName)
    {
        var regex = new Regex($".*{Regex.Escape(variableName)}.*", RegexOptions.IgnoreCase);
        foreach (var group in matchedGroups)
        {
            Console.WriteLine($"Variable Group ID: {group.Id}, Name: {group.Name}");
            var table = new ConsoleTable("Variable Name", "Value");
            foreach (var variable in group.Variables)
            {
                if (string.IsNullOrEmpty(variableName) || regex.IsMatch(variable.Key))
                    table.AddRow(variable.Key, variable.Value.Value);
            }

            table.Write();
            Console.WriteLine();
        }
    }
}