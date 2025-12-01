using CycloneDX.MSBuild.Tests.TestHelpers;
using FluentAssertions;
using Xunit;

namespace CycloneDX.MSBuild.Tests.IntegrationTests;

/// <summary>
/// Integration tests for projects with SBOM generation disabled.
/// </summary>
public class DisabledProjectTests : IDisposable
{
    private readonly string _projectPath;
    private readonly string _projectDirectory;
    private readonly ProjectBuilder _builder;

    public DisabledProjectTests()
    {
        var currentDir = Directory.GetCurrentDirectory();
        _projectDirectory = Path.Combine(currentDir, "..", "..", "..", "..", "Integration.Tests", "DisabledProject");
        _projectPath = Path.Combine(_projectDirectory, "DisabledProject.csproj");
        _builder = new ProjectBuilder(_projectPath);

        // Clean before tests
        _builder.Clean();
    }

    [Fact]
    public void Build_WithGenerationDisabled_ShouldNotGenerateSbom()
    {
        // Act
        var result = _builder.Build("Debug");

        // Assert
        result.Success.Should().BeTrue($"build should succeed. Output: {result.Output}\nError: {result.Error}");

        // SBOM should NOT be generated
        var sbomPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "sbom.json");
        File.Exists(sbomPath).Should().BeFalse($"SBOM should not be generated at {sbomPath} when disabled");
    }

    [Fact]
    public void Build_WithGenerationDisabled_ShouldNotShowSbomGenerationInOutput()
    {
        // Act
        var result = _builder.Build("Debug");

        // Assert
        result.Success.Should().BeTrue("build should succeed");

        // Output should not contain SBOM generation messages
        result.Output.Should().NotContain("Generating CycloneDX SBOM",
            "SBOM generation messages should not appear when disabled");
    }

    [Fact]
    public void Publish_WithGenerationDisabled_ShouldNotCopySbom()
    {
        // Act
        var result = _builder.Publish("Debug");

        // Assert
        result.Success.Should().BeTrue("publish should succeed");

        // SBOM should NOT be in publish directory
        var sbomPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "publish", "sbom.json");
        File.Exists(sbomPath).Should().BeFalse("SBOM should not be copied to publish directory when generation is disabled");
    }

    [Fact]
    public void Build_ExplicitlyDisabled_OverridesDefaultBehavior()
    {
        // This test verifies that explicitly setting GenerateCycloneDxSbom=false
        // overrides the default behavior (which is true)

        // Act
        var result = _builder.Build("Debug");

        // Assert
        result.Success.Should().BeTrue("build should succeed even with SBOM disabled");

        var binDebugPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0");
        if (Directory.Exists(binDebugPath))
        {
            var sbomFiles = Directory.GetFiles(binDebugPath, "*.json")
                .Where(f => Path.GetFileName(f).Contains("sbom", StringComparison.OrdinalIgnoreCase))
                .ToList();

            sbomFiles.Should().BeEmpty("no SBOM files should be generated when explicitly disabled");
        }
    }

    public void Dispose()
    {
        _builder?.Dispose();
        GC.SuppressFinalize(this);
    }
}
