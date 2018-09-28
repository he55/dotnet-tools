using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using McMaster.Extensions.CommandLineUtils;

namespace dotnet_hash
{
    [Command(Description = "dotnet-hash is tool for get hash value.")]
    [Subcommand("string", typeof(StringCommand))]
    [Subcommand("file", typeof(FileCommand))]
    public class Program
    {
        public static int Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                return CommandLineApplication.Execute<Program>(args);
            }
            finally
            {
                stopWatch.Stop();
                System.Console.WriteLine("run time: {0}", stopWatch.Elapsed);
            }
        }

        public int OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
            return 0;
        }
    }
}
