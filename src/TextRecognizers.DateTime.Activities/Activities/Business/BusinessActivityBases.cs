using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;

namespace TextRecognizers.DateTimes
{
    /// <summary>
    /// Base for business-date activities that need to know which days are the weekend.
    /// Supplies the <see cref="Weekend"/> preset drop-down and an optional
    /// <see cref="CustomWeekendDays"/> override.
    /// </summary>
    /// <typeparam name="TResult">The activity's single output value.</typeparam>
    public abstract class WeekendActivityBase<TResult> : CodeActivity<TResult>
    {
        /// <summary>Which days count as the weekend. Defaults to Saturday &amp; Sunday.</summary>
        [Category("Options")]
        [DisplayName("Weekend")]
        [Description("Which days count as the weekend. Defaults to Saturday & Sunday; choose Friday & Saturday for a Middle East week.")]
        public WeekendOption Weekend { get; set; } = WeekendOption.SaturdaySunday;

        /// <summary>Optional explicit weekend days; overrides <see cref="Weekend"/> when set.</summary>
        [Category("Options")]
        [DisplayName("Custom Weekend Days")]
        [Description("Optional. A list of DayOfWeek values to use as the weekend instead of the Weekend preset. Leave empty to use the preset.")]
        public InArgument<IEnumerable<DayOfWeek>> CustomWeekendDays { get; set; }

        /// <summary>Resolves the effective set of weekend days for this run.</summary>
        protected ISet<DayOfWeek> GetWeekend(CodeActivityContext context) =>
            BusinessDates.ResolveWeekend(Weekend, CustomWeekendDays?.Get(context));
    }

    /// <summary>
    /// Base for business-date activities that also consider holidays. Adds the optional
    /// <see cref="Holidays"/> input on top of the weekend configuration.
    /// </summary>
    /// <typeparam name="TResult">The activity's single output value.</typeparam>
    public abstract class BusinessDateActivityBase<TResult> : WeekendActivityBase<TResult>
    {
        /// <summary>Optional list of dates to treat as non-working holidays (times ignored).</summary>
        [Category("Input")]
        [DisplayName("Holidays")]
        [Description("Optional. A list of dates to treat as non-working holidays. Times of day are ignored.")]
        public InArgument<IEnumerable<DateTime>> Holidays { get; set; }

        /// <summary>Resolves the effective set of holiday dates for this run.</summary>
        protected ISet<DateTime> GetHolidays(CodeActivityContext context) =>
            BusinessDates.ResolveHolidays(Holidays?.Get(context));
    }
}
