param(
    [string]$Version = '2.0.1',
    [ValidateSet('win-x64', 'win-x86', 'win-arm64')][string]$Runtime = 'win-x64'
)
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$package = Join-Path $root "artifacts/package/$Runtime"
$name = "EdgePortable-v$Version-$Runtime"
if ($Version -notmatch '^\d+\.\d+\.\d+$') { throw 'Use a three-part numeric release version.' }
$output = Join-Path $root 'artifacts/releases'
$stage = Join-Path $output ('staging-' + [guid]::NewGuid().ToString('N'))
$archive = Join-Path $output "$name.zip"
if (Test-Path -LiteralPath $archive) { throw "Archive already exists: $archive" }
New-Item -ItemType Directory -Path $stage -Force | Out-Null
foreach ($relative in @('Portable Edge (Chromium) Updater.exe', 'PACKAGE.txt', 'Bin/7zr.exe')) {
    $destination = Join-Path $stage $relative
    New-Item -ItemType Directory -Path (Split-Path $destination -Parent) -Force | Out-Null
    Copy-Item -LiteralPath (Join-Path $package $relative) -Destination $destination
}
foreach ($relative in @('README.md', 'CHANGELOG.md', 'LICENSE', 'THIRD-PARTY-NOTICES.md', 'docs/MAINTENANCE.md')) {
    $destination = Join-Path $stage $relative
    New-Item -ItemType Directory -Path (Split-Path $destination -Parent) -Force | Out-Null
    Copy-Item -LiteralPath (Join-Path $root $relative) -Destination $destination
}
New-Item -ItemType Directory -Path (Join-Path $stage 'licenses') -Force | Out-Null
Get-ChildItem -LiteralPath (Join-Path $root 'licenses') -File | Copy-Item -Destination (Join-Path $stage 'licenses')
$launchers = @(Get-ChildItem -LiteralPath (Join-Path $package 'Bin/Launcher') -Filter '*.exe')
if ($launchers.Count -ne 9) { throw 'Expected all nine published launchers.' }
New-Item -ItemType Directory -Path (Join-Path $stage 'Bin/Launcher') -Force | Out-Null
$launchers | Copy-Item -Destination (Join-Path $stage 'Bin/Launcher')
$manifest = Get-ChildItem -LiteralPath $stage -File -Recurse | Sort-Object FullName | ForEach-Object {
    $relative = [IO.Path]::GetRelativePath($stage, $_.FullName).Replace('\', '/')
    '{0}  {1}' -f (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash.ToLowerInvariant(), $relative
}
$manifest | Set-Content -LiteralPath (Join-Path $stage 'FILES.sha256') -Encoding utf8NoBOM
Compress-Archive -Path (Join-Path $stage '*') -DestinationPath $archive -CompressionLevel Optimal
$sum = '{0}  {1}' -f (Get-FileHash -LiteralPath $archive -Algorithm SHA256).Hash.ToLowerInvariant(), ([IO.Path]::GetFileName($archive))
$sum | Set-Content -LiteralPath (Join-Path $output "$name.sha256") -Encoding utf8NoBOM
Write-Output "Release archive: $archive"
Write-Output $sum
