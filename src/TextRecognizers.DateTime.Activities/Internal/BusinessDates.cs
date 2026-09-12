using System;
using System.Collections.Generic;

namespace TextRecognizers.DateTimes
{
    /// <summary>
    /// Pure business-day arithmetic, kept free of any UiPath types so it is trivial to
    /// unit-test. A "business day" is any day that is neither a weekend day nor a holiday.
    /// </summary>
    internal static class BusinessDates
    {
        /// <summary>
        /// Resolves the weekend day-set: the explicit <paramref name="custom"/> list when
        /// it has any entries, otherwise the <paramref name="preset"/>.
        /// </summary>
        public static ISet<DayOfWeek> ResolveWeekend(WeekendOption preset, IEnumerable<DayOfWeek>? custom)
        {
            if (custom != null)
            {
                var set = new HashSet<DayOfWeek>(custom);
                if (set.Count > 0)
                    return set;
            }

            return WeekendPresets.ToDays(preset);
        }

        /// <summary>
        /// Normalises the supplied holidays into a set of dates (times stripped). A null
        /// list simply yields an empty set, i.e. "no holidays".
        /// </summary>
        public static ISet<DateTime> ResolveHolidays(IEnumerable<DateTime>? holidays)
        {
            var set = new HashSet<DateTime>();
            if (holidays != null)
            {
                foreach (var holiday in holidays)
                    set.Add(holiday.Date);
            }

            return set;
        }

        public static bool IsWeekend(DateTime date, ISet<DayOfWeek> weekend) =>
            weekend.Contains(date.DayOfWeek);

        public static bool IsHoliday(DateTime date, ISet<DateTime> holidays) =>
            holidays.Contains(date.Date);

        public static bool IsBusinessDay(DateTime date, ISet<DayOfWeek> weekend, ISet<DateTime> holidays) =>
            !IsWeekend(date, weekend) && !IsHoliday(date, holidays);

        /// <summary>
        /// Moves <paramref name="count"/> business days forward from <paramref name="start"/>
        /// (or backward when <paramref name="count"/> is negative), skipping weekends and
        /// holidays. A count of zero returns the start date unchanged.
        /// </summary>
        public static DateTime AddBusinessDays(DateTime start, int count, ISet<DayOfWeek> weekend, ISet<DateTime> holidays)
        {
            var step = count >= 0 ? 1 : -1;
            var remaining = Math.Abs(count);
            var date = start;

            while (remaining > 0)
            {
                date = date.AddDays(step);
                if (IsBusinessDay(date, weekend, holidays))
                    remaining--;
            }

            return date;
        }

        /// <summary>
        /// Counts the business days in the closed interval between two dates (both
        /// endpoints included, à la Excel NETWORKDAYS). The result is negative when
        /// <paramref name="end"/> falls before <paramref name="start"/>.
        /// </summary>
        public static int BusinessDaysBetween(DateTime start, DateTime end, ISet<DayOfWeek> weekend, ISet<DateTime> holidays)
        {
            var from = start.Date;
            var to = end.Date;
            var sign = 1;

            if (from > to)
            {
                (from, to) = (to, from);
                sign = -1;
            }

            var count = 0;
            for (var date = from; date <= to; date = date.AddDays(1))
            {
                if (IsBusinessDay(date, weekend, holidays))
                    count++;
            }

            return sign * count;
        }

        /// <summary>
        /// Returns the Nth business day of a month. <paramref name="n"/> = 1 is the first
        /// business day; a negative value counts from the end (-1 = last business day).
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="n"/> is zero or there are fewer than |n| business
        /// days in the month.
        /// </exception>
        public static DateTime NthBusinessDayOfMonth(int year, int month, int n, ISet<DayOfWeek> weekend, ISet<DateTime> holidays)
        {
            if (n == 0)
                throw new ArgumentOutOfRangeException(nameof(n), "N must be non-zero (1 = first business day, -1 = last).");

            var daysInMonth = DateTime.DaysInMonth(year, month);
            var seen = 0;

            if (n > 0)
            {
                for (var day = 1; day <= daysInMonth; day++)
                {
                    var date = new DateTime(year, month, day);
                    if (IsBusinessDay(date, weekend, holidays) && ++seen == n)
                        return date;
                }
            }
            else
            {
                for (var day = daysInMonth; day >= 1; day--)
                {
                    var date = new DateTime(year, month, day);
                    if (IsBusinessDay(date, weekend, holidays) && ++seen == -n)
                        return date;
                }
            }

            throw new ArgumentOutOfRangeException(
                nameof(n), $"There is no business day number {n} in {year}-{month:D2} for the given weekend/holidays.");
        }
    }
}
