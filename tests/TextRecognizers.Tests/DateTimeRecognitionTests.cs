using System;
using System.Collections.Generic;
using System.Data;
using TextRecognizers.Core;
using TextRecognizers.DateTimes;
using Xunit;
using static TextRecognizers.Tests.Wf;

namespace TextRecognizers.Tests
{
    public class DateTimeRecognitionTests
    {
        // A fixed reference time keeps relative phrases like "next Friday" deterministic.
        // 2026-06-10 is a Wednesday; 06-19 is the following Friday.
        private static readonly DateTime Reference = new DateTime(2026, 6, 10);

        private static DateTimeRecognitionResult Parse(string text, CultureOption culture = CultureOption.English)
        {
            var outputs = Out(new ParseDateTime
            {
                Text = Lit(text),
                Culture = culture,
                ReferenceTime = Lit(Reference),
            });
            return (DateTimeRecognitionResult)outputs["Result"];
        }

        [Fact]
        public void Parses_absolute_datetime()
        {
            var result = Parse("next Friday at 3pm");
            Assert.Equal(DateTimeSubtype.DateTime, result.Subtype);
            Assert.Equal(new DateTime(2026, 6, 19, 15, 0, 0), result.Value);
        }

        [Fact]
        public void Parses_date_range()
        {
            var result = Parse("from March 1 to March 15");
            Assert.True(result.IsRange);
            Assert.Equal(3, result.RangeStart!.Value.Month);
            Assert.Equal(1, result.RangeStart!.Value.Day);
            Assert.Equal(15, result.RangeEnd!.Value.Day);
        }

        [Fact]
        public void Reports_no_match()
        {
            var outputs = Out(new ParseDateTime { Text = Lit("nothing to see here"), ReferenceTime = Lit(Reference) });
            Assert.False((bool)outputs["Success"]);
            Assert.Null(outputs["Result"]);
        }

        [Fact]
        public void Recognizes_multiple_matches_in_order()
        {
            var outputs = Out(new RecognizeDateTime
            {
                Text = Lit("call me tomorrow or next Tuesday at noon"),
                ReferenceTime = Lit(Reference),
            });
            var matches = (List<DateTimeRecognitionResult>)outputs["Matches"];

            Assert.True((bool)outputs["HasMatches"]);
            Assert.Equal(2, matches.Count);
            Assert.Equal(new DateTime(2026, 6, 11), matches[0].Value);            // tomorrow
            Assert.Equal(new DateTime(2026, 6, 16, 12, 0, 0), matches[1].Value);  // next Tuesday at noon
        }

        [Fact]
        public void Datatable_has_one_row_per_match()
        {
            var outputs = Out(new RecognizeDateTime { Text = Lit("tomorrow and next week"), ReferenceTime = Lit(Reference) });
            var matches = (List<DateTimeRecognitionResult>)outputs["Matches"];
            var table = (DataTable)outputs["MatchesTable"];

            Assert.Equal(matches.Count, table.Rows.Count);
            Assert.Equal(9, table.Columns.Count);
        }

        [Theory]
        [InlineData(CultureOption.Spanish)]
        [InlineData(CultureOption.French)]
        [InlineData(CultureOption.Chinese)]
        public void Supported_cultures_do_not_throw(CultureOption culture)
        {
            var exception = Record.Exception(() => Parse("2026-06-15", culture));
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(CultureOption.Korean)]
        [InlineData(CultureOption.Swedish)]
        [InlineData(CultureOption.Bulgarian)]
        public void Unsupported_cultures_throw_a_clear_error(CultureOption culture)
        {
            var exception = Assert.Throws<NotSupportedException>(() => Parse("2026-06-15", culture));
            Assert.Contains("does not support", exception.Message);
        }
    }
}
