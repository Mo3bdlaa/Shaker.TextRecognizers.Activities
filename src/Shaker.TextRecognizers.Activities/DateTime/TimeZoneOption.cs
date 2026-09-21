using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace TextRecognizers.DateTimes
{
    /// <summary>
    /// The time zone whose current time anchors relative phrases such as "tomorrow"
    /// when an activity's <c>Reference Time</c> input is left empty.
    /// </summary>
    /// <remarks>
    /// Exposed as an <see langword="enum"/> so Studio's property panel renders it as a
    /// drop-down list instead of asking the user to type a raw zone id.
    /// <para>
    /// Each entry is a real geographic zone, not a fixed offset, so daylight saving time is
    /// applied automatically: <see cref="UtcPlus0000_London"/> anchors at UTC+00:00 in winter
    /// and UTC+01:00 in summer. The offset shown in each name is the zone's <em>standard</em>
    /// (winter) offset, which is the same convention Windows uses in its own zone names.
    /// </para>
    /// </remarks>
    [TypeConverter(typeof(TimeZoneOptionConverter))]
    public enum TimeZoneOption
    {
        /// <summary>The time zone this machine is configured to use. This is the default.</summary>
        SystemDefault = 0,

        /// <summary>Coordinated Universal Time; never shifts for daylight saving.</summary>
        Utc,

        /// <summary>(UTC-11:00) Midway.</summary>
        UtcMinus1100_Midway,
        /// <summary>(UTC-10:00) Honolulu.</summary>
        UtcMinus1000_Honolulu,
        /// <summary>(UTC-09:30) Marquesas.</summary>
        UtcMinus0930_Marquesas,
        /// <summary>(UTC-09:00) Anchorage.</summary>
        UtcMinus0900_Anchorage,
        /// <summary>(UTC-08:00) Los Angeles.</summary>
        UtcMinus0800_LosAngeles,
        /// <summary>(UTC-07:00) Denver.</summary>
        UtcMinus0700_Denver,
        /// <summary>(UTC-07:00) Phoenix; no daylight saving.</summary>
        UtcMinus0700_Phoenix,
        /// <summary>(UTC-06:00) Chicago.</summary>
        UtcMinus0600_Chicago,
        /// <summary>(UTC-06:00) Mexico City.</summary>
        UtcMinus0600_MexicoCity,
        /// <summary>(UTC-05:00) New York.</summary>
        UtcMinus0500_NewYork,
        /// <summary>(UTC-05:00) Bogota.</summary>
        UtcMinus0500_Bogota,
        /// <summary>(UTC-04:00) Halifax.</summary>
        UtcMinus0400_Halifax,
        /// <summary>(UTC-04:00) Santiago.</summary>
        UtcMinus0400_Santiago,
        /// <summary>(UTC-03:30) St John's.</summary>
        UtcMinus0330_StJohns,
        /// <summary>(UTC-03:00) Sao Paulo.</summary>
        UtcMinus0300_SaoPaulo,
        /// <summary>(UTC-03:00) Buenos Aires.</summary>
        UtcMinus0300_BuenosAires,
        /// <summary>(UTC-02:00) Fernando de Noronha.</summary>
        UtcMinus0200_Noronha,
        /// <summary>(UTC-01:00) Azores.</summary>
        UtcMinus0100_Azores,
        /// <summary>(UTC+00:00) London.</summary>
        UtcPlus0000_London,
        /// <summary>(UTC+01:00) Berlin.</summary>
        UtcPlus0100_Berlin,
        /// <summary>(UTC+01:00) Paris.</summary>
        UtcPlus0100_Paris,
        /// <summary>(UTC+01:00) Madrid.</summary>
        UtcPlus0100_Madrid,
        /// <summary>(UTC+01:00) Lagos; no daylight saving.</summary>
        UtcPlus0100_Lagos,
        /// <summary>(UTC+02:00) Cairo.</summary>
        UtcPlus0200_Cairo,
        /// <summary>(UTC+02:00) Athens.</summary>
        UtcPlus0200_Athens,
        /// <summary>(UTC+02:00) Bucharest.</summary>
        UtcPlus0200_Bucharest,
        /// <summary>(UTC+02:00) Jerusalem.</summary>
        UtcPlus0200_Jerusalem,
        /// <summary>(UTC+02:00) Johannesburg; no daylight saving.</summary>
        UtcPlus0200_Johannesburg,
        /// <summary>(UTC+03:00) Riyadh; no daylight saving.</summary>
        UtcPlus0300_Riyadh,
        /// <summary>(UTC+03:00) Baghdad.</summary>
        UtcPlus0300_Baghdad,
        /// <summary>(UTC+03:00) Moscow; no daylight saving.</summary>
        UtcPlus0300_Moscow,
        /// <summary>(UTC+03:00) Nairobi; no daylight saving.</summary>
        UtcPlus0300_Nairobi,
        /// <summary>(UTC+03:30) Tehran.</summary>
        UtcPlus0330_Tehran,
        /// <summary>(UTC+04:00) Dubai; no daylight saving.</summary>
        UtcPlus0400_Dubai,
        /// <summary>(UTC+04:00) Baku.</summary>
        UtcPlus0400_Baku,
        /// <summary>(UTC+04:30) Kabul.</summary>
        UtcPlus0430_Kabul,
        /// <summary>(UTC+05:00) Karachi.</summary>
        UtcPlus0500_Karachi,
        /// <summary>(UTC+05:00) Tashkent.</summary>
        UtcPlus0500_Tashkent,
        /// <summary>(UTC+05:30) Kolkata.</summary>
        UtcPlus0530_Kolkata,
        /// <summary>(UTC+05:30) Colombo.</summary>
        UtcPlus0530_Colombo,
        /// <summary>(UTC+05:45) Kathmandu.</summary>
        UtcPlus0545_Kathmandu,
        /// <summary>(UTC+06:00) Dhaka.</summary>
        UtcPlus0600_Dhaka,
        /// <summary>(UTC+06:00) Almaty.</summary>
        UtcPlus0600_Almaty,
        /// <summary>(UTC+06:30) Yangon.</summary>
        UtcPlus0630_Yangon,
        /// <summary>(UTC+07:00) Bangkok.</summary>
        UtcPlus0700_Bangkok,
        /// <summary>(UTC+07:00) Jakarta.</summary>
        UtcPlus0700_Jakarta,
        /// <summary>(UTC+08:00) Shanghai.</summary>
        UtcPlus0800_Shanghai,
        /// <summary>(UTC+08:00) Hong Kong.</summary>
        UtcPlus0800_HongKong,
        /// <summary>(UTC+08:00) Singapore.</summary>
        UtcPlus0800_Singapore,
        /// <summary>(UTC+08:00) Perth.</summary>
        UtcPlus0800_Perth,
        /// <summary>(UTC+08:45) Eucla.</summary>
        UtcPlus0845_Eucla,
        /// <summary>(UTC+09:00) Tokyo.</summary>
        UtcPlus0900_Tokyo,
        /// <summary>(UTC+09:00) Seoul.</summary>
        UtcPlus0900_Seoul,
        /// <summary>(UTC+09:30) Adelaide.</summary>
        UtcPlus0930_Adelaide,
        /// <summary>(UTC+09:30) Darwin; no daylight saving.</summary>
        UtcPlus0930_Darwin,
        /// <summary>(UTC+10:00) Sydney.</summary>
        UtcPlus1000_Sydney,
        /// <summary>(UTC+10:00) Brisbane; no daylight saving.</summary>
        UtcPlus1000_Brisbane,
        /// <summary>(UTC+10:30) Lord Howe.</summary>
        UtcPlus1030_LordHowe,
        /// <summary>(UTC+11:00) Noumea.</summary>
        UtcPlus1100_Noumea,
        /// <summary>(UTC+12:00) Auckland.</summary>
        UtcPlus1200_Auckland,
        /// <summary>(UTC+12:00) Fiji.</summary>
        UtcPlus1200_Fiji,
        /// <summary>(UTC+12:45) Chatham.</summary>
        UtcPlus1245_Chatham,
        /// <summary>(UTC+13:00) Nuku'alofa.</summary>
        UtcPlus1300_Nukualofa,
        /// <summary>(UTC+14:00) Kiritimati.</summary>
        UtcPlus1400_Kiritimati,
    }
}
