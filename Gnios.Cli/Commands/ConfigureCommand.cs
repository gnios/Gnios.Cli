using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using Newtonsoft.Json;
using Spectre.Console;

namespace Gnios.Cli.Commands;

public class ConfigureCommand : Command
{
    public ConfigureCommand()
        : base("configure", "configure the app")
    {
        Handler = CommandHandler.Create(async () => await InitializeAndSaveVariables());
    }

    private async Task InitializeAndSaveVariables()
    {
        var appSettingPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

        // Load the entire configuration
        var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(appSettingPath));

        // Get the AppConfiguration section
        var appConfigSection = jsonObject["AppConfiguration"].ToString();
        var appConfigSectionJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(appConfigSection);

        // Ask the user for new values
        foreach (var key in appConfigSectionJson.Keys.ToList())
        {
            var currentValue = appConfigSectionJson[key]?.ToString() ?? string.Empty;
            var input = AnsiConsole.Ask<string>($"Enter value for [green]{key}[/] (current value: [yellow]{currentValue}[/]):", currentValue);
            appConfigSectionJson[key] = input;
        }

        // Update the AppConfiguration section in the configuration
        jsonObject["AppConfiguration"] = appConfigSectionJson;

        // Save the entire configuration back to the appsettings.json file
        var updatedJson = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
        await File.WriteAllTextAsync(appSettingPath, updatedJson);
    }
}