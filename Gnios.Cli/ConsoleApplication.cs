﻿using System;
using System.CommandLine;
using System.Threading.Tasks;
using Gnios.Cli.Commands;
using Gnios.Cli.Settings;
using Gnios.Cli.Commands;
using Gnios.Cli.Commands.AzureDevops;
using Microsoft.Extensions.Configuration;

namespace Gnios.Cli;

public class ConsoleApplication
{
    private readonly AppConfiguration _appConfig;

    public ConsoleApplication(AppConfiguration appConfig)
    {
        _appConfig = appConfig;
    }

    public async Task<int> Run(string[] args)
    {
        var rootCommand = new RootCommand { new ConfigureCommand() };
        rootCommand.Add(new AzureDevopsRootCommand(_appConfig, rootCommand));

        // string[] debugArgs = {"devops","search", "-vg", "MyVariableGroup", "-vn", "MyVariableName"};
        // return await rootCommand.InvokeAsync(debugArgs);
        return await rootCommand.InvokeAsync(args);
    }
}