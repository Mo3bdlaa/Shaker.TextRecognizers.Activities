using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace TextRecognizers.Units
{
    /// <summary>
    /// Finds every measurement of the selected <see cref="MeasurementActivityBase.Kind"/>
    /// (currency, temperature, age or dimension) in a piece of text.
    /// </summary>
    [Category("TextRecognizers.Measurements")]
    [DisplayName("Recognize Measurements")]
    [Description("Finds every measurement (currency, temperature, age or dimension) in text and returns its value + unit.")]
    public sealed class RecognizeMeasurements : MeasurementActivityBase
    {
        /// <summary>All measurements found, in order of appearance.</summary>
        [Category("Output")]
        [DisplayName("Matches")]
        [Description("All measurements found in the text, in order of appearance.")]
        public OutArgument<List<MeasurementResult>> Matches { get; set; }

        /// <summary>True when at least one measurement was found.</summary>
        [Category("Output")]
        [DisplayName("Has Matches")]
        [Description("True when at least one measurement was found in the text.")]
        public OutArgument<bool> HasMatches { get; set; }

        /// <summary>The same matches as a <see cref="DataTable"/>, for "For Each Row".</summary>
        [Category("Output")]
        [DisplayName("Matches (Table)")]
        [Description("The same matches as a DataTable. Columns: Text, Kind, Value, Unit, StartIndex, Length.")]
        public OutArgument<DataTable> MatchesTable { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var matches = UnitRecognition.Recognize(GetText(context), GetCultureCode(context), GetKind(context));

            Matches.Set(context, new List<MeasurementResult>(matches));
            HasMatches.Set(context, matches.Count > 0);
            MatchesTable.Set(context, UnitRecognition.ToDataTable(matches));
        }
    }
}
