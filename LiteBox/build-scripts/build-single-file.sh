#!/bin/bash
echo "Building LiteBox Optimizer as single file executable..."

# Clean previous builds
rm -rf ./publish

# Build for Windows x64 (main target)
echo "Building for Windows x64..."
dotnet publish LiteBoxOptimizer.csproj -c Release -r win-x64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=true \
    -p:PublishReadyToRun=true \
    -p:IncludeNativeLibrariesForSelfExtract=true \
    -p:EnableCompressionInSingleFile=true \
    -o "./publish/win-x64"

echo "Build completed! Check the publish/win-x64 folder."
echo "File info:"
ls -lh "./publish/win-x64/LiteBoxOptimizer.exe" 2>/dev/null || echo "Build failed or file not found"
