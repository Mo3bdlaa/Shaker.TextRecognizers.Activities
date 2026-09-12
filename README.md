# Text Recognizers — UiPath Activities

Ready-to-use UiPath activities that turn **natural-language text into strongly-typed
values**, powered by [Microsoft.Recognizers.Text](https://github.com/microsoft/Recognizers-Text).

Instead of wrestling a `List<ModelResult>` and a loosely-typed resolution dictionary inside
`Assign` activities, you drag in one activity and get a real `DateTime`, number, currency or
boolean straight back.

- **Offline / airgapped-ready** — the recognition engine is grammar-based: no network calls,
  no model downloads. Every package is fully self-contained (the engine DLLs are embedded), so
  a single `.nupkg` installs with zero external restore. See
  [docs/airgapped-deployment.md](docs/airgapped-deployment.md).
- **Friendly to use** — every input and output has a tooltip, fixed choices are drop-downs
  (but still accept variables), and required fields are flagged.
- **One package per domain** — install only what you need; each is fully self-contained
  (the shared `Core` is embedded inside every package, so there is no extra package to install).

> Author: **Mohammed Shaker** · <https://mohammedshaker.com> · MIT licensed.

## Packages

| Package | What it gives you | Status |
|---|---|---|
| `TextRecognizers.DateTime.Activities` | Dates, times, ranges, durations, recurrences | ✅ 1.0.0 |
| `TextRecognizers.Number.Activities` | Numbers, ordinals, percentages | ✅ 1.0.0 |
| `TextRecognizers.NumberWithUnit.Activities` | Currency, temperature, age, dimension | ✅ 1.0.0 |
| `TextRecognizers.Sequence.Activities` | Phone, email, URL, IP, GUID, hashtag, mention | ✅ 1.0.0 |
| `TextRecognizers.Choice.Activities` | Boolean (yes / no) | ✅ 1.0.0 |

## Quick start

1. Install **`TextRecognizers.DateTime.Activities`** from your feed — it is fully self-contained,
   so there is nothing else to install. For airgapped feeds, see the [deployment guide](docs/airgapped-deployment.md).
2. Drag **Parse Date/Time** onto the canvas.
3. Set **Text** to the string you want to read, e.g. an email body or an Excel cell.
4. Read the outputs: **Success** (was anything found?) and **Result** (the typed value).

```
Parse Date/Time
  Text  = "Please reschedule to next Friday at 3pm"
  →  Result.Value = 2026-06-19 15:00:00
     Result.Subtype = DateTime
```

Relative phrases such as *tomorrow* or *in 2 hours* are resolved against **Reference Time**
(defaults to now). Leave it empty unless you need a different anchor.

## DateTime activities

**Recognize Date/Time** — find *every* date/time mention in a string.

| Direction | Name | Type | Notes |
|---|---|---|---|
| In | Text | `String` | *Required.* The text to scan. |
| In | Language | `CultureOption` | Drop-down; default English. |
| In | Reference Time | `DateTime` | Anchor for relative phrases; default now. |
| Out | Matches | `List<DateTimeRecognitionResult>` | One per mention, in order. |
| Out | Has Matches | `Boolean` | |
| Out | Matches (Table) | `DataTable` | Same data for `For Each Row`. |

**Parse Date/Time** — extract the single best value (for a cell, a field, one phrase).

| Direction | Name | Type | Notes |
|---|---|---|---|
| In | Text / Language / Reference Time | — | As above. |
| Out | Success | `Boolean` | True when something was found. |
| Out | Result | `DateTimeRecognitionResult` | Null when nothing was found. |

**`DateTimeRecognitionResult`** carries the parsed value(s):

| Member | Meaning |
|---|---|
| `Text`, `StartIndex`, `Length` | The matched substring and where it sat in the input. |
| `Subtype` | `Date`, `Time`, `DateTime`, `DatePeriod`, `TimePeriod`, `DateTimePeriod`, `Duration`, `Set`. |
| `Value` | The point-in-time value (for dates/times). |
| `RangeStart`, `RangeEnd` | Start/end (for periods). |
| `Duration` | A `TimeSpan` (for durations). |
| `IsRange`, `IsDuration`, `IsSet` | Quick branching flags. |
| `Timex`, `Values` | Raw TIMEX3 and every candidate interpretation, for advanced use. |

## Number, measurements, sequences & booleans

The other domains follow the same shape — a **Recognize…** activity (all matches + `DataTable`)
and a **Parse…** activity (single best value + `Success`), with a **Kind** drop-down where it
helps. See each package's own README for inputs, outputs and examples.

| Package | Activities | Kinds |
|---|---|---|
| **Number** | Recognize Numbers · Parse Number | Number, Ordinal, Percentage |
| **NumberWithUnit** | Recognize Measurements · Parse Measurement | Currency, Temperature, Age, Dimension (value **+ unit**) |
| **Sequence** | Recognize Sequences · Parse Sequence | Email, PhoneNumber, Url, IpAddress, Guid, Hashtag, Mention |
| **Choice** | Recognize Booleans · Parse Boolean | yes / no (with a confidence score) |

## Supported languages

The DateTime recognizer supports **11 of the 14** `CultureOption` languages: English, Spanish,
French, German, Italian, Portuguese, Dutch, Chinese, Japanese, Turkish, Hindi. Korean, Swedish
and Bulgarian are **not** supported by this particular recognizer — picking one raises a clear
error rather than silently parsing as English. (Other domains support different sets; each
package documents its own.)

## Build from source

Requires the .NET SDK (built and tested with .NET 10; the packages target `net6.0-windows`).

```powershell
dotnet build   TextRecognizers.slnx -c Release
dotnet test    tests/TextRecognizers.Tests/TextRecognizers.Tests.csproj
dotnet pack    TextRecognizers.slnx -c Release -o build/packages
```

## Repository layout

```
src/      Core (shared, embedded into each package) + one activity project per domain
          + a Studio design assembly (per-activity panel icons)
tests/    xUnit tests (run each activity through WorkflowInvoker, as Studio does)
docs/     deployment + design notes
assets/   source icons
build/    packaging output
```

## License

[MIT](LICENSE) © 2026 Mohammed Shaker. Microsoft.Recognizers.Text is © Microsoft, also MIT.
