using System.Activities;
using System.ComponentModel;
using TextRecognizers.Core;

namespace TextRecognizers.Sequences
{
    /// <summary>
    /// Base for the sequence activities. Adds the <see cref="Kind"/> selector (email / phone /
    /// URL / IP / GUID / hashtag / mention) on top of the common Text and Language inputs.
    /// </summary>
    public abstract class SequenceActivityBase : RecognizerActivity
    {
        /// <summary>Which kind of sequence to extract.</summary>
        [Category("Options")]
        [DisplayName("Kind")]
        [Description("What to extract: Email, PhoneNumber, Url, IpAddress, Guid, Hashtag or Mention.")]
        public SequenceKind Kind { get; set; } = SequenceKind.Email;

        /// <summary>Reads the selected <see cref="Kind"/>.</summary>
        protected SequenceKind GetKind(CodeActivityContext context) => Kind;
    }
}
