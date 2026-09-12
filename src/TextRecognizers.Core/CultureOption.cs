using System;
using System.Collections.Generic;

namespace TextRecognizers.Core
{
    /// <summary>
    /// The natural language a recognizer should assume when scanning text.
    /// </summary>
    /// <remarks>
    /// Exposed as an <see langword="enum"/> and used as a plain activity property, so Studio's
    /// property panel renders it as a friendly drop-down list instead of asking the user to type
    /// a raw culture code such as <c>"en-us"</c>.
    /// <para>
    /// Not every recognizer supports every language; each domain package documents the
    /// set of languages it actually understands.
    /// </para>
    /// </remarks>
    public enum CultureOption
    {
        /// <summary>English (en-us).</summary>
        English = 0,

        /// <summary>Spanish (es-es).</summary>
        Spanish,

        /// <summary>French (fr-fr).</summary>
        French,

        /// <summary>German (de-de).</summary>
        German,

        /// <summary>Italian (it-it).</summary>
        Italian,

        /// <summary>Portuguese (pt-br).</summary>
        Portuguese,

        /// <summary>Dutch (nl-nl).</summary>
        Dutch,

        /// <summary>Chinese (zh-cn).</summary>
        Chinese,

        /// <summary>Japanese (ja-jp).</summary>
        Japanese,

        /// <summary>Korean (ko-kr).</summary>
        Korean,

        /// <summary>Turkish (tr-tr).</summary>
        Turkish,

        /// <summary>Hindi (hi-in).</summary>
        Hindi,

        /// <summary>Swedish (sv-se).</summary>
        Swedish,

        /// <summary>Bulgarian (bg-bg).</summary>
        Bulgarian,
    }

    /// <summary>
    /// Translates the friendly <see cref="CultureOption"/> drop-down value into the
    /// culture-code string understood by Microsoft.Recognizers.Text.
    /// </summary>
    public static class CultureCodes
    {
        // These codes mirror the public constants on Microsoft.Recognizers.Text.Culture.
        // They are part of that library's stable public contract, which is why Core can
        // map to them directly and stay completely free of any third-party dependency.
        private static readonly IReadOnlyDictionary<CultureOption, string> Map =
            new Dictionary<CultureOption, string>
            {
                [CultureOption.English] = "en-us",
                [CultureOption.Spanish] = "es-es",
                [CultureOption.French] = "fr-fr",
                [CultureOption.German] = "de-de",
                [CultureOption.Italian] = "it-it",
                [CultureOption.Portuguese] = "pt-br",
                [CultureOption.Dutch] = "nl-nl",
                [CultureOption.Chinese] = "zh-cn",
                [CultureOption.Japanese] = "ja-jp",
                [CultureOption.Korean] = "ko-kr",
                [CultureOption.Turkish] = "tr-tr",
                [CultureOption.Hindi] = "hi-in",
                [CultureOption.Swedish] = "sv-se",
                [CultureOption.Bulgarian] = "bg-bg",
            };

        /// <summary>
        /// Returns the Microsoft.Recognizers culture code (e.g. <c>"en-us"</c>) for the
        /// supplied <paramref name="option"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if a new enum value is added without a matching entry in the map.
        /// </exception>
        public static string ToCultureCode(CultureOption option) =>
            Map.TryGetValue(option, out var code)
                ? code
                : throw new ArgumentOutOfRangeException(
                    nameof(option), option, "No culture code is mapped for this CultureOption value.");
    }
}
