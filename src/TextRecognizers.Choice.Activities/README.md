# TextRecognizers.Choice.Activities

UiPath activities that read **yes/no answers** out of natural-language text and return a real
`Boolean`. Powered by Microsoft.Recognizers.Text and **fully offline**.

## Activities

- **Parse Boolean** — interprets text as a single yes/no answer. Outputs `Value` (`Boolean`),
  `Score` (confidence 0–1), and a `Success` flag.
- **Recognize Booleans** — finds every yes/no answer in a string (typed list + `DataTable`).

Understands far more than just "yes"/"no" — e.g. "sure", "absolutely", "nope", "I don't think so".

## Example

```
Parse Boolean
  Text = "yeah, that works for me"
  → Value = true,  Score = 1.0,  Success = true

Parse Boolean
  Text = "nope, not today"
  → Value = false, Success = true
```

> Tip: always check **Success** before trusting **Value** — a missing answer also reads as `false`.

MIT © 2026 Mohammed Shaker · https://mohammedshaker.com
