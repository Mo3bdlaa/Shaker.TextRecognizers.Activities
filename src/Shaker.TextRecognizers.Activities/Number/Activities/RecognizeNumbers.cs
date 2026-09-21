using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace TextRecognizers.Numbers
{
    /// <summary>
    /// Finds every number (or ordinal / percentage, per <see cref="NumberActivityBase.Kind"/>)
    /// in a piece of text and returns them as a list of ready-to-use results.
    /// </summary>
    [Category("TextRecognizers.Numbers")]
    [DisplayName("Recognize Numbers")]
    [Description("Finds every number (or ordinal / percentage) in a piece of text and returns them as a list of ready-to-use results.")]
    public sealed class RecognizeNumbers : NumberActivityBase
    {
        /// <summary>All numeric values found, in order of appearance.</summary>
        [Category("Output")]
        [DisplayName("Matches")]
        [Description("All numeric values found in the text, in order of appearance.")]
        public OutArgument<List<NumberRecognitionResult>> Matches { get; set; }

        /// <summary>True when at least one number was found.</summary>
        [Category("Output")]
        [DisplayName("Has Matches")]
        [Description("True when at least one number was found in the text.")]
        public OutArgument<bool> HasMatches { get; set; }

        /// <summary>The same matches as a <see cref="DataTable"/>, for "For Each Row".</summary>
        [Category("Output")]
        [DisplayName("Matches (Table)")]
        [Description("The same matches as a DataTable for easy use with \"For Each Row\". Columns: Text, Kind, Value, RawValue, StartIndex, Length.")]
        public OutArgument<DataTable> MatchesTable { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var matches = NumberRecognition.Recognize(GetText(context), GetCultureCode(context), GetKind(context));

            Matches.Set(context, new List<NumberRecognitionResult>(matches));
            HasMatches.Set(context, matches.Count > 0);
            MatchesTable.Set(context, NumberRecognition.ToDataTable(matches));
        }
    }
}
