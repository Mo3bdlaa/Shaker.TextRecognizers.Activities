# Offline / airgapped deployment

These activities are designed to run in fully **airgapped** UiPath environments — no internet
access at design time or run time. This guide explains why that works and how to deploy.

## Why it is safe offline

1. **The recognition engine never touches the network.** Microsoft.Recognizers.Text is
   grammar/pattern-based. It downloads no models and makes no calls; results are deterministic.
2. **Each package is self-contained.** The `Microsoft.Recognizers.*` engine assemblies (and the
   `Microsoft.Extensions.*` caching libraries they use) **and** the shared `TextRecognizers.Core`
   assembly are **embedded inside the `.nupkg`**, so installing the package does **not** trigger
   any external NuGet restore.

You can confirm this for any domain package by opening its `.nupkg` (it is a ZIP) and looking at
`lib/net6.0-windows7.0/` — the engine DLLs and `TextRecognizers.Core.dll` are present, and the
package declares **no `<dependency>` entries** at all.

## What you must place in the offline feed

Just **one file per domain** — the domain package itself:

```
TextRecognizers.DateTime.Activities.<version>.nupkg
```

The package has **no dependencies** to mirror: the recognizer engine and the shared core are
embedded. (`System.Activities` and its `Nito.*` dependency are the workflow runtime and are
already provided by Studio / the Robot.)

## Deploy

Pick whichever matches your setup:

### A) Orchestrator tenant/host feed (recommended)
1. In Orchestrator, go to **Tenant → Settings → (or Host) → Libraries / Packages feed**.
2. Upload the domain package(s) you need.
3. In Studio, make sure **Manage Packages → Settings** points at the Orchestrator/official feed.
4. **Manage Packages → All Packages**, search `TextRecognizers`, install the domain package.

### B) A shared folder / local NuGet feed
1. Copy the `.nupkg` files into your offline feed folder (e.g. `\\fileserver\uipath-feed`).
2. In Studio: **Manage Packages → Settings → Add** a source pointing at that folder; **Save**.
3. Install the package from that source.

## Verify after install

- The activities appear in Studio's panel and run without any network access.
- A quick check: **Parse Date/Time** with Text `"tomorrow"` returns a `Result` with tomorrow's date.

## Building the packages (on a connected machine)

Produce the `.nupkg` files on a machine that *does* have access to nuget.org + the UiPath feed,
then carry them in:

```powershell
dotnet pack TextRecognizers.slnx -c Release -o build/packages
```

The embedding happens automatically during `pack` (see the `IncludeRecognizerAssembliesInPackage`
target in the domain project). The resulting files under `build/packages/` are what you move into
the airgapped feed.
