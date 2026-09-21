# Studio appearance: categories & icons

This note covers how the activities show up in Studio's activities panel, what is already wired,
and the one remaining piece that has to be validated inside a real Studio.

## ✅ Toolbox categories — already done (attributes only)

Each activity class carries a class-level `[Category("…")]` attribute, and dots create a
hierarchy. So the panel groups them like this:

```
TextRecognizers
├─ DateTimes                Recognize Date/Time, Parse Date/Time
├─ Numbers                  Recognize Numbers, Parse Number
├─ Measurements             Recognize Measurements, Parse Measurement
├─ Sequences                Recognize Sequences, Parse Sequence
└─ Choices                  Recognize Booleans, Parse Boolean
```

No design assembly is needed for this — it is pure metadata that Studio reads from the activity.

## ✅ Package icon — already done

The package carries an icon in **Manage Packages**: a 128×128 `icon.png` declared with
`<PackageIcon>`, so it is packed into the `.nupkg` and shows beside the package in Studio.

It is generated from `assets/icons/text-span.svg` by `tools/render-package-icons.py` — edit
the glyph, re-run it, and the icon refreshes. See
[assets/icons/README.md](../assets/icons/README.md).

## ✅ Per-activity icons in the panel — one design assembly

The little icon next to each activity in the panel comes from a **design assembly**
(`Shaker.TextRecognizers.Activities.Design`) that renders a WPF `ActivityDesigner.Icon`.
Every activity has its own:

| Activity | Icon |
|---|---|
| Recognize Date/Time | calendar |
| Parse Date/Time | clock |
| Recognize Numbers | hash `#` |
| Parse Number | numeral `1` |
| Recognize Measurements | ruler |
| Parse Measurement | gauge |
| Recognize Sequences | envelope |
| Parse Sequence | link |
| Recognize Booleans | toggle |
| Parse Boolean | check in a circle |

The designer classes live in one folder per domain inside the design project, mirroring the
activity package's own layout.

Each activity has its own designer class, so **Recognize** and **Parse** are told apart at a
glance: the Recognize icon stands for the whole domain, and the Parse icon for pulling one
value out of it.

The assembly is a `net6.0-windows` `<UseWPF>` project that references the activity project and
the WF presentation assemblies (resolved from the local UiPath Studio install via
`UiPathStudioDir`). Icons are inline `DrawingBrush` vector geometry — no image files to ship. All
ten designers are registered through a single `IRegisterMetadata` (`DesignerMetadata.cs`) that
Studio discovers automatically.

### Building (the design assembly is not in the solution)
The `*.Design` project is kept **out of `Shaker.TextRecognizers.slnx`** so the main build/test/pack
works on machines without Studio. Build it once on a machine that has UiPath Studio installed,
then pack — the package picks up `Shaker.TextRecognizers.Activities.Design.dll` automatically
(the runtime `.csproj` embeds it next to the runtime DLL via an `Exists(...)` condition):

```powershell
# 1) build the design assembly (requires UiPath Studio installed)
dotnet build src/Shaker.TextRecognizers.Activities.Design/Shaker.TextRecognizers.Activities.Design.csproj -c Release

# 2) pack — the design DLL is embedded into the .nupkg
dotnet pack Shaker.TextRecognizers.slnx -c Release -o build/packages
```

If Studio is **not** installed, skip step 1: the package still builds and works, just without the
per-activity panel icons (the `Exists(...)` condition simply finds no design DLL to embed).

The `assets/icons/*.svg` files are the same glyphs in source form, for reuse as package icons or docs.

## How to validate in Studio
1. Run the two steps above.
2. Add `build/packages` as a package source in Studio (Manage Packages → Settings).
3. Install the package and confirm: activities appear under **TextRecognizers**, each shows its icon,
   tooltips show on hover, and the **Language**/**Kind** fields render as drop-downs.
