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
        [Description("The date/time that relative phrases like \"tomorrow\" are measured from. Leave empty to use the current time.")]
        public InArgument<DateTime> ReferenceTime { get; set; }

        /// <summary>
        /// Reads <see cref="ReferenceTime"/>, falling back to <see cref="DateTime.Now"/>
        /// when the caller left it unset (the default <c>DateTime</c> value).
        /// </summary>
        protected DateTime GetReferenceTime(CodeActivityContext context)
        {
            // ReferenceTime is optional, so the property itself can be null when the user
            // leaves it unbound. Guard with ?. and fall back to the current time both when
            // the argument is absent and when it holds the default DateTime value.
            var reference = ReferenceTime?.Get(context) ?? default;
            return reference == default ? DateTime.Now : reference;
        }
    }
}
