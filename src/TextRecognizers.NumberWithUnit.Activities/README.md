# TextRecognizers.NumberWithUnit.Activities

UiPath activities that pull **measurements** out of natural-language text — returning both the
numeric value and its unit. Powered by Microsoft.Recognizers.Text and **fully offline**.

## Activities

- **Recognize Measurements** — finds every measurement in a string (typed list + `DataTable`).
- **Parse Measurement** — extracts the single best one as separate **Value** (`Double`) and
  **Unit** (`String`) outputs, with a `Success` flag.

A **Kind** drop-down selects what to look for: **Currency** ("$10", "20 euros"),
**Temperature** ("20 degrees Celsius"), **Age** ("25 years old"), or **Dimension** ("3 km", "5 kg").

## Example

```
Parse Measurement
  Text = "the parcel weighs 5 kg",  Kind = Dimension
  → Value = 5,  Unit = "Kilogram"

Parse Measurement
  Text = "it costs $19.99",  Kind = Currency
  → Value = 19.99,  Unit = "Dollar"
```

MIT © 2026 Mohammed Shaker · https://mohammedshaker.com
