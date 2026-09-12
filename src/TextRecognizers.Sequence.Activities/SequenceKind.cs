namespace TextRecognizers.Sequences
{
    /// <summary>
    /// What kind of sequence to extract. Shown as a drop-down so the user picks the target
    /// pattern rather than guessing which activity to use.
    /// </summary>
    public enum SequenceKind
    {
        /// <summary>Email addresses, e.g. "jane.doe@example.com".</summary>
        Email = 0,

        /// <summary>Phone numbers, e.g. "+1 (555) 123-4567".</summary>
        PhoneNumber,

        /// <summary>URLs, e.g. "https://example.com/path".</summary>
        Url,

        /// <summary>IP addresses (v4/v6), e.g. "192.168.0.1".</summary>
        IpAddress,

        /// <summary>GUIDs, e.g. "d3b07384-d9a0-4c9b-8f1e-1a2b3c4d5e6f".</summary>
        Guid,

        /// <summary>Hashtags, e.g. "#automation".</summary>
        Hashtag,

        /// <summary>Mentions, e.g. "@uipath".</summary>
        Mention,
    }
}
