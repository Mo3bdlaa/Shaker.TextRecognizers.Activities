# Activity icons

Source icons for the Text Recognizers activities. Simple single-stroke line
glyphs drawn with `stroke="currentColor"`, so they adapt to Studio's light/dark theme.

| File | Used by |
|------|---------|
| `calendar.svg` | Recognize Date/Time, and the DateTime category |
| `clock.svg` | Parse Date/Time |

## Wiring them into Studio

UiPath renders activity/toolbox icons through a **design assembly** (a
`*.Activities.Design` project that implements the `UiPath.Studio.Activities.Api`
design hook). That visual result can only be verified inside an actual UiPath Studio,
so the design assembly is built and validated as a dedicated pass.

To use these as raster icons, export each SVG to a 16×16 (and 24×24) PNG, e.g. with
Inkscape:

```
inkscape calendar.svg -w 16 -h 16 -o calendar-16.png
```

Swap in your own branded artwork anytime — keep the same file names and the wiring
picks them up unchanged.
