using System;
using System.Globalization;

namespace LunchViewer
{
    public static class DateUtils
    {
        private static readonly CultureInfo culture_info = CultureInfo.CreateSpecificCulture("en-GB");

        public static int GetCurrentWeekNumber()
        {
            return culture_info.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        public static string GetCurrentDay()
        {
            return culture_info.TextInfo.ToTitleCase(DateTime.Now.ToString("dddd", culture_info));
        }

        public static string GetCurrentDay(CultureInfo ci)
        {
            return ci.TextInfo.ToTitleCase(DateTime.Now.ToString("dddd", ci));
        }

        public static string GetCurrentDateFormatted()
        {
            return DateTime.Now.ToString("dd. MMMM yyyy", culture_info);
        }
    }
}
