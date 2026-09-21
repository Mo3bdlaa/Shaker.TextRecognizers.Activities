using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.NumberWithUnit;

namespace TextRecognizers.Units
{
    /// <summary>
    /// Runs Microsoft.Recognizers' currency/temperature/age/dimension models and maps each
    /// raw <see cref="ModelResult"/> into a strongly typed <see cref="MeasurementResult"/>.
    /// </summary>
    internal static class UnitRecognition
    {
        private static readonly ConcurrentDictionary<string, IModel> ModelCache =
            new ConcurrentDictionary<string, IModel>();

        public static IList<MeasurementResult> Recognize(string text, string cultureCode, MeasurementKind kind)
        {
            var results = new List<MeasurementResult>();
            if (string.IsNullOrWhiteSpace(text))
                return results;

            foreach (var modelResult in GetModel(cultureCode, kind).Parse(text))
                results.Add(Map(modelResult, kind));

            return results;
        }

        public static DataTable ToDataTable(IEnumerable<MeasurementResult> results)
        {
            var table = new DataTable("MeasurementMatches");
            table.Columns.Add("Text", typeof(string));
            table.Columns.Add("Kind", typeof(string));
            table.Columns.Add("Value", typeof(double));
            table.Columns.Add("Unit", typeof(string));
            table.Columns.Add("StartIndex", typeof(int));
            table.Columns.Add("Length", typeof(int));

            foreach (var result in results)
            {
                var row = table.NewRow();
                row["Text"] = result.Text;
                row["Kind"] = result.Kind.ToString();
                row["Value"] = result.Value;
                row["Unit"] = result.Unit;
                row["StartIndex"] = result.StartIndex;
                row["Length"] = result.Length;
                table.Rows.Add(row);
            }

            return table;
        }

        private static IModel GetModel(string cultureCode, MeasurementKind kind)
        {
            return ModelCache.GetOrAdd($"{cultureCode}|{kind}", _ =>
            {
                try
                {
                    var recognizer = new NumberWithUnitRecognizer(cultureCode);

                    // fallbackToDefaultCulture:false surfaces unsupported languages clearly.
                    return kind switch
                    {
                        MeasurementKind.Temperature => recognizer.GetTemperatureModel(cultureCode, fallbackToDefaultCulture: false),
                        MeasurementKind.Age => recognizer.GetAgeModel(cultureCode, fallbackToDefaultCulture: false),
                        MeasurementKind.Dimension => recognizer.GetDimensionModel(cultureCode, fallbackToDefaultCulture: false),
                        _ => recognizer.GetCurrencyModel(cultureCode, fallbackToDefaultCulture: false),
                    };
                }
                catch (Exception ex)
                {
                    throw new NotSupportedException(
                        $"The measurement recognizer does not support the selected language ('{cultureCode}'). " +
                        "Pick a language it supports (English always works).", ex);
                }
            });
        }

        private static MeasurementResult Map(ModelResult modelResult, MeasurementKind kind)
        {
            var result = new MeasurementResult
            {
                Text = modelResult.Text ?? string.Empty,
                StartIndex = modelResult.Start,
                Length = modelResult.End - modelResult.Start + 1,
                Kind = kind,
                Value = double.NaN,
            };

            // NumberWithUnit resolutions carry the number under "value" and the unit under "unit".
            if (modelResult.Resolution != null)
            {
                if (modelResult.Resolution.TryGetValue("value", out var rawValue) && rawValue != null &&
                    double.TryParse(rawValue.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                {
                    result.Value = value;
                }

                if (modelResult.Resolution.TryGetValue("unit", out var rawUnit) && rawUnit != null)
                {
                    result.Unit = rawUnit.ToString() ?? string.Empty;
                }
            }

            return result;
        }
    }
}
