using System;
using System.Collections.Generic;

namespace TextRecognizers.DateTimes
{
    /// <summary>
    /// The kind of temporal value a recognition result represents. Derived from the
    /// recognizer's type name (e.g. <c>"datetimeV2.daterange"</c>).
    /// </summary>
    public enum DateTimeSubtype
    {
        /// <summary>The subtype could not be determined.</summary>
        Unknown = 0,

        /// <summary>A calendar date with no time component, e.g. "5 April".</summary>
        Date,

        /// <summary>A time of day with no date component, e.g. "3pm".</summary>
        Time,

        /// <summary>A date together with a time, e.g. "5 April at 3pm".</summary>
        DateTime,

        /// <summary>A range of dates, e.g. "next week".</summary>
        DatePeriod,

        /// <summary>A range of times, e.g. "from 2pm to 4pm".</summary>
        TimePeriod,

        /// <summary>A range spanning dates and times, e.g. "tomorrow afternoon".</summary>
        DateTimePeriod,

        /// <summary>A length of time, e.g. "two hours".</summary>
        Duration,

        /// <summary>A recurring set, e.g. "every Monday".</summary>
        Set,
    }

    /// <summary>
    /// A single resolved interpretation of a date/time mention. A phrase can be
    /// ambiguous (for example "Friday" could mean the next or the previous Friday), so a
    /// <see cref="DateTimeRecognitionResult"/> may carry more than one of these.
    /// </summary>
    public sealed class DateTimeResolutionValue
    {
        /// <summary>The recognizer's value type, e.g. "date", "time", "datetimerange", "duration", "set".</summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>The raw TIMEX3 expression for this interpretation.</summary>
        public string Timex { get; set; } = string.Empty;

        /// <summary>The single point in time, when this interpretation is a date, time or datetime.</summary>
        public DateTime? Value { get; set; }

        /// <summary>The inclusive start, when this interpretation is a period/range.</summary>
        public DateTime? Start { get; set; }

        /// <summary>The exclusive end, when this interpretation is a period/range.</summary>
        public DateTime? End { get; set; }

        /// <summary>The length of time, when this interpretation is a duration.</summary>
        public TimeSpan? Duration { get; set; }

        /// <summary>True when this interpretation describes a period/range (has a start and/or end).</summary>
        public bool IsRange => Start.HasValue || End.HasValue;
    }

    /// <summary>
    /// One date/time mention found in a piece of text, with the recognizer's loosely
    /// typed resolution already translated into ready-to-use .NET values. This is the
    /// object UiPath workflows consume - no dictionary spelunking required.
    /// </summary>
    public sealed class DateTimeRecognitionResult
    {
        /// <summary>The exact substring that was recognised, e.g. "next Friday at 3pm".</summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>Zero-based index of <see cref="Text"/> within the original input.</summary>
        public int StartIndex { get; set; }

        /// <summary>Length of <see cref="Text"/> in characters.</summary>
        public int Length { get; set; }

        /// <summary>The kind of temporal value (date, time, range, duration, ...).</summary>
        public DateTimeSubtype Subtype { get; set; }

        /// <summary>The raw TIMEX3 expression of the best interpretation, for advanced use.</summary>
        public string Timex { get; set; } = string.Empty;

        /// <summary>Every candidate interpretation, best first.</summary>
        public IList<DateTimeResolutionValue> Values { get; } = new List<DateTimeResolutionValue>();

        // ----- Convenience accessors over the first (best) interpretation -----
        // These let a workflow read result.Value / result.RangeStart directly instead of
        // indexing into Values, which keeps the common case dead simple.

        /// <summary>The best interpretation, or <see langword="null"/> when there are none.</summary>
        private DateTimeResolutionValue? Best => Values.Count > 0 ? Values[0] : null;

        /// <summary>True when the best interpretation is a period/range.</summary>
        public bool IsRange => Best?.IsRange ?? false;

        /// <summary>True when the best interpretation is a duration.</summary>
        public bool IsDuration => Best?.Duration.HasValue ?? false;

        /// <summary>True when the best interpretation is a recurring set.</summary>
        public bool IsSet => Subtype == DateTimeSubtype.Set;

        /// <summary>The single point-in-time value of the best interpretation, when applicable.</summary>
        public DateTime? Value => Best?.Value;

        /// <summary>The start of the best interpretation, when it is a period/range.</summary>
        public DateTime? RangeStart => Best?.Start;

        /// <summary>The end of the best interpretation, when it is a period/range.</summary>
        public DateTime? RangeEnd => Best?.End;

        /// <summary>The length of the best interpretation, when it is a duration.</summary>
        public TimeSpan? Duration => Best?.Duration;
    }
}
