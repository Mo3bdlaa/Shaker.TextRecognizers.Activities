# Changelog

All notable changes to the Text Recognizers activity suite are documented here.
The format is based on [Keep a Changelog](https://keepachangelog.com/), and the project
adheres to [Semantic Versioning](https://semver.org/).

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
  - **Time Zone** drop-down choosing whose *now* anchors relative phrases when
    **Reference Time** is left empty; defaults to the machine's own zone. Entries are real
    geographic zones labelled with their standard offset, e.g. `(UTC+02:00) Cairo`, so
    daylight saving is applied automatically - `(UTC+00:00) London` anchors at UTC+00:00 in
    winter and UTC+01:00 in summer. Half- and quarter-hour offsets are covered
    (Kolkata +05:30, Kathmandu +05:45, Chatham +12:45).
- **TextRecognizers.Number.Activities** — Recognize/Parse numbers, ordinals and percentages.
- **TextRecognizers.NumberWithUnit.Activities** — Recognize/Parse currency, temperature,
  age and dimension (value + unit).
- **TextRecognizers.Sequence.Activities** — Recognize/Parse emails, phone numbers, URLs,
  IP addresses, GUIDs, hashtags and mentions.
- **TextRecognizers.Choice.Activities** — Recognize/Parse boolean (yes/no) answers with a
  confidence score.
- Fully **offline / airgapped** packaging: the Microsoft.Recognizers engine DLLs are
  embedded in each package, so a single `.nupkg` installs with no external restore.
- 39 unit tests across all five domains, run through WorkflowInvoker.

[1.0.0]: https://mohammedshaker.com
