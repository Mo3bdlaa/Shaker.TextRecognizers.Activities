# Icons

Source vectors for the suite. Single-stroke line glyphs drawn with `stroke="currentColor"`,
so they adapt to Studio's light/dark theme.

## Per-activity icons

Every activity carries its own glyph in Studio's panel, so **Recognize** and **Parse** are
told apart at a glance: the Recognize glyph stands for the whole domain, the Parse glyph for
pulling a single value out of it.

| File | Activity |
|------|----------|
| `calendar.svg` | Recognize Date/Time |
| `clock.svg` | Parse Date/Time |
| `hash.svg` | Recognize Numbers |
| `digit-one.svg` | Parse Number |
| `ruler.svg` | Recognize Measurements |
| `gauge.svg` | Parse Measurement |
| `envelope.svg` | Recognize Sequences |
| `link.svg` | Parse Sequence |
| `toggle.svg` | Recognize Booleans |
| `check-circle.svg` | Parse Boolean |

These ship as inline WPF `GeometryDrawing` inside each `*.Design` project - no image file is
packaged for them. The SVG here is the source the WPF geometry mirrors; edit both together.

## Package icon

| File | Used by |
|------|---------|
| `text-span.svg` | all five packages |

One mark for the whole suite - lines of text with a single span picked out, which is what the
suite does - so the packages read as one set in Manage Packages. It is rendered to
`src/<package>/icon.png` (128x128) and packed via `<PackageIcon>`:

```
pip install cairosvg
python tools/render-package-icons.py
```

Edit `text-span.svg` and re-run that to refresh every package icon at once. Swap in your own
branded artwork anytime - keep the same file name and the wiring picks it up unchanged.

## Wiring them into Studio

UiPath renders activity/toolbox icons through a **design assembly** (a `*.Design` project).
That visual result can only be verified inside an actual UiPath Studio, so the design
assemblies are built and validated as a dedicated pass - see
[../../docs/studio-design-assembly.md](../../docs/studio-design-assembly.md).
