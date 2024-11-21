# Gnios.Cli

This project is a comprehensive solution designed to host multiple Command Line Interfaces (CLIs) for various purposes.

## Commands

| Command       | Description                          | Options                                                                                     |
|---------------|--------------------------------------|---------------------------------------------------------------------------------------------|
| `configure`   | Configure the app                    | None                                                                                        |
| `devops sync` | Synchronize variable groups          | None                                                                                        |
| `devops search` | Search for a variable group and variables | `-vg`, `--variableGroup` (VariableGroupName: The name of the variable group to search for) <br> `-vn`, `--variableName` (VariableName: The name of the variable to search for) |

## Examples

### Configure the app
```gnios configure```

### Synchronize variable groups

```gnios configure```

### Search for a variable group and variables
```gnios devops search -vg MyVariableGroup -vn MyVariableName```

