using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using TextRecognizers.Sequences;

namespace TextRecognizers.Sequences.Design
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
            builder.AddCustomAttributes(typeof(RecognizeSequences), new DesignerAttribute(typeof(RecognizeSequencesDesigner)));
            builder.AddCustomAttributes(typeof(ParseSequence), new DesignerAttribute(typeof(ParseSequenceDesigner)));

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
