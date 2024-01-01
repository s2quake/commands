namespace JSSoft.Commands.AppUI;

static class FrameworkExtensions
{
#if NETFRAMEWORK
    public static void AppendJoin(this System.Text.StringBuilder @this, string separator, params string[] values)
    {
        @this.Append(string.Join(separator, values));
    }
#endif 
}
