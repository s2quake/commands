# Ntreev.Library.Commands

## Summary

Parses a string to set a value to a specified property or invoke a specified method.

Like Terminal or Powershell, it provides a REPL environment to provide a command-based development environment.

## Requirements

```plain
dotnet sdk 7.0.400
c# 11
```

## Clone

```plain
git clone https://github.com/s2quake/commands.git --recursive
```

> This repository contains two submodules, so the ``--recursive`` option must be included when cloning.
>
> Or run ``git submodule udpate --init --`` on the repository path.

## Build - NET 7.0

```plain
dotnet build
```

## Other Framework Build

```shell
# net6.0
dotnet build -p:TargetFrameworks=net6.0 --framework net6.0

# net5.0
dotnet build -p:TargetFrameworks=net5.0 --framework net5.0

# netcoreapp3.1
dotnet build -p:TargetFrameworks=netcoreapp3.1 --framework netcoreapp3.1

# netstandard2.1
dotnet build -p:TargetFrameworks=netstandard2.1 --framework netstandard2.1

```

## Run Examples

```shell
# Run the property settings example project
dotnet run --project JSSoft.Library.Commands/JSSoft.Library.Commands.Parse --framework net7.0 -- --help

# Run the method call example project
dotnet run --project JSSoft.Library.Commands/JSSoft.Library.Commands.Invoke --framework net7.0 -- --help

# Run the CommandContext Execution example Project
dotnet run --project JSSoft.Library.Commands/JSSoft.Library.Commands.Sets --framework net7.0 -- --help

# Run the CommandContext Execution Example Project in the REPL environment
dotnet run --project JSSoft.Library.Commands/JSSoft.Library.Commands.Repl --framework net7.0

# Run the CommandContext Execution Example Project with Avalonia UI
dotnet run --project JSSoft.Library.Commands/JSSoft.Library.Commands.AppUI --framework net7.0
```

## Parse

This is the most basic way to parse the command. Provides a function to set a value for a specified property.

```csharp
var settings = new Settings();
var parser = new CommandParser(settings);
parser.Parse(args);
```

> See the JSSoft.Library.Commands/JSSoft.Library.Commands.Parse project

## Invoke

As an extension of Parse, it provides the ability to call a specified method by parsing the command.

```csharp
var commands = new Commands();
var invoker = new CommandInvoker(commands);
invoker.Invoke(args);
```

> See the JSSoft.Library.Commands/JSSoft.Library.Commands.Invoke project

## CommandContext

It provides various functions to manage and process more commands.

```csharp
var commands = new ICommand[]
{
    new LoginCommand(),
    new LogoutCommand(),
    new ExitCommand()
};
var commandContext = new CommandContext(commands);
commandContext.Execute(args);

or 

await commandContext.ExecuteAsync(args);
```

> See the JSSoft.Library.Commands/JSSoft.Library.Commands.Sets project

It can be combined with user input such as EditBox, TextBox, InputText to build a console or REPL-like environment.

```csharp
var commands = new ICommand[]
{
    new LoginCommand(),
    new LogoutCommand(),
    new ExitCommand()
};
var commandContext = new CommandContext(commands);
var terminal = new SystemTerminal(commandContext);
await terminal.StartAsync(CancellationToken.None);
```

> See the JSSoft.Library.Commands/JSSoft.Library.Commands.Repl project

## Property

### Required argument definition

To define the value required for command syntax, define **CommandPropertyRequired** in the property.

```csharp
[CommandPropertyRequired]
public string Value1 { get; set; } = string.Empty;

[CommandPropertyRequired]
public int Value2 { get; set; }
```

```plain
"value"   // error! value for Value2 does not exists.
3         // format error!
"value" 3 // Value1 is "value", Value2 is 3
```

You can set default values ​​like this: If there is no value in the command syntax, it is replaced with the default value.

```csharp
[CommandPropertyRequired]
public string Value1 { get; set; } = string.Empty;

[CommandPropertyRequired(DefaultValue = 1)]
public int Value2 { get; set; }
```

```plain
"value" 2 // Value1 is "value", Value2 is 2
"value"   // Value1 is "value", Value2 is 1
```

### Explicit required argument definition

An explicit required argument indicates that the command syntax must have a value, but must include a switch statement, such as --value "2".

```csharp
[CommandPropertyRequired]
public string Value1 { get; set; } = string.Empty;

[CommandPropertyExplicitRequired]
public int Value2 { get; set; }
```

```plain
"value"            // error!
"value" 2          // error!
"value" --value2   // error!
"value" --value2 3 // Value1 is "value", Value2 is 3
--value2 3 "value" // Value1 is "value", Value2 is 3
```

In order to use the default values ​​of explicit required arguments, the command syntax must include a switch statement such as --value.

```csharp
[CommandPropertyRequired]
public string Value1 { get; set; } = string.Empty;

[CommandPropertyExplicitRequired(DefaultValue = 1)]
public int Value2 { get; set; }
```

```plain
"value"            // error!
"value" 2          // error!
"value" --value2   // Value1 is "value", Value2 is 1
"value" --value2 3 // Value1 is "value", Value2 is 3
--value2 3 "value" // Value1 is "value", Value2 is 3
--value2 "value"   // error! "value" is not int
```

### Optional argument definition

The optional argument can be set whether or not to use a value using a switch statement.

```csharp
[CommandProperty]
public string Value { get; set; } = string.Empty;
```

```plain
--value      // error
--value text // value is "text"
```

To use the default, the command syntax must include a switch statement such as --value.

```csharp
[CommandProperty(DefaultValue = "1")]
public string Value { get; set; } = string.Empty;
```

```plain
--value      // value is "1"
--value text // value is "text"
```

A bool type switch statement that does not use a value should be defined as follows.

```csharp
[CommandPropertySwitch]
public bool Switch { get; set; }
```

### Variable arguments definition

Variable arguments represent the values ​​of the remaining arguments that were not parsed in the command syntax.

The property type of a variable arguments must be an array and must be defined for only one property.

```csharp
[CommandPropertyArray]
public string[] Values { get; set; } = Array.Empty<string>();
```

```plain
-- value1 value2 value3 "value4"
```

## Method

### Method definition

To execute an attribute method through command syntax, you must define a **CommandMethod** in the method as follows.

Each parameter of the method is automatically defined as a required argument.

```csharp
[CommandMethod]
public void Save(string message)
{
}
```

```plain
save "message"
```

If you want to additionally define optional arguments in the method, you can use **CommandMethodProperty** and add the name of the property defined as CommandProperty.

```csharp
[CommandMethod]
[CommandMethodProperty("Value")]
public void Save(string message)
{
}
```

```plain
save "comment"
save "comment" --value text
```

You can use params as below as a variable arguments.

```csharp
[CommandMethod]
public void Save(string message, params string[] args)
{
}
```

```plain
save "comment"
save "comment" -- "1" "text" "string"
```

### Enable or Disable Method

Define the properties by prefixing "Can" to the method name as shown below.

```csharp
public bool Can{MethodName} { get; }
```

example:

```csharp
public bool CanSave => true;
```

## Static properties and methods

Properties and methods defined as static can be included in the object and used.

```csharp
static class GlobalSettings
{
    [CommandProperty]
    public static string ID { get; set; } = string.Empty;

    [CommandProperty]
    public static string Password { get; set; }
}

[CommandStaticProperty(typeof(GlobalSettings))]
class Settings
{
}
```

```csharp
static class StaticCommand
{
    [CommandMethod]
    [CommandMethodProperty(nameof(Value))]
    public static void List()
    {
    }

    [CommandProperty]
    public static int Value { get; set; }
}

[CommandStaticMethod(typeof(StaticCommand))]
class Commands
{
}
```

## Naming

The property and method names defined as CommandProperty and CommandMethod are changed to [kebab-case (spinal-case, Train-Case, Lisp-case)](https://en.wikipedia.org/wiki/Letter_case).

### Property name example

| Property name | Changed property name |
| --------- | ---------------- |
| Value     | --value          |
| Message   | --message        |
| IsLocked  | --is-locked      |

> When using a name and a short name

```csharp
[CommandProperty("custom-value", 'v')]
public string Value { get; set; } = string.Empty;
```

```plain
--custom-value or -v
```

> When using only short names

```csharp
[CommandProperty('v')]
public string Value { get; set; } = string.Empty;
```

```plain
-v
```

> When using a short name and a default name

```csharp
[CommandProperty('v', AllowName = true)]
public string Value { get; set; } = string.Empty;
```

```plain
-v or --value
```

### Method name example

| Method name | Changed method name  |
| ----------- | ------------------ |
| Save        | save               |
| LockTable   | lock-table         |

You can also set the method name yourself.

```csharp
[CommandMethod("save")]
public void Save(string message)
{
}
```

## Command

You can define commands in the CommandContext.

```csharp
class ExitCommand : CommandBase
{
    [CommandPropertyRequired(DefaultValue = 0)]
    public int ExitCode { get; set; }

    protected override void OnExecute()
    {
        Environment.Exit(ExitCode);
    }
}
```

```plain
exit
exit 0
```

## SubCommand

You can define commands that have subcommands in CommandContext.

```csharp
class UserCommand : CommandMethodBase
{
    [CommandMethod]
    [CommandMethodProperty(nameof(Message))]
    public void Create(string userID)
    {
    }

    [CommandMethod]
    public void Delete(string userID)
    {
    }

    [CommandMethod]
    public void List()
    {
    }

    [CommandProperty]
    public string Message { get; set; }
}
```

```plain
user create "user1"
user create "user1" --message "new user"
user delete "user1"
user list
```

## SubCommand extension

By implementing a partial class, you can add subcommand to the already implemented command.

```csharp
[PartialCommand]
class UserPartialCommand : CommandMethodBase
{
    public UserPartialCommand()
        : base("user")
    {
    }

    [CommandMethod]
    public void SendMessage(string userID, string message)
    {
    }
}
```

## SubCommand AsyncMethod

You can use asynchronous methods, as shown in the example below.
The parameters CancellationToken and IProgress<ProgressInfo> are optional, but should always be the last declaration.

For more information, see the ``Choosing the overloads to provide`` topic in the [TAP](https://learn.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap).

The name of an asynchronous method is used without the suffix Async.

```csharp
class UserCommand : CommandMethodBase
{
    [CommandMethod]
    public Task Invoke1Async()
    {
        return Task.CompletedTask;
    }

    [CommandMethod]
    public Task Invoke2Async(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    [CommandMethod]
    public Task Invoke3Async(IProgress<ProgressInfo> progress)
    {
        return Task.CompletedTask;
    }

    [CommandMethod]
    public Task Invoke4Async(CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        return Task.CompletedTask;
    }
}
```

## License

Released under the MIT License.

Copyright (c) 2020 Jeesu Choi

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
