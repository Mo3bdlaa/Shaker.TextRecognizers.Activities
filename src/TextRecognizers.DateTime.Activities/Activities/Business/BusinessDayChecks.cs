using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;

namespace TextRecognizers.DateTimes
{
    /// <summary>
    /// Returns true when the given date is a working day - that is, not a weekend day and
    /// not one of the supplied holidays.
    /// </summary>
    [Category("TextRecognizers.DateTimes.Business")]
    [DisplayName("Is Business Day")]
    [Description("Returns true when the given date is a working day - not a weekend and not in the Holidays list.")]
    public sealed class IsBusinessDay : BusinessDateActivityBase<bool>
    {
        /// <summary>The date to test.</summary>
        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Date")]
        [Description("The date to test.")]
        public InArgument<DateTime> Date { get; set; }

        protected override bool Execute(CodeActivityContext context) =>
            BusinessDates.IsBusinessDay(Date.Get(context), GetWeekend(context), GetHolidays(context));
    }

    /// <summary>Returns true when the given date falls on a weekend (per the Weekend setting).</summary>
    [Category("TextRecognizers.DateTimes.Business")]
    [DisplayName("Is Weekend")]
    [Description("Returns true when the given date falls on a weekend day, per the Weekend setting.")]
    public sealed class IsWeekend : WeekendActivityBase<bool>
    {
        /// <summary>The date to test.</summary>
        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Date")]
        [Description("The date to test.")]
        public InArgument<DateTime> Date { get; set; }

        protected override bool Execute(CodeActivityContext context) =>
            BusinessDates.IsWeekend(Date.Get(context), GetWeekend(context));
    }

    /// <summary>
    /// Returns true when the given date appears in the supplied Holidays list. This check
    /// is independent of the weekend, so it has no Weekend setting.
    /// </summary>
    [Category("TextRecognizers.DateTimes.Business")]
    [DisplayName("Is Holiday")]
    [Description("Returns true when the given date is in the supplied Holidays list. Times of day are ignored.")]
    public sealed class IsHoliday : CodeActivity<bool>
    {
        /// <summary>The date to test.</summary>
        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Date")]
        [Description("The date to test.")]
        public InArgument<DateTime> Date { get; set; }

        /// <summary>The list of dates to treat as holidays.</summary>
        [Category("Input")]
        [DisplayName("Holidays")]
        [Description("A list of dates to treat as holidays. Times of day are ignored.")]
        public InArgument<IEnumerable<DateTime>> Holidays { get; set; }

        protected override bool Execute(CodeActivityContext context) =>
            BusinessDates.IsHoliday(Date.Get(context), BusinessDates.ResolveHolidays(Holidays?.Get(context)));
    }
}
