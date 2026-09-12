using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace TextRecognizers.DateTimes
{
    /// <summary>
    /// Finds every date/time mention in a piece of text and returns them as a list of
    /// ready-to-use results. Use this when a single string may contain several dates or
    /// times and you want them all.
    /// </summary>
    [Category("TextRecognizers.DateTimes")]
    [DisplayName("Recognize Date/Time")]
    [Description("Finds every date/time mention in a piece of text and returns them as a list of ready-to-use results.")]
    public sealed class RecognizeDateTime : DateTimeActivityBase
    {
        /// <summary>All date/time values found, in order of appearance.</summary>
        [Category("Output")]
        [DisplayName("Matches")]
        [Description("All date/time values found in the text, in order of appearance.")]
        public OutArgument<List<DateTimeRecognitionResult>> Matches { get; set; }

        /// <summary>True when at least one date/time was found.</summary>
        [Category("Output")]
        [DisplayName("Has Matches")]
        [Description("True when at least one date/time was found in the text.")]
        public OutArgument<bool> HasMatches { get; set; }

        /// <summary>The same matches as a <see cref="DataTable"/>, for use with "For Each Row".</summary>
        [Category("Output")]
        [DisplayName("Matches (Table)")]
        [Description("The same matches as a DataTable for easy use with \"For Each Row\". Columns: Text, Subtype, Value, RangeStart, RangeEnd, Duration, Timex, StartIndex, Length.")]
        public OutArgument<DataTable> MatchesTable { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var text = GetText(context);
            var cultureCode = GetCultureCode(context);
            var reference = GetReferenceTime(context);

            var matches = DateTimeRecognition.Recognize(text, cultureCode, reference);

            Matches.Set(context, new List<DateTimeRecognitionResult>(matches));
            HasMatches.Set(context, matches.Count > 0);
            MatchesTable.Set(context, DateTimeRecognition.ToDataTable(matches));
        }
    }
}
