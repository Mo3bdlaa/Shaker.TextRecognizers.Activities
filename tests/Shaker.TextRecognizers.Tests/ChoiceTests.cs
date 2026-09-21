using TextRecognizers.Choices;
using Xunit;
using static TextRecognizers.Tests.Wf;

namespace TextRecognizers.Tests
{
    public class ChoiceTests
    {
        private static (bool value, bool success) Parse(string text)
        {
            var outputs = Out(new ParseBoolean { Text = Lit(text) });
            return ((bool)outputs["Value"], (bool)outputs["Success"]);
        }

        [Theory]
        [InlineData("yes")]
        [InlineData("yeah, that works for me")]
        [InlineData("sure")]
        public void Recognizes_affirmative_answers(string text)
        {
            var (value, success) = Parse(text);
            Assert.True(success);
            Assert.True(value);
        }

        [Theory]
        [InlineData("no")]
        [InlineData("nope, not today")]
        public void Recognizes_negative_answers(string text)
        {
            var (value, success) = Parse(text);
            Assert.True(success);
            Assert.False(value);
        }

        [Fact]
        public void No_answer_reports_failure() => Assert.False(Parse("the quick brown fox").success);
    }
}
