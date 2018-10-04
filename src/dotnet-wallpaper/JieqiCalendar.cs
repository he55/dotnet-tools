using System;

namespace dotnet_wallpaper
{
    public class JieqiCalendar
    {
        private static readonly string[] s_solarName =
        {
            "小寒", "大寒", "立春", "雨水", "惊蛰", "春分",
            "清明", "谷雨", "立夏", "小满", "芒种", "夏至",
            "小暑", "大暑", "立秋", "处暑", "白露", "秋分",
            "寒露", "霜降", "立冬", "小雪", "大雪", "冬至"
        };

        private static readonly int[] s_solarData =
        {
            0, 21208, 42467, 63836, 85337, 107014,
            128867, 150921, 173149, 195551, 218072, 240693,
            263343, 285989, 308563, 331033, 353350, 375494,
            397447, 419210, 440795, 462224, 483532, 504758
        };

        // 以 1900/1/6 02:05:00 +08:00 作为基准时间
        private static readonly DateTimeOffset s_baseDate =
            new DateTimeOffset(1900, 1, 6, 2, 5, 0, TimeSpan.FromHours(8));

        private readonly DateTimeOffset _date;
        private readonly int _year;

        public JieqiCalendar() :
            this(DateTimeOffset.Now)
        {
        }

        public JieqiCalendar(DateTimeOffset date)
        {
            if (date < s_baseDate)
            {
                throw new ArgumentException(nameof(date));
            }

            _date = date;
            _year = date.Year;

            Jq();
            Jq2();
        }

        public JieqiInfo Previous { get; private set; }
        public JieqiInfo Now { get; private set; }
        public JieqiInfo Next { get; private set; }
        public JieqiInfo[] All { get; private set; }

        private void Jq()
        {
            for (int i = 0; i < 24; i++)
            {
                var jieqi = GetJieqi(_year, i);

                if (jieqi.DayOfYear > _date.DayOfYear)
                {
                    // i 表示的是下一个节气
                    switch (i)
                    {
                        case 0: // 下一个节气在本年，前一个节气和当前节气在上一年
                            Previous = new JieqiInfo(_year - 1, 22);
                            Now = new JieqiInfo(_year - 1, 23);
                            Next = new JieqiInfo(_year, 0);
                            return;
                        case 1: // 下一个节气和当前节气在本年，前一个节气在上一年
                            Previous = new JieqiInfo(_year - 1, 23);
                            Now = new JieqiInfo(_year, 0);
                            Next = new JieqiInfo(_year, 1);
                            return;
                        default: // 下一个节气、前一个节气和当前节气在本年
                            Previous = new JieqiInfo(_year, i - 2);
                            Now = new JieqiInfo(_year, i - 1);
                            Next = new JieqiInfo(_year, i);
                            return;
                    }
                }
            }

            // 下一个节气在次年，前一个节气和当前节气在本年
            Previous = new JieqiInfo(_year, 22);
            Now = new JieqiInfo(_year, 23);
            Next = new JieqiInfo(_year + 1, 0);
        }

        private void Jq2()
        {
            var jqs = new JieqiInfo[24];

            for (int i = 0; i < 24; i++)
            {
                jqs[i] = new JieqiInfo(_year, i);
            }

            All = jqs;
        }

        private static DateTimeOffset GetJieqi(int year, int index)
        {
            // 以基准时间作参照，y 年的第 i 个节气的时间（按分钟计算）
            double minutes = 525948.76 * (year - 1900) + s_solarData[index];
            return s_baseDate.AddMinutes(minutes);
        }

        public readonly struct JieqiInfo
        {
            public int Year { get; }
            public int Index { get; }
            public int Number { get; }
            public DateTimeOffset Date { get; }
            public string Name { get; }

            public JieqiInfo(int year, int index)
            {
                Year = year;
                Index = index;
                Number = index + 1;
                Date = JieqiCalendar.GetJieqi(year, index);
                Name = JieqiCalendar.s_solarName[index];
            }

            public override string ToString()
            {
                return $"{Name} [{Date:yyyy-MM-dd}]";
            }
        }
    }
}
