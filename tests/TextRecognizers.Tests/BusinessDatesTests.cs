using System;
using System.Collections.Generic;
using TextRecognizers.DateTimes;
using Xunit;
using static TextRecognizers.Tests.Wf;

namespace TextRecognizers.Tests
{
    public class BusinessDatesTests
    {
        // Anchors: 2026-06-12 Fri | 06-13 Sat | 06-14 Sun | 06-15 Mon | 06-19 Fri | 06-30 Tue
        private static DateTime D(int y, int m, int d) => new DateTime(y, m, d);

        private static Dictionary<string, object> WithHolidays(params DateTime[] dates) =>
            new Dictionary<string, object> { ["Holidays"] = new List<DateTime>(dates) };

        [Fact]
        public void AddBusinessDays_skips_the_weekend()
        {
            Assert.Equal(D(2026, 6, 15), Result(new AddBusinessDays { Date = Lit(D(2026, 6, 12)), BusinessDays = Lit(1) }));
        }

        [Fact]
        public void AddBusinessDays_skips_a_holiday()
        {
            var result = Result(new AddBusinessDays { Date = Lit(D(2026, 6, 12)), BusinessDays = Lit(1) },
                                 WithHolidays(D(2026, 6, 15)));
            Assert.Equal(D(2026, 6, 16), result);
        }

        [Fact]
        public void AddBusinessDays_negative_goes_backwards()
        {
            Assert.Equal(D(2026, 6, 12), Result(new AddBusinessDays { Date = Lit(D(2026, 6, 15)), BusinessDays = Lit(-1) }));
        }

        [Fact]
        public void IsBusinessDay_false_on_saturday_true_on_friday()
        {
            Assert.False(Result(new IsBusinessDay { Date = Lit(D(2026, 6, 13)) }));
            Assert.True(Result(new IsBusinessDay { Date = Lit(D(2026, 6, 12)) }));
        }

        [Fact]
        public void IsWeekend_respects_friday_saturday_preset()
        {
            Assert.True(Result(new IsWeekend { Date = Lit(D(2026, 6, 12)), Weekend = WeekendOption.FridaySaturday }));
            Assert.False(Result(new IsWeekend { Date = Lit(D(2026, 6, 14)), Weekend = WeekendOption.FridaySaturday }));
        }

        [Fact]
        public void IsHoliday_matches_ignoring_time_of_day()
        {
            var result = Result(new IsHoliday { Date = Lit(D(2026, 12, 25)) },
                                new Dictionary<string, object> { ["Holidays"] = new List<DateTime> { new DateTime(2026, 12, 25, 9, 30, 0) } });
            Assert.True(result);
        }

        [Fact]
        public void BusinessDaysBetween_is_inclusive_and_signed()
        {
            Assert.Equal(5, Result(new BusinessDaysBetween { StartDate = Lit(D(2026, 6, 15)), EndDate = Lit(D(2026, 6, 19)) }));
            Assert.Equal(-5, Result(new BusinessDaysBetween { StartDate = Lit(D(2026, 6, 19)), EndDate = Lit(D(2026, 6, 15)) }));
        }

        [Fact]
        public void NthBusinessDayOfMonth_first_and_last()
        {
            Assert.Equal(D(2026, 6, 1), Result(new NthBusinessDayOfMonth { Year = Lit(2026), Month = Lit(6), N = Lit(1) }));
            Assert.Equal(D(2026, 6, 30), Result(new NthBusinessDayOfMonth { Year = Lit(2026), Month = Lit(6), N = Lit(-1) }));
        }

        [Fact]
        public void NextAndPrevious_business_day()
        {
            Assert.Equal(D(2026, 6, 15), Result(new NextBusinessDay { Date = Lit(D(2026, 6, 12)) }));     // Fri -> Mon
            Assert.Equal(D(2026, 6, 12), Result(new PreviousBusinessDay { Date = Lit(D(2026, 6, 15)) })); // Mon -> Fri
        }
    }
}
