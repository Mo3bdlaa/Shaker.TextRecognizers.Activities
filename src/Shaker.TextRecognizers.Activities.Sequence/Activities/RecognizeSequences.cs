using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace TextRecognizers.Sequences
{
    /// <summary>
    /// Finds every sequence of the selected <see cref="SequenceActivityBase.Kind"/>
    /// (email, phone, URL, …) in a piece of text.
    /// </summary>
    [Category("TextRecognizers.Sequences")]
    [DisplayName("Recognize Sequences")]
    [Description("Finds every email / phone / URL / IP / GUID / hashtag / mention (per Kind) in a piece of text.")]
    public sealed class RecognizeSequences : SequenceActivityBase
    {
        /// <summary>All sequences found, in order of appearance.</summary>
        [Category("Output")]
        [DisplayName("Matches")]
        [Description("All sequences found in the text, in order of appearance.")]
        public OutArgument<List<SequenceResult>> Matches { get; set; }

        /// <summary>True when at least one sequence was found.</summary>
        [Category("Output")]
        [DisplayName("Has Matches")]
        [Description("True when at least one sequence was found in the text.")]
        public OutArgument<bool> HasMatches { get; set; }

        /// <summary>The same matches as a <see cref="DataTable"/>, for "For Each Row".</summary>
        [Category("Output")]
        [DisplayName("Matches (Table)")]
        [Description("The same matches as a DataTable. Columns: Text, Kind, Value, StartIndex, Length.")]
        public OutArgument<DataTable> MatchesTable { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var matches = SequenceRecognition.Recognize(GetText(context), GetCultureCode(context), GetKind(context));

            Matches.Set(context, new List<SequenceResult>(matches));
            HasMatches.Set(context, matches.Count > 0);
            MatchesTable.Set(context, SequenceRecognition.ToDataTable(matches));
        }
    }
}
