namespace TextRecognizers.Units
{
    /// <summary>
    /// What kind of measurement to recognize. Shown as a drop-down so the user picks the
    /// behaviour rather than guessing which activity to use.
    /// </summary>
    public enum MeasurementKind
    {
        /// <summary>Money amounts, e.g. "$10", "20 euros".</summary>
        Currency = 0,

        /// <summary>Temperatures, e.g. "20 degrees Celsius", "98.6 F".</summary>
        Temperature,

        /// <summary>Ages, e.g. "25 years old".</summary>
        Age,

        /// <summary>Dimensions - length, weight, speed, volume, etc., e.g. "3 km", "5 kg".</summary>
        Dimension,
    }
}
