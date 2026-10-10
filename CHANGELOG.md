# Changelog

## 2.0.1 — 2026-09-24

- Make the updater and all nine launchers consistently English, regardless of the
  Windows display language. Translate menus, options, profile choices and browser
  registration descriptions; remove the incomplete German/Russian language switching.
- Keep regional number and date formatting unchanged.
- Check every updater menu and launcher profile choice under English, Dutch, German
  and Russian UI cultures in the UI regression tests.

## 2.0.0 — 2026-09-24

- Fix blank pages in portable Edge installations caused by missing Windows sandbox
  read/execute permissions. Browser sandboxing stays enabled; profile folders are
  excluded from the repair.
- Migrate the updater and all nine launchers to .NET 10 SDK-style projects for
  Visual Studio, VS Code C# Dev Kit, and `dotnet` builds.
- Share launcher code, preserve URL/file arguments, and resolve portable profile
  paths reliably. Closing the profile picker no longer causes a file-read failure.
- Replace obsolete networking APIs and HTML/string scraping with asynchronous
  HTTP and JSON metadata parsing. Track browser versions separately for x86/x64.
- Download from Microsoft's HTTPS CDN and verify SHA-256 before installation.
- Stage and validate updates, check extraction failures, and roll back a failed
  installation. Preserve existing profiles and retain older browser versions.
- Stop downloading updater/launcher replacements from the original repository.
- Use Windows Settings to choose the default browser instead of modifying protected
  UserChoice registry values.
- Add regression tests, UI loading tests, an isolated rendering test, build CI,
  publishing scripts, and maintenance documentation.

The published binary package targets Windows x64 and includes .NET 10.0.11 and
7zr.exe 26.03. No separate .NET installation is needed. Edge browser binaries and
user profiles are not included. The source supports publishing for other Windows
host architectures, but this release's end-to-end validation was performed on x64.

Existing installations must replace their root launcher as well as the updater
and `Bin` files; see the README. Original .NET Framework 4.x compatibility is removed.
