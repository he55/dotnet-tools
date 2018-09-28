using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using SharpCompress.Archives;
using SharpCompress.Common;

namespace dotnet_zip
{
    [Command(Description = "decompression file.")]
    public class DecompressionCommand : ICommand
    {
        [Required]
        [FileExists]
        [Argument(0, Description = "zip file.")]
        public string Source { get; }

        [LegalFilePath]
        [Option(Description = "output directory.")]
        public string Output { get; }

        public int OnExecute(CommandLineApplication app)
        {
            string outputPath = Output == null ? Path.GetFileNameWithoutExtension(Source) : Output;

            var extractionOptions = new ExtractionOptions()
            {
                ExtractFullPath = true,
                Overwrite = true
            };

            try
            {
                ArchiveFactory.WriteToDirectory(Source, outputPath, extractionOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }

            return 0;
        }
    }
}
