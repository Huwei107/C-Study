//-----------------------------------------------------------------------
// <copyright company="工品一号" file="DateTimeHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2017-2-13 17:03:16
//  功能描述:   【请输入类描述】
//  历史版本:
//          2017-2-13 17:03:16 刘少林 创建DateTimeHelper类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.ComponentModel;

namespace FX.MainForms
{
    public sealed class HelperDateTime
    {
        /// <summary>
        /// 得到本周第一天(以星期一为第一天)
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime GetWeekFirstDayMon(DateTime datetime)
        {
            //星期一为第一天
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);

            //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = (-1) * weeknow;

            //本周第一天
            string FirstDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(FirstDay);
        }

        /// <summary>
        /// 得到本周最后一天(以星期天为最后一天)
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime GetWeekLastDaySun(DateTime datetime)
        {
            //星期天为最后一天
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            weeknow = (weeknow == 0 ? 7 : weeknow);
            int daydiff = (7 - weeknow);

            //本周最后一天
            string LastDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(LastDay);
        }
        /// <summary>
        /// 获取2个时间点之间相差的数值,当returnValueKind为天时返回相隔天数值，当为年时返回相隔年数值
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="returnValueKind">返回值单位类型(其中年,月,天为大约值,并非精确值)</param>
        /// <returns>相差的总秒数值</returns>
        public static double GetTimeIntervalValue(DateTime start, DateTime end, TimeValueKind returnValueKind)
        {
            DateTime temp = DateTime.Now;
            TimeSpan interval = TimeSpan.Zero;
            if (start >= end)
            {
                interval = start - end;
            }
            else
            {
                interval = end - start;
            }
            //一年此代码段设定为365.25天
            //每四年就有一年366天，那麼表示平均一年是 （365＊3＋366）/4 ＝ 365.25天
            //每年每月天数设定值为(31*7+28+30*4)/12
            //公历是一定的，1、3、5、7、8、10、12月都是31天，2月是28天，剩余的都是30天
            switch (returnValueKind)
            {
                case TimeValueKind.Year:
                    return interval.TotalDays / 365.25;
                case TimeValueKind.Month:
                    return interval.TotalDays / ((31 * 7 + 28 + 30 * 4) / 12D);
                case TimeValueKind.Week:
                    return interval.TotalDays / 7;
                case TimeValueKind.Day:
                    return interval.TotalDays;
                case TimeValueKind.Hour:
                    return interval.TotalHours;
                case TimeValueKind.Minute:
                    return interval.TotalMinutes;
                case TimeValueKind.Second:
                    return interval.TotalSeconds;
                case TimeValueKind.Millisecond:
                    return interval.TotalMilliseconds;
                default:
                    return 0;
            }
        }//end method

        /// <summary>
        /// 检测指定日期是否周末(默认周末为2天,从星期六开始算起)
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <returns></returns>
        public bool IsWeekend(DateTime date)
        {
            return IsWeekend(date, 2, DayOfWeek.Saturday);
        }//end method

        /// <summary>
        /// 指定日期是否是国庆假期之间的日期(中国国庆日10月1日)
        /// </summary>
        /// <param name="date">指定日期值</param>
        /// <param name="dayCount">国家规定的假期天数</param>
        /// <returns>布尔值,true周末,false非周末</returns>
        public bool IsNationalDay(DateTime date, int dayCount)
        {
            if (dayCount < 0 || dayCount > 7)
            {
                //国家规定假期数为1-7天
                //默认为1天
                dayCount = 1;
            }
            //年份
            int year = date.Year;
            //月份
            int month = date.Month;
            //天值
            int day = date.Day;
            if (year < 1949 || month != 10)
            {
                //1949中国建国,国庆从1949年10月才有
                return false;
            }
            return day > 1 && day <= dayCount;
        }//end method

        /// <summary>
        /// 检测指定日期是否周末
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <param name="dayCount">不同时期,国家周末规定天数不同,中国现在定为2天(周六,周天)</param>
        /// <param name="week">星期几开始算周末,中国现在定为2天(周六,周天)</param>
        /// <returns>布尔值,true周末,false非周末</returns>
        public bool IsWeekend(DateTime date, int dayCount, DayOfWeek week)
        {
            bool result = false;
            if (dayCount < 0 || dayCount > 5)
            {
                //周末天数应该大于0且小于5
                //默认为2天周末
                dayCount = 2;
            }
            //星期几开始算周末区间
            int start = (int)week;
            int[] doubleWeek = { 0, 1, 2, 3, 4, 5, 6, 0, 1, 2, 3, 4, 5, 6 };
            int checkValue = ((int)date.DayOfWeek);
            for (int i = start; i < dayCount; i++)
            {
                if (doubleWeek[i] == checkValue)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }//end method

        /// <summary>
        /// 指定日期是否是节假日(0:普通日期,1:春节,2:清明节,3:劳动节,4:暑假,5:中秋,6:国庆,7:元旦,8:寒假,9:周末)
        /// </summary>
        /// <param name="date">给定日期</param>
        /// <returns>整型数值(0:普通日期,1:春节,2:清明节,3:劳动节,4:暑假,5:中秋,6:国庆,7:元旦,8:寒假,9:周末)</returns>
        public int IsHoliday(DateTime date)
        {
            int result = 0;
            string qingming = "三月初三";
            string zhongqiu = "八月十五";
            string chuyi = "正月初一";
            string chuer = "正月初二";
            string traditionalYear = GetTraditionalDate(date);
            //判断元旦日
            if (date.Month == 1 && date.Day == 1) { result = 7; }
            //判断春节时间段(除夕,初一,初二)
            else if (traditionalYear.IndexOf(chuyi) >= 0 || traditionalYear.IndexOf(chuer) >= 0) { result = 1; }
            else if (GetTraditionalDate(date.AddDays(1)).IndexOf(chuyi) >= 0) { result = 1; }
            //判断清明节日
            else if (traditionalYear.IndexOf(qingming) >= 0) { result = 2; }
            //判断劳动节日
            else if (date.Month == 5 && date.Day == 1) { result = 3; }
            //判断中秋节日
            else if (traditionalYear.IndexOf(zhongqiu) >= 0) { result = 5; }
            //判断国庆节日
            else if (IsNationalDay(date, 3)) { result = 6; }
            //判断暑假时间段(大致范围)
            else if (date.Month == 7 || date.Month == 8) { result = 4; }
            //判断寒假时间段
            else if ((date.Month == 1 && date.Day >= 20) || date.Month == 2) { result = 8; }
            //判断周末日期
            else if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) { result = 9; }
            else { result = 0; }
            return result;
        }//end method

        /// <summary>
        /// 获取当前月包含的总天数
        /// </summary>
        /// <returns>当前月包含的总天数</returns>
        public int DaysInMonth()
        {
            return DaysInMonth(DateTime.Now);
        }//end method

        /// <summary>
        /// 获取当前月份的最后一天日期
        /// </summary>
        /// <returns>当前月的最后一天日期</returns>
        public static DateTime LastDayInMonth()
        {
            return LastDayInMonth(DateTime.Now);
        }//end method

        /// <summary>
        /// 获取指定日期中月份的最后一天日期
        /// </summary>
        /// <param name="date">指定日期值</param>
        /// <returns>指定日期中月份的最后一天日期</returns>
        public static DateTime LastDayInMonth(DateTime date)
        {
            //获取指定日期月份中总天数
            int days = DaysInMonth(date);
            //获取指定日期月份中第几天
            int day = date.Day;
            return date.AddDays(days - day);
        }//end method

        /// <summary>
        /// 获取当前日期中月份的第一天日期
        /// </summary>
        /// <returns>当前日期中月份的第一天日期</returns>
        public static DateTime FirstDayInMonth()
        {
            return FirstDayInMonth(DateTime.Now);
        }//end method

        /// <summary>
        /// 获取指定日期中月份的第一天日期
        /// </summary>
        /// <param name="date">指定日期值</param>
        /// <returns>指定日期中月份的第一天日期</returns>
        public static DateTime FirstDayInMonth(DateTime date)
        {
            //获取指定日期月份中第几天
            int day = date.Day;
            return day > 1 ? date.AddDays(1 - day) : date;
        }//end method

        /// <summary>
        /// 获取当前月中第几天
        /// </summary>
        /// <returns>当前月中第几天</returns>
        public static int DayInMonth()
        {
            return DateTime.Now.Day;
        }//end method

        /// <summary>
        /// 获取当前日期中年份的第几天
        /// </summary>
        /// <returns>当前日期中年份的第几天</returns>
        public static int DayInYear()
        {
            return DateTime.Now.DayOfYear;
        }//end method

        /// <summary>
        /// 获取指定日期中年份的第几天
        /// </summary>
        /// <param name="date">指定日期</param>
        /// <returns>指定日期中年份的第几天</returns>
        public static int DayInYear(DateTime date)
        {
            return date.DayOfYear;
        }//end method

        /// <summary>
        /// 获取指定时间月份中包含的总天数
        /// </summary>
        /// <param name="date">指定时间值</param>
        /// <returns>指定时间月份中包含的总天数</returns>
        public static int DaysInMonth(DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month);
        }//end method

        /// <summary>
        /// 获得当前时间别称(例如:上午，下午，晚上等别称)
        /// </summary>
        /// <returns>时间别称</returns>
        public static string GetTimeIntervalAlias()
        {
            return GetTimeIntervalAlias(DateTime.Now);
        }//end method

        /// <summary>
        /// 获得指定时间别称(例如:上午，下午，晚上等别称)
        /// </summary>
        /// <param name="time">时间实体</param>
        /// <returns>时间别称</returns>
        public static string GetTimeIntervalAlias(DateTime time)
        {
            //System.Globalization.DateTimeStyles
            DateTime dt0 = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
            DateTime dt3 = new DateTime(time.Year, time.Month, time.Day, 3, 59, 59);
            DateTime dt6 = new DateTime(time.Year, time.Month, time.Day, 6, 59, 59);
            DateTime dt8 = new DateTime(time.Year, time.Month, time.Day, 8, 59, 59);
            DateTime dt11 = new DateTime(time.Year, time.Month, time.Day, 11, 59, 59);
            DateTime dt14 = new DateTime(time.Year, time.Month, time.Day, 14, 59, 59);
            DateTime dt17 = new DateTime(time.Year, time.Month, time.Day, 17, 59, 59);
            DateTime dt19 = new DateTime(time.Year, time.Month, time.Day, 19, 59, 59);
            DateTime dt22 = new DateTime(time.Year, time.Month, time.Day, 22, 59, 59);
            DateTime dt23 = new DateTime(time.Year, time.Month, time.Day, 23, 59, 59);
            if (time >= dt0 && time <= dt3)
            {
                return HelperEnum.GetEnumDesc(TimeIntervalAlias.BeforeDawn);
            }
            if (time > dt3 && time <= dt6)
            {
                return HelperEnum.GetEnumDesc(TimeIntervalAlias.Daybreak);
            }
            if (time > dt6 && time <= dt8)
            {
                return HelperEnum.GetEnumDesc(TimeIntervalAlias.Morning);
            }
            if (time > dt8 && time <= dt11)
            {
                return HelperEnum.GetEnumDesc(TimeIntervalAlias.Forenoon);
            }
            if (time > dt11 && time <= dt14)
            {
                return HelperEnum.GetEnumDesc(TimeIntervalAlias.Midnoon);
            }
            if (time > dt14 && time <= dt17)
            {
                return HelperEnum.GetEnumDesc(TimeIntervalAlias.Afternoon);
            }
            if (time > dt17 && time <= dt19)
            {
                return HelperEnum.GetEnumDesc(TimeIntervalAlias.TowardEvening);
            }
            if (time > dt19 && time <= dt22)
            {
                return HelperEnum.GetEnumDesc(TimeIntervalAlias.Evening);
            }
            else
            {
                return HelperEnum.GetEnumDesc(TimeIntervalAlias.DeepNight);
            }
        }//end method

        /// <summary>
        /// 是否时间字符
        /// </summary>
        /// <param name="timeval">待检测字符串</param>
        /// <returns>布尔值,true是时间字符串格式,false不是时间字符串格式</returns>
        public static bool IsTimeFormat(string timeval)
        {
            if (timeval == null || timeval.Trim().Length == 0)
            {
                return false;
            }
            return Regex.IsMatch(timeval, RegexFormat.NormalTimeRegexFormat);
        }//end method

        /// <summary>
        /// 获取当前日期对应的星期值
        /// </summary>
        /// <returns>星期值</returns>
        public static string GetWeek()
        {
            return GetWeek(DateTime.Now);
        }//end method

        /// <summary>
        /// 获取指定时间对应的星期值
        /// </summary>
        /// <param name="dateTime">时间对象</param>
        /// <returns>星期值</returns>
        public static string GetWeek(DateTime dateTime)
        {
            return HelperEnum.GetEnumDescByInt(typeof(DayOfWeek), (int)dateTime.DayOfWeek);
        }//end method

        /// <summary>
        /// 获取今天农历年字符串
        /// </summary>
        /// <returns>农历年字符串</returns>
        public static string GetTraditionalDate()
        {
            return GetTraditionalDate(DateTime.Now);
        }//end metho

        /// <summary>
        /// 获取指定日期的农历格式字符串
        /// </summary>
        /// <param name="dateTime">指定(公历)日期对象字符串</param>
        /// <returns></returns>
        /// <remarks>获取失败，返回空字符串</remarks>
        public static string GetTraditionalDate(string dateTime)
        {
            DateTime tmpTime = DateTime.Now;
            if (DateTime.TryParse(dateTime, out tmpTime))
            {
                return GetTraditionalDate(tmpTime);
            }
            else
            {
                return string.Empty;
            }
        }//end method

        /// <summary>
        /// 获取指定日期的农历格式字符串
        /// </summary>
        /// <param name="dateTime">指定(公历)日期对象</param>
        /// <returns>农历格式字符串</returns>
        public static string GetTraditionalDate(DateTime dateTime)
        {
            return new TraditionalDate().GetTraditionalDate(dateTime, false);
        }//end method

        ///<summary>
        /// 显示今天农历类
        ///</summary>
        private class TraditionalDate
        {
            private DateTime m_Date; //今天的日期
            private int cny; //农历的年对应甲子循环中的年
            private int cnm; //农历的月-1
            private int cnd; //农历日
            private int icnm; //农历闰月

            /// <summary>
            /// 返回格式化的农历显示
            /// </summary>
            /// <param name="dataTime">指定时间</param>
            /// <param name="optional">布尔值,true添加附加信息,false不添加附加信息(附加信息为"戊子(鼠)年")</param>
            /// <returns>格式如:[戊子(鼠)年]润四月廿三</returns>
            public string GetTraditionalDate(DateTime dataTime, bool optional)
            {
                m_Date = dataTime;    //DateTime.Today; //2010-2-22
                var cnCalendar = new ChineseLunisolarCalendar();
                cny = cnCalendar.GetSexagenaryYear(m_Date); //27 计算与指定日期对应的甲子（60 年）循环中的年。
                cnm = cnCalendar.GetMonth(m_Date); //1 返回指定日期中的月份-1
                cnd = cnCalendar.GetDayOfMonth(m_Date); //9 计算指定日期中的月中日期。
                icnm = cnCalendar.GetLeapMonth(cnCalendar.GetYear(m_Date)); //0 计算指定纪元年份的闰月。
                string txcns = "";
                const string szText1 = "癸甲乙丙丁戊己庚辛壬";
                const string szText2 = "亥子丑寅卯辰巳午未申酉戌";
                const string szText3 = "猪鼠牛虎免龙蛇马羊猴鸡狗";
                int tn = cny % 10; //天干
                int dn = cny % 12; //地支
                if (optional)
                {
                    txcns += szText1.Substring(tn, 1);
                    txcns += szText2.Substring(dn, 1);
                }
                txcns += "(" + szText3.Substring(dn, 1) + ")年"; //格式化月份显示
                string[] cnMonth = { "", "正月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月", "十二月" };
                string sMonth = "";

                //润月的处理
                if (icnm > 0 && cnm <= icnm)
                {
                    cnMonth[icnm] = "闰" + cnMonth[icnm - 1];
                    sMonth = cnMonth[cnCalendar.GetMonth(m_Date)];
                }
                else if (icnm > 0 && cnm > icnm)
                {
                    sMonth = cnMonth[cnCalendar.GetMonth(m_Date) - 1];
                }
                else
                {
                    sMonth = cnMonth[cnCalendar.GetMonth(m_Date)];
                }
                txcns += sMonth;
                string[] cnDay ={ "", "初一", "初二", "初三", "初四", "初五", "初六", "初七"
                , "初八", "初九", "初十", "十一", "十二", "十三", "十四", "十五", "十六"
                , "十七", "十八", "十九", "二十", "廿一", "廿二", "廿三", "廿四", "廿五"
                , "廿六", "廿七", "廿八", "廿九", "三十" };
                txcns += cnDay[cnd];
                return txcns;
            }//end method

        }//end class

        public enum TimeIntervalAlias
        {
            /// <summary>
            /// 凌晨：(0-3点)
            /// </summary>
            [Description("凌晨")]
            BeforeDawn = 0,

            /// <summary>
            /// 黎明：(4-6点)
            /// </summary>
            [Description("黎明")]
            Daybreak = 4,

            /// <summary>
            /// 早晨：(7-8点)
            /// </summary>
            [Description("早晨")]
            Morning = 7,

            /// <summary>
            /// 上午：(9-11点)
            /// </summary>
            [Description("上午")]
            Forenoon = 9,

            /// <summary>
            /// 中午：(12-14点)
            /// </summary>
            [Description("中午")]
            Midnoon = 12,

            /// <summary>
            /// 下午：(15-17点)
            /// </summary>
            [Description("下午")]
            Afternoon = 15,

            /// <summary>
            /// 傍晚：(18-19点)
            /// </summary>
            [Description("傍晚")]
            TowardEvening = 18,

            /// <summary>
            /// 晚上：20-22点
            /// </summary>
            [Description("晚上")]
            Evening = 20,

            /// <summary>
            /// 深夜：23-0点
            /// </summary>
            [Description("深夜")]
            DeepNight = 23,
        }

        public enum TimeValueKind
        {
            /// <summary>
            /// 年份
            /// </summary>
            [Description("年")]
            Year = 0,

            /// <summary>
            /// 月份
            /// </summary>
            [Description("月")]
            Month = 1,

            /// <summary>
            /// 周
            /// </summary>
            [Description("周")]
            Week = 2,

            /// <summary>
            /// 天
            /// </summary>
            [Description("天")]
            Day = 3,

            /// <summary>
            /// 小时
            /// </summary>
            [Description("小时")]
            Hour = 4,

            /// <summary>
            /// 分钟
            /// </summary>
            [Description("分钟")]
            Minute = 5,

            /// <summary>
            /// 秒
            /// </summary>
            [Description("秒")]
            Second = 6,

            /// <summary>
            /// 毫秒
            /// </summary>
            [Description("毫秒")]
            Millisecond = 7
        }

    }//end class

}//end namespace
