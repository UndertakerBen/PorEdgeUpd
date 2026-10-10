param(
    [ValidateSet('win-x64', 'win-x86', 'win-arm64')][string]$Runtime = 'win-x64',
    [switch]$SelfContained,
    [string]$SevenZipPath
)
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$version = (Get-Content -LiteralPath (Join-Path $root 'Version.txt') -Raw).Trim()
$destination = Join-Path $root "artifacts/package/$Runtime"
$contained = $SelfContained.IsPresent.ToString().ToLowerInvariant()
$projects = @(Get-Item -LiteralPath (Join-Path $root 'Portable Edge (Chromium) Updater.csproj')) + @(Get-ChildItem (Join-Path $root 'Launcher') -Recurse -Filter *.csproj)
foreach ($project in $projects) {
    $output = if ($project.DirectoryName -eq $root) { $destination } else { Join-Path $destination 'Bin/Launcher' }
    & dotnet publish $project.FullName -c Release -r $Runtime --self-contained $contained -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o $output --nologo -v:q
    if ($LASTEXITCODE -ne 0) { throw "Publish failed: $($project.Name)" }
}
# Launchers must be single-file: the updater copies just the selected executable.
if ($SevenZipPath) {
    Copy-Item -LiteralPath $SevenZipPath -Destination (Join-Path $destination 'Bin/7zr.exe') -Force
}
Copy-Item -LiteralPath (Join-Path $root 'README.md') -Destination $destination -Force
Copy-Item -LiteralPath (Join-Path $root 'LICENSE') -Destination $destination -Force
Copy-Item -LiteralPath (Join-Path $root 'CHANGELOG.md') -Destination $destination -Force
Copy-Item -LiteralPath (Join-Path $root 'THIRD-PARTY-NOTICES.md') -Destination $destination -Force
New-Item -ItemType Directory -Path (Join-Path $destination 'licenses') -Force | Out-Null
Get-ChildItem -LiteralPath (Join-Path $root 'licenses') -File | Copy-Item -Destination (Join-Path $destination 'licenses') -Force
New-Item -ItemType Directory -Path (Join-Path $destination 'docs') -Force | Out-Null
Copy-Item -LiteralPath (Join-Path $root 'docs/MAINTENANCE.md') -Destination (Join-Path $destination 'docs') -Force
@"
Portable Edge Updater $version
Runtime: $Runtime
Self-contained: $contained
Built: $([DateTime]::UtcNow.ToString('u'))
"@ | Set-Content -LiteralPath (Join-Path $destination 'PACKAGE.txt')
Write-Output "Package: $destination"
