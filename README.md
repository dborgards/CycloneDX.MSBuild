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

#### Import Metadata Template

```xml
<PropertyGroup>
  <!-- Import metadata from template file -->
  <CycloneDxImportMetadataPath>$(MSBuildProjectDirectory)/sbom-metadata.xml</CycloneDxImportMetadataPath>
</PropertyGroup>
```

The metadata template allows you to provide project-specific details that will be included in the generated SBOM. This is useful for adding custom component information, licenses, and descriptions to your SBOM.

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
| **Publish Support** | ❌ | ⏳ Planned |
| **Multi-Targeting** | ✅ | ✅ |
| **Development Dependency** | ✅ | ✅ |

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
