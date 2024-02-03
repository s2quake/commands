// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

namespace JSSoft.Terminals.Hosting.Ansi;

static class ModeUtility
{
    private static readonly Dictionary<string, IMode> ModeByName;

    static ModeUtility()
    {
        var types = typeof(ModeUtility).Assembly.GetTypes();
        var query = from type in types
                    where typeof(IMode).IsAssignableFrom(type) == true
                    where type.IsAbstract != true
                    select type;
        var items = query.ToArray();
        var modeByName = new Dictionary<string, IMode>(items.Length);
        foreach (var item in items)
        {
            var obj = (IMode)Activator.CreateInstance(item)!;
            modeByName.Add(obj.Name, obj);
        }
        ModeByName = modeByName;
    }

    public static IMode GetMode(string name) => ModeByName[name];

    public static bool TryGetMode(string name, out IMode value)
        => ModeByName.TryGetValue(name, out value!);

    public static void Process(TerminalLineCollection lines, SequenceContext context)
    {
        var options = context.Option.Split(';', options: StringSplitOptions.RemoveEmptyEntries);
        foreach (var item in options)
        {
            if (TryGetMode(item, out var mode) == true)
            {
                mode.Process(lines, context);
            }
            else
            {
                Console.WriteLine($"Mode '{mode}' is not supported.");
            }
        }
    }
}
