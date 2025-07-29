@echo off
echo Building LiteBox Optimizer as single file executable...

REM Clean previous builds
if exist "publish" rmdir /s /q "publish"

REM Build for Windows x64
echo Building for Windows x64...
dotnet publish LiteBoxOptimizer.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true -p:PublishReadyToRun=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -o "./publish/win-x64"

echo Build completed! Check the publish/win-x64 folder for your executable.
if exist "publish\win-x64\LiteBoxOptimizer.exe" (
    echo File created successfully: LiteBoxOptimizer.exe
) else (
    echo Build failed or file not found!
)
pause
