using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Number;

namespace TextRecognizers.Numbers
{
    /// <summary>
    /// Runs Microsoft.Recognizers' number/ordinal/percentage models and maps each raw
    /// <see cref="ModelResult"/> into a strongly typed <see cref="NumberRecognitionResult"/>.
    /// </summary>
    internal static class NumberRecognition
    {
        // Models are safe to reuse and a little expensive to build, so cache one per
        // language + kind for the lifetime of the process.
        private static readonly ConcurrentDictionary<string, IModel> ModelCache =
            new ConcurrentDictionary<string, IModel>();

        /// <summary>Finds every numeric mention of the requested <paramref name="kind"/> in the text.</summary>
        public static IList<NumberRecognitionResult> Recognize(string text, string cultureCode, NumberKind kind)
        {
            var results = new List<NumberRecognitionResult>();
            if (string.IsNullOrWhiteSpace(text))
                return results;

            foreach (var modelResult in GetModel(cultureCode, kind).Parse(text))
                results.Add(Map(modelResult, kind));

            return results;
        }

        /// <summary>Projects the typed results into a one-row-per-match <see cref="DataTable"/>.</summary>
        public static DataTable ToDataTable(IEnumerable<NumberRecognitionResult> results)
        {
            var table = new DataTable("NumberMatches");
            table.Columns.Add("Text", typeof(string));
            table.Columns.Add("Kind", typeof(string));
            table.Columns.Add("Value", typeof(double));
            table.Columns.Add("RawValue", typeof(string));
            table.Columns.Add("StartIndex", typeof(int));
            table.Columns.Add("Length", typeof(int));

            foreach (var result in results)
            {
                var row = table.NewRow();
                row["Text"] = result.Text;
                row["Kind"] = result.Kind.ToString();
                row["Value"] = result.Value;
                row["RawValue"] = result.RawValue;
                row["StartIndex"] = result.StartIndex;
                row["Length"] = result.Length;
                table.Rows.Add(row);
            }

            return table;
        }

        private static IModel GetModel(string cultureCode, NumberKind kind)
        {
            return ModelCache.GetOrAdd($"{cultureCode}|{kind}", _ =>
            {
                try
                {
                    var recognizer = new NumberRecognizer(cultureCode);

                    // fallbackToDefaultCulture:false surfaces unsupported languages instead of
                    // silently recognising them with the English model.
                    return kind switch
                    {
                        NumberKind.Ordinal => recognizer.GetOrdinalModel(cultureCode, fallbackToDefaultCulture: false),
                        NumberKind.Percentage => recognizer.GetPercentageModel(cultureCode, fallbackToDefaultCulture: false),
                        _ => recognizer.GetNumberModel(cultureCode, fallbackToDefaultCulture: false),
                    };
                }
                catch (Exception ex)
                {
                    throw new NotSupportedException(
                        $"The Number recognizer does not support the selected language ('{cultureCode}'). " +
                        "Pick a language it supports (English always works).", ex);
                }
            });
        }

        private static NumberRecognitionResult Map(ModelResult modelResult, NumberKind kind)
        {
            var result = new NumberRecognitionResult
            {
                Text = modelResult.Text ?? string.Empty,
                StartIndex = modelResult.Start,
                Length = modelResult.End - modelResult.Start + 1,
                Kind = kind,
            };

            // The number models put the resolved value under the "value" key.
            if (modelResult.Resolution != null &&
                modelResult.Resolution.TryGetValue("value", out var raw) &&
                raw != null)
            {
                result.RawValue = raw.ToString() ?? string.Empty;
                result.Value = ParseValue(result.RawValue);
            }
            else
            {
                result.Value = double.NaN;
            }

            return result;
        }

        private static double ParseValue(string raw)
        {
            // Percentages come back like "50%"; drop the sign so the numeric value parses.
            var trimmed = raw.EndsWith("%", StringComparison.Ordinal) ? raw.Substring(0, raw.Length - 1) : raw;
            return double.TryParse(trimmed, NumberStyles.Any, CultureInfo.InvariantCulture, out var value)
                ? value
                : double.NaN;
        }
    }
}
