# UiPath Marketplace submission

Draft copy for the Create Listing form, plus what has to be true of the package before
submitting. Field limits below are the ones the Marketplace form enforces.

Submit at [marketplace.uipath.com](https://marketplace.uipath.com) → **Publisher Account** →
**Create Listing**. The form has four parts: Select your file, Description, Files & terms,
and Pricing (paid listings only).

---

## Before you submit

The `.nupkg` you upload must be built on a machine with **UiPath Studio installed**. A build
without Studio silently omits the `*.Design.dll` from each package — the activities still
work, but every per-activity icon is missing, because the embed is guarded by an
`Exists(...)` condition. See [studio-design-assembly.md](studio-design-assembly.md).

```powershell
# on the Studio machine - builds the design assembly, packs, and verifies the icons
# are really in the .nupkg. Fails rather than producing an icon-less package.
./tools/pack-with-icons.ps1
```

Packing any other way warns (`TRX0001`) when the design assembly is missing, so an
icon-less package cannot be produced unnoticed.

And install one package into a scratch Studio project to check: the activities appear under
**TextRecognizers**, each carries its own icon, and **Language** / **Kind** / **Time Zone**
render as drop-downs with readable labels.

---

## One listing

The suite ships as a single package, `Shaker.TextRecognizers.Activities`, so this is one
listing with one file. The activities group themselves by domain in the panel
(DateTimes, Numbers, Measurements, Sequences, Choices), so a user installs once and gets
all five groups.

---

## Part 2 — Description

### Listing title — 50 characters max

```
Text Recognizers - Text to Typed Values
```

### Card summary — 200 characters max

```
Turns natural-language text into typed values: dates, numbers, measurements, emails, yes/no answers. Grammar-based and fully offline - no network calls, no model downloads, no API keys.
```

### Tags — 5 max, 20 characters each, single words

```
NLP   Parsing   DateTime   Offline   Recognizers
```

`DateTime` is the highest-volume search term of the five domains; swap it if the listing
should lead with another.

### Application

Pick from the dropdown. These activities read plain strings, so they pair with whatever
supplies the text — **Excel**, **Outlook**, **SAP**, or add a custom entry. There is no
single mandatory application.

### Overview — 5,000 characters max

> Instead of wrestling a `List<ModelResult>` and a loosely-typed resolution dictionary inside
> Assign activities, you drag in one activity and get a real DateTime, Double or Boolean back.
>
> **What it does**
>
> Ten activities in five groups. Every group pairs a **Recognize…** — every match in the
> string, as a typed list plus a DataTable for For Each Row — with a **Parse…** — the single
> best value plus a Success flag, the right shape for one cell or one field.
>
> - **DateTimes** — dates, times, ranges, durations and recurrences. Results carry the point
>   value, range start/end, duration, subtype, the raw TIMEX expression, and the matched text
>   with its position.
> - **Numbers** — Number (including decimals and fractions, "two and a half" to 2.5), Ordinal
>   and Percentage.
> - **Measurements** — Currency, Temperature, Age and Dimension, returning the value and its
>   unit separately.
> - **Sequences** — Email, PhoneNumber, Url, IpAddress, Guid, Hashtag and Mention.
> - **Choices** — yes/no answers with a confidence score, understanding far more than
>   "yes"/"no": "sure", "absolutely", "nope", "I don't think so".
>
> **Fully offline**
>
> The recognition engine is grammar-based: it makes no network calls and downloads no models.
> Every dependency is embedded in the package, so a single .nupkg installs with no external
> NuGet restore — suitable for airgapped and regulated environments.
>
> **Relative phrases and time zones**
>
> "next Friday at 3pm", "tomorrow", "in 2 hours" resolve against an optional Reference Time
> input. Leave it empty and a Time Zone drop-down decides whose "now" is used — real
> geographic zones labelled with their standard offset, e.g. (UTC+02:00) Cairo, so daylight
> saving is applied automatically. Defaults to the robot machine's own zone.
>
> **Languages**
>
> English, Spanish, French, German, Italian, Portuguese, Dutch, Chinese, Japanese, Turkish,
> Hindi. Picking a language this recognizer does not support raises a clear error rather than
> silently parsing as English.
>
> **Designed to be readable in Studio**
>
> Every input and output has a tooltip, fixed choices are drop-downs that still accept
> variables, required fields are flagged, and each activity carries its own panel icon.
>
> Built on Microsoft.Recognizers.Text. MIT licensed.

---

## Part 3 — Files & terms

| Item | Value |
|---|---|
| Primary file | `Shaker.TextRecognizers.Activities.1.0.0.nupkg` (Studio-built) |
| License | **MIT** — permissive, which the guidelines recommend for free listings |
| Support | **Community Support** (best-effort, via the Marketplace forum) |
| Terms of use | Must state licensing conditions and term, warranties, support, fees, and link a freely accessible privacy policy |

**Third-party dependency to declare:** Microsoft.Recognizers.Text (MIT, © Microsoft), whose
engine DLLs are embedded in the package. You are responsible for its license terms being
respected; MIT permits redistribution with attribution, which `LICENSE` already carries.

If you list as an individual rather than a company, the icon may be your own artwork or a
free non-copyright image. The package icon in this repo is original vector work.

---

## Part 4 — Pricing

Only appears for paid listings. A free listing skips it entirely, and free listings may use
an open-source license — which is what MIT is here.

---

## After you submit: certification

Publishing is not immediate. The Marketplace security certification runs in stages:

1. **Content quality check** — reviewed against the Standards for Quality Content.
2. **SDK embed and resubmit** — on passing, you are sent an SDK library that must be
   embedded in the code and the package **resubmitted**. This step is expected, not a
   rejection.
3. **Security and functionality checks** — on passing these, the listing is published.

### Where this package already stands

| Requirement | Status |
|---|---|
| `CompanyName.{PackageName}` naming | `Shaker.TextRecognizers.Activities` |
| Author, description, tags, license in metadata | present (MIT) |
| Package icon | present, 128×128, original artwork |
| Every activity has at least one output | all ten do |
| No "UiPath" in package ID or DLL names | clean |
| No credentials or hard-coded secrets | none in the source |
| Versioned | 1.0.0 |
| Design assembly present in the uploaded package | **only if built on the Studio machine** |

---

## Sources

- [Submit a listing](https://docs.uipath.com/marketplace/automation-cloud/latest/user-guide/publishing-guidelines-how-to-submit)
- [Description fields](https://docs.uipath.com/marketplace/automation-cloud/latest/user-guide/description-4)
- [Files & terms](https://docs.uipath.com/marketplace/automation-cloud/latest/user-guide/files-and-terms)
- [Standards for quality content](https://docs.uipath.com/marketplace/automation-cloud/latest/user-guide/standards-for-quality-content)
- [Security certification overview](https://docs.uipath.com/marketplace/automation-cloud/latest/user-guide/certification-program-overview)
- [Building activity packages](https://docs.uipath.com/marketplace/automation-cloud/latest/user-guide/building-activity-packages)
