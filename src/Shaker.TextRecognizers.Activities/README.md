# Shaker.TextRecognizers.Activities

UiPath activities that turn **natural-language text into strongly-typed values** — dates,
numbers, measurements, emails and URLs, yes/no answers. Powered by Microsoft.Recognizers.Text
and **fully offline**: the engine is grammar-based and embedded in the package, so it makes no
network calls, downloads no models, and installs with no external restore.

Instead of wrestling a `List<ModelResult>` and a loosely-typed resolution dictionary inside
`Assign` activities, you drag in one activity and get a real `DateTime`, `Double` or `Boolean`
straight back.

Every activity pairs a **Recognize…** (all matches, plus a `DataTable` for `For Each Row`) with
a **Parse…** (single best value, plus a `Success` flag). Each carries its own icon in the panel.

## Date & time — `TextRecognizers.DateTimes`

**Recognize Date/Time** · **Parse Date/Time**

Results expose `Value` (point in time), `RangeStart`/`RangeEnd` (periods), `Duration`, the
`Subtype`, the raw `Timex`, and the matched text with its position. Relative phrases resolve
against an optional **Reference Time**; leave it empty and a **Time Zone** drop-down decides
whose *now* is used — real zones labelled with their standard offset, e.g. `(UTC+02:00) Cairo`,
with daylight saving applied automatically.

```
Parse Date/Time
  Text = "Please reschedule to next Friday at 3pm"
  → Result.Value = 2026-06-19 15:00:00,  Result.Subtype = DateTime
```

## Numbers — `TextRecognizers.Numbers`

**Recognize Numbers** · **Parse Number** — a **Kind** drop-down selects **Number** (incl.
decimals and fractions, "two and a half" → 2.5), **Ordinal** ("1st", "second" → 1, 2) or
**Percentage** ("fifty percent", "50%" → 50).

```
Parse Number
  Text = "the discount is fifty percent",  Kind = Percentage
  → Value = 50
```

## Measurements — `TextRecognizers.Measurements`

**Recognize Measurements** · **Parse Measurement** — returns the numeric **Value** and its
**Unit** separately. **Kind**: **Currency**, **Temperature**, **Age** or **Dimension**.

```
Parse Measurement
  Text = "the parcel weighs 5 kg",  Kind = Dimension
  → Value = 5,  Unit = "Kilogram"
```

## Sequences — `TextRecognizers.Sequences`

**Recognize Sequences** · **Parse Sequence** — **Kind**: **Email**, **PhoneNumber**, **Url**,
**IpAddress**, **Guid**, **Hashtag** or **Mention**. These patterns are largely
language-independent, so **Language** has little effect here.

```
Recognize Sequences
  Text = "ping me at jane@acme.com or +1 555-123-4567",  Kind = Email
  → Matches = ["jane@acme.com"]
```

## Yes / no — `TextRecognizers.Choices`

**Recognize Booleans** · **Parse Boolean** — returns a real `Boolean` plus a confidence
`Score`. Understands far more than "yes"/"no": "sure", "absolutely", "nope", "I don't think so".

```
Parse Boolean
  Text = "yeah, that works for me"
  → Value = true,  Score = 1.0,  Success = true
```

> Tip: always check **Success** before trusting **Value** — a missing answer also reads as `false`.

## Languages

The DateTime recognizer supports 11 of the 14 `CultureOption` languages: English, Spanish,
French, German, Italian, Portuguese, Dutch, Chinese, Japanese, Turkish, Hindi. Korean, Swedish
and Bulgarian are not supported by that recognizer — picking one raises a clear error rather
than silently parsing as English. Other domains support different sets.

MIT © 2026 Mohammed Shaker · https://mohammedshaker.com
