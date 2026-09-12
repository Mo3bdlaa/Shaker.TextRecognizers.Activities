namespace TextRecognizers.Sequences
{
    /// <summary>
    /// One sequence (email, phone number, URL, …) found in text, with its normalised value.
    /// </summary>
    public sealed class SequenceResult
    {
        /// <summary>The exact substring that was recognised.</summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>Zero-based index of <see cref="Text"/> within the original input.</summary>
        public int StartIndex { get; set; }

        /// <summary>Length of <see cref="Text"/> in characters.</summary>
        public int Length { get; set; }

        /// <summary>Which kind of sequence this is.</summary>
        public SequenceKind Kind { get; set; }

        /// <summary>
        /// The normalised value from the recognizer (e.g. the canonical phone number). Falls
        /// back to the matched <see cref="Text"/> when the recognizer provides no separate value.
        /// </summary>
        public string Value { get; set; } = string.Empty;
    }
}
