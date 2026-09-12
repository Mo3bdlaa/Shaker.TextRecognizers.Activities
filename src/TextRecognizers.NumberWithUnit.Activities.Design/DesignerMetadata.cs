using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using TextRecognizers.Units;

namespace TextRecognizers.Units.Design
{
    /// <summary>
    /// Registers the designers (and therefore the panel icons) for the Measurement activities.
    /// UiPath Studio discovers this <see cref="IRegisterMetadata"/> implementation in the *.Design
    /// assembly and applies it, so the activities themselves never reference the design code.
    /// </summary>
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();

            builder.AddCustomAttributes(typeof(RecognizeMeasurements), new DesignerAttribute(typeof(MeasurementDesigner)));
            builder.AddCustomAttributes(typeof(ParseMeasurement), new DesignerAttribute(typeof(MeasurementDesigner)));

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
