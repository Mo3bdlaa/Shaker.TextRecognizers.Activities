using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Microsoft.Recognizers.Text;
using TextRecognizers.Core;
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
            {
                results.Add(Map(modelResult, text));

                // The model reports one answer as the match and puts any further ones it found
                // in the same string under "otherResults". Without these, a string like
                // "Yes to the refund. No to the replacement." yields only the Yes.
                results.AddRange(MapOtherResults(modelResult, text, results));
            }

            // otherResults arrive after the primary match regardless of where they sit in the
            // text, so order by position to keep "in order of appearance" true.
            results.Sort((left, right) => left.StartIndex.CompareTo(right.StartIndex));

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

        private static BooleanResult Map(ModelResult modelResult, string source)
        {
            var result = new BooleanResult
            {
                Text = MatchText.Slice(source, modelResult.Start, modelResult.End),
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

        /// <summary>
        /// Maps the extra answers the model tucked into <c>otherResults</c>. Those carry the
        /// matched text, value and score but no offsets, so each is located in
        /// <paramref name="source"/> by its own text, skipping spans already taken.
        /// </summary>
        private static IEnumerable<BooleanResult> MapOtherResults(
            ModelResult modelResult, string source, IReadOnlyCollection<BooleanResult> taken)
        {
            if (modelResult.Resolution == null ||
                !modelResult.Resolution.TryGetValue("otherResults", out var others) ||
                others is not IEnumerable sequence)
            {
                yield break;
            }

            var claimed = new List<BooleanResult>(taken);

            foreach (var other in sequence)
            {
                if (other == null)
                    continue;

                // otherResults entries are an anonymous type, so read them by property name.
                var type = other.GetType();
                var matched = type.GetProperty("Text")?.GetValue(other) as string;
                if (string.IsNullOrEmpty(matched))
                    continue;

                var start = NextUnclaimedIndex(source, matched, claimed);
                if (start < 0)
                    continue;

                var result = new BooleanResult
                {
                    Text = MatchText.Slice(source, start, start + matched.Length - 1),
                    StartIndex = start,
                    Length = matched.Length,
                };

                if (type.GetProperty("Value")?.GetValue(other) is bool value)
                    result.Value = value;

                if (type.GetProperty("Score")?.GetValue(other) is double score)
                    result.Score = score;

                claimed.Add(result);
                yield return result;
            }
        }

        /// <summary>
        /// The first occurrence of <paramref name="matched"/> in <paramref name="source"/> that
        /// does not overlap a span already accounted for, or -1 when there is none.
        /// </summary>
        private static int NextUnclaimedIndex(string source, string matched, IEnumerable<BooleanResult> claimed)
        {
            var spans = claimed.Select(c => (Start: c.StartIndex, End: c.StartIndex + c.Length - 1)).ToList();

            for (var index = source.IndexOf(matched, StringComparison.OrdinalIgnoreCase);
                 index >= 0;
                 index = source.IndexOf(matched, index + 1, StringComparison.OrdinalIgnoreCase))
            {
                var end = index + matched.Length - 1;
                if (!spans.Any(span => index <= span.End && span.Start <= end))
                    return index;
            }

            return -1;
        }
    }
}
