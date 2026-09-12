using System;
using System.Activities;
using System.ComponentModel;
using TextRecognizers.Core;

namespace TextRecognizers.DateTimes
{
    /// <summary>
    /// Base class shared by the DateTime recognition activities. On top of the common
    /// <c>Text</c> and <c>Language</c> inputs from <see cref="RecognizerActivity"/>, it
    /// adds the <see cref="ReferenceTime"/> that relative phrases are measured against.
    /// </summary>
    public abstract class DateTimeActivityBase : RecognizerActivity
    {
        /// <summary>
        /// The date/time that relative phrases such as "tomorrow" or "in 2 hours" are
        /// measured from. Leave empty to use the current time when the activity runs.
        /// </summary>
        [Category("Options")]
        [DisplayName("Reference Time")]
        [Description("The date/time that relative phrases like \"tomorrow\" are measured from. Leave empty to use the current time in the selected Time Zone.")]
        public InArgument<DateTime> ReferenceTime { get; set; }

        /// <summary>
        /// Which clock <see cref="ReferenceTime"/> falls back to when it is left empty. A plain
        /// enum property, so Studio renders it as a drop-down; defaults to
        /// <see cref="TimeZoneOption.SystemDefault"/> (this machine's own time zone).
        /// </summary>
        [Category("Options")]
        [DisplayName("Time Zone")]
        [Description("Whose \"now\" relative phrases are measured from when Reference Time is empty. Defaults to this machine's time zone. Ignored when Reference Time is set.")]
        public TimeZoneOption TimeZone { get; set; } = TimeZoneOption.SystemDefault;

        /// <summary>
        /// Reads <see cref="ReferenceTime"/>, falling back to the current time in the
        /// selected <see cref="TimeZone"/> when the caller left it unset (the default
        /// <c>DateTime</c> value).
        /// </summary>
        protected DateTime GetReferenceTime(CodeActivityContext context)
        {
            // ReferenceTime is optional, so the property itself can be null when the user
            // leaves it unbound. Guard with ?. and fall back to the current time both when
            // the argument is absent and when it holds the default DateTime value.
            var reference = ReferenceTime?.Get(context) ?? default;

            // An explicit Reference Time is already a concrete moment, so Time Zone has
            // nothing left to decide - it only chooses which clock "now" is read from.
            return reference == default ? TimeZones.Now(TimeZone) : reference;
        }
    }
}
