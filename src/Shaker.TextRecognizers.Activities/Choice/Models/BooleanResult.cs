namespace TextRecognizers.Choices
{
    /// <summary>
    /// One boolean (yes/no) answer found in text, with the recognizer's confidence score.
    /// </summary>
    public sealed class BooleanResult
    {
        /// <summary>The exact substring that was recognised, e.g. "absolutely".</summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>Zero-based index of <see cref="Text"/> within the original input.</summary>
        public int StartIndex { get; set; }

        /// <summary>Length of <see cref="Text"/> in characters.</summary>
        public int Length { get; set; }

        /// <summary>The boolean value: <see langword="true"/> for yes, <see langword="false"/> for no.</summary>
        public bool Value { get; set; }

        /// <summary>The recognizer's confidence in this answer, from 0 to 1.</summary>
        public double Score { get; set; }
    }
}
