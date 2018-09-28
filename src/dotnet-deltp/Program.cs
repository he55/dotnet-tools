using System;
using System.IO;

namespace dotnet_deltp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var xlPath = "/Applications/Thunder.app";
            var xlPlugInsPath = "/Applications/Thunder.app/Contents/PlugIns";
            var xlPlugIns = new string[]
            {
                // "Thunder Extension.appex",
                "activitycenter.xlplugin",
                "advertising.xlplugin",
                // "applications.xlplugin",
                "bbassistant.xlplugin",
                // "browserhelper.xlplugin",
                // "details.xlplugin",
                "featuredpage.xlplugin",
                "feedback.xlplugin",
                "iOSThunder.xlplugin",
                // "liveupdate.xlplugin",
                "lixianspace.xlplugin",
                "myvip.xlplugin",
                // "preferences.xlplugin",
                // "searchtask.xlplugin",
                "softmanager.xlplugin",
                "subtitle.xlplugin",
                "userlogin.xlplugin",
                "viprenew.xlplugin",
                "viptask.xlplugin",
                "viptips.xlplugin",
                "xiazaibao.xlplugin",
                "xlbrowser.xlplugin",
                "xlplayer.xlplugin"
            };

            if (!Directory.Exists(xlPath))
            {
                Console.WriteLine("not install: {0}", xlPath);
                return;
            }

            foreach (var plug in xlPlugIns)
            {
                var plugPath = Path.Combine(xlPlugInsPath, plug);
                if (!Directory.Exists(plugPath))
                {
                    Console.WriteLine("deleted: {0}", plugPath);
                    continue;
                }

                Directory.Delete(plugPath, true);
                Console.WriteLine("delete: {0}", plugPath);
            }

            Console.WriteLine("done");
        }
    }
}
