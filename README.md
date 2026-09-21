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
- **One package, grouped by domain** — a single install puts every activity in the panel under
  its own group, and the recognition engine ships inside it, so there is nothing else to add.

> Author: **Mohammed Shaker** · <https://mohammedshaker.com> · MIT licensed.

## One package

Everything installs as a single package, with the activities grouped by domain in the
activities panel.

| Toolbox group | Activities | Kinds |
|---|---|---|
| **DateTimes** | Recognize Date/Time · Parse Date/Time | dates, times, ranges, durations, recurrences |
| **Numbers** | Recognize Numbers · Parse Number | Number, Ordinal, Percentage |
| **Measurements** | Recognize Measurements · Parse Measurement | Currency, Temperature, Age, Dimension (value **+ unit**) |
| **Sequences** | Recognize Sequences · Parse Sequence | Email, PhoneNumber, Url, IpAddress, Guid, Hashtag, Mention |
| **Choices** | Recognize Booleans · Parse Boolean | yes / no (with a confidence score) |

`Shaker.TextRecognizers.Activities` — ✅ 1.0.0

## Quick start

1. Install **`Shaker.TextRecognizers.Activities`** from your feed — it is fully self-contained,
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

**Time Zone** picks *whose* now that is — useful when the robot runs in one region but reads
text written in another. It defaults to **System Default**, the robot machine's own zone, and
is ignored when Reference Time is set (that is already a concrete moment). Entries are real
zones shown with their standard offset, e.g. `(UTC+02:00) Cairo`, so daylight saving is applied
for you: `(UTC+00:00) London` anchors at UTC+00:00 in winter and UTC+01:00 in summer. Pick
`(UTC+00:00) UTC` for a clock that never shifts.

## DateTime activities

**Recognize Date/Time** — find *every* date/time mention in a string.

| Direction | Name | Type | Notes |
|---|---|---|---|
| In | Text | `String` | *Required.* The text to scan. |
| In | Language | `CultureOption` | Drop-down; default English. |
| In | Reference Time | `DateTime` | Anchor for relative phrases; default now. |
| In | Time Zone | `TimeZoneOption` | Drop-down; whose *now* to use. Default: this machine. |
| Out | Matches | `List<DateTimeRecognitionResult>` | One per mention, in order. |
| Out | Has Matches | `Boolean` | |
| Out | Matches (Table) | `DataTable` | Same data for `For Each Row`. |

**Parse Date/Time** — extract the single best value (for a cell, a field, one phrase).

| Direction | Name | Type | Notes |
|---|---|---|---|
| In | Text / Language / Reference Time / Time Zone | — | As above. |
| Out | Success | `Boolean` | True when something was found. |
| Out | Result | `DateTimeRecognitionResult` | Null when nothing was found. |

**`DateTimeRecognitionResult`** carries the parsed value(s):

| Member | Meaning |
|---|---|
| `Text`, `StartIndex`, `Length` | The matched substring, exactly as it appears in the input (casing included), and where it sat. |
| `Subtype` | `Date`, `Time`, `DateTime`, `DatePeriod`, `TimePeriod`, `DateTimePeriod`, `Duration`, `Set`. |
| `Value` | The point-in-time value (for dates/times). |
| `RangeStart`, `RangeEnd` | Start/end (for periods). |
| `Duration` | A `TimeSpan` (for durations). |
| `IsRange`, `IsDuration`, `IsSet` | Quick branching flags. |
| `Timex`, `Values` | Raw TIMEX3 and every candidate interpretation, for advanced use. |

## Number, measurements, sequences & booleans

The other groups follow the same shape — a **Recognize…** activity (all matches + `DataTable`)
and a **Parse…** activity (single best value + `Success`), with a **Kind** drop-down where it
helps. See the [package README](src/Shaker.TextRecognizers.Activities/README.md) for inputs, outputs and examples.

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

Requires the .NET SDK (built and tested with .NET 10). The package targets **`net6.0`** and
**`net6.0-windows`**, so Studio resolves it for both *Windows* and *Cross-platform* projects;
the WPF designers carrying the per-activity icons live in the Windows one.

```powershell
dotnet build Shaker.TextRecognizers.slnx -c Release
dotnet test  tests/Shaker.TextRecognizers.Tests/Shaker.TextRecognizers.Tests.csproj

# or just ./tools/pack-with-icons.ps1, which also verifies the result
dotnet pack src/Shaker.TextRecognizers.Activities/Shaker.TextRecognizers.Activities.csproj `
  -c Release -o build/packages --no-build -p:RequireDesignAssembly=true
```

## Repository layout

```
src/      the activity package, one folder per domain (+ Core, the shared base)
          + a Studio design assembly (per-activity panel icons)
tests/    xUnit tests (run each activity through WorkflowInvoker, as Studio does)
docs/     deployment + design notes
assets/   source icons
build/    packaging output
```

## License

[MIT](LICENSE) © 2026 Mohammed Shaker. Microsoft.Recognizers.Text is © Microsoft, also MIT.
