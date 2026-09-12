# TextRecognizers.Number.Activities

UiPath activities that turn natural-language numbers into real `Double` values — powered by
Microsoft.Recognizers.Text and **fully offline** (the engine is embedded, so the package
installs with no external restore).

## Activities

- **Recognize Numbers** — finds every numeric mention in a string. Outputs a typed list
  (`Matches`), a `DataTable` (`Matches (Table)`), and `Has Matches`.
- **Parse Number** — extracts the single best value. Outputs `Success`, a `Value` (`Double`),
  and the full `Result`.

Both take a **Kind** drop-down: **Number** (incl. decimals and fractions — "two and a half" → 2.5),
**Ordinal** ("1st", "second" → 1, 2) or **Percentage** ("fifty percent", "50%" → 50).

## Example

```
Parse Number
  Text = "the discount is fifty percent",  Kind = Percentage
  → Value = 50

Recognize Numbers
  Text = "buy 3 boxes and 1.5 kg",  Kind = Number
  → Matches = [3, 1.5]
```

MIT © 2026 Mohammed Shaker · https://mohammedshaker.com
