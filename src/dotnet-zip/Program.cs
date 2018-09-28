using McMaster.Extensions.CommandLineUtils;

namespace dotnet_zip
{
    [Command(Description = "dotnet-zip is a decompression tool.\nSupported Archive Formats: Zip, GZip, Tar, Rar, 7Zip.")]
    [Subcommand("cc", typeof(CompressionCommand))]
    [Subcommand("dc", typeof(DecompressionCommand))]
    public class Program
    {
        public static int Main(string[] args)
        {
            return CommandLineApplication.Execute<Program>(args);
        }

        public int OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return 0;
        }
    }
}
