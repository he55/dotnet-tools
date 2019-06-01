using System;

namespace dotnet_date
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var internetTime = DateTimeOffset.Now.GetInternetTime();
            var localTime = DateTimeOffset.Now;

            Console.WriteLine("Current date and time:");
            Console.WriteLine("Current date and time2:");
            Console.WriteLine("{0,-8}: {1}", "Local", localTime.ToString());
            Console.WriteLine("{0,-8}: {1}", "Internet", internetTime?.ToString() ?? "null");
        }
    }
}
