using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Chinese24SolarTerms
{
    internal class Program
    {
        const string libCocoa = "libCocoa.dylib";

        [DllImport(libCocoa)]
        static extern IntPtr getDesktopImage();

        [DllImport(libCocoa)]
        static extern bool setDesktopImage(string path);

        static void Main(string[] args)
        {
            Chinese24SolarTermsCalendar calendar = new Chinese24SolarTermsCalendar(DateTimeOffset.Now);
            Console.WriteLine($"上一个节气: {calendar.PreviousSolarTerm}");
            Console.WriteLine($"当前节气:   {calendar.CurrentSolarTerm}");
            Console.WriteLine($"下一个节气: {calendar.NextSolarTerm}");

            int year = DateTimeOffset.Now.Year;
            Console.WriteLine($"\n{year} 年所有节气:");
            SolarTermInfo[] solarTermInfos = Chinese24SolarTermsCalendar.GetSolarTermsWithYear(year);
            for (int i = 0; i < solarTermInfos.Length; i++)
            {
                Console.WriteLine($"{solarTermInfos[i]}");
            }

            if (args.Length > 0 && File.Exists(args[0]))
            {
                setDesktopImage(args[0]);
                return;
            }

            string imagePath = Path.Combine(AppContext.BaseDirectory, $"images/{calendar.CurrentSolarTerm.Index + 1}.jpg");
            if (File.Exists(imagePath))
            {
                setDesktopImage(imagePath);
            }
        }
    }
}
