using System.Activities;
using System.ComponentModel;
using TextRecognizers.Core;

namespace TextRecognizers.Choices
{
    /// <summary>
    /// Interprets a piece of text as a yes/no answer. Ideal for reading free-text confirmations
    /// (chat replies, form fields, email bodies) into a real <see cref="bool"/>.
    /// </summary>
    [Category("TextRecognizers.Choices")]
    [DisplayName("Parse Boolean")]
    [Description("Interprets text as a yes/no answer and returns a Boolean Value, a confidence Score and a Success flag.")]
    public sealed class ParseBoolean : RecognizerActivity
    {
        /// <summary>True when a yes/no answer was found.</summary>
        [Category("Output")]
        [DisplayName("Success")]
        [Description("True when a yes/no answer was found in the text.")]
        public OutArgument<bool> Success { get; set; }

        /// <summary>The boolean value (true = yes, false = no). False when nothing was found.</summary>
        [Category("Output")]
        [DisplayName("Value")]
        [Description("The boolean value: true for yes, false for no. False when nothing was found - check Success to tell them apart.")]
        public OutArgument<bool> Value { get; set; }

        /// <summary>The recognizer's confidence in the answer, from 0 to 1.</summary>
        [Category("Output")]
        [DisplayName("Score")]
        [Description("The recognizer's confidence in the answer, from 0 to 1.")]
        public OutArgument<double> Score { get; set; }

        /// <summary>The full best match, or <see langword="null"/> when nothing was found.</summary>
        [Category("Output")]
        [DisplayName("Result")]
        [Description("The full best match (text, value, score, indices). Null when nothing was found.")]
        public OutArgument<BooleanResult> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var matches = ChoiceRecognition.Recognize(GetText(context), GetCultureCode(context));
            var best = matches.Count > 0 ? matches[0] : null;

            Success.Set(context, best != null);
            Value.Set(context, best?.Value ?? false);
            Score.Set(context, best?.Score ?? 0d);
            Result.Set(context, best!);
        }
    }
}
