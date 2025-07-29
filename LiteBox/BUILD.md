# LiteBox Optimizer - Build Instructions

## Quick Start

### Build Single File Executable

#### Windows (using CMD):
```batch
build-scripts\build-single-file.bat
```

#### Linux/macOS:
```bash
./build-scripts/build-single-file.sh
```

#### Manual command:
```bash
dotnet publish LiteBoxOptimizer.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true -o "./publish/win-x64"
```

### Optimized Build (smaller size):
```bash
./build-scripts/build-optimized.sh
```

## Output

The executable will be created in:
- Standard build: `./publish/win-x64/LiteBoxOptimizer.exe`
- Optimized build: `./publish/win-x64-optimized/LiteBoxOptimizer.exe`

## Requirements

- .NET 8.0 SDK
- Target: Windows 11 (primary platform)

## File Size

- Standard build: ~100-130 MB
- Optimized build: ~80-100 MB

The executable is completely standalone and includes the .NET runtime.
