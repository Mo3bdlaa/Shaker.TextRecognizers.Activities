namespace TextRecognizers.Numbers
{
    /// <summary>
    /// One numeric mention found in text, with the recognizer's resolution already turned
    /// into a ready-to-use <see cref="double"/>.
    /// </summary>
    public sealed class NumberRecognitionResult
    {
        /// <summary>The exact substring that was recognised, e.g. "fifty percent".</summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>Zero-based index of <see cref="Text"/> within the original input.</summary>
        public int StartIndex { get; set; }

        /// <summary>Length of <see cref="Text"/> in characters.</summary>
        public int Length { get; set; }

        /// <summary>Whether this is a plain number, an ordinal or a percentage.</summary>
        public NumberKind Kind { get; set; }

        /// <summary>
        /// The numeric value. For percentages this is the percentage itself
        /// (e.g. "50%" → <c>50</c>). <see cref="double.NaN"/> if the value could not be parsed.
        /// </summary>
        public double Value { get; set; }

        /// <summary>The raw resolution string from the recognizer (e.g. "50%", "0.5", "3").</summary>
        public string RawValue { get; set; } = string.Empty;
    }
}
