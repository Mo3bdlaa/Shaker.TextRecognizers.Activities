using System;
using System.Collections.Generic;

namespace TextRecognizers.DateTimes
{
    /// <summary>
    /// A ready-made weekend definition. Picking one of these from the drop-down avoids
    /// having to spell out which days are non-working. For an unusual pattern, supply a
    /// custom list of days on the activity instead.
    /// </summary>
    public enum WeekendOption
    {
        /// <summary>Saturday and Sunday - the most common weekend.</summary>
        SaturdaySunday = 0,

        /// <summary>Friday and Saturday - common across the Middle East.</summary>
        FridaySaturday,

        /// <summary>Thursday and Friday.</summary>
        ThursdayFriday,

        /// <summary>Sunday only.</summary>
        SundayOnly,

        /// <summary>Friday only.</summary>
        FridayOnly,

        /// <summary>No weekend - every day is treated as a working day.</summary>
        None,
    }

    /// <summary>Maps a <see cref="WeekendOption"/> preset onto the set of weekend days.</summary>
    internal static class WeekendPresets
    {
        public static ISet<DayOfWeek> ToDays(WeekendOption option) => option switch
        {
            WeekendOption.SaturdaySunday => new HashSet<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday },
            WeekendOption.FridaySaturday => new HashSet<DayOfWeek> { DayOfWeek.Friday, DayOfWeek.Saturday },
            WeekendOption.ThursdayFriday => new HashSet<DayOfWeek> { DayOfWeek.Thursday, DayOfWeek.Friday },
            WeekendOption.SundayOnly => new HashSet<DayOfWeek> { DayOfWeek.Sunday },
            WeekendOption.FridayOnly => new HashSet<DayOfWeek> { DayOfWeek.Friday },
            WeekendOption.None => new HashSet<DayOfWeek>(),
            _ => new HashSet<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday },
        };
    }
}
