using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using TextRecognizers.Choices;

namespace TextRecognizers.Choices.Design
{
    /// <summary>
    /// Registers the designers (and therefore the panel icons) for the Choice activities. UiPath
    /// Studio discovers this <see cref="IRegisterMetadata"/> implementation in the *.Design
    /// assembly and applies it, so the activities themselves never reference the design code.
    /// </summary>
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();

            builder.AddCustomAttributes(typeof(RecognizeBooleans), new DesignerAttribute(typeof(ChoiceDesigner)));
            builder.AddCustomAttributes(typeof(ParseBoolean), new DesignerAttribute(typeof(ChoiceDesigner)));

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
