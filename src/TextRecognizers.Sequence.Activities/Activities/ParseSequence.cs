using System.Activities;
using System.ComponentModel;

namespace TextRecognizers.Sequences
{
    /// <summary>
    /// Extracts the single best sequence (email, phone, URL, …) from a piece of text and returns
    /// its value as a string. Handy for validating or pulling one value out of a field.
    /// </summary>
    [Category("TextRecognizers.Sequences")]
    [DisplayName("Parse Sequence")]
    [Description("Extracts the single best email / phone / URL / … (per Kind) from text, with a Success flag.")]
    public sealed class ParseSequence : SequenceActivityBase
    {
        /// <summary>True when at least one sequence was found.</summary>
        [Category("Output")]
        [DisplayName("Success")]
        [Description("True when at least one sequence was found in the text.")]
        public OutArgument<bool> Success { get; set; }

        /// <summary>The value of the best match (empty when nothing was found).</summary>
        [Category("Output")]
        [DisplayName("Value")]
        [Description("The value of the best match (e.g. the email address). Empty when nothing was found.")]
        public OutArgument<string> Value { get; set; }

        /// <summary>The full best match, or <see langword="null"/> when nothing was found.</summary>
        [Category("Output")]
        [DisplayName("Result")]
        [Description("The full best match (text, kind, value, indices). Null when nothing was found.")]
        public OutArgument<SequenceResult> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var matches = SequenceRecognition.Recognize(GetText(context), GetCultureCode(context), GetKind(context));
            var best = matches.Count > 0 ? matches[0] : null;

            Success.Set(context, best != null);
            Value.Set(context, best?.Value ?? string.Empty);
            Result.Set(context, best!);
        }
    }
}
