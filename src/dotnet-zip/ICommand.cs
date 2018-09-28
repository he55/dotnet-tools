using McMaster.Extensions.CommandLineUtils;

namespace dotnet_zip
{
    public interface ICommand
    {
        string Source { get; }
        string Output { get; }
        int OnExecute(CommandLineApplication app);
    }
}
