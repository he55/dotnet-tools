using System;
using System.IO;
using System.Text;

namespace tree
{
    class Program
    {
        static bool s_isOutputRedirected = Console.IsOutputRedirected;
        static int s_bufferWidth = Console.BufferWidth;
        static int s_depth;
        static int s_line;

        static int Main(string[] args)
        {
            string dir = args.Length == 0 ? "." : args[0];
            if (!Directory.Exists(dir))
            {
                Console.WriteLine("Usage: dotnet-tree [directory]\nlist contents of directories in a tree-like format.");
                return 1;
            }

            PrintDirectory(dir);
            return 0;
        }

        static void PrintDirectory(string path)
        {
            if (s_depth > 15)
            {
                Console.WriteLine("err.");
                Environment.Exit(1);
            }

            string[] paths = Directory.GetFileSystemEntries(path);
            ++s_depth;
            for (int i = 0; i < paths.Length; i++)
            {
                int index = paths.Length - 1;
                string formatValue = string.Format(
                    "{0} {1}",
                    i == index ? "\\--" : "|--",
                    Path.GetFileName(paths[i]));

                PrintName(formatValue);

                if (Directory.Exists(paths[i]))
                {
                    if (i == index)
                    {
                        PrintDirectory(paths[i]);
                    }
                    else
                    {
                        s_line ^= 1 << s_depth;
                        PrintDirectory(paths[i]);
                        s_line ^= 1 << s_depth;
                    }
                }
            }
            --s_depth;
        }

        static void PrintName(string name)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 1; i < s_depth; i++)
            {
                int value = (s_line >> i) & 1;
                stringBuilder.Append(value == 1 ? "|   " : "    ");
            }

            stringBuilder.Append(name);
            string formatName = stringBuilder.ToString();

            if (s_isOutputRedirected)
            {
                Console.WriteLine(formatName);
            }
            else
            {
                Console.WriteLine(formatName.Length > s_bufferWidth ? formatName.Substring(0, s_bufferWidth) : formatName);
            }
        }
    }
}
