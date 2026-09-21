using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using TextRecognizers.Core;

namespace TextRecognizers.Choices
{
    /// <summary>
    /// Finds every yes/no answer in a piece of text and returns them with confidence scores.
    /// </summary>
    [Category("TextRecognizers.Choices")]
    [DisplayName("Recognize Booleans")]
    [Description("Finds every yes/no answer in a piece of text and returns each as a boolean with a confidence score.")]
    public sealed class RecognizeBooleans : RecognizerActivity
    {
        /// <summary>All boolean answers found, in order of appearance.</summary>
        [Category("Output")]
        [DisplayName("Matches")]
        [Description("All boolean answers found in the text, in order of appearance.")]
        public OutArgument<List<BooleanResult>> Matches { get; set; }

        /// <summary>True when at least one answer was found.</summary>
        [Category("Output")]
        [DisplayName("Has Matches")]
        [Description("True when at least one yes/no answer was found in the text.")]
        public OutArgument<bool> HasMatches { get; set; }

        /// <summary>The same matches as a <see cref="DataTable"/>, for "For Each Row".</summary>
        [Category("Output")]
        [DisplayName("Matches (Table)")]
        [Description("The same matches as a DataTable. Columns: Text, Value, Score, StartIndex, Length.")]
        public OutArgument<DataTable> MatchesTable { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var matches = ChoiceRecognition.Recognize(GetText(context), GetCultureCode(context));

            Matches.Set(context, new List<BooleanResult>(matches));
            HasMatches.Set(context, matches.Count > 0);
            MatchesTable.Set(context, ChoiceRecognition.ToDataTable(matches));
        }
    }
}
