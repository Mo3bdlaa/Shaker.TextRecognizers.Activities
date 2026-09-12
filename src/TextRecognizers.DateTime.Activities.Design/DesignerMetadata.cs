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

            // Business-date activities -> calendar-with-check icon.
            builder.AddCustomAttributes(typeof(IsBusinessDay), new DesignerAttribute(typeof(BusinessDesigner)));
            builder.AddCustomAttributes(typeof(IsWeekend), new DesignerAttribute(typeof(BusinessDesigner)));
            builder.AddCustomAttributes(typeof(IsHoliday), new DesignerAttribute(typeof(BusinessDesigner)));
            builder.AddCustomAttributes(typeof(AddBusinessDays), new DesignerAttribute(typeof(BusinessDesigner)));
            builder.AddCustomAttributes(typeof(NextBusinessDay), new DesignerAttribute(typeof(BusinessDesigner)));
            builder.AddCustomAttributes(typeof(PreviousBusinessDay), new DesignerAttribute(typeof(BusinessDesigner)));
            builder.AddCustomAttributes(typeof(BusinessDaysBetween), new DesignerAttribute(typeof(BusinessDesigner)));
            builder.AddCustomAttributes(typeof(NthBusinessDayOfMonth), new DesignerAttribute(typeof(BusinessDesigner)));

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
