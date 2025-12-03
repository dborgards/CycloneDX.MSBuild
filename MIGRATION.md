# Migration Guide: Microsoft.Sbom.Targets to CycloneDX.MSBuild

This guide helps you migrate from Microsoft.Sbom.Targets (SPDX SBOM) to CycloneDX.MSBuild (CycloneDX SBOM).

## Overview

Both packages provide similar functionality but use different SBOM standards:
- **Microsoft.Sbom.Targets**: Generates SPDX 2.2 SBOMs
- **CycloneDX.MSBuild**: Generates CycloneDX 1.2-1.6 SBOMs

## Quick Migration

### 1. Remove Microsoft.Sbom.Targets

```bash
dotnet remove package Microsoft.Sbom.Targets
```

Or remove from your `.csproj`:

```xml
<ItemGroup>
  <!-- Remove this -->
  <PackageReference Include="Microsoft.Sbom.Targets" Version="*" PrivateAssets="all" />
</ItemGroup>
```

### 2. Add CycloneDX.MSBuild

```bash
dotnet add package CycloneDX.MSBuild
```

Or add to your `.csproj`:

```xml
<ItemGroup>
  <PackageReference Include="CycloneDX.MSBuild" Version="1.0.0" PrivateAssets="all" />
</ItemGroup>
```

### 3. Update Property Names

See the Property Mapping table below to update your configuration.

### 4. Test Your Build

```bash
dotnet clean
dotnet build
```

## Property Mapping

### Core Configuration

| Microsoft.Sbom.Targets | CycloneDX.MSBuild | Notes |
|------------------------|-------------------|-------|
| `EnableSbomGeneration` | `GenerateCycloneDxSbom` | Enable/disable SBOM generation |
| `SbomGenerationBuildDropPath` | `CycloneDxOutputDirectory` | Custom output directory |
| `SbomGenerationPackageName` | N/A | Not needed; auto-detected from project |
| `SbomGenerationPackageVersion` | N/A | Not needed; auto-detected from project |

### Output Configuration

| Microsoft.Sbom.Targets | CycloneDX.MSBuild | Notes |
|------------------------|-------------------|-------|
| N/A (always JSON) | `CycloneDxOutputFormat` | Supports `json` or `xml` |
| N/A (fixed naming) | `CycloneDxOutputFilename` | Customize SBOM filename |

### Dependency Configuration

| Microsoft.Sbom.Targets | CycloneDX.MSBuild | Notes |
|------------------------|-------------------|-------|
| N/A | `CycloneDxExcludeDev` | Exclude development dependencies |
| N/A | `CycloneDxExcludeTestProjects` | Exclude test projects from SBOM |

### Advanced Options

| Microsoft.Sbom.Targets | CycloneDX.MSBuild | Notes |
|------------------------|-------------------|-------|
| `SbomGenerationManifestInfo` | `CycloneDxImportMetadataPath` | Import custom metadata |
| N/A | `CycloneDxDisableSerialNumber` | Control SBOM serial number |
| N/A | `CycloneDxEnableGitHubLicenses` | Enable GitHub license resolution |
| N/A | `CycloneDxToolVersion` | Pin specific tool version |

### Error Handling

| Microsoft.Sbom.Targets | CycloneDX.MSBuild | Notes |
|------------------------|-------------------|-------|
| `SbomGenerationFailOnError` | `CycloneDxContinueOnError` | **Inverted logic!** See below |

**Important**: The error handling logic is inverted:
- Microsoft: `SbomGenerationFailOnError=true` → Build fails on SBOM error
- CycloneDX: `CycloneDxContinueOnError=true` → Build continues on SBOM error (default)

To match Microsoft's `SbomGenerationFailOnError=true`:
```xml
<CycloneDxContinueOnError>false</CycloneDxContinueOnError>
```

## Migration Examples

### Example 1: Basic Configuration

**Before (Microsoft.Sbom.Targets)**:
```xml
<PropertyGroup>
  <EnableSbomGeneration>true</EnableSbomGeneration>
</PropertyGroup>

<ItemGroup>
  <PackageReference Include="Microsoft.Sbom.Targets" Version="1.0.0" PrivateAssets="all" />
</ItemGroup>
```

**After (CycloneDX.MSBuild)**:
```xml
<PropertyGroup>
  <GenerateCycloneDxSbom>true</GenerateCycloneDxSbom>
</PropertyGroup>

<ItemGroup>
  <PackageReference Include="CycloneDX.MSBuild" Version="1.0.0" PrivateAssets="all" />
</ItemGroup>
```

### Example 2: Custom Output Location

**Before (Microsoft.Sbom.Targets)**:
```xml
<PropertyGroup>
  <EnableSbomGeneration>true</EnableSbomGeneration>
  <SbomGenerationBuildDropPath>$(MSBuildProjectDirectory)/artifacts/sbom</SbomGenerationBuildDropPath>
</PropertyGroup>
```

**After (CycloneDX.MSBuild)**:
```xml
<PropertyGroup>
  <GenerateCycloneDxSbom>true</GenerateCycloneDxSbom>
  <CycloneDxOutputDirectory>$(MSBuildProjectDirectory)/artifacts/sbom</CycloneDxOutputDirectory>
</PropertyGroup>
```

### Example 3: Fail Build on SBOM Error

**Before (Microsoft.Sbom.Targets)**:
```xml
<PropertyGroup>
  <EnableSbomGeneration>true</EnableSbomGeneration>
  <SbomGenerationFailOnError>true</SbomGenerationFailOnError>
</PropertyGroup>
```

**After (CycloneDX.MSBuild)**:
```xml
<PropertyGroup>
  <GenerateCycloneDxSbom>true</GenerateCycloneDxSbom>
  <CycloneDxContinueOnError>false</CycloneDxContinueOnError> <!-- Note: inverted logic -->
</PropertyGroup>
```

### Example 4: Disable for Specific Projects

**Before (Microsoft.Sbom.Targets)**:
```xml
<PropertyGroup>
  <EnableSbomGeneration>false</EnableSbomGeneration>
</PropertyGroup>
```

**After (CycloneDX.MSBuild)**:
```xml
<PropertyGroup>
  <GenerateCycloneDxSbom>false</GenerateCycloneDxSbom>
</PropertyGroup>
```

### Example 5: Conditional Generation

**Before (Microsoft.Sbom.Targets)**:
```xml
<PropertyGroup Condition="'$(Configuration)' == 'Release'">
  <EnableSbomGeneration>true</EnableSbomGeneration>
</PropertyGroup>
```

**After (CycloneDX.MSBuild)**:
```xml
<PropertyGroup Condition="'$(Configuration)' == 'Release'">
  <GenerateCycloneDxSbom>true</GenerateCycloneDxSbom>
</PropertyGroup>
```

### Example 6: Metadata Template

**Before (Microsoft.Sbom.Targets)**:
```xml
<PropertyGroup>
  <SbomGenerationManifestInfo>$(MSBuildProjectDirectory)/sbom-manifest.json</SbomGenerationManifestInfo>
</PropertyGroup>
```

**After (CycloneDX.MSBuild)**:
```xml
<PropertyGroup>
  <CycloneDxImportMetadataPath>$(MSBuildProjectDirectory)/sbom-metadata.xml</CycloneDxImportMetadataPath>
</PropertyGroup>
```

**Note**: The metadata file format is different:
- Microsoft uses JSON manifest format
- CycloneDX uses XML template in CycloneDX schema format

## Key Differences

### SBOM Format

The most significant difference is the SBOM format itself:

| Aspect | Microsoft.Sbom.Targets | CycloneDX.MSBuild |
|--------|------------------------|-------------------|
| **Format** | SPDX 2.2 | CycloneDX 1.2-1.6 |
| **File Extension** | `.spdx.json` | `.json` or `.xml` |
| **Schema** | SPDX JSON schema | CycloneDX schema |
| **License Format** | SPDX license identifiers | SPDX license identifiers (compatible) |
| **Package URL** | ✅ Supported | ✅ Supported |
| **Vulnerability Info** | ❌ Limited | ✅ Rich support (not in generated SBOM, but format supports it) |

### Tool Architecture

| Aspect | Microsoft.Sbom.Targets | CycloneDX.MSBuild |
|--------|------------------------|-------------------|
| **Tool Distribution** | Embedded in package | Installed as .NET local tool |
| **Tool Updates** | Requires package update | Can update independently |
| **Installation** | Automatic (part of package) | Automatic on first build |
| **Network Required** | No | Yes (first build only) |

### Feature Differences

**Features in CycloneDX.MSBuild but not in Microsoft.Sbom.Targets**:

1. **Publish Integration**: Automatically copies SBOM to publish directory
2. **XML Output Format**: Support for XML in addition to JSON
3. **GitHub License Resolution**: Automatically fetch license info from GitHub
4. **Configurable Filename**: Customize SBOM filename
5. **Tool Version Pinning**: Control which tool version to use

**Features in Microsoft.Sbom.Targets but not in CycloneDX.MSBuild**:

1. **Package Supplier Info**: Some additional package supplier metadata
2. **SPDX Format**: Native SPDX format support (use official SPDX tools if SPDX is required)

### Output Location

**Microsoft.Sbom.Targets**:
- Default: `_manifest/spdx_2.2/manifest.spdx.json`
- Custom: Via `SbomGenerationBuildDropPath`

**CycloneDX.MSBuild**:
- Default: `bin/[Configuration]/[TargetFramework]/sbom.json`
- Custom: Via `CycloneDxOutputDirectory` and `CycloneDxOutputFilename`

### Multi-Target Behavior

Both packages handle multi-target frameworks similarly:
- Generate SBOM once at outer build level
- Don't generate for each individual target framework
- Both work correctly with multi-targeting

### NuGet Package Inclusion

**Microsoft.Sbom.Targets**:
- SBOM included at `/_manifest/spdx_2.2/` in NuGet package

**CycloneDX.MSBuild**:
- SBOM included at `/sbom/` in NuGet package

## CI/CD Migration

### GitHub Actions

**Before (Microsoft.Sbom.Targets)**:
```yaml
- name: Build
  run: dotnet build -p:EnableSbomGeneration=true

- name: Upload SBOM
  uses: actions/upload-artifact@v3
  with:
    name: sbom
    path: '**/manifest.spdx.json'
```

**After (CycloneDX.MSBuild)**:
```yaml
- name: Build
  run: dotnet build -p:GenerateCycloneDxSbom=true

- name: Upload SBOM
  uses: actions/upload-artifact@v3
  with:
    name: sbom
    path: '**/sbom.json'
```

### Azure Pipelines

**Before (Microsoft.Sbom.Targets)**:
```yaml
- task: DotNetCoreCLI@2
  inputs:
    command: 'build'
    arguments: '-p:EnableSbomGeneration=true'

- task: PublishBuildArtifacts@1
  inputs:
    pathtoPublish: '$(Build.SourcesDirectory)/_manifest'
    artifactName: 'sbom'
```

**After (CycloneDX.MSBuild)**:
```yaml
- task: DotNetCoreCLI@2
  inputs:
    command: 'build'
    arguments: '-p:GenerateCycloneDxSbom=true'

- task: PublishBuildArtifacts@1
  inputs:
    pathtoPublish: '$(Build.SourcesDirectory)/**/bin/**/sbom.json'
    artifactName: 'sbom'
```

## Format Conversion

If you need both SPDX and CycloneDX formats, you can convert between them:

### CycloneDX to SPDX

```bash
# Install CycloneDX CLI
npm install -g @cyclonedx/cyclonedx-cli

# Convert CycloneDX to SPDX
cyclonedx-cli convert --input-file sbom.json --output-file sbom.spdx.json --output-format spdx-json
```

### Using Both Packages (Side-by-Side)

You can use both packages simultaneously if you need both formats:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.Sbom.Targets" Version="1.0.0" PrivateAssets="all" />
  <PackageReference Include="CycloneDX.MSBuild" Version="1.0.0" PrivateAssets="all" />
</ItemGroup>

<PropertyGroup>
  <!-- Both enabled -->
  <EnableSbomGeneration>true</EnableSbomGeneration>
  <GenerateCycloneDxSbom>true</GenerateCycloneDxSbom>

  <!-- Use different output locations to avoid conflicts -->
  <SbomGenerationBuildDropPath>$(MSBuildProjectDirectory)/sbom/spdx</SbomGenerationBuildDropPath>
  <CycloneDxOutputDirectory>$(MSBuildProjectDirectory)/sbom/cyclonedx</CycloneDxOutputDirectory>
</PropertyGroup>
```

## Validation After Migration

After migrating, verify your SBOM is generated correctly:

### 1. Check SBOM Exists

```bash
dotnet build
ls bin/Debug/net8.0/sbom.json  # Or your custom output location
```

### 2. Validate SBOM Format

```bash
# Install CycloneDX CLI
npm install -g @cyclonedx/cyclonedx-cli

# Validate the SBOM
cyclonedx-cli validate --input-file bin/Debug/net8.0/sbom.json
```

### 3. Inspect SBOM Content

```bash
# Pretty-print JSON
cat bin/Debug/net8.0/sbom.json | jq .

# Check component count
cat bin/Debug/net8.0/sbom.json | jq '.components | length'

# List all dependencies
cat bin/Debug/net8.0/sbom.json | jq '.components[].name'
```

### 4. Test Build Pipeline

```bash
# Clean build
dotnet clean
dotnet restore
dotnet build

# Test pack
dotnet pack

# Verify SBOM in package
unzip -l bin/Debug/MyPackage.1.0.0.nupkg | grep sbom

# Test publish
dotnet publish
ls bin/Debug/net8.0/publish/sbom.json
```

## Common Migration Issues

### Issue 1: SBOM Not Generated After Migration

**Cause**: Properties not migrated correctly

**Solution**: Double-check property names match CycloneDX.MSBuild conventions

### Issue 2: Build Fails After Migration

**Cause**: Tool not installed (requires network on first build)

**Solution**: Ensure network access and run `dotnet tool restore`

### Issue 3: SBOM in Wrong Location

**Cause**: Different default output locations

**Solution**: Use `CycloneDxOutputDirectory` to customize location

### Issue 4: CI/CD Pipeline Breaks

**Cause**: Artifact paths changed

**Solution**: Update artifact collection paths to match new SBOM location

### Issue 5: Different SBOM Content

**Cause**: Different tools may include different dependency information

**Solution**: This is expected; CycloneDX and SPDX have different schemas and may represent dependencies differently

## When to Migrate

### Good Reasons to Migrate:

- ✅ You prefer CycloneDX format over SPDX
- ✅ You need vulnerability enrichment (CycloneDX has better support)
- ✅ You want XML output in addition to JSON
- ✅ You need publish integration (automatic SBOM copy to publish directory)
- ✅ Your tooling/platform requires CycloneDX format

### Good Reasons NOT to Migrate:

- ❌ You have compliance requirements for SPDX format
- ❌ Your tooling only supports SPDX
- ❌ Microsoft.Sbom.Targets works perfectly for your needs
- ❌ You can't allow network access during builds (CycloneDX requires it for tool installation)

## Getting Help

If you encounter issues during migration:

1. **Check troubleshooting docs**: See [README.md](README.md#troubleshooting) for common issues
2. **Compare outputs**: Generate both formats and compare content
3. **Open an issue**: [GitHub Issues](https://github.com/dborgards/CycloneDX.MSBuild/issues)
4. **Ask in discussions**: [GitHub Discussions](https://github.com/dborgards/CycloneDX.MSBuild/discussions)

## Additional Resources

- [CycloneDX Specification](https://cyclonedx.org/)
- [SPDX Specification](https://spdx.dev/)
- [CycloneDX vs SPDX Comparison](https://cyclonedx.org/guides/sbom/spdx-vs-cyclonedx/)
- [CycloneDX .NET Tool](https://github.com/CycloneDX/cyclonedx-dotnet)
- [Microsoft SBOM Tool](https://github.com/microsoft/sbom-tool)

---

*Last updated: December 1, 2025*
