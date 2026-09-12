using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Choice;

namespace TextRecognizers.Choices
{
    /// <summary>
    /// Runs Microsoft.Recognizers' boolean model and maps each raw <see cref="ModelResult"/>
    /// into a typed <see cref="BooleanResult"/> (value + confidence score).
    /// </summary>
    internal static class ChoiceRecognition
    {
        private static readonly ConcurrentDictionary<string, IModel> ModelCache =
            new ConcurrentDictionary<string, IModel>();

        public static IList<BooleanResult> Recognize(string text, string cultureCode)
        {
            var results = new List<BooleanResult>();
            if (string.IsNullOrWhiteSpace(text))
                return results;

            foreach (var modelResult in GetModel(cultureCode).Parse(text))
                results.Add(Map(modelResult));

            return results;
        }

        public static DataTable ToDataTable(IEnumerable<BooleanResult> results)
        {
            var table = new DataTable("BooleanMatches");
            table.Columns.Add("Text", typeof(string));
            table.Columns.Add("Value", typeof(bool));
            table.Columns.Add("Score", typeof(double));
            table.Columns.Add("StartIndex", typeof(int));
            table.Columns.Add("Length", typeof(int));

            foreach (var result in results)
            {
                var row = table.NewRow();
                row["Text"] = result.Text;
                row["Value"] = result.Value;
                row["Score"] = result.Score;
                row["StartIndex"] = result.StartIndex;
                row["Length"] = result.Length;
                table.Rows.Add(row);
            }

            return table;
        }

        private static IModel GetModel(string cultureCode)
        {
            return ModelCache.GetOrAdd(cultureCode, code =>
            {
                try
                {
                    // fallbackToDefaultCulture:false surfaces unsupported languages clearly.
                    return new ChoiceRecognizer(code).GetBooleanModel(code, fallbackToDefaultCulture: false);
                }
                catch (Exception ex)
                {
                    throw new NotSupportedException(
                        $"The boolean recognizer does not support the selected language ('{code}'). " +
                        "Pick a language it supports (English always works).", ex);
                }
            });
        }

        private static BooleanResult Map(ModelResult modelResult)
        {
            var result = new BooleanResult
            {
                Text = modelResult.Text ?? string.Empty,
                StartIndex = modelResult.Start,
                Length = modelResult.End - modelResult.Start + 1,
            };

            if (modelResult.Resolution != null)
            {
                // The boolean model resolves "value" (true/false) and a confidence "score".
                if (modelResult.Resolution.TryGetValue("value", out var rawValue) && rawValue != null)
                    result.Value = bool.TryParse(rawValue.ToString(), out var value) && value;

                if (modelResult.Resolution.TryGetValue("score", out var rawScore) && rawScore != null &&
                    double.TryParse(rawScore.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var score))
                    result.Score = score;
            }

            return result;
        }
    }
}
