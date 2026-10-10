# Maintenance notes

The solution contains the updater and all nine launcher variants. Shared launch and
sandbox code lives in `Shared/`. Projects use .NET 10 Windows Forms and SDK-style
MSBuild so `dotnet` and C# Dev Kit load the same projects. Generated settings classes
from the old project are excluded: the settings files are empty and unused.

## Rendering failure

Reproduced on a copy of Edge **153.0.4234.48** from the existing portable installation:
headless navigation with a new profile timed out before the repair. With read/execute
access for `S-1-15-2-1` (ALL APPLICATION PACKAGES) and `S-1-15-2-2`
(ALL RESTRICTED APPLICATION PACKAGES), the same browser rendered the test HTML and
executed JavaScript successfully. No sandbox-disabling flags are used.

`SandboxAccess.Ensure` grants these two identities read/execute access to:

- The browser directory itself, without inheritance.
- Its top-level EXE and DLL files.
- Files and directories inside numeric version directories containing `msedge.dll`.

The portable root, `Profile.txt`, profile folders, and unrelated directories are not
granted access. Existing ACL entries are preserved; repeat runs do not add duplicate
rules. Reparse points encountered in the selected binary tree are rejected. The
filesystem must support Windows ACLs, and the user must be able to set these
permissions when they are missing. An error is shown if repair is not possible.

The launcher repairs existing installations before launching; the installer does it
after installing a new version. A separate profile is always used for automated
rendering tests. Never run tests against a person's normal browser profile.

## Download and installation

Metadata comes from Microsoft's CDP API; policy templates use its enterprise JSON
endpoint. Versions are indexed separately for x86 and x64. Only a full installer
matching the exact architecture and version is selected, never a delta package.
The legacy HTTP-only Microsoft `msedge.f` download hosts are mapped to their HTTPS
`msedge.sf` counterparts. Downloaded installers and templates must match the SHA-256
provided by the HTTPS metadata response before use.

Downloads and extraction are awaited, cancelled on close, and checked for errors.
Each operation has its own staging directory. The existing browser version is kept
until extraction and validation succeed. Reinstalling the same version uses a
rollback directory; executable and version-log writes use temporary files. Older
version directories are retained. Remove obsolete versions manually only with Edge
closed and after confirming the current installation works.

The fork does not download updater/launcher replacements from the original author's
repository. Publish and distribute the fork's own package explicitly. The inherited
`Launcher/Launcher.7z` has been removed from Git tracking; the new build produces its
own launchers from source. Personal IDE settings, build outputs, local profiles,
test downloads, and deployment backups are not part of the source or release archives.

## Verification

```powershell
dotnet build 'Portable Edge (Chromium) Updater.sln'
dotnet run --project tests/PortableEdge.Tests
dotnet run --project tests/PortableEdge.Tests --no-build -- --ui-smoke "$pwd"
dotnet run --project tests/PortableEdge.Tests --no-build -- --online
dotnet run --project tests/PortableEdge.Tests --no-build -- --install-smoke "$pwd/artifacts/install-smoke" 'C:/path/to/7zr.exe'
./scripts/Test-Rendering.ps1 -EdgePath './artifacts/install-smoke/installed/msedge.exe' -OutputDirectory './artifacts/render-test'
```

The online tests require internet access. The install smoke test downloads a full
Stable x64 browser and installs it only beneath the specified test output directory.
The render test requires PowerShell 7 and starts Edge headlessly with a unique fresh
profile. Its internal `--edge-skip-compat-layer-relaunch` flag is for preserving test
process handles; production launchers do not use it.

## Release packaging

Publish the self-contained runtime variant and include the official 7zr.exe. Verify
its SHA-256 against the upstream release asset metadata before distribution. Update
the changelog, bundled runtime notices, and third-party component versions together.

```powershell
./scripts/Publish.ps1 -SelfContained -SevenZipPath 'C:/path/to/7zr.exe'
./scripts/Package-Release.ps1 -Version 2.0.0 -Runtime win-x64
```

The packaging script selects executables and documentation explicitly. It excludes
PDBs, local build settings, browser binaries, test profiles, logs, and backups. It
writes a ZIP, an external SHA-256 checksum, and an internal file checksum manifest
under `artifacts/releases/`. Refuse to overwrite an existing release artifact;
publish a new version when changing a released package.

Reference documentation:

- [.NET Windows Forms migration](https://learn.microsoft.com/dotnet/desktop/winforms/migration/)
- [Chromium Windows sandbox and filesystem ACL requirements](https://chromium.googlesource.com/chromium/src/+/main/docs/design/sandbox.md)
- [Microsoft enterprise release metadata](https://edgeupdates.microsoft.com/api/products?view=enterprise)
- [Windows default-app settings](https://learn.microsoft.com/windows/apps/develop/launch/launch-default-apps-settings)
