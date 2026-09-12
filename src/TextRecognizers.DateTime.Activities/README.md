# TextRecognizers.DateTime.Activities

UiPath activities that turn natural-language text into real `DateTime` values, plus a set of
business-date helpers — powered by Microsoft.Recognizers.Text and **fully offline** (the engine
is embedded, so the package installs with no external restore).

## Recognition

- **Recognize Date/Time** — finds every date/time mention in a string. Outputs a typed list
  (`Matches`), a `DataTable` (`Matches (Table)`) for `For Each Row`, and `Has Matches`.
- **Parse Date/Time** — extracts the single best value. Outputs `Success` and a typed `Result`.

Each result exposes `Value` (point in time), `RangeStart`/`RangeEnd` (periods), `Duration`
(durations), `Subtype`, `Timex`, and the matched `Text`/indices. Relative phrases (*tomorrow*,
*in 2 hours*) resolve against the optional **Reference Time** input (default: now).

**Languages:** English, Spanish, French, German, Italian, Portuguese, Dutch, Chinese, Japanese,
Turkish, Hindi. (Korean, Swedish and Bulgarian are not supported by the DateTime recognizer and
raise a clear error.)

## Business dates

Pure working-day math — no recognition. A configurable **Weekend** (Saturday/Sunday default,
Friday/Saturday for the Middle East, or a custom day list) and an optional **Holidays** list:

`Is Business Day` · `Is Weekend` · `Is Holiday` · `Add Business Days` ·
`Next Business Day` · `Previous Business Day` · `Business Days Between` ·
`Nth Business Day Of Month`

## Example

```
Parse Date/Time
  Text = "the invoice is due in 30 days"
  → Result.Value  (a DateTime, 30 days from the reference time)

Add Business Days
  Date = Today,  Business Days = 5,  Holidays = {company holidays}
  → Result  (5 working days ahead, skipping weekends + holidays)
```

Full reference and the airgapped deployment guide live in the project repository.
MIT © 2026 Mohammed Shaker · https://mohammedshaker.com
