using System.Activities;
using System.ComponentModel;

namespace TextRecognizers.DateTimes
{
    /// <summary>
    /// Extracts the single best date/time value from a piece of text. Use this when the
    /// input is expected to contain one date/time (for example a cell value or a form
    /// field) and you want it as a ready-to-use .NET <see cref="System.DateTime"/>.
    /// </summary>
    [Category("TextRecognizers.DateTimes")]
    [DisplayName("Parse Date/Time")]
    [Description("Extracts the single best date/time value from a piece of text and returns it as a ready-to-use result, with a Success flag.")]
    public sealed class ParseDateTime : DateTimeActivityBase
    {
        /// <summary>True when at least one date/time was found in the text.</summary>
        [Category("Output")]
        [DisplayName("Success")]
        [Description("True when at least one date/time was found in the text.")]
        public OutArgument<bool> Success { get; set; }

        /// <summary>The best date/time match, or <see langword="null"/> when nothing was found.</summary>
        [Category("Output")]
        [DisplayName("Result")]
        [Description("The best date/time match, with its parsed value(s). Null when nothing was found.")]
        public OutArgument<DateTimeRecognitionResult> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var text = GetText(context);
            var cultureCode = GetCultureCode(context);
            var reference = GetReferenceTime(context);

            var matches = DateTimeRecognition.Recognize(text, cultureCode, reference);

            // "Best" is simply the first match; the recognizer returns them left-to-right.
            var best = matches.Count > 0 ? matches[0] : null;

            Success.Set(context, best != null);
            // null is a valid "nothing found" output; the null-forgiving operator just
            // tells the nullable analyser we deliberately allow it here.
            Result.Set(context, best!);
        }
    }
}
