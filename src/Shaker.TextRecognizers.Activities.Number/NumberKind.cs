namespace TextRecognizers.Numbers
{
    /// <summary>
    /// What kind of numeric value to recognize. Shown as a drop-down so the user picks the
    /// behaviour rather than guessing which activity to use.
    /// </summary>
    public enum NumberKind
    {
        /// <summary>Plain numbers, including decimals and fractions, e.g. "3", "1.5", "two and a half".</summary>
        Number = 0,

        /// <summary>Ordinals, e.g. "1st", "second", "third".</summary>
        Ordinal,

        /// <summary>Percentages, e.g. "50%", "fifty percent".</summary>
        Percentage,
    }
}
