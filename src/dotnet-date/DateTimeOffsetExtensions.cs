using System.IO;
using System.Net.Sockets;

namespace System
{
    public static class DateTimeOffsetExtensions
    {
        public static DateTimeOffset? GetInternetTime(this DateTimeOffset date)
        {
            // NIST Internet Time Servers
            // https://www.nist.gov/pml/time-and-frequency-division/services/internet-time-service-its
            // https://tf.nist.gov/tf-cgi/servers.cgi

            try
            {
                using (var tcpClient = new TcpClient("time.nist.gov", 13))
                using (var networkStream = tcpClient.GetStream())
                using (var streamReader = new StreamReader(networkStream))
                {
                    // JJJJJ YR-MO-DA HH:MM:SS TT L DUT1 msADV UTC(NIST) OTM
                    var str = streamReader.ReadToEnd();

                    if (str == "" ||
                        str.Substring(38, 9) != "UTC(NIST)" || // This signature should be there.
                        str.Substring(30, 1) != "0" || // Server reports non-optimun status, time off by as much as 5 seconds.
                        !DateTimeOffset.TryParse(str.Substring(7, 17) + " +00:00", out DateTimeOffset result))
                    {
                        return null;
                    }
                    return result.ToLocalTime();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
