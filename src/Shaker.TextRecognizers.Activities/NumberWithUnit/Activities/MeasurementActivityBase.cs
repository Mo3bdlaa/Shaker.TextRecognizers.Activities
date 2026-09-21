using System.Activities;
using System.ComponentModel;
using TextRecognizers.Core;

namespace TextRecognizers.Units
{
    /// <summary>
    /// Base for the measurement activities. Adds the <see cref="Kind"/> selector
    /// (currency / temperature / age / dimension) on top of the common Text and Language inputs.
    /// </summary>
    public abstract class MeasurementActivityBase : RecognizerActivity
    {
        /// <summary>What kind of measurement to recognize.</summary>
        [Category("Options")]
        [DisplayName("Kind")]
        [Description("What to recognize: Currency, Temperature, Age or Dimension.")]
        public MeasurementKind Kind { get; set; } = MeasurementKind.Currency;

        /// <summary>Reads the selected <see cref="Kind"/>.</summary>
        protected MeasurementKind GetKind(CodeActivityContext context) => Kind;
    }
}
