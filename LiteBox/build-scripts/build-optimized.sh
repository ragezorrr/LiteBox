#!/bin/bash
echo "Building LiteBox Optimizer as optimized single file executable..."

# Clean previous builds
rm -rf ./publish

# Build optimized version for Windows x64 with minimal size
echo "Building optimized version for Windows x64..."
dotnet publish LiteBoxOptimizer.csproj -c Release -r win-x64 \
    --self-contained true \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=true \
    -p:PublishReadyToRun=false \
    -p:EnableCompressionInSingleFile=true \
    -p:IncludeNativeLibrariesForSelfExtract=true \
    -p:TrimMode=link \
    -p:SuppressTrimAnalysisWarnings=true \
    -p:DebuggerSupport=false \
    -p:EnableUnsafeUTF7Encoding=false \
    -p:HttpActivityPropagationSupport=false \
    -p:InvariantGlobalization=true \
    -o "./publish/win-x64-optimized"

echo "Optimized build completed! Check the publish/win-x64-optimized folder."
echo "File size:"
ls -lh "./publish/win-x64-optimized/LiteBoxOptimizer.exe" 2>/dev/null || echo "Build failed or file not found"
