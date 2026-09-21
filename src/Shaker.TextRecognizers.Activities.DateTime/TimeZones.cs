using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TextRecognizers.DateTimes
{
    /// <summary>
    /// Translates the friendly <see cref="TimeZoneOption"/> drop-down value into a real
    /// <see cref="TimeZoneInfo"/> and reads the current wall-clock time in that zone.
    /// </summary>
    /// <remarks>
    /// Zones are identified by their IANA id ("Africa/Cairo"). Since .NET 6,
    /// <see cref="TimeZoneInfo.FindSystemTimeZoneById(string)"/> accepts IANA ids on Windows
    /// too - it converts to the Windows id itself - so one id works on every platform a
    /// robot might run on.
    /// </remarks>
    public static class TimeZones
    {
        // Each entry is the zone's IANA id plus the label shown in the Studio drop-down.
        // The offset in the label is the zone's STANDARD (winter) offset; the actual offset
        // used at run time comes from TimeZoneInfo and follows daylight saving.
        private static readonly IReadOnlyDictionary<TimeZoneOption, (string Id, string Label)> Map =
            new Dictionary<TimeZoneOption, (string, string)>
            {
                [TimeZoneOption.SystemDefault] = (null!, "System Default"),
                [TimeZoneOption.Utc] = ("Etc/UTC", "(UTC+00:00) UTC"),

                [TimeZoneOption.UtcMinus1100_Midway] = ("Pacific/Pago_Pago", "(UTC-11:00) Midway"),
                [TimeZoneOption.UtcMinus1000_Honolulu] = ("Pacific/Honolulu", "(UTC-10:00) Honolulu"),
                [TimeZoneOption.UtcMinus0930_Marquesas] = ("Pacific/Marquesas", "(UTC-09:30) Marquesas"),
                [TimeZoneOption.UtcMinus0900_Anchorage] = ("America/Anchorage", "(UTC-09:00) Anchorage"),
                [TimeZoneOption.UtcMinus0800_LosAngeles] = ("America/Los_Angeles", "(UTC-08:00) Los Angeles"),
                [TimeZoneOption.UtcMinus0700_Denver] = ("America/Denver", "(UTC-07:00) Denver"),
                [TimeZoneOption.UtcMinus0700_Phoenix] = ("America/Phoenix", "(UTC-07:00) Phoenix"),
                [TimeZoneOption.UtcMinus0600_Chicago] = ("America/Chicago", "(UTC-06:00) Chicago"),
                [TimeZoneOption.UtcMinus0600_MexicoCity] = ("America/Mexico_City", "(UTC-06:00) Mexico City"),
                [TimeZoneOption.UtcMinus0500_NewYork] = ("America/New_York", "(UTC-05:00) New York"),
                [TimeZoneOption.UtcMinus0500_Bogota] = ("America/Bogota", "(UTC-05:00) Bogota"),
                [TimeZoneOption.UtcMinus0400_Halifax] = ("America/Halifax", "(UTC-04:00) Halifax"),
                [TimeZoneOption.UtcMinus0400_Santiago] = ("America/Santiago", "(UTC-04:00) Santiago"),
                [TimeZoneOption.UtcMinus0330_StJohns] = ("America/St_Johns", "(UTC-03:30) St John's"),
                [TimeZoneOption.UtcMinus0300_SaoPaulo] = ("America/Sao_Paulo", "(UTC-03:00) Sao Paulo"),
                [TimeZoneOption.UtcMinus0300_BuenosAires] = ("America/Argentina/Buenos_Aires", "(UTC-03:00) Buenos Aires"),
                [TimeZoneOption.UtcMinus0200_Noronha] = ("America/Noronha", "(UTC-02:00) Fernando de Noronha"),
                [TimeZoneOption.UtcMinus0100_Azores] = ("Atlantic/Azores", "(UTC-01:00) Azores"),
                [TimeZoneOption.UtcPlus0000_London] = ("Europe/London", "(UTC+00:00) London"),
                [TimeZoneOption.UtcPlus0100_Berlin] = ("Europe/Berlin", "(UTC+01:00) Berlin"),
                [TimeZoneOption.UtcPlus0100_Paris] = ("Europe/Paris", "(UTC+01:00) Paris"),
                [TimeZoneOption.UtcPlus0100_Madrid] = ("Europe/Madrid", "(UTC+01:00) Madrid"),
                [TimeZoneOption.UtcPlus0100_Lagos] = ("Africa/Lagos", "(UTC+01:00) Lagos"),
                [TimeZoneOption.UtcPlus0200_Cairo] = ("Africa/Cairo", "(UTC+02:00) Cairo"),
                [TimeZoneOption.UtcPlus0200_Athens] = ("Europe/Athens", "(UTC+02:00) Athens"),
                [TimeZoneOption.UtcPlus0200_Bucharest] = ("Europe/Bucharest", "(UTC+02:00) Bucharest"),
                [TimeZoneOption.UtcPlus0200_Jerusalem] = ("Asia/Jerusalem", "(UTC+02:00) Jerusalem"),
                [TimeZoneOption.UtcPlus0200_Johannesburg] = ("Africa/Johannesburg", "(UTC+02:00) Johannesburg"),
                [TimeZoneOption.UtcPlus0300_Riyadh] = ("Asia/Riyadh", "(UTC+03:00) Riyadh"),
                [TimeZoneOption.UtcPlus0300_Baghdad] = ("Asia/Baghdad", "(UTC+03:00) Baghdad"),
                [TimeZoneOption.UtcPlus0300_Moscow] = ("Europe/Moscow", "(UTC+03:00) Moscow"),
                [TimeZoneOption.UtcPlus0300_Nairobi] = ("Africa/Nairobi", "(UTC+03:00) Nairobi"),
                [TimeZoneOption.UtcPlus0330_Tehran] = ("Asia/Tehran", "(UTC+03:30) Tehran"),
                [TimeZoneOption.UtcPlus0400_Dubai] = ("Asia/Dubai", "(UTC+04:00) Dubai"),
                [TimeZoneOption.UtcPlus0400_Baku] = ("Asia/Baku", "(UTC+04:00) Baku"),
                [TimeZoneOption.UtcPlus0430_Kabul] = ("Asia/Kabul", "(UTC+04:30) Kabul"),
                [TimeZoneOption.UtcPlus0500_Karachi] = ("Asia/Karachi", "(UTC+05:00) Karachi"),
                [TimeZoneOption.UtcPlus0500_Tashkent] = ("Asia/Tashkent", "(UTC+05:00) Tashkent"),
                [TimeZoneOption.UtcPlus0530_Kolkata] = ("Asia/Kolkata", "(UTC+05:30) Kolkata"),
                [TimeZoneOption.UtcPlus0530_Colombo] = ("Asia/Colombo", "(UTC+05:30) Colombo"),
                [TimeZoneOption.UtcPlus0545_Kathmandu] = ("Asia/Kathmandu", "(UTC+05:45) Kathmandu"),
                [TimeZoneOption.UtcPlus0600_Dhaka] = ("Asia/Dhaka", "(UTC+06:00) Dhaka"),
                [TimeZoneOption.UtcPlus0600_Almaty] = ("Asia/Almaty", "(UTC+06:00) Almaty"),
                [TimeZoneOption.UtcPlus0630_Yangon] = ("Asia/Yangon", "(UTC+06:30) Yangon"),
                [TimeZoneOption.UtcPlus0700_Bangkok] = ("Asia/Bangkok", "(UTC+07:00) Bangkok"),
                [TimeZoneOption.UtcPlus0700_Jakarta] = ("Asia/Jakarta", "(UTC+07:00) Jakarta"),
                [TimeZoneOption.UtcPlus0800_Shanghai] = ("Asia/Shanghai", "(UTC+08:00) Shanghai"),
                [TimeZoneOption.UtcPlus0800_HongKong] = ("Asia/Hong_Kong", "(UTC+08:00) Hong Kong"),
                [TimeZoneOption.UtcPlus0800_Singapore] = ("Asia/Singapore", "(UTC+08:00) Singapore"),
                [TimeZoneOption.UtcPlus0800_Perth] = ("Australia/Perth", "(UTC+08:00) Perth"),
                [TimeZoneOption.UtcPlus0845_Eucla] = ("Australia/Eucla", "(UTC+08:45) Eucla"),
                [TimeZoneOption.UtcPlus0900_Tokyo] = ("Asia/Tokyo", "(UTC+09:00) Tokyo"),
                [TimeZoneOption.UtcPlus0900_Seoul] = ("Asia/Seoul", "(UTC+09:00) Seoul"),
                [TimeZoneOption.UtcPlus0930_Adelaide] = ("Australia/Adelaide", "(UTC+09:30) Adelaide"),
                [TimeZoneOption.UtcPlus0930_Darwin] = ("Australia/Darwin", "(UTC+09:30) Darwin"),
                [TimeZoneOption.UtcPlus1000_Sydney] = ("Australia/Sydney", "(UTC+10:00) Sydney"),
                [TimeZoneOption.UtcPlus1000_Brisbane] = ("Australia/Brisbane", "(UTC+10:00) Brisbane"),
                [TimeZoneOption.UtcPlus1030_LordHowe] = ("Australia/Lord_Howe", "(UTC+10:30) Lord Howe"),
                [TimeZoneOption.UtcPlus1100_Noumea] = ("Pacific/Noumea", "(UTC+11:00) Noumea"),
                [TimeZoneOption.UtcPlus1200_Auckland] = ("Pacific/Auckland", "(UTC+12:00) Auckland"),
                [TimeZoneOption.UtcPlus1200_Fiji] = ("Pacific/Fiji", "(UTC+12:00) Fiji"),
                [TimeZoneOption.UtcPlus1245_Chatham] = ("Pacific/Chatham", "(UTC+12:45) Chatham"),
                [TimeZoneOption.UtcPlus1300_Nukualofa] = ("Pacific/Tongatapu", "(UTC+13:00) Nuku'alofa"),
                [TimeZoneOption.UtcPlus1400_Kiritimati] = ("Pacific/Kiritimati", "(UTC+14:00) Kiritimati"),
            };

        // Looking a zone up walks the machine's zone database, so cache the resolved objects.
        private static readonly ConcurrentDictionary<TimeZoneOption, TimeZoneInfo> ZoneCache =
            new ConcurrentDictionary<TimeZoneOption, TimeZoneInfo>();

        /// <summary>
        /// The label shown for <paramref name="option"/> in Studio's drop-down, e.g.
        /// <c>"(UTC+02:00) Cairo"</c>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if a new enum value is added without a matching entry in the map.
        /// </exception>
        public static string GetDisplayName(TimeZoneOption option) =>
            Map.TryGetValue(option, out var entry)
                ? entry.Label
                : throw new ArgumentOutOfRangeException(
                    nameof(option), option, "No time zone is mapped for this TimeZoneOption value.");

        /// <summary>Every option paired with its drop-down label, in declaration order.</summary>
        public static IEnumerable<KeyValuePair<TimeZoneOption, string>> GetDisplayNames()
        {
            foreach (var entry in Map)
                yield return new KeyValuePair<TimeZoneOption, string>(entry.Key, entry.Value.Label);
        }

        /// <summary>
        /// Resolves <paramref name="option"/> to a <see cref="TimeZoneInfo"/>.
        /// <see cref="TimeZoneOption.SystemDefault"/> yields <see cref="TimeZoneInfo.Local"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// Thrown when the machine's time zone database does not know the zone.
        /// </exception>
        public static TimeZoneInfo GetTimeZone(TimeZoneOption option)
        {
            if (option == TimeZoneOption.SystemDefault)
                return TimeZoneInfo.Local;

            return ZoneCache.GetOrAdd(option, key =>
            {
                if (!Map.TryGetValue(key, out var entry))
                    throw new ArgumentOutOfRangeException(
                        nameof(option), key, "No time zone is mapped for this TimeZoneOption value.");

                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById(entry.Id);
                }
                catch (Exception ex)
                {
                    // Either the machine's zone database is missing the zone, or the process
                    // is running in globalization-invariant mode (where IANA ids cannot be
                    // converted). Both are environment problems, so say so plainly.
                    throw new NotSupportedException(
                        $"This machine's time zone database does not have '{entry.Id}' ({entry.Label}). " +
                        "Pick a different time zone, or leave Time Zone on System Default.", ex);
                }
            });
        }

        /// <summary>
        /// The wall-clock time in <paramref name="option"/>'s zone at the instant
        /// <paramref name="utcNow"/>. Kept separate from <see cref="Now"/> so the conversion
        /// is a pure function and can be tested against a fixed instant.
        /// </summary>
        public static DateTime At(DateTime utcNow, TimeZoneOption option)
        {
            if (option == TimeZoneOption.Utc)
                return DateTime.SpecifyKind(utcNow, DateTimeKind.Unspecified);

            // ConvertTimeFromUtc returns an Unspecified DateTime - a plain wall-clock reading,
            // which is exactly what the recognizer anchors relative phrases against.
            return TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.SpecifyKind(utcNow, DateTimeKind.Utc), GetTimeZone(option));
        }

        /// <summary>The current wall-clock time in <paramref name="option"/>'s zone.</summary>
        public static DateTime Now(TimeZoneOption option) =>
            option == TimeZoneOption.SystemDefault ? DateTime.Now : At(DateTime.UtcNow, option);
    }
}
