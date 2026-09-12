using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using TextRecognizers.Numbers;

namespace TextRecognizers.Numbers.Design
{
    /// <summary>
    /// Registers the designers (and therefore the panel icons) for the Number activities. UiPath
    /// Studio discovers this <see cref="IRegisterMetadata"/> implementation in the *.Design
    /// assembly and applies it, so the activities themselves never reference the design code.
    /// </summary>
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();

            builder.AddCustomAttributes(typeof(RecognizeNumbers), new DesignerAttribute(typeof(NumberDesigner)));
            builder.AddCustomAttributes(typeof(ParseNumber), new DesignerAttribute(typeof(NumberDesigner)));

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
