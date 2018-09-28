using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using McMaster.Extensions.CommandLineUtils;

namespace dotnet_hash
{
    [Command(Description = "string command")]
    public class StringCommand : ICommand
    {
        [Required]
        [Argument(0, Description = "strings")]
        public string[] Arguments { get; }

        [Option(CommandOptionType.NoValue, Description = "is upper")]
        public bool IsUpper { get; }

        [Option("-ha|--hash-name <HASH_NAME>", Description = "hash name")]
        [AllowedValues("md5", "sha1", "sha256", "sha384", "sha512")]
        public string HashName { get; } = "md5";

        public int OnExecute(CommandLineApplication app)
        {
            using (var algorithm = HashAlgorithm.Create(HashName))
            {
                foreach (var item in Arguments)
                {
                    var buffer = Encoding.Default.GetBytes(item);
                    var hash = algorithm.ComputeHash(buffer);
                    var hashCode = Helper.GetHashCode(hash, IsUpper);

                    Console.WriteLine("{0}: '{1}'", hashCode, item);
                }
            }
            return 0;
        }
    }
}
