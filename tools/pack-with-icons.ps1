<#
.SYNOPSIS
    Builds the design assembly, packs the suite, and verifies the per-activity icons
    actually made it into the package.

.DESCRIPTION
    Run this on a machine with UiPath Studio installed. The design assembly carries the
    per-activity panel icons and references System.Activities.Presentation, which ships
    with Studio and is published on no public feed - so this is the only place the
    complete package can be produced. A build anywhere else silently omits it and every
    activity shows a blank icon.

    RequireDesignAssembly=true makes that omission a hard error rather than a warning, so
    this script cannot quietly produce an icon-less package.

.PARAMETER Output
    Where to write the .nupkg. Defaults to build/packages.

.PARAMETER UiPathStudioDir
    Override if Studio is not at the default install location.

.EXAMPLE
    ./tools/pack-with-icons.ps1
#>
[CmdletBinding()]
param(
    [string] $Output = "build/packages",
    [string] $UiPathStudioDir
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot
Push-Location $root

try {
    $designProj = "src/Shaker.TextRecognizers.Activities.Design/Shaker.TextRecognizers.Activities.Design.csproj"

    $buildArgs = @("build", $designProj, "-c", "Release")
    if ($UiPathStudioDir) { $buildArgs += "-p:UiPathStudioDir=$UiPathStudioDir" }

    Write-Host "==> Building the design assembly (per-activity icons)" -ForegroundColor Cyan
    dotnet @buildArgs
    if ($LASTEXITCODE -ne 0) {
        throw "The design assembly failed to build. It needs UiPath Studio installed; pass -UiPathStudioDir if Studio is not at the default location."
    }

    Write-Host "==> Packing" -ForegroundColor Cyan
    dotnet pack Shaker.TextRecognizers.slnx -c Release -o $Output -p:RequireDesignAssembly=true
    if ($LASTEXITCODE -ne 0) { throw "Pack failed." }

    # Trust the artifact, not the build log: look inside it.
    Write-Host "==> Verifying the icons are in the package" -ForegroundColor Cyan
    $pkg = Get-ChildItem $Output -Filter "Shaker.TextRecognizers.Activities.*.nupkg" |
           Sort-Object LastWriteTime -Descending | Select-Object -First 1
    if (-not $pkg) { throw "No package found in $Output." }

    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $zip = [System.IO.Compression.ZipFile]::OpenRead($pkg.FullName)
    try {
        $hasDesign = $zip.Entries | Where-Object { $_.Name -eq "Shaker.TextRecognizers.Activities.Design.dll" }
        $hasIcon   = $zip.Entries | Where-Object { $_.Name -eq "icon.png" }
    } finally { $zip.Dispose() }

    if (-not $hasDesign) { throw "$($pkg.Name) has no design assembly - the activities would show no icons." }
    if (-not $hasIcon)   { throw "$($pkg.Name) has no package icon." }

    Write-Host ""
    Write-Host "OK  $($pkg.Name)  ($([math]::Round($pkg.Length/1MB,2)) MB)" -ForegroundColor Green
    Write-Host "    per-activity icons: present"
    Write-Host "    package icon:       present"
    Write-Host ""
    Write-Host "This is the artifact to upload to UiPath Marketplace."
}
finally { Pop-Location }
