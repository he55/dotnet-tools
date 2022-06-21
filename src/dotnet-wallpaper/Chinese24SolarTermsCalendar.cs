using System;

namespace Chinese24SolarTerms
{
    public class Chinese24SolarTermsCalendar
    {
        private readonly DateTimeOffset _datetime;
        private readonly int _year;

        public Chinese24SolarTermsInfo PreviousSolarTerm { get; private set; }
        public Chinese24SolarTermsInfo CurrentSolarTerm { get; private set; }
        public Chinese24SolarTermsInfo NextSolarTerm { get; private set; }

        public Chinese24SolarTermsCalendar(DateTimeOffset datetime)
        {
            if (datetime < Chinese24SolarTermsData.s_baseDateTime)
            {
                throw new ArgumentException(nameof(datetime));
            }

            _datetime = datetime;
            _year = datetime.Year;

            GetCurrentSolarTerm();
        }

        public static Chinese24SolarTermsInfo[] GetSolarTermsWithYear(int year)
        {
            Chinese24SolarTermsInfo[] solarTerms = new Chinese24SolarTermsInfo[24];
            for (int i = 0; i < 24; i++)
            {
                solarTerms[i] = new Chinese24SolarTermsInfo(year, i);
            }

            return solarTerms;
        }

        private void GetCurrentSolarTerm()
        {
            for (int i = 0; i < 24; i++)
            {
                DateTimeOffset datetime = Chinese24SolarTermsData.GetDateTimeWithSolarTerm(_year, i);
                if (datetime.DayOfYear > _datetime.DayOfYear)
                {
                    // i 表示的是下一个节气
                    switch (i)
                    {
                        case 0: // 下一个节气在本年，前一个节气和当前节气在上一年
                            PreviousSolarTerm = new Chinese24SolarTermsInfo(_year - 1, 22);
                            CurrentSolarTerm = new Chinese24SolarTermsInfo(_year - 1, 23);
                            NextSolarTerm = new Chinese24SolarTermsInfo(_year, 0);
                            return;
                        case 1: // 下一个节气和当前节气在本年，前一个节气在上一年
                            PreviousSolarTerm = new Chinese24SolarTermsInfo(_year - 1, 23);
                            CurrentSolarTerm = new Chinese24SolarTermsInfo(_year, 0);
                            NextSolarTerm = new Chinese24SolarTermsInfo(_year, 1);
                            return;
                        default: // 下一个节气、前一个节气和当前节气在本年
                            PreviousSolarTerm = new Chinese24SolarTermsInfo(_year, i - 2);
                            CurrentSolarTerm = new Chinese24SolarTermsInfo(_year, i - 1);
                            NextSolarTerm = new Chinese24SolarTermsInfo(_year, i);
                            return;
                    }
                }
            }

            // 下一个节气在次年，前一个节气和当前节气在本年
            PreviousSolarTerm = new Chinese24SolarTermsInfo(_year, 22);
            CurrentSolarTerm = new Chinese24SolarTermsInfo(_year, 23);
            NextSolarTerm = new Chinese24SolarTermsInfo(_year + 1, 0);
        }
    }

    public readonly struct Chinese24SolarTermsInfo
    {
        public int Year { get; }
        public int Index { get; }
        public DateTimeOffset DateTime { get; }
        public string Name { get; }

        public Chinese24SolarTermsInfo(int year, int index)
        {
            Year = year;
            Index = index;
            DateTime = Chinese24SolarTermsData.GetDateTimeWithSolarTerm(year, index);
            Name = Chinese24SolarTermsData.s_solarName[index];
        }

        public override string ToString()
        {
            return $"{Name} [{DateTime:yyyy-MM-dd}]";
        }
    }

    internal static class Chinese24SolarTermsData
    {
        internal static readonly string[] s_solarName =
        {
            "小寒", "大寒", "立春", "雨水", "惊蛰", "春分",
            "清明", "谷雨", "立夏", "小满", "芒种", "夏至",
            "小暑", "大暑", "立秋", "处暑", "白露", "秋分",
            "寒露", "霜降", "立冬", "小雪", "大雪", "冬至"
        };

        internal static readonly int[] s_solarData =
        {
            0, 21208, 42467, 63836, 85337, 107014,
            128867, 150921, 173149, 195551, 218072, 240693,
            263343, 285989, 308563, 331033, 353350, 375494,
            397447, 419210, 440795, 462224, 483532, 504758
        };

        // 以 1900/1/6 02:05:00 +08:00 作为基准时间
        internal static readonly DateTimeOffset s_baseDateTime =
            new DateTimeOffset(1900, 1, 6, 2, 5, 0, TimeSpan.FromHours(8));

        internal static DateTimeOffset GetDateTimeWithSolarTerm(int year, int index)
        {
            // 以基准时间作参照，y 年的第 i 个节气的时间（按分钟计算）
            double minutes = 525948.76 * (year - 1900) + s_solarData[index];
            return s_baseDateTime.AddMinutes(minutes);
        }
    }
}
