using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace dotnet_info
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var ls = new List<(string, string)>();
            ls.Add("Header", "Machine");
            ls.Add("MachineName", Environment.MachineName);
            ls.Add("UserDomainName", Environment.UserDomainName);
            ls.Add("UserName", Environment.UserName);
            ls.Add("OSVersion", Environment.OSVersion.ToString());
            ls.Add("OSDescription", RuntimeInformation.OSDescription);
            ls.Add("OSArchitecture", RuntimeInformation.OSArchitecture.ToString());
            ls.Add("ProcessArchitecture", RuntimeInformation.ProcessArchitecture.ToString());
            ls.Add("RuntimeDirectory", RuntimeEnvironment.GetRuntimeDirectory());
            ls.Add("LogicalDrives", string.Join(':', Environment.GetLogicalDrives()));
            ls.Add("SystemDirectory", Environment.SystemDirectory);
            ls.Add("Line", "");

            ls.Add("Header", "Region/Time");
            ls.Add("CurrentRegion", RegionInfo.CurrentRegion.ToString());
            ls.Add("CurrentCulture", CultureInfo.CurrentCulture.ToString());
            ls.Add("CurrentUICulture", CultureInfo.CurrentUICulture.ToString());
            ls.Add("Local", TimeZoneInfo.Local.ToString());
            ls.Add("Now", DateTimeOffset.Now.ToString());
            ls.Add("UtcNow", DateTimeOffset.UtcNow.ToString());
            ls.Add("Line", "");

            ls.Add("Header", "Network");
            foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
            {
                var adrs = item.GetIPProperties().UnicastAddresses;
                if (adrs.Count > 0)
                {
                    ls.Add("Name", item.Name);
                    ls.Add("NetworkInterfaceType", item.NetworkInterfaceType.ToString());
                    foreach (var item2 in adrs)
                    {
                        ls.Add(item2.Address.AddressFamily.ToString(), item2.Address.ToString());
                    }
                    ls.Add("Line", "");
                }
            }

            ls.Add("Header", "Environment");
            foreach (var item in Environment.GetEnvironmentVariables().Cast<DictionaryEntry>().OrderBy(d => d.Key))
            {
                ls.Add(item.Key.ToString(), item.Value.ToString());
            }

            foreach (var item in ls)
            {
                switch (item.Item1)
                {
                    case "Line":
                        Console.WriteLine();
                        break;
                    case "Header":
                        int width = (Console.WindowWidth == 0) ? 68 : Console.WindowWidth;
                        int averageWidth = (width - item.Item2.Length) / 2;
                        Console.WriteLine("{0}{1}{0}", new string('-', averageWidth), item.Item2);
                        break;
                    default:
                        Console.WriteLine("{0,-20}: {1}", item.Item1, item.Item2);
                        break;
                }
            }
        }
    }

    public static class ListExtensions
    {
        public static void Add<T1, T2>(this ICollection<(T1, T2)> collection, T1 name, T2 value)
        {
            collection.Add((name, value));
        }
    }
}
