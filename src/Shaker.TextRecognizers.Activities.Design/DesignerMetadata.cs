using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using TextRecognizers.Choices;
using TextRecognizers.Choices.Design;
using TextRecognizers.DateTimes;
using TextRecognizers.DateTimes.Design;
using TextRecognizers.Numbers;
using TextRecognizers.Numbers.Design;
using TextRecognizers.Sequences;
using TextRecognizers.Sequences.Design;
using TextRecognizers.Units;
using TextRecognizers.Units.Design;

namespace TextRecognizers.Design
{
    /// <summary>
    /// Registers the designers - and therefore the icons - for every activity in the suite.
    /// UiPath Studio discovers this single <see cref="IRegisterMetadata"/> implementation in
    /// the design assembly and applies it, so the activities themselves never reference the
    /// design code.
    /// </summary>
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();

            // One designer per activity, so each carries its own icon in the panel: the
            // Recognize glyph stands for the domain, the Parse glyph for pulling one value out.
            builder.AddCustomAttributes(typeof(RecognizeDateTime), new DesignerAttribute(typeof(RecognizeDateTimeDesigner)));
            builder.AddCustomAttributes(typeof(ParseDateTime), new DesignerAttribute(typeof(ParseDateTimeDesigner)));

            builder.AddCustomAttributes(typeof(RecognizeNumbers), new DesignerAttribute(typeof(RecognizeNumbersDesigner)));
            builder.AddCustomAttributes(typeof(ParseNumber), new DesignerAttribute(typeof(ParseNumberDesigner)));

            builder.AddCustomAttributes(typeof(RecognizeMeasurements), new DesignerAttribute(typeof(RecognizeMeasurementsDesigner)));
            builder.AddCustomAttributes(typeof(ParseMeasurement), new DesignerAttribute(typeof(ParseMeasurementDesigner)));

            builder.AddCustomAttributes(typeof(RecognizeSequences), new DesignerAttribute(typeof(RecognizeSequencesDesigner)));
            builder.AddCustomAttributes(typeof(ParseSequence), new DesignerAttribute(typeof(ParseSequenceDesigner)));

            builder.AddCustomAttributes(typeof(RecognizeBooleans), new DesignerAttribute(typeof(RecognizeBooleansDesigner)));
            builder.AddCustomAttributes(typeof(ParseBoolean), new DesignerAttribute(typeof(ParseBooleanDesigner)));

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
