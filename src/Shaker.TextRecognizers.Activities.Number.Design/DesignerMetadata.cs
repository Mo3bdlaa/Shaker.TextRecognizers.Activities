using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using TextRecognizers.Numbers;

namespace TextRecognizers.Numbers.Design
{
    /// <summary>
    /// Registers the designers (and therefore the icons) for these activities. UiPath
    /// Studio discovers this <see cref="IRegisterMetadata"/> implementation in the *.Design
    /// assembly and applies it, so the activities themselves never reference the design code.
    /// </summary>
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();

            // One designer per activity, so each carries its own icon in the panel.
            builder.AddCustomAttributes(typeof(RecognizeNumbers), new DesignerAttribute(typeof(RecognizeNumbersDesigner)));
            builder.AddCustomAttributes(typeof(ParseNumber), new DesignerAttribute(typeof(ParseNumberDesigner)));

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
