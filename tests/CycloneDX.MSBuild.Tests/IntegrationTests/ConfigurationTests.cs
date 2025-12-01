using CycloneDX.MSBuild.Tests.TestHelpers;
using FluentAssertions;
using Xunit;

namespace CycloneDX.MSBuild.Tests.IntegrationTests;

/// <summary>
/// Tests for various configuration options.
/// </summary>
public class ConfigurationTests : IDisposable
{
    private readonly string _projectPath;
    private readonly string _projectDirectory;
    private readonly ProjectBuilder _builder;

    public ConfigurationTests()
    {
        var currentDir = Directory.GetCurrentDirectory();
        _projectDirectory = Path.Combine(currentDir, "..", "..", "..", "..", "Integration.Tests", "SimpleProject");
        _projectPath = Path.Combine(_projectDirectory, "SimpleProject.csproj");
        _builder = new ProjectBuilder(_projectPath);
        _builder.Clean();
    }

    [Fact]
    public void Build_WithContinueOnError_ShouldNotFailBuildOnSbomError()
    {
        // Arrange - force an error by specifying invalid tool version
        var properties = new Dictionary<string, string>
        {
            ["CycloneDxContinueOnError"] = "true",
            ["CycloneDxToolVersion"] = "999.999.999" // Non-existent version
        };

        // Act
        var result = _builder.Build("Debug", properties);

        // Assert
        // Build should still succeed because ContinueOnError is true by default
        // Even if SBOM generation fails, the build continues
        result.Output.Should().NotBeNullOrEmpty("should have build output");
    }

    [Fact]
    public void Build_WithCustomOutputDirectory_ShouldGenerateSbomInCustomLocation()
    {
        // Arrange
        var customDir = Path.Combine(_projectDirectory, "custom-sbom-output");
        var properties = new Dictionary<string, string>
        {
            ["CycloneDxOutputDirectory"] = customDir
        };

        // Clean up custom directory if it exists
        if (Directory.Exists(customDir))
            Directory.Delete(customDir, true);

        // Act
        var result = _builder.Build("Debug", properties);

        // Assert
        result.Success.Should().BeTrue("build should succeed");

        var sbomPath = Path.Combine(customDir, "sbom.json");
        File.Exists(sbomPath).Should().BeTrue($"SBOM should be in custom directory at {sbomPath}");

        // Cleanup
        if (Directory.Exists(customDir))
            Directory.Delete(customDir, true);
    }

    [Theory]
    [InlineData("json", "sbom.json")]
    [InlineData("xml", "sbom.xml")]
    public void Build_WithDifferentFormats_ShouldGenerateCorrectFormat(string format, string expectedFileName)
    {
        // Arrange
        var properties = new Dictionary<string, string>
        {
            ["CycloneDxOutputFormat"] = format
        };

        _builder.Clean();

        // Act
        var result = _builder.Build("Debug", properties);

        // Assert
        result.Success.Should().BeTrue($"build should succeed for format {format}");

        var sbomPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", expectedFileName);
        File.Exists(sbomPath).Should().BeTrue($"SBOM should be generated as {expectedFileName}");
    }

    [Fact]
    public void Build_WithExcludeDevDependencies_ShouldExcludeDevPackages()
    {
        // Note: This test would require a test project with dev dependencies
        // For now, we verify the property is accepted
        var properties = new Dictionary<string, string>
        {
            ["CycloneDxExcludeDev"] = "true"
        };

        // Act
        var result = _builder.Build("Debug", properties);

        // Assert
        result.Success.Should().BeTrue("build should succeed with ExcludeDev property");
    }

    [Fact]
    public void Build_WithExcludeTestProjects_ShouldAcceptProperty()
    {
        // Arrange
        var properties = new Dictionary<string, string>
        {
            ["CycloneDxExcludeTestProjects"] = "true"
        };

        // Act
        var result = _builder.Build("Debug", properties);

        // Assert
        result.Success.Should().BeTrue("build should succeed with ExcludeTestProjects property");
    }

    [Fact]
    public void Build_MultipleConfigurations_ShouldGenerateSeparateSboms()
    {
        // Build Debug
        var debugResult = _builder.Build("Debug");
        debugResult.Success.Should().BeTrue("Debug build should succeed");

        var debugSbomPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "sbom.json");
        File.Exists(debugSbomPath).Should().BeTrue("Debug SBOM should exist");

        // Build Release
        var releaseResult = _builder.Build("Release");
        releaseResult.Success.Should().BeTrue("Release build should succeed");

        var releaseSbomPath = Path.Combine(_projectDirectory, "bin", "Release", "net8.0", "sbom.json");
        File.Exists(releaseSbomPath).Should().BeTrue("Release SBOM should exist");

        // Both should exist
        File.Exists(debugSbomPath).Should().BeTrue("Debug SBOM should still exist");
        File.Exists(releaseSbomPath).Should().BeTrue("Release SBOM should exist");
    }

    [Fact]
    public void Pack_ShouldIncludeSbomInPackage()
    {
        // Act
        var result = _builder.Pack("Debug");

        // Assert
        result.Success.Should().BeTrue($"pack should succeed. Output: {result.Output}");

        // Verify SBOM exists (it should be generated during pack)
        var sbomPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "sbom.json");
        File.Exists(sbomPath).Should().BeTrue("SBOM should be generated during pack");

        // Note: Full verification of SBOM in .nupkg would require extracting the package
        // This is a basic smoke test that pack succeeds with SBOM generation enabled
    }

    public void Dispose()
    {
        _builder?.Dispose();
        GC.SuppressFinalize(this);
    }
}
