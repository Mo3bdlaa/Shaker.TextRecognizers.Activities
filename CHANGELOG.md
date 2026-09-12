# Changelog

All notable changes to the Text Recognizers activity suite are documented here.
The format is based on [Keep a Changelog](https://keepachangelog.com/), and the project
adheres to [Semantic Versioning](https://semver.org/).

## [2.0.0] - 2026-09-12

### Removed
- **All business-date activities** and their supporting types: Is Business Day, Is Weekend,
  Is Holiday, Add Business Days, Next Business Day, Previous Business Day,
  Business Days Between and Nth Business Day Of Month, along with the `WeekendOption`
  drop-down, the `Weekend` / `Custom Weekend Days` / `Holidays` inputs and the
  `calendar-with-check` panel icon.

  This is a breaking change: workflows that use any of those activities will not load
  against 2.0.0. Stay on 1.0.0 if you need them.

  `TextRecognizers.DateTime.Activities` now covers date/time **recognition** only -
  Recognize Date/Time and Parse Date/Time are unchanged. The other four packages are
  unaffected and are versioned alongside it.

- 30 unit tests across all five domains, run through WorkflowInvoker (was 39; the nine
  business-date tests were removed with the feature).

## [1.0.0] - 2026-06-10

First release. All five recognizer domains, fully offline.

### Added
- **Shared core** — base activity, the `CultureOption` language drop-down (14 languages),
  and common helpers, embedded directly into every domain package (not a separate package).
- **TextRecognizers.DateTime.Activities**
  - **Recognize Date/Time** — finds every date/time mention in text; returns a typed list,
    a `DataTable`, and a "has matches" flag.
  - **Parse Date/Time** — extracts the single best date/time as a ready-to-use value,
    with a `Success` flag.
  - Strongly-typed results: subtype, TIMEX, indices, and convenience accessors for point
    values, ranges, durations and recurrences.
  - Clear error when a language is not supported by the DateTime recognizer
    (Korean, Swedish and Bulgarian are not supported by this recognizer).
  - **Business-date activities**: Is Business Day, Is Weekend, Is Holiday,
    Add Business Days, Next/Previous Business Day, Business Days Between,
    Nth Business Day Of Month — with a configurable weekend (Saturday/Sunday default,
    Friday/Saturday and other presets, or a custom day list) and user-supplied holidays.
- **TextRecognizers.Number.Activities** — Recognize/Parse numbers, ordinals and percentages.
- **TextRecognizers.NumberWithUnit.Activities** — Recognize/Parse currency, temperature,
  age and dimension (value + unit).
- **TextRecognizers.Sequence.Activities** — Recognize/Parse emails, phone numbers, URLs,
  IP addresses, GUIDs, hashtags and mentions.
- **TextRecognizers.Choice.Activities** — Recognize/Parse boolean (yes/no) answers with a
  confidence score.
- Fully **offline / airgapped** packaging: the Microsoft.Recognizers engine DLLs are
  embedded in each package, so a single `.nupkg` installs with no external restore.
- 39 unit tests across all five domains (recognition + business dates), run through WorkflowInvoker.

[2.0.0]: https://mohammedshaker.com
[1.0.0]: https://mohammedshaker.com
