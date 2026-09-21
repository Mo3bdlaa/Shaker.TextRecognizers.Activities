using System.Collections.Generic;
using TextRecognizers.Choices;
using TextRecognizers.DateTimes;
using TextRecognizers.Numbers;
using TextRecognizers.Sequences;
using Xunit;
using static TextRecognizers.Tests.Wf;

namespace TextRecognizers.Tests
{
    /// <summary>
    /// Cases from the Studio test pass that the package got wrong. Each one asserts the
    /// documented behaviour, so a regression shows up here rather than in Studio.
    /// </summary>
    public class ReportedIssuesTests
    {
        // --- Matched text keeps the input's own casing ---------------------------------

        [Theory]
        [InlineData("Monday 28 September at 10am")]
        [InlineData("Next Friday At 3PM")]
        public void DateTime_match_text_is_the_exact_substring(string input)
        {
            var outputs = Out(new ParseDateTime { Text = Lit(input) });
            var result = (DateTimeRecognitionResult)outputs["Result"];

            // The recognizer lower-cases its own Text; the offsets it reports are exact, and
            // Text is sliced from the input at those offsets.
            Assert.Equal(input.Substring(result.StartIndex, result.Length), result.Text);
        }

        [Fact]
        public void Sequence_match_text_is_the_exact_substring()
        {
            const string input = "Mail Jane.Doe@Acme.COM today";
            var outputs = Out(new ParseSequence { Text = Lit(input), Kind = SequenceKind.Email });
            var result = (SequenceResult)outputs["Result"];

            Assert.Equal(input.Substring(result.StartIndex, result.Length), result.Text);
        }

        [Fact]
        public void Number_match_text_is_the_exact_substring()
        {
            const string input = "Three Hundred And Twelve items";
            var outputs = Out(new ParseNumber { Text = Lit(input), Kind = NumberKind.Number });
            var result = (NumberRecognitionResult)outputs["Result"];

            Assert.Equal(input.Substring(result.StartIndex, result.Length), result.Text);
        }

        // --- Relative ordinals carry no number, so they are not reported ----------------

        [Theory]
        [InlineData("next")]
        [InlineData("last")]
        public void Relative_ordinals_are_not_reported_as_NaN(string input)
        {
            var outputs = Out(new ParseNumber { Text = Lit(input), Kind = NumberKind.Ordinal });

            Assert.False((bool)outputs["Success"]);
            Assert.Equal(0d, (double)outputs["Value"]);

            var matches = (IList<NumberRecognitionResult>)Out(
                new RecognizeNumbers { Text = Lit(input), Kind = NumberKind.Ordinal })["Matches"];
            Assert.Empty(matches);
        }

        [Fact]
        public void Absolute_ordinals_still_resolve()
        {
            var outputs = Out(new ParseNumber { Text = Lit("twenty-first"), Kind = NumberKind.Ordinal });

            Assert.True((bool)outputs["Success"]);
            Assert.Equal(21d, (double)outputs["Value"]);
        }

        // --- Every yes/no answer in the string, not just the first ----------------------

        [Fact]
        public void Recognize_booleans_finds_every_answer()
        {
            const string input = "Yes to the refund. No to the replacement.";
            var matches = (IList<BooleanResult>)Out(new RecognizeBooleans { Text = Lit(input) })["Matches"];

            Assert.Equal(2, matches.Count);
            Assert.True(matches[0].Value);
            Assert.False(matches[1].Value);

            // In order of appearance, and each locatable in the input.
            Assert.True(matches[0].StartIndex < matches[1].StartIndex);
            foreach (var match in matches)
                Assert.Equal(input.Substring(match.StartIndex, match.Length), match.Text);
        }

        [Fact]
        public void A_single_answer_is_still_reported_once()
        {
            var matches = (IList<BooleanResult>)Out(new RecognizeBooleans { Text = Lit("yeah, that works for me") })["Matches"];

            Assert.Single(matches);
            Assert.True(matches[0].Value);
        }

        // --- What the recognizer does and does not know, as the README states it ---------
        //
        // These pin the vocabulary the docs promise. The recognizer owns this list, not us -
        // if a future engine version widens it, these fail and the README gets updated.

        [Theory]
        [InlineData("yes", true)]
        [InlineData("yeah", true)]
        [InlineData("yep", true)]
        [InlineData("yup", true)]
        [InlineData("sure", true)]
        [InlineData("ok", true)]
        [InlineData("no", false)]
        [InlineData("nope", false)]
        [InlineData("no way", false)]
        public void Recognised_yes_no_forms(string input, bool expected)
        {
            var outputs = Out(new ParseBoolean { Text = Lit(input) });

            Assert.True((bool)outputs["Success"], $"expected \"{input}\" to be recognised");
            Assert.Equal(expected, (bool)outputs["Value"]);
        }

        [Theory]
        [InlineData("absolutely")]
        [InlineData("definitely")]
        [InlineData("of course")]
        [InlineData("nah")]
        [InlineData("never")]
        [InlineData("I don't think so")]
        [InlineData("maybe later")]
        public void Forms_the_recognizer_does_not_know(string input)
        {
            var outputs = Out(new ParseBoolean { Text = Lit(input) });

            Assert.False((bool)outputs["Success"], $"expected \"{input}\" to go unrecognised");
        }

        // --- Phone spans come from the recognizer, country code included or not ----------

        [Fact]
        public void Phone_span_is_whatever_the_recognizer_matched()
        {
            const string us = "call +1 555-123-4567 now";
            var usResult = (SequenceResult)Out(new ParseSequence { Text = Lit(us), Kind = SequenceKind.PhoneNumber })["Result"];

            // The recognizer's span for this national format leaves the country code out, as
            // the README says. Text still matches the span exactly.
            Assert.Equal(us.Substring(usResult.StartIndex, usResult.Length), usResult.Text);
            Assert.Equal("555-123-4567", usResult.Text);

            const string uk = "call +44 20 7946 0958 now";
            var ukResult = (SequenceResult)Out(new ParseSequence { Text = Lit(uk), Kind = SequenceKind.PhoneNumber })["Result"];
            Assert.Equal("+44 20 7946 0958", ukResult.Text);
        }
    }
}
