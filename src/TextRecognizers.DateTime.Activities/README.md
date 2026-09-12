# TextRecognizers.DateTime.Activities

UiPath activities that turn natural-language text into real `DateTime` values — powered by
Microsoft.Recognizers.Text and **fully offline** (the engine is embedded, so the package
installs with no external restore).

## Recognition

- **Recognize Date/Time** — finds every date/time mention in a string. Outputs a typed list
  (`Matches`), a `DataTable` (`Matches (Table)`) for `For Each Row`, and `Has Matches`.
- **Parse Date/Time** — extracts the single best value. Outputs `Success` and a typed `Result`.

Each result exposes `Value` (point in time), `RangeStart`/`RangeEnd` (periods), `Duration`
(durations), `Subtype`, `Timex`, and the matched `Text`/indices. Relative phrases (*tomorrow*,
*in 2 hours*) resolve against the optional **Reference Time** input (default: now).

**Time Zone** chooses whose *now* that default is — a drop-down of real zones labelled with
their standard offset, e.g. `(UTC+02:00) Cairo`. Daylight saving is applied automatically, so
`(UTC+00:00) London` anchors at UTC+00:00 in winter and UTC+01:00 in summer. Defaults to
**System Default** (the machine's own zone); ignored when Reference Time is set.

**Languages:** English, Spanish, French, German, Italian, Portuguese, Dutch, Chinese, Japanese,
Turkish, Hindi. (Korean, Swedish and Bulgarian are not supported by the DateTime recognizer and
raise a clear error.)

## Example

```
Parse Date/Time
  Text = "the invoice is due in 30 days"
  → Result.Value  (a DateTime, 30 days from the reference time)
```

Full reference and the airgapped deployment guide live in the project repository.
MIT © 2026 Mohammed Shaker · https://mohammedshaker.com
