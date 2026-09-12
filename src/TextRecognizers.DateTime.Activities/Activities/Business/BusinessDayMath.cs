using System;
using System.Activities;
using System.ComponentModel;

namespace TextRecognizers.DateTimes
{
    /// <summary>
    /// Adds (or, with a negative count, subtracts) a number of working days to a date,
    /// skipping weekends and holidays.
    /// </summary>
    [Category("TextRecognizers.DateTimes.Business")]
    [DisplayName("Add Business Days")]
    [Description("Adds a number of working days to a date, skipping weekends and holidays. Use a negative number to go backwards.")]
    public sealed class AddBusinessDays : BusinessDateActivityBase<DateTime>
    {
        /// <summary>The starting date.</summary>
        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Date")]
        [Description("The starting date.")]
        public InArgument<DateTime> Date { get; set; }

        /// <summary>How many working days to add (negative to subtract).</summary>
        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Business Days")]
        [Description("How many working days to add. Use a negative number to go backwards.")]
        public InArgument<int> BusinessDays { get; set; }

        protected override DateTime Execute(CodeActivityContext context) =>
            BusinessDates.AddBusinessDays(Date.Get(context), BusinessDays.Get(context), GetWeekend(context), GetHolidays(context));
    }

    /// <summary>Returns the first working day strictly after the given date.</summary>
    [Category("TextRecognizers.DateTimes.Business")]
    [DisplayName("Next Business Day")]
    [Description("Returns the first working day after the given date, skipping weekends and holidays.")]
    public sealed class NextBusinessDay : BusinessDateActivityBase<DateTime>
    {
        /// <summary>The starting date.</summary>
        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Date")]
        [Description("The starting date.")]
        public InArgument<DateTime> Date { get; set; }

        protected override DateTime Execute(CodeActivityContext context) =>
            BusinessDates.AddBusinessDays(Date.Get(context), 1, GetWeekend(context), GetHolidays(context));
    }

    /// <summary>Returns the first working day strictly before the given date.</summary>
    [Category("TextRecognizers.DateTimes.Business")]
    [DisplayName("Previous Business Day")]
    [Description("Returns the first working day before the given date, skipping weekends and holidays.")]
    public sealed class PreviousBusinessDay : BusinessDateActivityBase<DateTime>
    {
        /// <summary>The starting date.</summary>
        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Date")]
        [Description("The starting date.")]
        public InArgument<DateTime> Date { get; set; }

        protected override DateTime Execute(CodeActivityContext context) =>
            BusinessDates.AddBusinessDays(Date.Get(context), -1, GetWeekend(context), GetHolidays(context));
    }

    /// <summary>
    /// Counts the working days between two dates, inclusive of both endpoints (like Excel
    /// NETWORKDAYS). Negative when End is before Start.
    /// </summary>
    [Category("TextRecognizers.DateTimes.Business")]
    [DisplayName("Business Days Between")]
    [Description("Counts the working days between two dates, inclusive of both endpoints. Negative when End is before Start.")]
    public sealed class BusinessDaysBetween : BusinessDateActivityBase<int>
    {
        /// <summary>The first date (inclusive).</summary>
        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Start Date")]
        [Description("The first date (inclusive).")]
        public InArgument<DateTime> StartDate { get; set; }

        /// <summary>The last date (inclusive).</summary>
        [Category("Input")]
        [RequiredArgument]
        [DisplayName("End Date")]
        [Description("The last date (inclusive).")]
        public InArgument<DateTime> EndDate { get; set; }

        protected override int Execute(CodeActivityContext context) =>
            BusinessDates.BusinessDaysBetween(StartDate.Get(context), EndDate.Get(context), GetWeekend(context), GetHolidays(context));
    }

    /// <summary>
    /// Returns the Nth working day of a month. N = 1 is the first business day; a negative
    /// N counts from the end (-1 = the last business day).
    /// </summary>
    [Category("TextRecognizers.DateTimes.Business")]
    [DisplayName("Nth Business Day Of Month")]
    [Description("Returns the Nth working day of a month. N=1 is the first business day; negative N counts from the end (-1 = last).")]
    public sealed class NthBusinessDayOfMonth : BusinessDateActivityBase<DateTime>
    {
        /// <summary>The year, e.g. 2026.</summary>
        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Year")]
        [Description("The year, e.g. 2026.")]
        public InArgument<int> Year { get; set; }

        /// <summary>The month, 1-12.</summary>
        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Month")]
        [Description("The month, from 1 (January) to 12 (December).")]
        public InArgument<int> Month { get; set; }

        /// <summary>Which business day to return (1 = first, -1 = last).</summary>
        [Category("Input")]
        [RequiredArgument]
        [DisplayName("N")]
        [Description("Which business day to return. 1 = first business day; -1 = last business day.")]
        public InArgument<int> N { get; set; }

        protected override DateTime Execute(CodeActivityContext context) =>
            BusinessDates.NthBusinessDayOfMonth(Year.Get(context), Month.Get(context), N.Get(context), GetWeekend(context), GetHolidays(context));
    }
}
