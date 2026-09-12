using System;
using TextRecognizers.DateTimes;
using Xunit;
using static TextRecognizers.Tests.Wf;

namespace TextRecognizers.Tests
{
    public class TimeZoneTests
    {
        // A fixed UTC instant in winter and one in summer, so daylight saving is exercised
        // from both sides rather than depending on when the suite happens to run.
        private static readonly DateTime WinterUtc = new DateTime(2026, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime SummerUtc = new DateTime(2026, 7, 15, 12, 0, 0, DateTimeKind.Utc);

        [Fact]
        public void Utc_is_the_instant_itself()
        {
            Assert.Equal(new DateTime(2026, 1, 15, 12, 0, 0), TimeZones.At(WinterUtc, TimeZoneOption.Utc));
        }

        [Fact]
        public void London_follows_daylight_saving()
        {
            // The headline of the named-zone model: the same option gives a different offset
            // in winter and summer, with no intervention.
            Assert.Equal(new DateTime(2026, 1, 15, 12, 0, 0), TimeZones.At(WinterUtc, TimeZoneOption.UtcPlus0000_London));
            Assert.Equal(new DateTime(2026, 7, 15, 13, 0, 0), TimeZones.At(SummerUtc, TimeZoneOption.UtcPlus0000_London));
        }

        [Fact]
        public void Fixed_offset_zones_do_not_shift()
        {
            // Riyadh observes no daylight saving, so it sits at +03:00 all year.
            Assert.Equal(new DateTime(2026, 1, 15, 15, 0, 0), TimeZones.At(WinterUtc, TimeZoneOption.UtcPlus0300_Riyadh));
            Assert.Equal(new DateTime(2026, 7, 15, 15, 0, 0), TimeZones.At(SummerUtc, TimeZoneOption.UtcPlus0300_Riyadh));
        }

        [Fact]
        public void Handles_half_and_quarter_hour_offsets()
        {
            Assert.Equal(new DateTime(2026, 1, 15, 17, 30, 0), TimeZones.At(WinterUtc, TimeZoneOption.UtcPlus0530_Kolkata));
            Assert.Equal(new DateTime(2026, 1, 15, 17, 45, 0), TimeZones.At(WinterUtc, TimeZoneOption.UtcPlus0545_Kathmandu));
            Assert.Equal(new DateTime(2026, 1, 15, 8, 30, 0), TimeZones.At(WinterUtc, TimeZoneOption.UtcMinus0330_StJohns));
        }

        [Fact]
        public void SystemDefault_uses_this_machines_zone()
        {
            var expected = TimeZoneInfo.ConvertTimeFromUtc(WinterUtc, TimeZoneInfo.Local);
            Assert.Equal(expected, TimeZones.At(WinterUtc, TimeZoneOption.SystemDefault));
        }

        [Fact]
        public void Every_option_has_a_label_showing_its_offset()
        {
            foreach (TimeZoneOption option in Enum.GetValues(typeof(TimeZoneOption)))
            {
                var label = TimeZones.GetDisplayName(option);
                Assert.False(string.IsNullOrWhiteSpace(label));

                // Every entry but System Default leads with the offset, which is the whole
                // point of the labels - "(UTC+02:00) Cairo" rather than a bare city name.
                if (option != TimeZoneOption.SystemDefault)
                    Assert.StartsWith("(UTC", label);
            }
        }

        [Fact]
        public void Every_option_resolves_to_a_real_zone()
        {
            foreach (TimeZoneOption option in Enum.GetValues(typeof(TimeZoneOption)))
                Assert.NotNull(TimeZones.GetTimeZone(option));
        }

        [Fact]
        public void Time_zone_changes_which_day_now_falls_on()
        {
            // Kiritimati (+14) and Midway (-11) are 25 hours apart, so the same instant
            // always lands on different calendar dates - a deterministic way to prove the
            // Time Zone input actually reaches the recognizer.
            var east = ParseTodayIn(TimeZoneOption.UtcPlus1400_Kiritimati);
            var west = ParseTodayIn(TimeZoneOption.UtcMinus1100_Midway);

            Assert.NotEqual(west.Value!.Value.Date, east.Value!.Value.Date);
        }

        [Fact]
        public void An_explicit_reference_time_overrides_the_time_zone()
        {
            var reference = new DateTime(2026, 6, 10, 9, 0, 0);
            var outputs = Out(new ParseDateTime
            {
                Text = Lit("today"),
                ReferenceTime = Lit(reference),
                TimeZone = TimeZoneOption.UtcPlus1400_Kiritimati,
            });

            var result = (DateTimeRecognitionResult)outputs["Result"];
            Assert.Equal(reference.Date, result.Value!.Value.Date);
        }

        private static DateTimeRecognitionResult ParseTodayIn(TimeZoneOption zone)
        {
            var outputs = Out(new ParseDateTime { Text = Lit("today"), TimeZone = zone });
            return (DateTimeRecognitionResult)outputs["Result"];
        }
    }
}
