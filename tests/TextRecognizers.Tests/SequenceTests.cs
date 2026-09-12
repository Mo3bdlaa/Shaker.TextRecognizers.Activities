using System.Collections.Generic;
using TextRecognizers.Sequences;
using Xunit;
using static TextRecognizers.Tests.Wf;

namespace TextRecognizers.Tests
{
    public class SequenceTests
    {
        private static (string value, bool success) Parse(string text, SequenceKind kind)
        {
            var outputs = Out(new ParseSequence { Text = Lit(text), Kind = kind });
            return ((string)outputs["Value"], (bool)outputs["Success"]);
        }

        [Fact]
        public void Parses_email()
        {
            var (value, success) = Parse("email me at jane@acme.com please", SequenceKind.Email);
            Assert.True(success);
            Assert.Equal("jane@acme.com", value);
        }

        [Fact]
        public void Parses_url()
        {
            var (value, success) = Parse("visit https://mohammedshaker.com today", SequenceKind.Url);
            Assert.True(success);
            Assert.Contains("mohammedshaker.com", value);
        }

        [Fact]
        public void Recognizes_multiple_emails()
        {
            var outputs = Out(new RecognizeSequences { Text = Lit("write to a@b.com and c@d.com"), Kind = SequenceKind.Email });
            var matches = (List<SequenceResult>)outputs["Matches"];
            Assert.Equal(2, matches.Count);
        }
    }
}
