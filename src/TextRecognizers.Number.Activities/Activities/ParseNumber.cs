using System.Activities;
using System.ComponentModel;

namespace TextRecognizers.Numbers
{
    /// <summary>
    /// Extracts the single best number (or ordinal / percentage) from a piece of text and
    /// returns it as a <see cref="double"/>. Use this when the input is expected to contain one
    /// numeric value, e.g. an amount typed into a form or pulled from a cell.
    /// </summary>
    [Category("TextRecognizers.Numbers")]
    [DisplayName("Parse Number")]
    [Description("Extracts the single best number (or ordinal / percentage) from text and returns it as a Double, with a Success flag.")]
    public sealed class ParseNumber : NumberActivityBase
    {
        /// <summary>True when at least one number was found.</summary>
        [Category("Output")]
        [DisplayName("Success")]
        [Description("True when at least one number was found in the text.")]
        public OutArgument<bool> Success { get; set; }

        /// <summary>The numeric value of the best match (0 when nothing was found).</summary>
        [Category("Output")]
        [DisplayName("Value")]
        [Description("The numeric value of the best match. For percentages this is the percentage itself (50% -> 50). 0 when nothing was found.")]
        public OutArgument<double> Value { get; set; }

        /// <summary>The full best match, or <see langword="null"/> when nothing was found.</summary>
        [Category("Output")]
        [DisplayName("Result")]
        [Description("The full best match (text, kind, value, indices). Null when nothing was found.")]
        public OutArgument<NumberRecognitionResult> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var matches = NumberRecognition.Recognize(GetText(context), GetCultureCode(context), GetKind(context));
            var best = matches.Count > 0 ? matches[0] : null;

            Success.Set(context, best != null);
            Value.Set(context, best?.Value ?? 0d);
            // null is a valid "nothing found" output.
            Result.Set(context, best!);
        }
    }
}
