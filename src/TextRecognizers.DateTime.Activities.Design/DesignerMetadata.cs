using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using TextRecognizers.DateTimes;

namespace TextRecognizers.DateTimes.Design
{
    /// <summary>
    /// Registers the designers (and therefore the icons) for the DateTime activities. UiPath
    /// Studio discovers this <see cref="IRegisterMetadata"/> implementation in the *.Design
    /// assembly and applies it, so the activities themselves never reference the design code.
    /// </summary>
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();

            // Recognition activities -> calendar icon.
            builder.AddCustomAttributes(typeof(RecognizeDateTime), new DesignerAttribute(typeof(DateTimeDesigner)));
            builder.AddCustomAttributes(typeof(ParseDateTime), new DesignerAttribute(typeof(DateTimeDesigner)));

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
