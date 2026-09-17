# Activity icons

Source icons for the Text Recognizers activities. Simple single-stroke line
glyphs drawn with `stroke="currentColor"`, so they adapt to Studio's light/dark theme.

| File | Used by |
|------|---------|
| `calendar.svg` | Recognize Date/Time, and the DateTime category |
| `clock.svg` | Parse Date/Time |
| `hash.svg` | Recognize/Parse Number |
| `ruler.svg` | Recognize/Parse Measurement |
| `envelope.svg` | Recognize/Parse Sequence |
| `toggle.svg` | Recognize/Parse Boolean |

These are the source vectors. The same geometry appears twice in the build:

- **In the activities panel** — redrawn as inline WPF `GeometryDrawing` inside each
  `*.Activities.Design` project, so no image file ships for it.
- **Beside the package in Manage Packages** — rendered to `src/<package>/icon.png`
  (128x128) and packed via `<PackageIcon>`.

## Regenerating the package icons

`tools/render-package-icons.py` renders one PNG per package from the glyphs above -
indigo (`#5B5BD6`, the same colour the panel icons use) on a soft tinted square:

```
pip install cairosvg
python tools/render-package-icons.py
```

Edit a glyph and re-run it to refresh every package icon at once. Swap in your own
branded artwork anytime - keep the same file names and the wiring picks it up unchanged.

## Wiring them into Studio

UiPath renders activity/toolbox icons through a **design assembly** (a
`*.Activities.Design` project). That visual result can only be verified inside an
actual UiPath Studio, so the design assembly is built and validated as a dedicated pass.
