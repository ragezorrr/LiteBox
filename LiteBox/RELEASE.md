# Release Process

## 1. Update Version
Edit `LiteBoxOptimizer.csproj` and update version numbers:
```xml
<Version>1.0.1</Version>
<AssemblyVersion>1.0.1.0</AssemblyVersion>
<FileVersion>1.0.1.0</FileVersion>
```

## 2. Build Release
```bash
./build-scripts/build-single-file.sh
```

## 3. Create Git Tag
```bash
git add .
git commit -m "Release v1.0.1"
git tag v1.0.1
git push origin main
git push origin v1.0.1
```

## 4. Create GitHub Release
1. Go to GitHub repository
2. Click "Releases" → "Create a new release"
3. Select the tag (v1.0.1)
4. Add release notes
5. Upload `./publish/win-x64/LiteBoxOptimizer.exe`
6. Click "Publish release"

## Files to Upload
- `LiteBoxOptimizer.exe` (from publish/win-x64/)
- Optional: Create a zip with the executable for easier downloading
