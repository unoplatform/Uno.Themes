# ALC Host Sample App

This sample demonstrates using .NET's `AssemblyLoadContext` (ALC) to dynamically load and host
either the **MaterialSampleApp** or **CupertinoSampleApp** within a host application's visual tree.

## How It Works

1. The host app creates an `AlcContentHost` (from Uno.UI) and sets it as `WindowHelper.ContentHostOverride`
2. A custom `SampleAppAssemblyLoadContext` loads the secondary app's assembly, sharing Uno framework assemblies with the host
3. The secondary app's `Program.Main()` is invoked on a background thread
4. When the secondary app sets `Window.Content`, Uno.UI redirects it into the host's `AlcContentHost`

## Prerequisites

- .NET 10 SDK
- Build either MaterialSampleApp or CupertinoSampleApp first (for a desktop target):

```bash
# Build MaterialSampleApp for desktop
dotnet build ../MaterialSampleApp/MaterialSampleApp.csproj -f net10.0-desktop

# Build CupertinoSampleApp for desktop
dotnet build ../CupertinoSampleApp/CupertinoSampleApp.csproj -f net10.0-desktop
```

## Building & Running

```bash
# Build the ALC host
dotnet build AlcHostSampleApp.csproj -f net10.0-desktop

# Run it
dotnet run --project AlcHostSampleApp.csproj -f net10.0-desktop
```

## Configuration

### Assembly Path Resolution

The app looks for sample app assemblies in these locations (in order):

1. **Environment variable**: `ALC_SAMPLE_MATERIALSAMPLEAPP_PATH` or `ALC_SAMPLE_CUPERTINO SAMPLEAPP_PATH`
2. **Same output directory**: If the DLL is copied alongside the host app
3. **Sibling project output**: `../MaterialSampleApp/bin/Debug/net10.0-desktop/MaterialSampleApp.dll`

### SDK Version

This project uses `Uno.Sdk.Private` version `6.6.0-alc.109` from the Uno Features NuGet feed,
which includes ALC support (`UnoEnableAlcAppSupport`).

## Architecture

```
AlcHostSampleApp (Host)
├── App.xaml / App.xaml.cs          — Host application entry
├── MainPage.xaml / .cs             — UI with load/unload buttons + AlcContentHost
├── SampleAppAssemblyLoadContext.cs — Custom ALC that shares Uno assemblies
├── SampleAppLoader.cs              — Orchestrates loading/starting secondary apps
└── Platforms/
    ├── Desktop/Program.cs
    └── WebAssembly/Program.cs
```

## Known Issues

- **WASM build**: The `Uno.Wasm.Bootstrap 10.1.0-dev.100` bundled with this SDK version has a
  `ShellTask` issue where `GeneratePWAContent` fails with `UnauthorizedAccessException` on the
  `unoresizetizer/` directory. Desktop builds work correctly.
- **Single host**: Only one `ContentHostOverride` can be active at a time — you cannot host
  multiple secondary ALC apps simultaneously.
