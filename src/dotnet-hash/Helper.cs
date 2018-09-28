using System;

namespace dotnet_hash
{
    public class Helper
    {
        public static string GetHashCode(byte[] value, bool isUpper)
        {
            var hashValue = BitConverter.ToString(value).Replace("-", "");

            if (!isUpper)
            {
                return hashValue.ToLower();
            }
            return hashValue;
        }
    }
}
