using McMaster.Extensions.CommandLineUtils;

namespace dotnet_hash
{
    public interface ICommand
    {
        string[] Arguments { get; }
        bool IsUpper { get; }
        string HashName { get; }
        int OnExecute(CommandLineApplication app);
    }
}
