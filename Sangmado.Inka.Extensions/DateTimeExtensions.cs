using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace Sangmado.Inka.Extensions
{
    public static class DateTimeExtensions
    {
        public static readonly DateTime TimeEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Add(TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now));
        public static readonly DateTime TimeUtcEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public const string TimeShortFormat = @"yyyy-MM-dd HH:mm:ss";
        public const string TimeLongFormat = @"yyyy-MM-dd HH:mm:ss.fffffff";
        public const string TimeUIDisplayFormat = @"MM/dd/yyyy hh:mm:ss tt";

        /// <summary>
        /// The "O" or "o" standard format specifier represents a custom date and time format string 
        /// using a pattern that preserves time zone information and emits a result string that complies with ISO 8601. 
        /// The "O" or "o" standard format specifier corresponds to 
        /// the "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK" custom format string for DateTime values 
        /// and to the "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffzzz" custom format string for DateTimeOffset values.
        /// </summary>
        public const string TimeIso8601Format = @"o";

        public static string ToFormatString(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToFormatString() : string.Empty;
        }

        public static string ToFormatShortString(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToFormatShortString() : string.Empty;
        }

        public static string ToFormatString(this DateTime dateTime)
        {
            return dateTime.ToString(TimeLongFormat, CultureInfo.InvariantCulture);
        }

        public static string ToFormatShortString(this DateTime dateTime)
        {
            return dateTime.ToString(TimeShortFormat, CultureInfo.InvariantCulture);
        }

        public static string ToIso8601FormatString(this DateTimeOffset? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToIso8601FormatString() : string.Empty;
        }

        public static string ToIso8601FormatString(this DateTimeOffset dateTime)
        {
            return dateTime.ToString(TimeIso8601Format, CultureInfo.InvariantCulture);
        }

        public static long TotalSecondsSinceEpoch(this DateTime dateTime)
        {
            if (dateTime < TimeUtcEpoch)
                return 0;
            return (long)(dateTime - TimeUtcEpoch).TotalSeconds;
        }

        public static long TotalMillisecondsSinceEpoch(this DateTime dateTime)
        {
            if (dateTime < TimeUtcEpoch)
                return 0;
            return (long)(dateTime - TimeUtcEpoch).TotalMilliseconds;
        }

        public static long TotalTicksSinceEpoch(this DateTime dateTime)
        {
            if (dateTime < TimeUtcEpoch)
                return 0;
            return (dateTime - TimeUtcEpoch).Ticks;
        }

        public static long TotalSecondsSinceEpoch(this DateTimeOffset dateTime)
        {
            if (dateTime < TimeUtcEpoch)
                return 0;
            return (long)(dateTime - TimeUtcEpoch).TotalSeconds;
        }

        public static long TotalMillisecondsSinceEpoch(this DateTimeOffset dateTime)
        {
            if (dateTime < TimeUtcEpoch)
                return 0;
            return (long)(dateTime - TimeUtcEpoch).TotalMilliseconds;
        }

        public static long TotalTicksSinceEpoch(this DateTimeOffset dateTime)
        {
            if (dateTime < TimeUtcEpoch)
                return 0;
            return (dateTime - TimeUtcEpoch).Ticks;
        }

        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero)
                return dateTime;
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }

        public static DateTimeOffset Truncate(this DateTimeOffset dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero)
                return dateTime;
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }

        public static DateTime TruncateToWholeMilliseconds(this DateTime dateTime)
        {
            return dateTime.Truncate(TimeSpan.FromMilliseconds(1));
        }

        public static DateTimeOffset TruncateToWholeMilliseconds(this DateTimeOffset dateTime)
        {
            return dateTime.Truncate(TimeSpan.FromMilliseconds(1));
        }

        public static DateTime TruncateToWholeSeconds(this DateTime dateTime)
        {
            return dateTime.Truncate(TimeSpan.FromSeconds(1));
        }

        public static DateTimeOffset TruncateToWholeSeconds(this DateTimeOffset dateTime)
        {
            return dateTime.Truncate(TimeSpan.FromSeconds(1));
        }

        public static bool Within(this DayOfWeek dayOfWeek, List<DayOfWeek> daysOfWeek)
        {
            return daysOfWeek.Contains(dayOfWeek);
        }

        public static bool IsFirstDayOfWeek(this DayOfWeek dayOfWeek, DayOfWeek firstDayOfWeek)
        {
            return dayOfWeek == firstDayOfWeek;
        }

        public static bool IsLastDayOfWeek(this DayOfWeek dayOfWeek, DayOfWeek firstDayOfWeek)
        {
            return dayOfWeek == LastDayOfWeek(firstDayOfWeek);
        }

        public static DayOfWeek LastDayOfWeek(DayOfWeek firstDayOfWeek)
        {
            return (DayOfWeek)(((int)firstDayOfWeek + 6) % 7);
        }

        #region DateTimeOffset Extensions

        public static DateTimeOffset ConvertTime(this DateTimeOffset dateTime, TimeZoneInfo timeZone)
        {
            return TimeZoneInfo.ConvertTime(dateTime, timeZone);
        }

        public static bool IsWeekendDay(this DateTimeOffset dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
        }

        public static bool IsWeekday(this DateTimeOffset dateTime)
        {
            return !IsWeekendDay(dateTime);
        }

        public static bool IsFirstDayOfWeek(this DateTimeOffset dateTime, DayOfWeek firstDayOfWeek)
        {
            return dateTime.DayOfWeek == firstDayOfWeek;
        }

        public static bool IsLastDayOfWeek(this DateTimeOffset dateTime, DayOfWeek firstDayOfWeek)
        {
            return dateTime.DayOfWeek == LastDayOfWeek(firstDayOfWeek);
        }

        public static DateTimeOffset GetFirstDayOfWeek(this DateTimeOffset dateTime, DayOfWeek firstDayOfWeek)
        {
            int delta = firstDayOfWeek - dateTime.DayOfWeek;
            if (delta > 0) delta -= 7;
            return dateTime.AddDays(delta);
        }

        public static DateTimeOffset GetLastDayOfWeek(this DateTimeOffset dateTime, DayOfWeek firstDayOfWeek)
        {
            return dateTime.GetFirstDayOfWeek(firstDayOfWeek).AddDays(7);
        }

        public static DateTimeOffset GetFirstDayOfMonth(this DateTimeOffset dateTime)
        {
            return dateTime.AddDays((-1 * dateTime.Day) + 1);
        }

        public static DateTimeOffset GetSecondDayOfMonth(this DateTimeOffset dateTime)
        {
            return dateTime.GetFirstDayOfMonth().AddDays(1);
        }

        public static DateTimeOffset GetThirdDayOfMonth(this DateTimeOffset dateTime)
        {
            return dateTime.GetFirstDayOfMonth().AddDays(2);
        }

        public static DateTimeOffset GetFourthDayOfMonth(this DateTimeOffset dateTime)
        {
            return dateTime.GetFirstDayOfMonth().AddDays(3);
        }

        public static DateTimeOffset GetLastDayOfMonth(this DateTimeOffset dateTime)
        {
            return dateTime.GetFirstDayOfNextMonth().AddDays(-1);
        }

        public static DateTimeOffset GetFirstDayOfNextMonth(this DateTimeOffset dateTime)
        {
            return dateTime.GetFirstDayOfNextSeveralMonths(1);
        }

        public static DateTimeOffset GetFirstDayOfNextSeveralMonths(this DateTimeOffset dateTime, int nextSeveralMonths)
        {
            return dateTime.AddMonths(nextSeveralMonths).GetFirstDayOfMonth();
        }

        public static DateTimeOffset GetNextWeekday(this DateTimeOffset dateTime)
        {
            var nextDay = dateTime.AddDays(1);
            while (!nextDay.IsWeekday())
            {
                nextDay = nextDay.AddDays(1);
            }

            return nextDay;
        }

        public static DateTimeOffset GetNextWeekendDay(this DateTimeOffset dateTime)
        {
            var nextDay = dateTime.AddDays(1);
            while (!nextDay.IsWeekendDay())
            {
                nextDay = nextDay.AddDays(1);
            }

            return nextDay;
        }

        public static DateTimeOffset GetFirstWeekdayOfMonth(this DateTimeOffset dateTime)
        {
            var nextDay = GetFirstDayOfMonth(dateTime);
            while (!nextDay.IsWeekday())
            {
                nextDay = nextDay.AddDays(1);
            }

            return nextDay;
        }

        public static DateTimeOffset GetSecondWeekdayOfMonth(this DateTimeOffset dateTime)
        {
            return dateTime.GetFirstWeekdayOfMonth().GetNextWeekday();
        }

        public static DateTimeOffset GetThirdWeekdayOfMonth(this DateTimeOffset dateTime)
        {
            return dateTime.GetSecondWeekdayOfMonth().GetNextWeekday();
        }

        public static DateTimeOffset GetFourthWeekdayOfMonth(this DateTimeOffset dateTime)
        {
            return dateTime.GetThirdWeekdayOfMonth().GetNextWeekday();
        }

        public static DateTimeOffset GetLastWeekdayOfMonth(this DateTimeOffset dateTime)
        {
            var previousDay = GetLastDayOfMonth(dateTime);
            while (!previousDay.IsWeekday())
            {
                previousDay = previousDay.AddDays(-1);
            }

            return previousDay;
        }

        public static DateTimeOffset GetFirstWeekendDayOfMonth(this DateTimeOffset dateTime)
        {
            var nextDay = GetFirstDayOfMonth(dateTime);
            while (!nextDay.IsWeekendDay())
            {
                nextDay = nextDay.AddDays(1);
            }

            return nextDay;
        }

        public static DateTimeOffset GetSecondWeekendDayOfMonth(this DateTimeOffset dateTime)
        {
            return dateTime.GetFirstWeekendDayOfMonth().GetNextWeekendDay();
        }

        public static DateTimeOffset GetThirdWeekendDayOfMonth(this DateTimeOffset dateTime)
        {
            return dateTime.GetSecondWeekendDayOfMonth().GetNextWeekendDay();
        }

        public static DateTimeOffset GetFourthWeekendDayOfMonth(this DateTimeOffset dateTime)
        {
            return dateTime.GetThirdWeekendDayOfMonth().GetNextWeekendDay();
        }

        public static DateTimeOffset GetLastWeekendDayOfMonth(this DateTimeOffset dateTime)
        {
            var previousDay = GetLastDayOfMonth(dateTime);
            while (!previousDay.IsWeekendDay())
            {
                previousDay = previousDay.AddDays(-1);
            }

            return previousDay;
        }

        public static DateTimeOffset GetFirstDayOfWeekOfMonth(this DateTimeOffset dateTime, DayOfWeek dayOfWeek)
        {
            var nextDay = GetFirstDayOfMonth(dateTime);
            while (nextDay.DayOfWeek != dayOfWeek)
            {
                nextDay = nextDay.AddDays(1);
            }

            return nextDay;
        }

        public static DateTimeOffset GetSecondDayOfWeekOfMonth(this DateTimeOffset dateTime, DayOfWeek dayOfWeek)
        {
            var nextDay = GetFirstDayOfWeekOfMonth(dateTime, dayOfWeek);
            nextDay = nextDay.AddDays(7);

            return nextDay;
        }

        public static DateTimeOffset GetThirdDayOfWeekOfMonth(this DateTimeOffset dateTime, DayOfWeek dayOfWeek)
        {
            var nextDay = GetFirstDayOfWeekOfMonth(dateTime, dayOfWeek);
            nextDay = nextDay.AddDays(7 * 2);

            return nextDay;
        }

        public static DateTimeOffset GetFourthDayOfWeekOfMonth(this DateTimeOffset dateTime, DayOfWeek dayOfWeek)
        {
            var nextDay = GetFirstDayOfWeekOfMonth(dateTime, dayOfWeek);
            nextDay = nextDay.AddDays(7 * 3);

            return nextDay;
        }

        public static DateTimeOffset GetLastDayOfWeekOfMonth(this DateTimeOffset dateTime, DayOfWeek dayOfWeek)
        {
            var previousDay = GetLastDayOfMonth(dateTime);
            while (previousDay.DayOfWeek != dayOfWeek)
            {
                previousDay = previousDay.AddDays(-1);
            }

            return previousDay;
        }

        public static bool IsFirstKindDayOfMonth(this DateTimeOffset dateTime, DayOfKind dayOfKind)
        {
            switch (dayOfKind)
            {
                case DayOfKind.Sunday:
                case DayOfKind.Monday:
                case DayOfKind.Tuesday:
                case DayOfKind.Wednesday:
                case DayOfKind.Thursday:
                case DayOfKind.Friday:
                case DayOfKind.Saturday:
                    return dateTime.Day == dateTime.GetFirstDayOfWeekOfMonth((DayOfWeek)((int)dayOfKind)).Day;
                case DayOfKind.Day:
                    return dateTime.Day == dateTime.GetFirstDayOfMonth().Day;
                case DayOfKind.Weekday:
                    return dateTime.Day == dateTime.GetFirstWeekdayOfMonth().Day;
                case DayOfKind.WeekendDay:
                    return dateTime.Day == dateTime.GetFirstWeekendDayOfMonth().Day;
                default:
                    break;
            }

            return false;
        }

        public static bool IsSecondKindDayOfMonth(this DateTimeOffset dateTime, DayOfKind dayOfKind)
        {
            switch (dayOfKind)
            {
                case DayOfKind.Sunday:
                case DayOfKind.Monday:
                case DayOfKind.Tuesday:
                case DayOfKind.Wednesday:
                case DayOfKind.Thursday:
                case DayOfKind.Friday:
                case DayOfKind.Saturday:
                    return dateTime.Day == dateTime.GetSecondDayOfWeekOfMonth((DayOfWeek)((int)dayOfKind)).Day;
                case DayOfKind.Day:
                    return dateTime.Day == dateTime.GetSecondDayOfMonth().Day;
                case DayOfKind.Weekday:
                    return dateTime.Day == dateTime.GetSecondWeekdayOfMonth().Day;
                case DayOfKind.WeekendDay:
                    return dateTime.Day == dateTime.GetSecondWeekendDayOfMonth().Day;
                default:
                    break;
            }

            return false;
        }

        public static bool IsThirdKindDayOfMonth(this DateTimeOffset dateTime, DayOfKind dayOfKind)
        {
            switch (dayOfKind)
            {
                case DayOfKind.Sunday:
                case DayOfKind.Monday:
                case DayOfKind.Tuesday:
                case DayOfKind.Wednesday:
                case DayOfKind.Thursday:
                case DayOfKind.Friday:
                case DayOfKind.Saturday:
                    return dateTime.Day == dateTime.GetThirdDayOfWeekOfMonth((DayOfWeek)((int)dayOfKind)).Day;
                case DayOfKind.Day:
                    return dateTime.Day == dateTime.GetThirdDayOfMonth().Day;
                case DayOfKind.Weekday:
                    return dateTime.Day == dateTime.GetThirdWeekdayOfMonth().Day;
                case DayOfKind.WeekendDay:
                    return dateTime.Day == dateTime.GetThirdWeekendDayOfMonth().Day;
                default:
                    break;
            }

            return false;
        }

        public static bool IsFourthKindDayOfMonth(this DateTimeOffset dateTime, DayOfKind dayOfKind)
        {
            switch (dayOfKind)
            {
                case DayOfKind.Sunday:
                case DayOfKind.Monday:
                case DayOfKind.Tuesday:
                case DayOfKind.Wednesday:
                case DayOfKind.Thursday:
                case DayOfKind.Friday:
                case DayOfKind.Saturday:
                    return dateTime.Day == dateTime.GetFourthDayOfWeekOfMonth((DayOfWeek)((int)dayOfKind)).Day;
                case DayOfKind.Day:
                    return dateTime.Day == dateTime.GetFourthDayOfMonth().Day;
                case DayOfKind.Weekday:
                    return dateTime.Day == dateTime.GetFourthWeekdayOfMonth().Day;
                case DayOfKind.WeekendDay:
                    return dateTime.Day == dateTime.GetFourthWeekendDayOfMonth().Day;
                default:
                    break;
            }

            return false;
        }

        public static bool IsLastKindDayOfMonth(this DateTimeOffset dateTime, DayOfKind dayOfKind)
        {
            switch (dayOfKind)
            {
                case DayOfKind.Sunday:
                case DayOfKind.Monday:
                case DayOfKind.Tuesday:
                case DayOfKind.Wednesday:
                case DayOfKind.Thursday:
                case DayOfKind.Friday:
                case DayOfKind.Saturday:
                    return dateTime.Day == dateTime.GetLastDayOfWeekOfMonth((DayOfWeek)((int)dayOfKind)).Day;
                case DayOfKind.Day:
                    return dateTime.Day == dateTime.GetLastDayOfMonth().Day;
                case DayOfKind.Weekday:
                    return dateTime.Day == dateTime.GetLastWeekdayOfMonth().Day;
                case DayOfKind.WeekendDay:
                    return dateTime.Day == dateTime.GetLastWeekendDayOfMonth().Day;
                default:
                    break;
            }

            return false;
        }

        public static bool IsKindDayOfMonth(this DateTimeOffset dateTime, DayOfMonth dayOfMonth, DayOfKind dayOfKind)
        {
            switch (dayOfMonth)
            {
                case DayOfMonth.First:
                    return IsFirstKindDayOfMonth(dateTime, dayOfKind);
                case DayOfMonth.Second:
                    return IsSecondKindDayOfMonth(dateTime, dayOfKind);
                case DayOfMonth.Third:
                    return IsThirdKindDayOfMonth(dateTime, dayOfKind);
                case DayOfMonth.Fourth:
                    return IsFourthKindDayOfMonth(dateTime, dayOfKind);
                case DayOfMonth.Fifth:
                case DayOfMonth.Sixth:
                case DayOfMonth.Seventh:
                case DayOfMonth.Eighth:
                case DayOfMonth.Ninth:
                case DayOfMonth.Tenth:
                case DayOfMonth.Eleventh:
                case DayOfMonth.Twelfth:
                case DayOfMonth.Thirteenth:
                case DayOfMonth.Fourteenth:
                case DayOfMonth.Fifteenth:
                case DayOfMonth.Sixteenth:
                case DayOfMonth.Seventeenth:
                case DayOfMonth.Eighteenth:
                case DayOfMonth.Nineteenth:
                case DayOfMonth.Twentieth:
                case DayOfMonth.TwentyFirst:
                case DayOfMonth.TwentySecond:
                case DayOfMonth.TwentyThird:
                case DayOfMonth.TwentyFourth:
                case DayOfMonth.TwentyFifth:
                case DayOfMonth.TwentySixth:
                case DayOfMonth.TwentySeventh:
                case DayOfMonth.TwentyEighth:
                case DayOfMonth.TwentyNineth:
                case DayOfMonth.Thirtieth:
                case DayOfMonth.ThirtyFirst:
                    {
                        if (dayOfKind != DayOfKind.Day)
                            throw new InvalidProgramException("Does not support given kind of day.");

                        int day = (int)dayOfMonth;
                        return dateTime.Day == day;
                    }
                case DayOfMonth.Last:
                    return IsLastKindDayOfMonth(dateTime, dayOfKind);
                default:
                    break;
            }

            throw new InvalidProgramException("Cannot parse the given month and day.");
        }

        public static bool IsMonthOfYear(this DateTimeOffset dateTime, MonthOfYear monthOfYear)
        {
            switch (monthOfYear)
            {
                case MonthOfYear.January:
                case MonthOfYear.February:
                case MonthOfYear.March:
                case MonthOfYear.April:
                case MonthOfYear.May:
                case MonthOfYear.June:
                case MonthOfYear.July:
                case MonthOfYear.August:
                case MonthOfYear.September:
                case MonthOfYear.October:
                case MonthOfYear.November:
                case MonthOfYear.December:
                    return dateTime.Month == (int)monthOfYear;
                default:
                    break;
            }

            throw new InvalidProgramException("Does not support given month of year.");
        }

        #endregion

        #region DateTime Extensions

        public static DateTime ConvertTime(this DateTime dateTime, TimeZoneInfo timeZone)
        {
            return TimeZoneInfo.ConvertTime(dateTime, timeZone);
        }

        public static bool IsWeekendDay(this DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
        }

        public static bool IsWeekday(this DateTime dateTime)
        {
            return !IsWeekendDay(dateTime);
        }

        public static bool IsFirstDayOfWeek(this DateTime dateTime, DayOfWeek firstDayOfWeek)
        {
            return dateTime.DayOfWeek == firstDayOfWeek;
        }

        public static bool IsLastDayOfWeek(this DateTime dateTime, DayOfWeek firstDayOfWeek)
        {
            return dateTime.DayOfWeek == LastDayOfWeek(firstDayOfWeek);
        }

        public static DateTime GetFirstDayOfWeek(this DateTime dateTime, DayOfWeek firstDayOfWeek)
        {
            int delta = firstDayOfWeek - dateTime.DayOfWeek;
            if (delta > 0) delta -= 7;
            return dateTime.AddDays(delta);
        }

        public static DateTime GetLastDayOfWeek(this DateTime dateTime, DayOfWeek firstDayOfWeek)
        {
            return dateTime.GetFirstDayOfWeek(firstDayOfWeek).AddDays(7);
        }

        public static DateTime GetFirstDayOfMonth(this DateTime dateTime)
        {
            return dateTime.AddDays((-1 * dateTime.Day) + 1);
        }

        public static DateTime GetSecondDayOfMonth(this DateTime dateTime)
        {
            return dateTime.GetFirstDayOfMonth().AddDays(1);
        }

        public static DateTime GetThirdDayOfMonth(this DateTime dateTime)
        {
            return dateTime.GetFirstDayOfMonth().AddDays(2);
        }

        public static DateTime GetFourthDayOfMonth(this DateTime dateTime)
        {
            return dateTime.GetFirstDayOfMonth().AddDays(3);
        }

        public static DateTime GetLastDayOfMonth(this DateTime dateTime)
        {
            return dateTime.GetFirstDayOfNextMonth().AddDays(-1);
        }

        public static DateTime GetFirstDayOfNextMonth(this DateTime dateTime)
        {
            return dateTime.GetFirstDayOfNextSeveralMonths(1);
        }

        public static DateTime GetFirstDayOfNextSeveralMonths(this DateTime dateTime, int nextSeveralMonths)
        {
            return dateTime.AddMonths(nextSeveralMonths).GetFirstDayOfMonth();
        }

        public static DateTime GetNextWeekday(this DateTime dateTime)
        {
            var nextDay = dateTime.AddDays(1);
            while (!nextDay.IsWeekday())
            {
                nextDay = nextDay.AddDays(1);
            }

            return nextDay;
        }

        public static DateTime GetNextWeekendDay(this DateTime dateTime)
        {
            var nextDay = dateTime.AddDays(1);
            while (!nextDay.IsWeekendDay())
            {
                nextDay = nextDay.AddDays(1);
            }

            return nextDay;
        }

        public static DateTime GetFirstWeekdayOfMonth(this DateTime dateTime)
        {
            var nextDay = GetFirstDayOfMonth(dateTime);
            while (!nextDay.IsWeekday())
            {
                nextDay = nextDay.AddDays(1);
            }

            return nextDay;
        }

        public static DateTime GetSecondWeekdayOfMonth(this DateTime dateTime)
        {
            return dateTime.GetFirstWeekdayOfMonth().GetNextWeekday();
        }

        public static DateTime GetThirdWeekdayOfMonth(this DateTime dateTime)
        {
            return dateTime.GetSecondWeekdayOfMonth().GetNextWeekday();
        }

        public static DateTime GetFourthWeekdayOfMonth(this DateTime dateTime)
        {
            return dateTime.GetThirdWeekdayOfMonth().GetNextWeekday();
        }

        public static DateTime GetLastWeekdayOfMonth(this DateTime dateTime)
        {
            var previousDay = GetLastDayOfMonth(dateTime);
            while (!previousDay.IsWeekday())
            {
                previousDay = previousDay.AddDays(-1);
            }

            return previousDay;
        }

        public static DateTime GetFirstWeekendDayOfMonth(this DateTime dateTime)
        {
            var nextDay = GetFirstDayOfMonth(dateTime);
            while (!nextDay.IsWeekendDay())
            {
                nextDay = nextDay.AddDays(1);
            }

            return nextDay;
        }

        public static DateTime GetSecondWeekendDayOfMonth(this DateTime dateTime)
        {
            return dateTime.GetFirstWeekendDayOfMonth().GetNextWeekendDay();
        }

        public static DateTime GetThirdWeekendDayOfMonth(this DateTime dateTime)
        {
            return dateTime.GetSecondWeekendDayOfMonth().GetNextWeekendDay();
        }

        public static DateTime GetFourthWeekendDayOfMonth(this DateTime dateTime)
        {
            return dateTime.GetThirdWeekendDayOfMonth().GetNextWeekendDay();
        }

        public static DateTime GetLastWeekendDayOfMonth(this DateTime dateTime)
        {
            var previousDay = GetLastDayOfMonth(dateTime);
            while (!previousDay.IsWeekendDay())
            {
                previousDay = previousDay.AddDays(-1);
            }

            return previousDay;
        }

        public static DateTime GetFirstDayOfWeekOfMonth(this DateTime dateTime, DayOfWeek dayOfWeek)
        {
            var nextDay = GetFirstDayOfMonth(dateTime);
            while (nextDay.DayOfWeek != dayOfWeek)
            {
                nextDay = nextDay.AddDays(1);
            }

            return nextDay;
        }

        public static DateTime GetSecondDayOfWeekOfMonth(this DateTime dateTime, DayOfWeek dayOfWeek)
        {
            var nextDay = GetFirstDayOfWeekOfMonth(dateTime, dayOfWeek);
            nextDay = nextDay.AddDays(7);

            return nextDay;
        }

        public static DateTime GetThirdDayOfWeekOfMonth(this DateTime dateTime, DayOfWeek dayOfWeek)
        {
            var nextDay = GetFirstDayOfWeekOfMonth(dateTime, dayOfWeek);
            nextDay = nextDay.AddDays(7 * 2);

            return nextDay;
        }

        public static DateTime GetFourthDayOfWeekOfMonth(this DateTime dateTime, DayOfWeek dayOfWeek)
        {
            var nextDay = GetFirstDayOfWeekOfMonth(dateTime, dayOfWeek);
            nextDay = nextDay.AddDays(7 * 3);

            return nextDay;
        }

        public static DateTime GetLastDayOfWeekOfMonth(this DateTime dateTime, DayOfWeek dayOfWeek)
        {
            var previousDay = GetLastDayOfMonth(dateTime);
            while (previousDay.DayOfWeek != dayOfWeek)
            {
                previousDay = previousDay.AddDays(-1);
            }

            return previousDay;
        }

        public static bool IsFirstKindDayOfMonth(this DateTime dateTime, DayOfKind dayOfKind)
        {
            switch (dayOfKind)
            {
                case DayOfKind.Sunday:
                case DayOfKind.Monday:
                case DayOfKind.Tuesday:
                case DayOfKind.Wednesday:
                case DayOfKind.Thursday:
                case DayOfKind.Friday:
                case DayOfKind.Saturday:
                    return dateTime.Day == dateTime.GetFirstDayOfWeekOfMonth((DayOfWeek)((int)dayOfKind)).Day;
                case DayOfKind.Day:
                    return dateTime.Day == dateTime.GetFirstDayOfMonth().Day;
                case DayOfKind.Weekday:
                    return dateTime.Day == dateTime.GetFirstWeekdayOfMonth().Day;
                case DayOfKind.WeekendDay:
                    return dateTime.Day == dateTime.GetFirstWeekendDayOfMonth().Day;
                default:
                    break;
            }

            return false;
        }

        public static bool IsSecondKindDayOfMonth(this DateTime dateTime, DayOfKind dayOfKind)
        {
            switch (dayOfKind)
            {
                case DayOfKind.Sunday:
                case DayOfKind.Monday:
                case DayOfKind.Tuesday:
                case DayOfKind.Wednesday:
                case DayOfKind.Thursday:
                case DayOfKind.Friday:
                case DayOfKind.Saturday:
                    return dateTime.Day == dateTime.GetSecondDayOfWeekOfMonth((DayOfWeek)((int)dayOfKind)).Day;
                case DayOfKind.Day:
                    return dateTime.Day == dateTime.GetSecondDayOfMonth().Day;
                case DayOfKind.Weekday:
                    return dateTime.Day == dateTime.GetSecondWeekdayOfMonth().Day;
                case DayOfKind.WeekendDay:
                    return dateTime.Day == dateTime.GetSecondWeekendDayOfMonth().Day;
                default:
                    break;
            }

            return false;
        }

        public static bool IsThirdKindDayOfMonth(this DateTime dateTime, DayOfKind dayOfKind)
        {
            switch (dayOfKind)
            {
                case DayOfKind.Sunday:
                case DayOfKind.Monday:
                case DayOfKind.Tuesday:
                case DayOfKind.Wednesday:
                case DayOfKind.Thursday:
                case DayOfKind.Friday:
                case DayOfKind.Saturday:
                    return dateTime.Day == dateTime.GetThirdDayOfWeekOfMonth((DayOfWeek)((int)dayOfKind)).Day;
                case DayOfKind.Day:
                    return dateTime.Day == dateTime.GetThirdDayOfMonth().Day;
                case DayOfKind.Weekday:
                    return dateTime.Day == dateTime.GetThirdWeekdayOfMonth().Day;
                case DayOfKind.WeekendDay:
                    return dateTime.Day == dateTime.GetThirdWeekendDayOfMonth().Day;
                default:
                    break;
            }

            return false;
        }

        public static bool IsFourthKindDayOfMonth(this DateTime dateTime, DayOfKind dayOfKind)
        {
            switch (dayOfKind)
            {
                case DayOfKind.Sunday:
                case DayOfKind.Monday:
                case DayOfKind.Tuesday:
                case DayOfKind.Wednesday:
                case DayOfKind.Thursday:
                case DayOfKind.Friday:
                case DayOfKind.Saturday:
                    return dateTime.Day == dateTime.GetFourthDayOfWeekOfMonth((DayOfWeek)((int)dayOfKind)).Day;
                case DayOfKind.Day:
                    return dateTime.Day == dateTime.GetFourthDayOfMonth().Day;
                case DayOfKind.Weekday:
                    return dateTime.Day == dateTime.GetFourthWeekdayOfMonth().Day;
                case DayOfKind.WeekendDay:
                    return dateTime.Day == dateTime.GetFourthWeekendDayOfMonth().Day;
                default:
                    break;
            }

            return false;
        }

        public static bool IsLastKindDayOfMonth(this DateTime dateTime, DayOfKind dayOfKind)
        {
            switch (dayOfKind)
            {
                case DayOfKind.Sunday:
                case DayOfKind.Monday:
                case DayOfKind.Tuesday:
                case DayOfKind.Wednesday:
                case DayOfKind.Thursday:
                case DayOfKind.Friday:
                case DayOfKind.Saturday:
                    return dateTime.Day == dateTime.GetLastDayOfWeekOfMonth((DayOfWeek)((int)dayOfKind)).Day;
                case DayOfKind.Day:
                    return dateTime.Day == dateTime.GetLastDayOfMonth().Day;
                case DayOfKind.Weekday:
                    return dateTime.Day == dateTime.GetLastWeekdayOfMonth().Day;
                case DayOfKind.WeekendDay:
                    return dateTime.Day == dateTime.GetLastWeekendDayOfMonth().Day;
                default:
                    break;
            }

            return false;
        }

        public static bool IsKindDayOfMonth(this DateTime dateTime, DayOfMonth dayOfMonth, DayOfKind dayOfKind)
        {
            switch (dayOfMonth)
            {
                case DayOfMonth.First:
                    return IsFirstKindDayOfMonth(dateTime, dayOfKind);
                case DayOfMonth.Second:
                    return IsSecondKindDayOfMonth(dateTime, dayOfKind);
                case DayOfMonth.Third:
                    return IsThirdKindDayOfMonth(dateTime, dayOfKind);
                case DayOfMonth.Fourth:
                    return IsFourthKindDayOfMonth(dateTime, dayOfKind);
                case DayOfMonth.Fifth:
                case DayOfMonth.Sixth:
                case DayOfMonth.Seventh:
                case DayOfMonth.Eighth:
                case DayOfMonth.Ninth:
                case DayOfMonth.Tenth:
                case DayOfMonth.Eleventh:
                case DayOfMonth.Twelfth:
                case DayOfMonth.Thirteenth:
                case DayOfMonth.Fourteenth:
                case DayOfMonth.Fifteenth:
                case DayOfMonth.Sixteenth:
                case DayOfMonth.Seventeenth:
                case DayOfMonth.Eighteenth:
                case DayOfMonth.Nineteenth:
                case DayOfMonth.Twentieth:
                case DayOfMonth.TwentyFirst:
                case DayOfMonth.TwentySecond:
                case DayOfMonth.TwentyThird:
                case DayOfMonth.TwentyFourth:
                case DayOfMonth.TwentyFifth:
                case DayOfMonth.TwentySixth:
                case DayOfMonth.TwentySeventh:
                case DayOfMonth.TwentyEighth:
                case DayOfMonth.TwentyNineth:
                case DayOfMonth.Thirtieth:
                case DayOfMonth.ThirtyFirst:
                    {
                        if (dayOfKind != DayOfKind.Day)
                            throw new InvalidProgramException("Does not support given kind of day.");

                        int day = (int)dayOfMonth;
                        return dateTime.Day == day;
                    }
                case DayOfMonth.Last:
                    return IsLastKindDayOfMonth(dateTime, dayOfKind);
                default:
                    break;
            }

            throw new InvalidProgramException("Cannot parse the given month and day.");
        }

        public static bool IsMonthOfYear(this DateTime dateTime, MonthOfYear monthOfYear)
        {
            switch (monthOfYear)
            {
                case MonthOfYear.January:
                case MonthOfYear.February:
                case MonthOfYear.March:
                case MonthOfYear.April:
                case MonthOfYear.May:
                case MonthOfYear.June:
                case MonthOfYear.July:
                case MonthOfYear.August:
                case MonthOfYear.September:
                case MonthOfYear.October:
                case MonthOfYear.November:
                case MonthOfYear.December:
                    return dateTime.Month == (int)monthOfYear;
                default:
                    break;
            }

            throw new InvalidProgramException("Does not support given month of year.");
        }

        #endregion
    }

    public enum DayOfKind
    {
        [Description("Sunday")]
        [XmlEnum(Name = "Sunday")]
        Sunday = 0,

        [Description("Monday")]
        [XmlEnum(Name = "Monday")]
        Monday = 1,

        [Description("Tuesday")]
        [XmlEnum(Name = "Tuesday")]
        Tuesday = 2,

        [Description("Wednesday")]
        [XmlEnum(Name = "Wednesday")]
        Wednesday = 3,

        [Description("Thursday")]
        [XmlEnum(Name = "Thursday")]
        Thursday = 4,

        [Description("Friday")]
        [XmlEnum(Name = "Friday")]
        Friday = 5,

        [Description("Saturday")]
        [XmlEnum(Name = "Saturday")]
        Saturday = 6,

        [Description("Day")]
        [XmlEnum(Name = "Day")]
        Day = 7,

        [Description("Weekday")]
        [XmlEnum(Name = "Weekday")]
        Weekday = 8,

        [Description("WeekendDay")]
        [XmlEnum(Name = "WeekendDay")]
        WeekendDay = 9,
    }

    public enum DayOfMonth
    {
        [Description("First")]
        [XmlEnum(Name = "First")]
        First = 1,

        [Description("Second")]
        [XmlEnum(Name = "Second")]
        Second = 2,

        [Description("Third")]
        [XmlEnum(Name = "Third")]
        Third = 3,

        [Description("Fourth")]
        [XmlEnum(Name = "Fourth")]
        Fourth = 4,

        [Description("Fifth")]
        [XmlEnum(Name = "Fifth")]
        Fifth = 5,

        [Description("Sixth")]
        [XmlEnum(Name = "Sixth")]
        Sixth = 6,

        [Description("Seventh")]
        [XmlEnum(Name = "Seventh")]
        Seventh = 7,

        [Description("Eighth")]
        [XmlEnum(Name = "Eighth")]
        Eighth = 8,

        [Description("Ninth")]
        [XmlEnum(Name = "Ninth")]
        Ninth = 9,

        [Description("Tenth")]
        [XmlEnum(Name = "Tenth")]
        Tenth = 10,

        [Description("Eleventh")]
        [XmlEnum(Name = "Eleventh")]
        Eleventh = 11,

        [Description("Twelfth")]
        [XmlEnum(Name = "Twelfth")]
        Twelfth = 12,

        [Description("Thirteenth")]
        [XmlEnum(Name = "Thirteenth")]
        Thirteenth = 13,

        [Description("Fourteenth")]
        [XmlEnum(Name = "Fourteenth")]
        Fourteenth = 14,

        [Description("Fifteenth")]
        [XmlEnum(Name = "Fifteenth")]
        Fifteenth = 15,

        [Description("Sixteenth")]
        [XmlEnum(Name = "Sixteenth")]
        Sixteenth = 16,

        [Description("Seventeenth")]
        [XmlEnum(Name = "Seventeenth")]
        Seventeenth = 17,

        [Description("Eighteenth")]
        [XmlEnum(Name = "Eighteenth")]
        Eighteenth = 18,

        [Description("Nineteenth")]
        [XmlEnum(Name = "Nineteenth")]
        Nineteenth = 19,

        [Description("Twentieth")]
        [XmlEnum(Name = "Twentieth")]
        Twentieth = 20,

        [Description("TwentyFirst")]
        [XmlEnum(Name = "TwentyFirst")]
        TwentyFirst = 21,

        [Description("TwentySecond")]
        [XmlEnum(Name = "TwentySecond")]
        TwentySecond = 22,

        [Description("TwentyThird")]
        [XmlEnum(Name = "TwentyThird")]
        TwentyThird = 23,

        [Description("TwentyFourth")]
        [XmlEnum(Name = "TwentyFourth")]
        TwentyFourth = 24,

        [Description("TwentyFifth")]
        [XmlEnum(Name = "TwentyFifth")]
        TwentyFifth = 25,

        [Description("TwentySixth")]
        [XmlEnum(Name = "TwentySixth")]
        TwentySixth = 26,

        [Description("TwentySeventh")]
        [XmlEnum(Name = "TwentySeventh")]
        TwentySeventh = 27,

        [Description("TwentyEighth")]
        [XmlEnum(Name = "TwentyEighth")]
        TwentyEighth = 28,

        [Description("TwentyNineth")]
        [XmlEnum(Name = "TwentyNineth")]
        TwentyNineth = 29,

        [Description("Thirtieth")]
        [XmlEnum(Name = "Thirtieth")]
        Thirtieth = 30,

        [Description("ThirtyFirst")]
        [XmlEnum(Name = "ThirtyFirst")]
        ThirtyFirst = 31,

        [Description("Last")]
        [XmlEnum(Name = "Last")]
        Last = 9999,
    }

    public enum MonthOfYear
    {
        [Description("January")]
        [XmlEnum(Name = "January")]
        January = 1,

        [Description("February")]
        [XmlEnum(Name = "February")]
        February = 2,

        [Description("March")]
        [XmlEnum(Name = "March")]
        March = 3,

        [Description("April")]
        [XmlEnum(Name = "April")]
        April = 4,

        [Description("May")]
        [XmlEnum(Name = "May")]
        May = 5,

        [Description("June")]
        [XmlEnum(Name = "June")]
        June = 6,

        [Description("July")]
        [XmlEnum(Name = "July")]
        July = 7,

        [Description("August")]
        [XmlEnum(Name = "August")]
        August = 8,

        [Description("September")]
        [XmlEnum(Name = "September")]
        September = 9,

        [Description("October")]
        [XmlEnum(Name = "October")]
        October = 10,

        [Description("November")]
        [XmlEnum(Name = "November")]
        November = 11,

        [Description("December")]
        [XmlEnum(Name = "December")]
        December = 12,
    }
}
