using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using McMaster.Extensions.CommandLineUtils;

namespace dotnet_zip
{
    [Command(Description = "compression file or directory.")]
    public class CompressionCommand : ICommand
    {
        [Required]
        [FileOrDirectoryExists]
        [Argument(0, Description = "file or directory.")]
        public string Source { get; }

        [LegalFilePath]
        [Option(Description = "output file.")]
        public string Output { get; }

        public int OnExecute(CommandLineApplication app)
        {
            string zipPath = Output == null ? Source.TrimEnd('/', '\\') + ".zip" : Output;
            if (File.Exists(zipPath))
            {
                Console.WriteLine("error: '{0}' is exists", zipPath);
                return -1;
            }

            app.ShowHelp();
            return 0;
        }
    }
}
