using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Sequence;

namespace TextRecognizers.Sequences
{
    /// <summary>
    /// Runs Microsoft.Recognizers' sequence models (phone/email/URL/IP/GUID/hashtag/mention)
    /// and maps each raw <see cref="ModelResult"/> into a typed <see cref="SequenceResult"/>.
    /// </summary>
    internal static class SequenceRecognition
    {
        private static readonly ConcurrentDictionary<string, IModel> ModelCache =
            new ConcurrentDictionary<string, IModel>();

        public static IList<SequenceResult> Recognize(string text, string cultureCode, SequenceKind kind)
        {
            var results = new List<SequenceResult>();
            if (string.IsNullOrWhiteSpace(text))
                return results;

            foreach (var modelResult in GetModel(cultureCode, kind).Parse(text))
                results.Add(Map(modelResult, kind));

            return results;
        }

        public static DataTable ToDataTable(IEnumerable<SequenceResult> results)
        {
            var table = new DataTable("SequenceMatches");
            table.Columns.Add("Text", typeof(string));
            table.Columns.Add("Kind", typeof(string));
            table.Columns.Add("Value", typeof(string));
            table.Columns.Add("StartIndex", typeof(int));
            table.Columns.Add("Length", typeof(int));

            foreach (var result in results)
            {
                var row = table.NewRow();
                row["Text"] = result.Text;
                row["Kind"] = result.Kind.ToString();
                row["Value"] = result.Value;
                row["StartIndex"] = result.StartIndex;
                row["Length"] = result.Length;
                table.Rows.Add(row);
            }

            return table;
        }

        private static IModel GetModel(string cultureCode, SequenceKind kind)
        {
            return ModelCache.GetOrAdd($"{cultureCode}|{kind}", _ =>
            {
                // Sequence patterns (email, URL, IP, …) are essentially language-independent,
                // so the model uses the recognizer's default-culture fallback rather than
                // failing for less-common languages.
                var recognizer = new SequenceRecognizer(cultureCode);
                return kind switch
                {
                    SequenceKind.PhoneNumber => recognizer.GetPhoneNumberModel(),
                    SequenceKind.Url => recognizer.GetURLModel(),
                    SequenceKind.IpAddress => recognizer.GetIpAddressModel(),
                    SequenceKind.Guid => recognizer.GetGUIDModel(),
                    SequenceKind.Hashtag => recognizer.GetHashtagModel(),
                    SequenceKind.Mention => recognizer.GetMentionModel(),
                    _ => recognizer.GetEmailModel(),
                };
            });
        }

        private static SequenceResult Map(ModelResult modelResult, SequenceKind kind)
        {
            var text = modelResult.Text ?? string.Empty;
            var result = new SequenceResult
            {
                Text = text,
                StartIndex = modelResult.Start,
                Length = modelResult.End - modelResult.Start + 1,
                Kind = kind,
                Value = text,
            };

            // Most sequence models echo a normalised "value"; fall back to the matched text.
            if (modelResult.Resolution != null &&
                modelResult.Resolution.TryGetValue("value", out var raw) && raw != null)
            {
                var value = raw.ToString();
                if (!string.IsNullOrEmpty(value))
                    result.Value = value;
            }

            return result;
        }
    }
}
