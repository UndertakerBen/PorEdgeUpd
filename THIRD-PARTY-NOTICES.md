# Third-party components

The project itself is MIT licensed; see `LICENSE`, which retains the original
author's copyright notice.

## .NET 10.0.11

The self-contained Windows release includes Microsoft .NET and Windows Desktop
runtime components obtained from Microsoft's official runtime NuGet packages.
Their licenses and third-party notices are included in `licenses/`:

- `dotnet-runtime-LICENSE.txt`
- `dotnet-runtime-THIRD-PARTY-NOTICES.txt`
- `dotnet-windowsdesktop-LICENSE.txt`

Sources: [dotnet/runtime](https://github.com/dotnet/runtime/tree/v10.0.11),
[dotnet/windowsdesktop](https://github.com/dotnet/windowsdesktop/tree/v10.0.11),
[dotnet/winforms](https://github.com/dotnet/winforms/tree/v10.0.11), and
[dotnet/wpf](https://github.com/dotnet/wpf/tree/v10.0.11).
Refresh the bundled notices when updating runtime dependencies.

## 7zr.exe 26.03

Copyright Igor Pavlov. 7zr.exe is part of the LZMA SDK and is in the public domain.
The release includes the unmodified executable from the official 7-Zip release:

- Download: https://github.com/ip7z/7zip/releases/download/26.03/7zr.exe
- Source: https://github.com/ip7z/7zip/tree/26.03
- License statement: https://www.7-zip.org/sdk.html
- SHA-256: `ad4c82fadcbdf93c03b4fc440f300509c7d60c5c2f4d183e35d9d70d6957037d`

## Microsoft Edge

Microsoft Edge is not bundled. The updater downloads browser installers and policy
templates from Microsoft. Edge and its associated marks belong to Microsoft;
this project is not affiliated with Microsoft.
