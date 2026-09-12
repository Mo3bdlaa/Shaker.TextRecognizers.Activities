# TextRecognizers.Sequence.Activities

UiPath activities that pull structured **sequences** — emails, phone numbers, URLs, IP
addresses, GUIDs, hashtags and mentions — out of free text. Powered by
Microsoft.Recognizers.Text and **fully offline**.

## Activities

- **Recognize Sequences** — finds every match in a string (typed list + `DataTable`).
- **Parse Sequence** — extracts the single best one as a `String` **Value**, with a `Success` flag.

A **Kind** drop-down selects the target: **Email**, **PhoneNumber**, **Url**, **IpAddress**,
**Guid**, **Hashtag** or **Mention**.

## Example

```
Recognize Sequences
  Text = "ping me at jane@acme.com or +1 555-123-4567",  Kind = Email
  → Matches = ["jane@acme.com"]

Parse Sequence
  Text = "visit https://mohammedshaker.com today",  Kind = Url
  → Value = "https://mohammedshaker.com"
```

These patterns are largely language-independent, so the **Language** input has little effect
here. MIT © 2026 Mohammed Shaker · https://mohammedshaker.com
