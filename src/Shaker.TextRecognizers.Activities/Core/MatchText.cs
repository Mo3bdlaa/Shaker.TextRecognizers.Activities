using System;

namespace TextRecognizers.Core
{
    /// <summary>
    /// Recovers the matched text exactly as it appears in the input.
    /// </summary>
    /// <remarks>
    /// The recognizers lower-case the <c>Text</c> they hand back - "Monday" comes out as
    /// "monday" - but the <c>Start</c> and <c>End</c> offsets they report are exact. Slicing
    /// the original input at those offsets gives the substring back with its own casing and
    /// spacing, without second-guessing what the engine matched.
    /// </remarks>
    internal static class MatchText
    {
        /// <summary>
        /// The substring of <paramref name="source"/> between the recognizer's
        /// <paramref name="start"/> and <paramref name="end"/> offsets, both inclusive.
        /// Returns an empty string when the offsets do not sit inside the input.
        /// </summary>
        public static string Slice(string source, int start, int end)
        {
            if (string.IsNullOrEmpty(source) || start < 0 || start >= source.Length || end < start)
                return string.Empty;

            // End is the index of the last matched character, so the length is one more than
            // the difference. Clamp in case a recognizer ever reports past the end.
            var length = Math.Min(end - start + 1, source.Length - start);
            return source.Substring(start, length);
        }
    }
}
