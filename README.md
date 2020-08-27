# JSSoft.Library.Commands

## 개요

명령 구문을 분석하여 속성의 값을 설정하거나 메서드를 호출할 수 있는 기능을 제공합니다.

명령어 집합을 구축하여 응용프로그램을 제어할 수 있도록 콘솔 환경을 제공합니다.

## 개발 환경

### [Microsoft Visual Studio Community 2019](https://visualstudio.microsoft.com/ko/downloads/)

### [Microsoft Visual Studio Code](https://code.visualstudio.com/)

### [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)


## 빌드 및 실행

```powershell
# clone
git clone https://github.com/s2quake/commands.git --recursive
# change directory
cd commands
# build
dotnet build --framework netcoreapp3.1
# run
dotnet run --project ./JSSoft.Library.Commands/JSSoft.Library.Commands.Repl --framework netcoreapp3.1
```

## Parse

명령 구문을 분석하는 가장 기본적인 방법입니다. 지정된 속성에 값을 설정해주는 기능을 제공합니다.

```csharp
var settings = new Settings();
var parser = new CommandLineParser(settings);
parser.Parse(Environment.CommandLine);
```

## Invoke

Parse의 확장 기능으로써 명령 구문을 분석하여 지정된 메서드를 호출하는 기능을 제공합니다.

```csharp
var commands = new Commands();
var parser = new CommandLineParser(commands);
parser.Invoke(Environment.CommandLine);
```

## CommandContext

보다 많은 명령어를 관리하며 처리할 수 있도록 다양한 기능을 제공합니다.

EditBox, TextBox, InputText 와 같은 사용자 입력과 조합하여 콘솔 또는 REPL 과 같은 환경을 구축할 수 있습니다.

```csharp
var commands = new ICommand[]
{
    new LoginCommand(),
    new LogoutCommand(),
    new ExitCommand()
};
var commandContext = new CommandContext(commands);
commandContext.Execute(Environment.CommandLine);
```

## 속성 정의

### 필수 인자

명령 구문시 필수로 요구되는 값을 정의하기 위해서는 속성에 CommandPropertyRequired 를 정의합니다.

```csharp
[CommandPropertyRequired]
public string Value1 { get; set; }

[CommandPropertyRequired]
public int Value2 { get; set; }
```

```plain
"value"   // error! value for Value2 does not exists.
3         // format error!
"value" 3 // Value1 is "value", Value2 is 3
```

기본값을 설정할 수 있습니다. 명령 구문에 값이 없을 경우 기본값으로 대체 됩니다.

```csharp
[CommandPropertyRequired]
public string Value1 { get; set; }

[CommandPropertyRequired(DefaultValue = 1)]
public int Value2 { get; set; }
```

```plain
"value" 2 // Value1 is "value", Value2 is 2
"value"   // Value1 is "value", Value2 is 1
```

### 명시적 필수 인자

명시적인 필수 인자는 명령 구문에 반드시 값이 있어야 하지만 --value "2" 처럼 스위치가 포함되야 합니다.

```csharp
[CommandPropertyRequired]
public string Value1 { get; set; }

[CommandPropertyRequired(IsExplicit = true)]
public int Value2 { get; set; }
```

```plain
"value"            // error!
"value" 2          // error!
"value" --value2   // error!
"value" --value2 3 // Value1 is "value", Value2 is 3
--value2 3 "value" // Value1 is "value", Value2 is 3
```

명시적 필수 인자의 기본값을 사용하기 위해서는 명령 구문에 --value 와 같이 스위치문이  포함되야 합니다.

```csharp
[CommandPropertyRequired]
public string Value1 { get; set; }

[CommandPropertyRequired(IsExplicit = true, DefaultValue = 1)]
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

### 선택 인자

선택 인자는 스위치문을 사용하여 값의 사용 유무를 정할 수 있습니다.

```csharp
[CommandProperty]
public string Value { get; set; }
```

```plain
--value      // error
--value text // value is "text"
```

기본값을 사용하기 위해서는 명령 구문에 --value 와 같이 스위치문이 포함되야 합니다.

```csharp
[CommandProperty(DefaultValue = "1")]
public string Value { get; set; }
```

```plain
--value      // value is "1"
--value text // value is "text"
```

> 속성이 bool 타입이고 기본값이 명시되어 없다면 자동으로 기본값 true로 설정됩니다.

### 가변 인자

가변 인자는 명령 구문에서 파싱되지 않은 나머지 인자들의 값을 나타냅니다.

가변 인자의 속성 타입은 반드시 배열이여야만 하고 오직 하나의 속성에만 정의되야 합니다.

```csharp
[CommandPropertyArray]
public string[] Values { get; set; }
```

```plain
-- value1 value2 value3 "value4"
```

## 메서드 정의

명령 구문을 통해 특성 메서드를 실행하기 위해서는 다음과 같이 해당 메서드에 CommandMethod를 정의해야 합니다.

메서드의 파라미터는 자동으로 필수 인자로 정의됩니다.

```csharp
[CommandMethod]
public void Save(string message)
{
}
```

```plain
save "message"
```

만약에 메서드에 추가적으로 선택인자를 정의하고 싶다면 CommandMethodProperty 를 사용합니다.

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

아래처럼 params 를 사용할 경우 가변 인자로 사용할 수 있습니다.

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

## 공용 속성 및 메서드

static 으로 정의된 속성 및 메서드를 해당 객체에 포함하여 사용할 수 있습니다.

```csharp
static class GlobalSettings
{
    [CommandProperty]
    public static string ID { get; set; }

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

## 이름

CommandProperty 및 CommandMethod 로 정의된 속성과 메서드 이름은 [kebab-case (spinal-case, Train-Case, Lisp-case)](https://en.wikipedia.org/wiki/Letter_case) 형태로 변경됩니다.

### 속성 이름 예제

| 속성 이름 | 변경된 속성 이름 |
| --------- | ---------------- |
| Value     | --value          |
| Message   | --message        |
| IsLocked  | --is-locked      |

> 이름과 짧은 이름을 사용할때

```csharp
[CommandProperty("custom-value", 'v')]
public string Value { get; set; }
```

```plain
--custom-value or -v
```

> 짧은 이름만 사용할때

```csharp
[CommandProperty('v')]
public string Value { get; set; }
```

```plain
-v
```

> 짧은 이름을 사용하고 기본 이름도 사용할때

```csharp
[CommandProperty('v', AllowName = true)]
public string Value { get; set; }
```

```plain
-v or --value
```

### 메서드 이름 예제

| 메서드 이름 | 변경된 메서드 이름 |
| ----------- | ------------------ |
| Save        | save               |
| LockTable   | lock-table         |

메서드 이름도 직접 설정할 수 있습니다.

```csharp
[CommandMethod("save")]
public void Save(string message)
{
}
```

## 명령어

CommandContext에 사용될 명령어를 정의하는 방법입니다.

```csharp
class ExitCommand : CommandBase
{
    [CommandPropertyRequired(DefaultValue = 0)]
    public int ExitCode
    {
        get; set;
    }

    protected override void OnExecute()
    {
        Environment.Exit(this.ExitCode);
    }
}
```

```plain
exit
exit 0
```

## 하위 명령어

CommandContext에 하위 명령어를 정의하는 방법입니다.

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
    public string Message
    {
        get; set;
    }
}
```

```plain
user create "user1"
user create "user1" --message "new user"
user delete "user1"
user list
```

## 하위 명령어 확장

PartialCommand 특성을 사용하여 하위 명령어를 확장할 수 있습니다.

```csharp
[PartialCommand]
class UserPartialCommand : CommandMethodBase
{
    public UserCommandExtension()
        : base("user")
    {
    }

    [CommandMethod]
    public void SendMessage(string userID, string message)
    {
    }
}
```

## License

MIT License

Copyright (c) 2020 Jeesu Choi

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
