namespace TextRecognizers.Units
{
    /// <summary>
    /// One measurement found in text - a numeric <see cref="Value"/> together with its
    /// <see cref="Unit"/> (e.g. 10 + "Dollar", 20 + "Celsius").
    /// </summary>
    public sealed class MeasurementResult
    {
        /// <summary>The exact substring that was recognised, e.g. "20 degrees Celsius".</summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>Zero-based index of <see cref="Text"/> within the original input.</summary>
        public int StartIndex { get; set; }

        /// <summary>Length of <see cref="Text"/> in characters.</summary>
        public int Length { get; set; }

        /// <summary>Whether this is currency, temperature, age or a dimension.</summary>
        public MeasurementKind Kind { get; set; }

        /// <summary>The numeric value. <see cref="double.NaN"/> if it could not be parsed.</summary>
        public double Value { get; set; }

        /// <summary>The unit, as named by the recognizer (e.g. "Dollar", "Celsius", "Year", "Kilometer").</summary>
        public string Unit { get; set; } = string.Empty;
    }
}
