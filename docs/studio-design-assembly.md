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
the WF presentation assemblies (compiled against the reference stubs in `stubs/`; see below).
Icons are inline `DrawingBrush` vector geometry — no image files to ship. All
ten designers are registered through a single `IRegisterMetadata` (`DesignerMetadata.cs`) that
Studio discovers automatically.

### Which project types the package supports

The package multi-targets **`net6.0`** and **`net6.0-windows`**. Both, not one: Studio reads the
target-framework groups in the package manifest to decide which project types it supports, and a
package offering only `net6.0-windows7.0` is reported as
*"This package is not compatible with Windows projects"*.

The designers are WPF, so they ship in `lib/net6.0-windows7.0/` only — `lib/net6.0` is the
frameworkless fallback and cannot load them. A Cross-platform project therefore gets the
activities with Studio's stock designers, and a Windows project gets the per-activity icons.

The same error appears for a second reason worth knowing: Studio ships `System.Activities`
**6.0.0.0**, and an assembly compiled against a higher version cannot be loaded by it. The
activities reference `UiPath.Workflow.Runtime 6.0.0-20220401-03` from UiPath's own feed, which
is that build — the `UiPath.Workflow*` packages on nuget.org carry 6.0.3.0 and do not load.

### Building

The design assembly is part of the solution and builds anywhere — no UiPath Studio needed:

```powershell
dotnet build Shaker.TextRecognizers.slnx -c Release
dotnet pack src/Shaker.TextRecognizers.Activities/Shaker.TextRecognizers.Activities.csproj `
  -c Release -o build/packages --no-build -p:RequireDesignAssembly=true
```

`RequireDesignAssembly=true` turns a missing design assembly into an error instead of a
warning, so an icon-less package cannot be produced by accident. `tools/pack-with-icons.ps1`
does both steps and then opens the `.nupkg` to check the icons are really inside.

Pack the package project rather than the solution: the design and stub projects are
`IsPackable=false`, and `--no-build` trips `NETSDK1085` on them.

### How it builds without Studio

`System.Activities.Presentation` (`ActivityDesigner`) and `System.Activities.Metadata`
(`IRegisterMetadata`, `AttributeTableBuilder`, `MetadataStore`) ship **only with UiPath
Studio**. Neither is on nuget.org or on UiPath's public feed, and neither is inside
`UiPath.Workflow`.

Compiling still needs *something* with the right identity, so `stubs/` holds two small
projects standing in for them — the same types and members the designers touch, under the
assembly names and version Studio loads. They are referenced with `Private="false"`, so they
never reach the output or the package, and Studio's real assemblies are what load at run
time. See [stubs/README.md](../../stubs/README.md).

Keep the stub signatures identical to the real ones: a mismatch compiles happily and fails
only when Studio loads the designer.

## How to validate in Studio
1. Run the two steps above.
2. Add `build/packages` as a package source in Studio (Manage Packages → Settings).
3. Install the package and confirm: activities appear under **TextRecognizers**, each shows its icon,
   tooltips show on hover, and the **Language**/**Kind** fields render as drop-downs.
