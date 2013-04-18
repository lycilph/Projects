using System;
using System.Globalization;

namespace LunchViewer.Utils
{
    public static class DateUtils
    {
        public static readonly CultureInfo dk_culture = CultureInfo.CreateSpecificCulture("da-DK");

        public static DateTime FirstDateOfWeek(int year, int week_of_year)
        {
            // Find 1. of jan and the first thursday (the 1st week of the year is defined as the one containing the first thursday)
            var jan1 = new DateTime(year, 1, 1);
            var days_offset = DayOfWeek.Thursday - jan1.DayOfWeek;

            var first_thursday = jan1.AddDays(days_offset);
            var first_week = dk_culture.Calendar.GetWeekOfYear(first_thursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var week_num = week_of_year;
            if (first_week <= 1)
                week_num -= 1;

            var result = first_thursday.AddDays(week_num * 7);
            return result.AddDays(-3); // Subtract 3 to go from thursday to monday
        }

        public static DateTime DateFromDayOfWeek(DateTime start_of_week, string day)
        {
            for (var i = 0; i < 7; i++)
            {
                var date = start_of_week.AddDays(i);
                var date_day_name = GetDay(dk_culture, date);

                if (day.ToLower().Contains(date_day_name.ToLower()))
                    return date;
            }
            return start_of_week;
        }

        public static string GetDay(DateTime date)
        {
            return GetDay(dk_culture, date);
        }

        public static string GetDay(string culture, DateTime date)
        {
            var culture_info = CultureInfo.CreateSpecificCulture(culture);
            return GetDay(culture_info, date);
        }

        public static string GetDay(CultureInfo culture_info, DateTime date)
        {
            return culture_info.TextInfo.ToTitleCase(date.ToString("dddd", culture_info));
        }

        public static string GetCurrentDay()
        {
            return GetDay(DateTime.Today);
        }

        public static string GetCurrentDay(string culture)
        {
            return GetDay(culture, DateTime.Today);
        }

        public static string GetCurrentDay(CultureInfo culture_info)
        {
            return GetDay(culture_info, DateTime.Today);
        }

        public static string GetCurrentDateFormatted()
        {
            return GetCurrentDateFormatted(dk_culture);
        }

        public static string GetCurrentDateFormatted(string culture)
        {
            var culture_info = CultureInfo.CreateSpecificCulture(culture);
            return GetCurrentDateFormatted(culture_info);
        }

        public static string GetCurrentDateFormatted(CultureInfo culture_info)
        {
            return DateTime.Today.ToString("dddd, dd. MMMM yyyy", culture_info);
        }

        public static int GetCurrentWeekNumber()
        {
            return GetCurrentWeekNumber(dk_culture);
        }

        public static int GetCurrentWeekNumber(CultureInfo culture_info)
        {
            return culture_info.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
    }
}
