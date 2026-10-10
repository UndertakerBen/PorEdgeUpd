# PorEdgeUpd
 Portable Edge (Chromium) Updater

Maintained fork for Windows, using **.NET 10**. The updater and all nine launchers
are SDK-style projects, compatible with VS Code's C# Dev Kit.

## Download

Get **EdgePortable-v2.0.1-win-x64.zip** from the
[GitHub releases](https://github.com/ScymicX/EdgePortable/releases/latest).
The Windows x64 release includes the .NET runtime and 7zr.exe. Extract the entire
archive into a writable folder, then open `Portable Edge (Chromium) Updater.exe`
to download Edge. After installation, use the generated launcher to start Edge.
The browser itself is downloaded from Microsoft and is not bundled in this release.
The updater and launchers use English on every Windows display language.

To upgrade an existing installation, close Edge and the updater, make a backup,
and extract the release into the existing portable root. Replace any existing root
launcher with the matching executable from `Bin/Launcher`; for example, copy
`Bin/Launcher/Edge Launcher.exe` over `Edge Launcher.exe`. Keep your profile folders
and `Profile.txt`. The new launcher repairs the browser sandbox permissions on launch.

See the [changelog](CHANGELOG.md) for changes and compatibility notes.

## Build and run

Install the .NET 10 SDK on Windows, open the root solution, then run:

```powershell
dotnet build 'Portable Edge (Chromium) Updater.sln'
dotnet run --project tests/PortableEdge.Tests
```

The plain build output is for development. To create a usable distribution with
single-file launchers, use PowerShell 7:

```powershell
./scripts/Publish.ps1 -SevenZipPath 'C:/path/to/7zr.exe'
```

Output: `artifacts/package/win-x64`. This default package requires the **.NET 10
Windows Desktop Runtime** on the target PC. Add `-SelfContained` to include the
runtime and produce a larger package that does not require .NET to be installed.
`-Runtime win-x86` produces launchers/updater for a 32-bit host; launcher names refer
to the downloaded browser architecture, independently of the host runtime.

Place the updater executable and its `Bin` folder together in the portable root.
For an existing installation, also replace its root `Edge Launcher.exe` (or the
channel-specific launcher) with the matching executable from `Bin/Launcher`.
Keep the `Edge`/channel folders, `Profile.txt` files and profile folders intact.
The updater needs the official `Bin/7zr.exe` to extract Edge installers; it is not
downloaded automatically or stored in this repository.

## Blank pages in an existing portable installation

The new launcher gives Chromium's sandbox processes read/execute access to the
browser binaries. This fixes the reproduced case where Edge opens and loads a
profile but cannot render pages. The change is limited to the browser program files;
it does not grant access to portable profiles or disable the browser sandbox.

Existing `Profile.txt` choices remain supported: a shared portable profile, a
channel-specific profile, or the normal system profile. Relative paths are resolved
against the portable root. Absolute paths and forwarded URLs/file paths are preserved.

See [maintenance and test notes](docs/MAINTENANCE.md) for the diagnosis, repair scope,
update behavior, and additional verification commands. Automatic updater/launcher
downloads from the original author's repository have been removed.
 
## Notice of Non-Affiliation and Disclaimer
PorEdgeUpd is not affiliated, associated, authorized, endorsed by, or in any way officially connected with Microsoft Corporation.
The name Edge as well as related names, marks, emblems and images are registered trademarks  of Microsoft Corporation.

For unpacking the downloaded Edge (Chromium) offline installer i use 7zr.exe

- 7zr.exe - https://www.7-zip.org/ (Public Domain)

Bundled component licenses and sources are listed in [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md).
