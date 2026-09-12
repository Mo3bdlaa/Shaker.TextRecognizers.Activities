using System.Activities;
using System.ComponentModel;

namespace TextRecognizers.Core
{
    /// <summary>
    /// Base class for every recognizer activity in the suite. It supplies the two
    /// inputs that all recognizers share - the <see cref="Text"/> to scan and the
    /// <see cref="Culture"/> (language) to use - together with small helpers that read
    /// those inputs at run time, so each concrete activity can focus purely on its own
    /// recognition logic and outputs.
    /// </summary>
    public abstract class RecognizerActivity : CodeActivity
    {
        /// <summary>
        /// The text to analyse, for example <c>"let's meet next Friday at 3pm"</c>.
        /// This input is required.
        /// </summary>
        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Text")]
        [Description("The text to scan for matches, e.g. \"let's meet next Friday at 3pm\".")]
        public InArgument<string> Text { get; set; }

        /// <summary>
        /// The language of <see cref="Text"/>. A plain enum property, so Studio renders it as a
        /// drop-down list; defaults to <see cref="CultureOption.English"/>.
        /// </summary>
        [Category("Options")]
        [DisplayName("Language")]
        [Description("The language of the text. Pick a language from the drop-down list.")]
        public CultureOption Culture { get; set; } = CultureOption.English;

        /// <summary>Reads <see cref="Text"/>, treating a <see langword="null"/> value as an empty string.</summary>
        protected string GetText(CodeActivityContext context) => Text.Get(context) ?? string.Empty;

        /// <summary>Reads <see cref="Culture"/> and converts it to a Microsoft.Recognizers culture code.</summary>
        protected string GetCultureCode(CodeActivityContext context) =>
            CultureCodes.ToCultureCode(Culture);
    }
}
