using CycloneDX.MSBuild.Tests.TestHelpers;
using FluentAssertions;
using Xunit;

namespace CycloneDX.MSBuild.Tests.IntegrationTests;

/// <summary>
/// Integration tests for simple single-target projects.
/// </summary>
[Collection("SimpleProject")]
public class SimpleProjectTests : IDisposable
{
    private readonly string _projectPath;
    private readonly string _projectDirectory;
    private readonly ProjectBuilder _builder;

    public SimpleProjectTests()
    {
        // Find the SimpleProject test project
        var currentDir = Directory.GetCurrentDirectory();
        _projectDirectory = Path.Combine(currentDir, "..", "..", "..", "..", "Integration.Tests", "SimpleProject");
        _projectPath = Path.Combine(_projectDirectory, "SimpleProject.csproj");
        _builder = new ProjectBuilder(_projectPath);

        // Clean before tests
        _builder.Clean();
    }

    [Fact]
    public void Build_ShouldGenerateSbomFile()
    {
        // Act
        var result = _builder.Build("Debug");

        // Assert
        result.Success.Should().BeTrue($"build should succeed. Output: {result.Output}\nError: {result.Error}");

        var sbomPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "sbom.json");
        File.Exists(sbomPath).Should().BeTrue($"SBOM should be generated at {sbomPath}");
    }

    [Fact]
    public void Build_SbomShouldContainValidStructure()
    {
        // Arrange
        var buildResult = _builder.Build("Debug");
        buildResult.Success.Should().BeTrue($"Build should succeed. Output: {buildResult.Output}");

        var sbomPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "sbom.json");
        File.Exists(sbomPath).Should().BeTrue($"SBOM file should exist at {sbomPath}");

        // Act
        using var sbom = SbomHelper.ReadJsonSbom(sbomPath);

        // Assert with detailed diagnostics
        var hasValidStructure = SbomHelper.IsValidBasicStructure(sbom);
        var bomFormat = SbomHelper.GetBomFormat(sbom);
        var specVersion = SbomHelper.GetSpecVersion(sbom);
        var version = SbomHelper.GetVersion(sbom);

        hasValidStructure.Should().BeTrue($"SBOM should have valid basic structure. BomFormat={bomFormat}, SpecVersion={specVersion}, Version={version}");
        bomFormat.Should().Be("CycloneDX");
        specVersion.Should().NotBeNullOrEmpty();
        version.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Build_SbomShouldContainProjectDependencies()
    {
        // Arrange
        _builder.Build("Debug");
        var sbomPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "sbom.json");

        // Act
        using var sbom = SbomHelper.ReadJsonSbom(sbomPath);

        // Assert - SimpleProject has Newtonsoft.Json and Serilog dependencies
        SbomHelper.ContainsPackage(sbom, "Newtonsoft.Json").Should().BeTrue("SBOM should contain Newtonsoft.Json dependency");
        SbomHelper.ContainsPackage(sbom, "Serilog").Should().BeTrue("SBOM should contain Serilog dependency");
    }

    [Fact]
    public void Build_SbomShouldHaveSerialNumber()
    {
        // Arrange
        _builder.Build("Debug");
        var sbomPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "sbom.json");

        // Act
        using var sbom = SbomHelper.ReadJsonSbom(sbomPath);

        // Assert
        var serialNumber = SbomHelper.GetSerialNumber(sbom);
        serialNumber.Should().NotBeNullOrEmpty("SBOM should have a serial number");
        serialNumber.Should().StartWith("urn:uuid:", "serial number should be a URN UUID");
    }

    [Fact]
    public void Build_WithXmlFormat_ShouldGenerateXmlSbom()
    {
        // Arrange
        var properties = new Dictionary<string, string>
        {
            ["CycloneDxOutputFormat"] = "xml"
        };

        // Act
        var result = _builder.Build("Debug", properties);

        // Assert
        result.Success.Should().BeTrue("build should succeed");

        var sbomPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "sbom.xml");
        File.Exists(sbomPath).Should().BeTrue($"XML SBOM should be generated at {sbomPath}");

        // Verify it's valid XML
        var xmlDoc = SbomHelper.ReadXmlSbom(sbomPath);
        xmlDoc.Should().NotBeNull("SBOM should be valid XML");
    }

    [Fact]
    public void Build_WithCustomOutputFilename_ShouldUseCustomName()
    {
        // Arrange
        var properties = new Dictionary<string, string>
        {
            ["CycloneDxOutputFilename"] = "custom-sbom"
        };

        // Act
        var result = _builder.Build("Debug", properties);

        // Assert
        result.Success.Should().BeTrue("build should succeed");

        var sbomPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "custom-sbom.json");
        File.Exists(sbomPath).Should().BeTrue($"SBOM should be generated with custom name at {sbomPath}");
    }

    [Fact]
    public void Build_MultipleBuilds_ShouldRegenerateSbom()
    {
        // Arrange
        _builder.Build("Debug");
        var sbomPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "sbom.json");
        var firstBuildTime = File.GetLastWriteTimeUtc(sbomPath);

        // Wait a moment to ensure timestamp difference
        Thread.Sleep(1000);

        // Act
        _builder.Build("Debug");

        // Assert
        var secondBuildTime = File.GetLastWriteTimeUtc(sbomPath);
        secondBuildTime.Should().BeAfter(firstBuildTime, "SBOM should be regenerated on subsequent builds");
    }

    [Fact]
    public void Publish_ShouldCopySbomToPublishDirectory()
    {
        // Arrange - Build first to ensure SBOM is generated
        var buildResult = _builder.Build("Debug");
        buildResult.Success.Should().BeTrue($"Build should succeed. Output: {buildResult.Output}");

        var buildSbomPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "sbom.json");
        File.Exists(buildSbomPath).Should().BeTrue($"SBOM should exist after build at {buildSbomPath}");

        // Act
        var publishResult = _builder.Publish("Debug");

        // Assert
        publishResult.Success.Should().BeTrue($"Publish should succeed. Output: {publishResult.Output}");

        var publishSbomPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "publish", "sbom.json");
        File.Exists(publishSbomPath).Should().BeTrue($"SBOM should be copied to publish directory at {publishSbomPath}. Publish output: {publishResult.Output}");
    }

    [Fact]
    public void Build_WithDisabledSerialNumber_ShouldNotIncludeSerialNumber()
    {
        // Arrange
        var properties = new Dictionary<string, string>
        {
            ["CycloneDxDisableSerialNumber"] = "true"
        };

        // Act
        _builder.Build("Debug", properties);
        var sbomPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "sbom.json");

        // Assert
        using var sbom = SbomHelper.ReadJsonSbom(sbomPath);
        var hasSerialNumber = sbom.RootElement.TryGetProperty("serialNumber", out _);
        hasSerialNumber.Should().BeFalse("SBOM should not have serial number when disabled");
    }

    [Fact]
    public void Build_SbomShouldContainMetadata()
    {
        // Arrange
        _builder.Build("Debug");
        var sbomPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "sbom.json");

        // Act
        using var sbom = SbomHelper.ReadJsonSbom(sbomPath);

        // Assert
        var metadata = SbomHelper.GetMetadataComponent(sbom);
        metadata.Should().NotBeNull("SBOM should contain metadata component");
    }

    public void Dispose()
    {
        _builder?.Dispose();
        GC.SuppressFinalize(this);
    }
}
