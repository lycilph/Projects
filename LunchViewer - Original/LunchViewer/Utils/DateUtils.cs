using System;
using System.Globalization;

namespace LunchViewer.Utils
{
    public static class DateUtils
    {
        public static int GetCurrentWeekNumber()
        {
            var culture_info = WPFLocalizeExtension.Engine.LocalizeDictionary.CurrentCulture;
            return culture_info.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        public static string GetCurrentDay()
        {
            var culture_info = WPFLocalizeExtension.Engine.LocalizeDictionary.CurrentCulture;
            return culture_info.TextInfo.ToTitleCase(DateTime.Now.ToString("dddd", culture_info));
        }

        public static string GetCurrentDay(CultureInfo ci)
        {
            return ci.TextInfo.ToTitleCase(DateTime.Now.ToString("dddd", ci));
        }

        public static string GetCurrentDateFormatted()
        {
            var culture_info = WPFLocalizeExtension.Engine.LocalizeDictionary.CurrentCulture;
            return DateTime.Now.ToString("dd. MMMM yyyy", culture_info);
        }
    }
}
