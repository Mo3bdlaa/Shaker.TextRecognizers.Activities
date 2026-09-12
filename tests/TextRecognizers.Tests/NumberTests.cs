using System.Collections.Generic;
using TextRecognizers.Numbers;
using Xunit;
using static TextRecognizers.Tests.Wf;

namespace TextRecognizers.Tests
{
    public class NumberTests
    {
        private static (bool success, double value) Parse(string text, NumberKind kind = NumberKind.Number)
        {
            var outputs = Out(new ParseNumber { Text = Lit(text), Kind = kind });
            return ((bool)outputs["Success"], (double)outputs["Value"]);
        }

        [Fact]
        public void Parses_a_plain_number() => Assert.Equal(3d, Parse("buy 3 boxes").value);

        [Fact]
        public void Parses_a_number_written_in_words() => Assert.Equal(2.5d, Parse("two and a half").value);

        [Fact]
        public void Parses_a_percentage() => Assert.Equal(50d, Parse("the discount is fifty percent", NumberKind.Percentage).value);

        [Fact]
        public void Parses_an_ordinal() => Assert.Equal(2d, Parse("finished in 2nd place", NumberKind.Ordinal).value);

        [Fact]
        public void No_match_reports_failure() => Assert.False(Parse("hello world").success);

        [Fact]
        public void Recognizes_multiple_numbers_in_order()
        {
            var outputs = Out(new RecognizeNumbers { Text = Lit("buy 3 boxes and 1.5 kg") });
            var matches = (List<NumberRecognitionResult>)outputs["Matches"];

            Assert.Equal(2, matches.Count);
            Assert.Equal(3d, matches[0].Value);
            Assert.Equal(1.5d, matches[1].Value);
        }
    }
}
