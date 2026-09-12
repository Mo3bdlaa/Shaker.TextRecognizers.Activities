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

## ◻️ Package icon — one line to add (optional)

To give a package its own icon in **Manage Packages**, drop a PNG in the project and reference it:

```xml
<PropertyGroup>
  <PackageIcon>icon.png</PackageIcon>
</PropertyGroup>
<ItemGroup>
  <None Include="icon.png" Pack="true" PackagePath="\" />
</ItemGroup>
```

The source glyphs live in `assets/icons/*.svg`; export one to a 128×128 PNG per package.

## ✅ Per-activity icons in the panel — design assemblies (one per domain)

The little icon next to each activity in the panel comes from a **design assembly**
(`*.Activities.Design`) that renders a WPF `ActivityDesigner.Icon`. Every domain now has one:

| Design assembly | Activities it decorates | Icon |
|---|---|---|
| `TextRecognizers.DateTime.Activities.Design` | Recognize/Parse Date/Time | calendar |
| `TextRecognizers.Number.Activities.Design` | Recognize/Parse Number | hash `#` |
| `TextRecognizers.NumberWithUnit.Activities.Design` | Recognize/Parse Measurement | ruler |
| `TextRecognizers.Sequence.Activities.Design` | Recognize/Parse Sequence | envelope |
| `TextRecognizers.Choice.Activities.Design` | Recognize/Parse Boolean | toggle |

Each assembly is a `net6.0-windows` `<UseWPF>` project that references its activities project and
the WF presentation assemblies (resolved from the local UiPath Studio install via
`UiPathStudioDir`). Icons are inline `DrawingBrush` vector geometry — no image files to ship. The
designers are registered through an `IRegisterMetadata` (`DesignerMetadata.cs`) that Studio
discovers automatically.

### Building (the design assemblies are not in the solution)
The `*.Design` projects are kept **out of `TextRecognizers.slnx`** so the main build/test/pack
works on machines without Studio. Build them once on a machine that has UiPath Studio installed,
then pack — each domain package picks up its `*.Design.dll` automatically (the runtime `.csproj`
embeds it next to the runtime DLL via an `Exists(...)` condition):

```powershell
# 1) build the design assemblies (requires UiPath Studio installed)
Get-ChildItem src -Recurse -Filter *.Activities.Design.csproj |
  ForEach-Object { dotnet build $_.FullName -c Release }

# 2) pack — the design DLLs are embedded into each .nupkg
dotnet pack TextRecognizers.slnx -c Release -o build/packages
```

If Studio is **not** installed, skip step 1: the packages still build and work, just without the
per-activity panel icons (the `Exists(...)` condition simply finds no design DLL to embed).

The `assets/icons/*.svg` files are the same glyphs in source form, for reuse as package icons or docs.

## How to validate in Studio
1. Run the two steps above.
2. Add `build/packages` as a package source in Studio (Manage Packages → Settings).
3. Install a package and confirm: activities appear under **TextRecognizers**, each shows its icon,
   tooltips show on hover, and the **Language**/**Kind** fields render as drop-downs.
