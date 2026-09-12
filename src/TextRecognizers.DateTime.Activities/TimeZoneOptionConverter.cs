using System;
using System.ComponentModel;
using System.Globalization;

namespace TextRecognizers.DateTimes
{
    /// <summary>
    /// Renders <see cref="TimeZoneOption"/> values as their friendly labels - so the Studio
    /// drop-down reads "(UTC+02:00) Cairo" rather than the bare enum name.
    /// </summary>
    /// <remarks>
    /// Applied to the enum through a <see cref="TypeConverterAttribute"/>, so any property
    /// panel that honours type converters picks it up with no extra wiring. A host that
    /// ignores converters simply falls back to the enum member names, which carry the same
    /// offset and city - just less prettily.
    /// </remarks>
    public sealed class TimeZoneOptionConverter : EnumConverter
    {
        /// <summary>Creates the converter for <see cref="TimeZoneOption"/>.</summary>
        public TimeZoneOptionConverter() : base(typeof(TimeZoneOption))
        {
        }

        /// <summary>Converts an option to its drop-down label.</summary>
        public override object? ConvertTo(
            ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is TimeZoneOption option)
                return TimeZones.GetDisplayName(option);

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>Converts a drop-down label (or a plain enum name) back to an option.</summary>
        public override object? ConvertFrom(
            ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string text)
            {
                foreach (var entry in TimeZones.GetDisplayNames())
                {
                    if (string.Equals(entry.Value, text, StringComparison.OrdinalIgnoreCase))
                        return entry.Key;
                }
            }

            // Not one of our labels - let the base converter try the enum member name, so a
            // value saved before this converter existed still round-trips.
            return base.ConvertFrom(context, culture, value);
        }
    }
}
