using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using TextRecognizers.Sequences;

namespace TextRecognizers.Sequences.Design
{
    /// <summary>
    /// Registers the designers (and therefore the panel icons) for the Sequence activities. UiPath
    /// Studio discovers this <see cref="IRegisterMetadata"/> implementation in the *.Design
    /// assembly and applies it, so the activities themselves never reference the design code.
    /// </summary>
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();

            builder.AddCustomAttributes(typeof(RecognizeSequences), new DesignerAttribute(typeof(SequenceDesigner)));
            builder.AddCustomAttributes(typeof(ParseSequence), new DesignerAttribute(typeof(SequenceDesigner)));

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
