# CycloneDX.MSBuild

[![License](https://img.shields.io/badge/license-MIT-brightgreen.svg)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/CycloneDX.MSBuild.svg)](https://www.nuget.org/packages/CycloneDX.MSBuild/)
[![CI](https://github.com/dborgards/CycloneDX.MSBuild/actions/workflows/ci.yml/badge.svg)](https://github.com/dborgards/CycloneDX.MSBuild/actions/workflows/ci.yml)
[![Release](https://github.com/dborgards/CycloneDX.MSBuild/actions/workflows/release.yml/badge.svg)](https://github.com/dborgards/CycloneDX.MSBuild/actions/workflows/release.yml)

MSBuild targets for automatic **CycloneDX SBOM** (Software Bill of Materials) generation during build and pack operations. Seamlessly integrates the [CycloneDX .NET tool](https://github.com/CycloneDX/cyclonedx-dotnet) into your build pipeline.

## 🎯 Overview

**CycloneDX.MSBuild** is a NuGet package inspired by [Microsoft.Sbom.Targets](https://github.com/microsoft/sbom-tool) that automatically generates CycloneDX SBOMs for your .NET projects. Simply add the package reference, and SBOMs are generated automatically during `dotnet build` or `dotnet pack`.

### Why CycloneDX.MSBuild?

- ✅ **Zero Configuration**: Works out of the box with sensible defaults
- ✅ **Security by Design**: Runs in build context, no elevated permissions required
- ✅ **Multi-Target Support**: Handles projects with multiple target frameworks
- ✅ **Flexible**: Highly configurable via MSBuild properties
- ✅ **Standards Compliant**: Generates CycloneDX 1.2-1.6 compatible SBOMs
- ✅ **Development Dependency**: Doesn't pollute your dependency tree

## 🚀 Quick Start

### Installation

Add the package to your project:

```bash
dotnet add package CycloneDX.MSBuild
```

Or add it manually to your `.csproj`:

```xml
<ItemGroup>
  <PackageReference Include="CycloneDX.MSBuild" Version="1.0.0" PrivateAssets="all" />
</ItemGroup>
```

### Build Your Project

```bash
dotnet build
```

That's it! Your SBOM will be generated at `bin/Debug/net8.0/sbom.json` (or your output directory).

## 📋 Features

### Automatic SBOM Generation

SBOMs are generated automatically:
- **During Build**: After successful compilation
- **During Pack**: Included in NuGet packages under `/sbom/`
- **During Publish**: Copied to the publish directory
- **Multi-Targeting**: Generates once per project, not per framework

### Supported Output Formats

- **JSON** (default) - CycloneDX JSON format
- **XML** - CycloneDX XML format

### CycloneDX Specification Versions

The tool automatically uses the latest CycloneDX specification version supported by the installed tool version (typically 1.6).

## ⚙️ Configuration

All configuration is done via MSBuild properties. Set them in your `.csproj`, `Directory.Build.props`, or command line.

### Core Settings

#### Enable/Disable SBOM Generation

```xml
<PropertyGroup>
  <!-- Disable for specific projects -->
  <GenerateCycloneDxSbom>false</GenerateCycloneDxSbom>
</PropertyGroup>
```

#### Error Handling

```xml
<PropertyGroup>
  <!-- Fail build if SBOM generation fails (default: true, continues on error) -->
  <CycloneDxContinueOnError>false</CycloneDxContinueOnError>
</PropertyGroup>
```

### Output Configuration

#### Output Format

```xml
<PropertyGroup>
  <!-- json (default) or xml -->
  <CycloneDxOutputFormat>xml</CycloneDxOutputFormat>
</PropertyGroup>
```

#### Output Location

```xml
<PropertyGroup>
  <!-- Custom output directory -->
  <CycloneDxOutputDirectory>$(MSBuildProjectDirectory)/sbom/</CycloneDxOutputDirectory>

  <!-- Custom filename (without extension) -->
  <CycloneDxOutputFilename>software-bom</CycloneDxOutputFilename>
</PropertyGroup>
```

### Advanced Options

#### Exclude Dependencies

```xml
<PropertyGroup>
  <!-- Exclude development dependencies -->
  <CycloneDxExcludeDev>true</CycloneDxExcludeDev>

  <!-- Exclude test projects -->
  <CycloneDxExcludeTestProjects>true</CycloneDxExcludeTestProjects>
</PropertyGroup>
```

#### Serial Number Control

```xml
<PropertyGroup>
  <!-- Disable auto-generated SBOM serial number -->
  <CycloneDxDisableSerialNumber>true</CycloneDxDisableSerialNumber>
</PropertyGroup>
```

#### Automatic Metadata Generation

**New in v1.1.0**: CycloneDX.MSBuild now automatically extracts assembly and package metadata from your project properties and includes it in the generated SBOM. This eliminates the need for manual metadata templates in most cases.

**Automatically Extracted Metadata:**
- **Version**: From `Version`, `VersionPrefix`, `AssemblyVersion`, or GitVersion properties (`GitVersion_FullSemVer`, `GitVersion_SemVer`)
- **Component Name**: From `AssemblyName` or project name
- **Authors/Supplier**: From `Authors` or `Company` properties
- **Description**: From `Description` or `PackageDescription` properties
- **Copyright**: From `Copyright` property
- **License**: From `PackageLicenseExpression` property
- **Package URL (purl)**: Automatically generated as `pkg:nuget/{name}@{version}`

**Example Project Configuration:**

```xml
<PropertyGroup>
  <Version>1.2.3</Version>
  <Authors>Your Name</Authors>
  <Company>Your Company Inc.</Company>
  <Description>A brief description of your application</Description>
  <Copyright>Copyright (c) 2024 Your Company Inc.</Copyright>
  <PackageLicenseExpression>MIT</PackageLicenseExpression>
</PropertyGroup>
```

**Generated SBOM Component Metadata:**

```json
{
  "component": {
    "type": "application",
    "bom-ref": "pkg:nuget/YourProject@1.2.3",
    "name": "YourProject",
    "version": "1.2.3",
    "description": "A brief description of your application",
    "supplier": {
      "name": "Your Name"
    },
    "copyright": "Copyright (c) 2024 Your Company Inc.",
    "licenses": [
      {
        "license": {
          "id": "MIT"
        }
      }
    ],
    "purl": "pkg:nuget/YourProject@1.2.3"
  }
}
```

**Disable Automatic Metadata Generation:**

If you prefer to use a manual metadata template or disable metadata generation entirely:

```xml
<PropertyGroup>
  <!-- Disable automatic metadata extraction -->
  <CycloneDxGenerateMetadata>false</CycloneDxGenerateMetadata>
</PropertyGroup>
```

**GitVersion Integration:**

If you're using [GitVersion](https://gitversion.net/) for semantic versioning, CycloneDX.MSBuild automatically detects and uses the GitVersion-generated version:

```xml
<!-- No configuration needed - GitVersion properties are automatically detected -->
<!-- Checks GitVersion_FullSemVer and GitVersion_SemVer properties -->
```

#### Import Metadata Template (Advanced)

**Note:** With automatic metadata generation enabled by default (see above), manual metadata templates are typically only needed for advanced scenarios or custom metadata structures.

```xml
<PropertyGroup>
  <!-- Import metadata from template file (overrides automatic generation) -->
  <CycloneDxImportMetadataPath>$(MSBuildProjectDirectory)/sbom-metadata.xml</CycloneDxImportMetadataPath>
</PropertyGroup>
```

When you specify a custom metadata template path, automatic metadata generation is skipped, and your template is used instead. This is useful for:
- Adding custom component information beyond standard properties
- Including additional metadata fields not extracted automatically
- Providing complex metadata structures

Example `sbom-metadata.xml`:

> **Note:** CycloneDX.MSBuild supports metadata templates using CycloneDX schema versions 1.2 through 1.6. You may use any supported version, but [1.6](https://cyclonedx.org/docs/1.6/) is recommended for new projects.

```xml
<?xml version="1.0" encoding="utf-8"?>
<bom xmlns="http://cyclonedx.org/schema/bom/1.6">
  <metadata>
    <component type="application" bom-ref="pkg:nuget/YourProject@1.0.0">
      <name>YourProject</name>
      <version>1.0.0</version>
      <description>
        <![CDATA[Your project description]]>
      </description>
      <licenses>
        <license>
          <name>Apache License 2.0</name>
          <id>Apache-2.0</id>
        </license>
      </licenses>
      <purl>pkg:nuget/YourProject@1.0.0</purl>
    </component>
  </metadata>
</bom>
```

#### GitHub License Resolution

```xml
<PropertyGroup>
  <!-- Enable GitHub license resolution -->
  <CycloneDxEnableGitHubLicenses>true</CycloneDxEnableGitHubLicenses>

  <!-- GitHub credentials (optional, for higher rate limits) -->
  <CycloneDxGitHubUsername>your-username</CycloneDxGitHubUsername>
  <CycloneDxGitHubToken>ghp_yourtoken</CycloneDxGitHubToken>
</PropertyGroup>
```

### Tool Version

```xml
<PropertyGroup>
  <!-- Pin specific CycloneDX tool version -->
  <CycloneDxToolVersion>5.5.0</CycloneDxToolVersion>
</PropertyGroup>
```

## 📦 NuGet Package Integration

When you run `dotnet pack`, the generated SBOM is automatically included in the NuGet package under the `/sbom/` directory.

```bash
dotnet pack
```

Consumers of your NuGet package can inspect the SBOM:

```bash
# Extract and view
unzip -q MyPackage.1.0.0.nupkg -d extracted
cat extracted/sbom/sbom.json
```

## 🚀 Publish Integration

When you run `dotnet publish`, the generated SBOM is automatically copied to the publish directory alongside your application.

```bash
dotnet publish -c Release
```

The SBOM will be available in your publish directory:

```bash
# View the SBOM in publish directory
cat bin/Release/net8.0/publish/sbom.json
```

This makes it easy to include the SBOM when deploying your application, ensuring supply chain transparency in production environments.

## 🏗️ Architecture

### MSBuild Integration

CycloneDX.MSBuild uses MSBuild's extensibility hooks:

1. **`.props` file** - Defines configurable properties with defaults
2. **`.targets` file** - Implements build targets for SBOM generation
3. **`buildMultiTargeting/`** - Special handling for multi-target projects

### Target Execution Order

```
Build → ValidateCycloneDxConfiguration → EnsureCycloneDxToolInstalled → GenerateCycloneDxSbom
```

### Security by Design

- **Sandboxed Execution**: Runs in build context without elevated privileges
- **Input Validation**: All properties are validated before use
- **Fail-Safe Defaults**: Continues build on errors by default
- **No Code Execution**: Only executes vetted CycloneDX tool
- **Dependency Pinning**: Explicit tool version control

### Clean Code Principles

- **Separation of Concerns**: Configuration (`.props`) separated from logic (`.targets`)
- **Single Responsibility**: Each target has one clear purpose
- **DRY**: Reusable property groups
- **Consistent Naming**: `CycloneDx*` prefix for all properties
- **Comprehensive Documentation**: XML comments for all properties

## 🧪 Testing

The repository includes integration tests for various scenarios:

```
tests/
├── SimpleProject/          # Basic single-target project
├── MultiTargetProject/     # Multi-targeting (.NET 6, 8, Standard 2.0)
└── DisabledProject/        # Project with SBOM generation disabled
```

## 🔄 Versioning

This project uses **automated semantic versioning** with:
- **MinVer**: Automatic version calculation from Git tags
- **semantic-release**: Automated releases, changelogs, and NuGet publishing

All commits must follow [Conventional Commits](https://www.conventionalcommits.org/) format.

📖 For detailed information, see [VERSIONING.md](VERSIONING.md)

## 🚀 CI/CD Pipeline

This project includes a comprehensive CI/CD pipeline with automated testing and publishing:

### Continuous Integration (CI)

Runs on all pull requests and feature branches:
- ✅ **Multi-platform testing** (Ubuntu, Windows, macOS)
- ✅ **Multi-version .NET testing** (.NET 6.0 and 8.0)
- ✅ **Code quality checks** (formatting, package validation)
- ✅ **Dependency security scanning** (vulnerable and deprecated packages)
- ✅ **Build artifact generation**

### Continuous Deployment (CD)

Runs on pushes to `main`, `master`, `beta`, or `alpha` branches:
1. **Build & Test** - Full test suite execution
2. **Package Validation** - NuGet package quality checks
3. **Semantic Release** - Automatic version determination from commits
4. **NuGet Publishing** - Automatic publishing to [NuGet.org](https://www.nuget.org/packages/CycloneDX.MSBuild/)
5. **GitHub Release** - Automated release notes and changelog

### Setting up Automated Publishing

To enable automated NuGet publishing:

1. Get a NuGet API key from [NuGet.org](https://www.nuget.org/account/apikeys)
2. Add it as a GitHub secret named `NUGET_API_KEY`
3. The release workflow will automatically publish on version bumps

📖 For detailed CI/CD documentation, see [.github/workflows/README.md](.github/workflows/README.md)

## 🔧 Development

### Project Structure

```
CycloneDX.MSBuild/
├── src/
│   └── CycloneDX.MSBuild/
│       ├── build/
│       │   ├── CycloneDX.MSBuild.props      # Configuration properties
│       │   └── CycloneDX.MSBuild.targets    # Build integration
│       ├── buildMultiTargeting/             # Multi-target support
│       └── CycloneDX.MSBuild.csproj         # Package definition
├── tests/
│   └── Integration.Tests/
└── README.md
```

### Building Locally

```bash
# Build the package
dotnet build src/CycloneDX.MSBuild/CycloneDX.MSBuild.csproj

# Pack the package
dotnet pack src/CycloneDX.MSBuild/CycloneDX.MSBuild.csproj

# Test with local projects
dotnet build tests/Integration.Tests/SimpleProject/SimpleProject.csproj
```

## 📚 Comparison with Microsoft.Sbom.Targets

| Feature | Microsoft.Sbom.Targets | CycloneDX.MSBuild |
|---------|------------------------|-------------------|
| **SBOM Format** | SPDX 2.2 | CycloneDX 1.2-1.6 |
| **Tool** | Microsoft.Sbom.Tool (embedded) | CycloneDX .NET tool (local tool, installed on demand) |
| **Build Support** | ✅ | ✅ |
| **Pack Support** | ✅ | ✅ |
| **Publish Support** | ❌ | ✅ |
| **Multi-Targeting** | ✅ | ✅ |
| **Development Dependency** | ✅ | ✅ |

## 🔍 Troubleshooting

### Common Issues and Solutions

#### SBOM Not Generated

**Problem**: SBOM file is not created after build.

**Solutions**:
1. **Check if generation is enabled**:
   ```bash
   dotnet build -p:GenerateCycloneDxSbom=true -v:detailed
   ```
   Look for "GenerateCycloneDxSbom" messages in the output.

2. **Verify the package is installed**:
   ```bash
   dotnet list package | grep CycloneDX.MSBuild
   ```

3. **Check for multi-target projects**: For projects with multiple target frameworks, the SBOM is only generated once at the outer build level. Check the project root or custom output directory.

4. **Look in the correct location**: Default is `bin/[Configuration]/[TargetFramework]/sbom.json`. Check your `CycloneDxOutputDirectory` setting if customized.

5. **Check build verbosity**: Run with detailed verbosity to see what's happening:
   ```bash
   dotnet build -v:detailed 2>&1 | grep -i cyclonedx
   ```

#### Tool Installation Failures

**Problem**: CycloneDX tool fails to install or is not found.

**Solutions**:
1. **Manually install the tool**:
   ```bash
   dotnet tool install --local CycloneDX --version 5.5.0
   ```

2. **Clear tool cache and retry**:
   ```bash
   dotnet tool uninstall --local CycloneDX
   dotnet build
   ```

3. **Check network connectivity**: Tool installation requires internet access to download from NuGet.org.

4. **Verify .NET SDK version**: Ensure you have .NET SDK 6.0 or later installed:
   ```bash
   dotnet --version
   ```

5. **Check for proxy/firewall issues**: If behind a corporate proxy, configure NuGet:
   ```bash
   dotnet nuget update source nuget.org --http-proxy http://proxy.company.com:8080
   ```

#### GitHub License Resolution Issues

**Problem**: GitHub API rate limiting or authentication errors.

**Solutions**:
1. **Rate limiting (60 requests/hour without auth)**:
   - Use GitHub token for higher limits (5000 requests/hour)
   - Set `CycloneDxGitHubUsername` and `CycloneDxGitHubToken` properties

2. **Generate a GitHub token**:
   - Go to GitHub Settings → Developer settings → Personal access tokens
   - Create token with `public_repo` scope (read-only)
   - Store securely (use environment variables, not source control)

3. **Use environment variables in CI/CD**:
   ```bash
   dotnet build -p:CycloneDxGitHubToken=$GITHUB_TOKEN
   ```

4. **Disable if not needed**:
   ```xml
   <CycloneDxEnableGitHubLicenses>false</CycloneDxEnableGitHubLicenses>
   ```

#### Build Performance Issues

**Problem**: Build time significantly increased after adding CycloneDX.MSBuild.

**Solutions**:
1. **Disable for Debug builds** (only generate for Release):
   ```xml
   <PropertyGroup Condition="'$(Configuration)' == 'Debug'">
     <GenerateCycloneDxSbom>false</GenerateCycloneDxSbom>
   </PropertyGroup>
   ```

2. **Use custom output directory** to avoid file locking issues:
   ```xml
   <CycloneDxOutputDirectory>$(MSBuildProjectDirectory)/sbom/</CycloneDxOutputDirectory>
   ```

3. **Exclude test projects** from SBOM generation:
   ```xml
   <PropertyGroup Condition="$(MSBuildProjectName.EndsWith('.Tests'))">
     <GenerateCycloneDxSbom>false</GenerateCycloneDxSbom>
   </PropertyGroup>
   ```

4. **Pin tool version** to avoid repeated downloads:
   ```xml
   <CycloneDxToolVersion>5.5.0</CycloneDxToolVersion>
   ```

#### Multi-Target Framework Issues

**Problem**: Multiple SBOMs generated or wrong output location for multi-target projects.

**Solution**: This is expected behavior. CycloneDX.MSBuild automatically detects multi-target projects and generates a single SBOM at the outer build level. If you're seeing multiple SBOMs, check:

1. **Verify buildMultiTargeting files are imported**: The package should automatically handle this.

2. **Check output location**: SBOM should be in the project directory or custom output directory, not in individual target framework folders.

3. **Clean and rebuild**:
   ```bash
   dotnet clean
   dotnet build
   ```

#### SBOM Contains Wrong Information

**Problem**: SBOM has incorrect dependencies, versions, or metadata.

**Solutions**:
1. **Ensure dependencies are restored**:
   ```bash
   dotnet restore
   dotnet build
   ```

2. **Check for package version conflicts**: Look at the build output for version resolution messages.

3. **Use metadata template** to override project information:
   ```xml
   <CycloneDxImportMetadataPath>sbom-metadata.xml</CycloneDxImportMetadataPath>
   ```

4. **Verify tool version**: Ensure you're using a recent version of the CycloneDX tool:
   ```xml
   <CycloneDxToolVersion>5.5.0</CycloneDxToolVersion>
   ```

5. **Clean package cache and rebuild**:
   ```bash
   dotnet nuget locals all --clear
   dotnet restore
   dotnet build
   ```

#### CI/CD Pipeline Issues

**Problem**: SBOM generation works locally but fails in CI/CD.

**Solutions**:
1. **Ensure .NET SDK is available**: CI environment must have .NET SDK 6.0+ installed.

2. **Check for file permissions**: CI systems may have restricted file access. Use:
   ```bash
   dotnet build -p:CycloneDxContinueOnError=true
   ```

3. **Network access required**: Tool installation needs internet access to NuGet.org.

4. **Use explicit restore**:
   ```bash
   dotnet restore
   dotnet tool restore
   dotnet build
   ```

5. **Cache the tool** in CI to improve performance:
   ```yaml
   # GitHub Actions example
   - uses: actions/cache@v3
     with:
       path: ~/.nuget/packages
       key: ${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj') }}
   ```

#### XML/JSON Format Issues

**Problem**: Generated SBOM has parsing errors or invalid format.

**Solutions**:
1. **Validate the SBOM**: Use CycloneDX validation tools:
   ```bash
   # Install validator
   npm install -g @cyclonedx/cyclonedx-cli

   # Validate SBOM
   cyclonedx validate --input-file sbom.json
   ```

2. **Check tool version**: Older versions may have bugs. Update to latest:
   ```xml
   <CycloneDxToolVersion>5.5.0</CycloneDxToolVersion>
   ```

3. **Try different format**: Switch between JSON and XML to isolate issues:
   ```xml
   <CycloneDxOutputFormat>xml</CycloneDxOutputFormat>
   ```

4. **Check for special characters**: Ensure project/package names don't have invalid XML/JSON characters.

### Debugging Tips

#### Enable Detailed Logging

```bash
dotnet build -v:detailed > build.log 2>&1
grep -i cyclonedx build.log
```

#### Check Tool Installation

```bash
dotnet tool list --local
```

Should show:
```
Package Id         Version      Commands
--------------------------------------------------------
CycloneDX          5.5.0        dotnet-CycloneDX
```

#### Verify MSBuild Targets Are Loaded

```bash
dotnet build -v:diagnostic 2>&1 | grep "CycloneDX.MSBuild.targets"
```

Should show the targets file being imported.

#### Test with Minimal Project

Create a simple test project:
```bash
dotnet new console -n TestSbom
cd TestSbom
dotnet add package CycloneDX.MSBuild
dotnet build
ls bin/Debug/net8.0/sbom.json
```

### FAQ

**Q: Does SBOM generation require internet access?**
A: Yes, the first build requires internet to download the CycloneDX tool from NuGet.org. Subsequent builds use the cached tool. GitHub license resolution (if enabled) also requires internet access.

**Q: Can I use this with .NET Framework projects?**
A: The package targets netstandard2.0, so it works with .NET Framework 4.6.1+ and all versions of .NET Core/.NET 5+. However, you need .NET SDK 6.0+ installed to run the CycloneDX tool.

**Q: Does this work with mono or alternative .NET implementations?**
A: The package should work with any MSBuild-compatible build system that supports .NET tools.

**Q: How do I exclude the SBOM from version control?**
A: Add to `.gitignore`:
```gitignore
# CycloneDX SBOM files
**/bin/**/sbom.json
**/bin/**/sbom.xml
sbom/
```

**Q: Can I validate the SBOM automatically during build?**
A: Not built-in yet, but you can add a custom MSBuild target that runs validation after SBOM generation. This is a planned feature.

**Q: Does the SBOM include transitive dependencies?**
A: Yes, the CycloneDX tool includes all transitive dependencies by default.

**Q: How do I report security vulnerabilities?**
A: See [SECURITY.md](SECURITY.md) for our security policy and reporting process.

**Q: What's the performance impact?**
A: SBOM generation typically adds 2-5 seconds to build time, depending on project size and number of dependencies. Consider disabling for Debug builds if needed.

### Getting Help

If you're still experiencing issues:

1. **Check existing issues**: [GitHub Issues](https://github.com/dborgards/CycloneDX.MSBuild/issues)
2. **Review CycloneDX tool docs**: [cyclonedx-dotnet](https://github.com/CycloneDX/cyclonedx-dotnet)
3. **Ask in discussions**: [GitHub Discussions](https://github.com/dborgards/CycloneDX.MSBuild/discussions)
4. **File a bug report**: Include build logs, project configuration, and environment details

## 🤝 Contributing

Contributions are welcome! Please feel free to submit issues, feature requests, or pull requests.

### Guidelines

- Follow existing code style and architecture
- Maintain security by design principles
- Add tests for new features
- Update documentation

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔗 Related Projects

- [CycloneDX .NET Tool](https://github.com/CycloneDX/cyclonedx-dotnet) - The underlying SBOM generation tool
- [CycloneDX Specification](https://cyclonedx.org/) - CycloneDX standard
- [Microsoft.Sbom.Targets](https://github.com/microsoft/sbom-tool) - Microsoft's SPDX SBOM tool
- [OWASP Dependency-Track](https://dependencytrack.org/) - SBOM analysis platform

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/CycloneDX/cyclonedx-msbuild/issues)
- **Discussions**: [GitHub Discussions](https://github.com/CycloneDX/cyclonedx-msbuild/discussions)
- **CycloneDX Community**: [https://cyclonedx.org/](https://cyclonedx.org/)

---

**Made with ❤️ for Software Supply Chain Security**
