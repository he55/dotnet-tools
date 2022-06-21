using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace pstree
{
    class Program
    {
        static int Main(string[] args)
        {
            if (OperatingSystem.IsWindows())
            {
                Console.WriteLine("Windows is not supported. Supports Unix, Linux and Windows Subsystem for Linux.");
                return 1;
            }

            if (args.Length > 0)
            {
                Console.WriteLine("Usage: dotnet-pstree\ndisplay a tree of processes.");
                return 1;
            }

            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = "ps";
            processStartInfo.Arguments = "-axwwo user,pid,ppid,pgid,command";
            processStartInfo.RedirectStandardOutput = true;

            Process process = Process.Start(processStartInfo);
            process.WaitForExit();

            List<PSInfo> pss = GetPSs(process.StandardOutput);
            PSInfo root = FindRootPS(pss);
            root.PrintTree();

            return 0;
        }

        static PSInfo FindRootPS(List<PSInfo> pss)
        {
            PSInfo root = null;
            for (int i = 0; i < pss.Count; i++)
            {
                if (pss[i].PPID == 0)
                {
                    root = pss[i];
                    continue;
                }

                for (int j = 0; j < pss.Count; j++)
                {
                    if (pss[i].PPID == pss[j].PID)
                    {
                        pss[j].Children.Add(pss[i]);
                        break;
                    }
                }
            }

            Debug.Assert(root != null);
            return root;
        }

        static List<PSInfo> GetPSs(StreamReader standardOutput)
        {
            List<PSInfo> ps = new List<PSInfo>();

            string header = standardOutput.ReadLine();
            if (string.IsNullOrEmpty(header))
            {
                return ps;
            }

            for (; ; )
            {
                string line = standardOutput.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                int index = 0;
                int flag = 0;
                PSInfo psInfo = new PSInfo();

                for (int i = 0; i < line.Length; i++)
                {
                    if (flag == 0 && line[i] != ' ')
                    {
                        index = i;
                        flag = 1;
                    }
                    else if (flag == 1 && psInfo._index == 4)
                    {
                        flag = 0;
                        psInfo.SetValue(line.Substring(index));
                        break;
                    }
                    else if (flag == 1 && line[i] == ' ')
                    {
                        flag = 0;
                        psInfo.SetValue(line.Substring(index, i - index));
                    }
                    else if (flag == 1 && i == line.Length - 1)
                    {
                        flag = 0;
                        psInfo.SetValue(line.Substring(index, i - index));
                    }
                }

                Debug.Assert(flag != 1);
                ps.Add(psInfo);
            }
            return ps;
        }
    }

    public class PSInfo
    {
        static bool s_isOutputRedirected = Console.IsOutputRedirected;
        static int s_bufferWidth = Console.BufferWidth;
        static int s_depth;
        static int s_line;

        internal int _index;

        public string User { get; set; }
        public int PID { get; set; }
        public int PPID { get; set; }
        public int PGID { get; set; }
        public string Command { get; set; }
        public List<PSInfo> Children { get; } = new List<PSInfo>();

        public void SetValue(string value)
        {
            Debug.Assert(_index <= 4);
            switch (_index)
            {
                case 0:
                    User = value;
                    break;
                case 1:
                    PID = int.Parse(value);
                    break;
                case 2:
                    PPID = int.Parse(value);
                    break;
                case 3:
                    PGID = int.Parse(value);
                    break;
                case 4:
                    Command = value;
                    break;
            }
            ++_index;
        }

        public void PrintTree()
        {
            Print(0);
        }

        void Print(int pt)
        {
            string prefix;
            if (pt == 0)
            {
                prefix = "-+=";
            }
            else if (Children.Count > 0 && pt == 4)
            {
                prefix = "|-+=";
            }
            else if (Children.Count > 0 && pt == 3)
            {
                prefix = "\\-+=";
            }
            else if (pt == 3)
            {
                prefix = "\\---";
            }
            else
            {
                prefix = "|--=";
            }

            StringBuilder stringBuilder = new StringBuilder();
            if (s_depth > 0)
            {
                stringBuilder.Append(" ");
            }

            for (int i = 1; i < s_depth; i++)
            {
                int value = (s_line >> i) & 1;
                stringBuilder.Append(value == 1 ? "| " : "  ");
            }

            string formatValue = string.Format(
                "{0}{1} {2} {3} {4}",
                stringBuilder.ToString(),
                prefix,
                PID,
                User,
                Command);

            if (s_isOutputRedirected)
            {
                Console.WriteLine(formatValue);
            }
            else
            {
                Console.WriteLine(formatValue.Length > s_bufferWidth ? formatValue.Substring(0, s_bufferWidth) : formatValue);
            }

            ++s_depth;
            for (int i = 0; i < Children.Count; i++)
            {
                if (i == Children.Count - 1)
                {
                    Children[i].Print(3);
                }
                else
                {
                    s_line ^= 1 << s_depth;
                    Children[i].Print(4);
                    s_line ^= 1 << s_depth;
                }
            }
            --s_depth;
        }
    }
}
