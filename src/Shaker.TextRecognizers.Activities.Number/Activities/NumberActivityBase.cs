using System.Activities;
using System.ComponentModel;
using TextRecognizers.Core;

namespace TextRecognizers.Numbers
{
    /// <summary>
    /// Base for the number activities. Adds the <see cref="Kind"/> selector (number / ordinal /
    /// percentage) on top of the common Text and Language inputs from <see cref="RecognizerActivity"/>.
    /// </summary>
    public abstract class NumberActivityBase : RecognizerActivity
    {
        /// <summary>What to recognize: plain numbers, ordinals or percentages.</summary>
        [Category("Options")]
        [DisplayName("Kind")]
        [Description("What to recognize: plain Numbers, Ordinals (1st, 2nd, …) or Percentages.")]
        public NumberKind Kind { get; set; } = NumberKind.Number;

        /// <summary>Reads the selected <see cref="Kind"/>.</summary>
        protected NumberKind GetKind(CodeActivityContext context) => Kind;
    }
}
