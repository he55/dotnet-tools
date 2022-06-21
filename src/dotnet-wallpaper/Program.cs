using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Chinese24SolarTerms
{
    public class Program
    {
        private const string libCocoa = "libCocoa.dylib";

        [DllImport(libCocoa)]
        public static extern IntPtr getDesktopImage();

        [DllImport(libCocoa)]
        public static extern bool setDesktopImage(string path);

        public static void Main(string[] args)
        {
            if (args.Length > 0 && File.Exists(args[0]))
            {
                setDesktopImage(args[0]);
                return;
            }

            var jieqi = new Chinese24SolarTermsCalendar(DateTimeOffset.Now);
            var imagePath = Path.Combine(AppContext.BaseDirectory, $"images/{jieqi.CurrentSolarTerm.Index + 1}.jpg");

            if (File.Exists(imagePath))
            {
                setDesktopImage(imagePath);
            }
        }
    }
}
