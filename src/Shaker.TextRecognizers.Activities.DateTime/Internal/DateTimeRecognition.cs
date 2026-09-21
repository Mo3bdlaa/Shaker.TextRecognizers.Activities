using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;

namespace TextRecognizers.DateTimes
{
    /// <summary>
    /// The engine behind the DateTime activities: it runs Microsoft.Recognizers and
    /// maps each raw <see cref="ModelResult"/> (whose <c>Resolution</c> is a loosely
    /// typed dictionary) into a strongly typed <see cref="DateTimeRecognitionResult"/>.
    /// Kept internal so the activities present the only public surface.
    /// </summary>
    internal static class DateTimeRecognition
    {
        // DateTime models are safe to reuse across calls and relatively expensive to
        // build, so we cache one per language for the lifetime of the process.
        private static readonly ConcurrentDictionary<string, DateTimeModel> ModelCache =
            new ConcurrentDictionary<string, DateTimeModel>();

        /// <summary>
        /// Finds every date/time mention in <paramref name="text"/>, resolving relative
        /// phrases (e.g. "tomorrow") against <paramref name="reference"/>.
        /// </summary>
        public static IList<DateTimeRecognitionResult> Recognize(string text, string cultureCode, DateTime reference)
        {
            var results = new List<DateTimeRecognitionResult>();
            if (string.IsNullOrWhiteSpace(text))
                return results;

            var model = GetModel(cultureCode);

            // Parse returns the matches in order of appearance within the text.
            foreach (var modelResult in model.Parse(text, reference))
                results.Add(Map(modelResult));

            return results;
        }

        /// <summary>
        /// Projects the typed results into a <see cref="DataTable"/> (one row per match)
        /// so a workflow can iterate them with a "For Each Row" without touching code.
        /// </summary>
        public static DataTable ToDataTable(IEnumerable<DateTimeRecognitionResult> results)
        {
            var table = new DataTable("DateTimeMatches");
            table.Columns.Add("Text", typeof(string));
            table.Columns.Add("Subtype", typeof(string));
            table.Columns.Add("Value", typeof(DateTime));
            table.Columns.Add("RangeStart", typeof(DateTime));
            table.Columns.Add("RangeEnd", typeof(DateTime));
            table.Columns.Add("Duration", typeof(TimeSpan));
            table.Columns.Add("Timex", typeof(string));
            table.Columns.Add("StartIndex", typeof(int));
            table.Columns.Add("Length", typeof(int));

            foreach (var result in results)
            {
                var row = table.NewRow();
                row["Text"] = result.Text;
                row["Subtype"] = result.Subtype.ToString();
                // Nullable values become DBNull so empty cells read as "no value" in Studio.
                row["Value"] = (object?)result.Value ?? DBNull.Value;
                row["RangeStart"] = (object?)result.RangeStart ?? DBNull.Value;
                row["RangeEnd"] = (object?)result.RangeEnd ?? DBNull.Value;
                row["Duration"] = (object?)result.Duration ?? DBNull.Value;
                row["Timex"] = result.Timex;
                row["StartIndex"] = result.StartIndex;
                row["Length"] = result.Length;
                table.Rows.Add(row);
            }

            return table;
        }

        private static DateTimeModel GetModel(string cultureCode)
        {
            return ModelCache.GetOrAdd(cultureCode, code =>
            {
                try
                {
                    // fallbackToDefaultCulture:false makes the recognizer report an
                    // unsupported language instead of silently parsing it as English
                    // (which would give subtly wrong results). We translate that failure
                    // into a clear, actionable error for the workflow designer.
                    return new DateTimeRecognizer(code).GetDateTimeModel(code, fallbackToDefaultCulture: false);
                }
                catch (Exception ex)
                {
                    throw new NotSupportedException(
                        $"The Date/Time recognizer does not support the selected language ('{code}'). " +
                        "Pick a language it supports (English always works).", ex);
                }
            });
        }

        /// <summary>Translates one raw recognizer result into our typed result object.</summary>
        private static DateTimeRecognitionResult Map(ModelResult modelResult)
        {
            var result = new DateTimeRecognitionResult
            {
                Text = modelResult.Text ?? string.Empty,
                StartIndex = modelResult.Start,
                // The recognizer's End is the index of the LAST matched character, so the
                // character length is (End - Start + 1).
                Length = modelResult.End - modelResult.Start + 1,
                Subtype = ParseSubtype(modelResult.TypeName ?? string.Empty),
            };

            // Resolution["values"] holds one dictionary per candidate interpretation.
            if (modelResult.Resolution != null &&
                modelResult.Resolution.TryGetValue("values", out var raw) &&
                raw is IList<Dictionary<string, string>> values)
            {
                foreach (var dict in values)
                    result.Values.Add(MapValue(dict));

                if (result.Values.Count > 0)
                    result.Timex = result.Values[0].Timex;
            }

            return result;
        }

        /// <summary>Maps a single resolution dictionary into a <see cref="DateTimeResolutionValue"/>.</summary>
        private static DateTimeResolutionValue MapValue(IReadOnlyDictionary<string, string> dict)
        {
            var value = new DateTimeResolutionValue
            {
                Type = dict.TryGetValue("type", out var type) ? type : string.Empty,
                Timex = dict.TryGetValue("timex", out var timex) ? timex : string.Empty,
            };

            if (value.Type == "duration")
            {
                // Durations are expressed as a number of seconds in the "value" field.
                if (dict.TryGetValue("value", out var seconds) &&
                    double.TryParse(seconds, NumberStyles.Any, CultureInfo.InvariantCulture, out var totalSeconds))
                {
                    value.Duration = TimeSpan.FromSeconds(totalSeconds);
                }
            }
            else if (dict.ContainsKey("start") || dict.ContainsKey("end"))
            {
                // Period/range values carry separate "start" and "end" timestamps.
                if (dict.TryGetValue("start", out var start) && TryParseDateTime(start, out var startValue))
                    value.Start = startValue;
                if (dict.TryGetValue("end", out var end) && TryParseDateTime(end, out var endValue))
                    value.End = endValue;
            }
            else if (dict.TryGetValue("value", out var point) && TryParseDateTime(point, out var pointValue))
            {
                // A single point in time (date, time or datetime).
                value.Value = pointValue;
            }

            return value;
        }

        /// <summary>
        /// Parses a recognizer timestamp such as "2026-06-12", "15:00:00" or
        /// "2026-06-12 15:00:00". Some values are unresolved partials (e.g. "XXXX-WXX-5")
        /// and simply will not parse - in that case we leave the typed value null.
        /// </summary>
        private static bool TryParseDateTime(string raw, out DateTime value) =>
            DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.None, out value);

        /// <summary>Maps a recognizer type name like "datetimeV2.daterange" to a <see cref="DateTimeSubtype"/>.</summary>
        private static DateTimeSubtype ParseSubtype(string typeName)
        {
            // Type names look like "datetimeV2.<kind>"; take the part after the last dot.
            var kind = typeName;
            var dot = typeName.LastIndexOf('.');
            if (dot >= 0 && dot < typeName.Length - 1)
                kind = typeName.Substring(dot + 1);

            return kind switch
            {
                "date" => DateTimeSubtype.Date,
                "time" => DateTimeSubtype.Time,
                "datetime" => DateTimeSubtype.DateTime,
                "daterange" => DateTimeSubtype.DatePeriod,
                "timerange" => DateTimeSubtype.TimePeriod,
                "datetimerange" => DateTimeSubtype.DateTimePeriod,
                "duration" => DateTimeSubtype.Duration,
                "set" => DateTimeSubtype.Set,
                _ => DateTimeSubtype.Unknown,
            };
        }
    }
}
