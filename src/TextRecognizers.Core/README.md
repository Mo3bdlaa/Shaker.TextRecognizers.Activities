# TextRecognizers.Core

Shared foundation for the **Text Recognizers** UiPath activity suite. This is **not** published
as its own package — its DLL is embedded directly into each domain activity package
(`TextRecognizers.DateTime.Activities`, `…Number.Activities`, etc.), so every package is fully
self-contained with nothing extra to install.

It provides:

- `RecognizerActivity` — the base activity with the common **Text** and **Language** inputs.
- `CultureOption` — the friendly language drop-down (rendered as a combo box in Studio, but still
  variable-bindable) and its mapping to Microsoft.Recognizers culture codes.

Lightweight by design: it has **no third-party dependencies**. The `System.Activities` workflow
runtime it builds against is supplied by Studio / the Robot at run time.

MIT © 2026 Mohammed Shaker · https://mohammedshaker.com
