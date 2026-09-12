using System.Activities;
using System.ComponentModel;

namespace TextRecognizers.Units
{
    /// <summary>
    /// Extracts the single best measurement (currency, temperature, age or dimension) from a
    /// piece of text, returning the numeric value and its unit separately.
    /// </summary>
    [Category("TextRecognizers.Measurements")]
    [DisplayName("Parse Measurement")]
    [Description("Extracts the single best measurement from text, returning its Value and Unit, with a Success flag.")]
    public sealed class ParseMeasurement : MeasurementActivityBase
    {
        /// <summary>True when at least one measurement was found.</summary>
        [Category("Output")]
        [DisplayName("Success")]
        [Description("True when at least one measurement was found in the text.")]
        public OutArgument<bool> Success { get; set; }

        /// <summary>The numeric value of the best match (0 when nothing was found).</summary>
        [Category("Output")]
        [DisplayName("Value")]
        [Description("The numeric value of the best match. 0 when nothing was found.")]
        public OutArgument<double> Value { get; set; }

        /// <summary>The unit of the best match (e.g. "Dollar", "Celsius").</summary>
        [Category("Output")]
        [DisplayName("Unit")]
        [Description("The unit of the best match, e.g. \"Dollar\", \"Celsius\", \"Year\", \"Kilometer\".")]
        public OutArgument<string> Unit { get; set; }

        /// <summary>The full best match, or <see langword="null"/> when nothing was found.</summary>
        [Category("Output")]
        [DisplayName("Result")]
        [Description("The full best match (text, kind, value, unit, indices). Null when nothing was found.")]
        public OutArgument<MeasurementResult> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var matches = UnitRecognition.Recognize(GetText(context), GetCultureCode(context), GetKind(context));
            var best = matches.Count > 0 ? matches[0] : null;

            Success.Set(context, best != null);
            Value.Set(context, best?.Value ?? 0d);
            Unit.Set(context, best?.Unit ?? string.Empty);
            Result.Set(context, best!);
        }
    }
}
