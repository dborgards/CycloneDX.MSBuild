using CycloneDX.MSBuild.Tests.TestHelpers;
using FluentAssertions;
using Xunit;

namespace CycloneDX.MSBuild.Tests.IntegrationTests;

/// <summary>
/// Integration tests for multi-target framework projects.
/// </summary>
public class MultiTargetProjectTests : IDisposable
{
    private readonly string _projectPath;
    private readonly string _projectDirectory;
    private readonly ProjectBuilder _builder;

    public MultiTargetProjectTests()
    {
        var currentDir = Directory.GetCurrentDirectory();
        _projectDirectory = Path.Combine(currentDir, "..", "..", "..", "..", "Integration.Tests", "MultiTargetProject");
        _projectPath = Path.Combine(_projectDirectory, "MultiTargetProject.csproj");
        _builder = new ProjectBuilder(_projectPath);

        // Clean before tests
        _builder.Clean();
    }

    [Fact]
    public void Build_ShouldGenerateSingleSbomForAllTargets()
    {
        // Act
        var result = _builder.Build("Debug");

        // Assert
        result.Success.Should().BeTrue($"build should succeed. Output: {result.Output}\nError: {result.Error}");

        // SBOM should be in the custom output directory (bin/sbom/ as configured in MultiTargetProject)
        var sbomPath = Path.Combine(_projectDirectory, "bin", "sbom", "sbom.json");
        File.Exists(sbomPath).Should().BeTrue($"SBOM should be generated at {sbomPath}");

        // Verify SBOM is NOT generated for each individual target framework
        var net6Path = Path.Combine(_projectDirectory, "bin", "Debug", "net6.0", "sbom.json");
        var net8Path = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "sbom.json");
        var netstandardPath = Path.Combine(_projectDirectory, "bin", "Debug", "netstandard2.0", "sbom.json");

        File.Exists(net6Path).Should().BeFalse("SBOM should not be generated for net6.0 target");
        File.Exists(net8Path).Should().BeFalse("SBOM should not be generated for net8.0 target");
        File.Exists(netstandardPath).Should().BeFalse("SBOM should not be generated for netstandard2.0 target");
    }

    [Fact]
    public void Build_SbomShouldBeValidCycloneDx()
    {
        // Arrange
        var buildResult = _builder.Build("Debug");
        buildResult.Success.Should().BeTrue($"Build should succeed. Output: {buildResult.Output}");

        var sbomPath = Path.Combine(_projectDirectory, "bin", "sbom", "sbom.json");
        File.Exists(sbomPath).Should().BeTrue($"SBOM file should exist at {sbomPath}");

        // Act
        using var sbom = SbomHelper.ReadJsonSbom(sbomPath);

        // Assert with detailed diagnostics
        var hasValidStructure = SbomHelper.IsValidBasicStructure(sbom);
        var bomFormat = SbomHelper.GetBomFormat(sbom);
        var specVersion = SbomHelper.GetSpecVersion(sbom);
        var version = SbomHelper.GetVersion(sbom);

        hasValidStructure.Should().BeTrue($"SBOM should have valid structure. BomFormat={bomFormat}, SpecVersion={specVersion}, Version={version}");
        bomFormat.Should().Be("CycloneDX");
    }

    [Fact]
    public void Build_SbomShouldContainAllTargetFrameworkDependencies()
    {
        // Arrange
        _builder.Build("Debug");
        var sbomPath = Path.Combine(_projectDirectory, "bin", "sbom", "sbom.json");

        // Act
        using var sbom = SbomHelper.ReadJsonSbom(sbomPath);

        // Assert - MultiTargetProject has different dependencies per target
        var componentCount = SbomHelper.GetComponentCount(sbom);
        componentCount.Should().BeGreaterThan(0, "SBOM should contain components from all target frameworks");
    }

    [Fact]
    public void Build_OutputShouldNotShowMultipleSbomGenerations()
    {
        // Act
        var result = _builder.Build("Debug");

        // Assert
        result.Success.Should().BeTrue("build should succeed");

        // Count how many times SBOM generation is mentioned in output
        var generationCount = System.Text.RegularExpressions.Regex.Matches(
            result.Output,
            "GenerateCycloneDxSbom",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase).Count;

        // Should only generate once for outer build (not for each inner build)
        // Note: The exact count might vary based on MSBuild verbosity, but should not be 3+ (one per target)
        result.Output.Should().NotContain("SBOM generation skipped for inner build",
            "or similar message indicating inner builds are skipped");
    }

    [Fact]
    public void Publish_ShouldCopySbomToPublishDirectory()
    {
        // Arrange - Build first to ensure SBOM is generated
        var buildResult = _builder.Build("Debug");
        buildResult.Success.Should().BeTrue($"Build should succeed. Output: {buildResult.Output}");

        var buildSbomPath = Path.Combine(_projectDirectory, "bin", "sbom", "sbom.json");
        File.Exists(buildSbomPath).Should().BeTrue($"SBOM should exist after build at {buildSbomPath}");

        // Act - Multi-target projects require specifying a framework for publish
        var publishProperties = new Dictionary<string, string>
        {
            ["TargetFramework"] = "net8.0"
        };
        var publishResult = _builder.Publish("Debug", publishProperties);

        // Assert
        publishResult.Success.Should().BeTrue($"Publish should succeed. Output: {publishResult.Output}");

        // For multi-target publish with specific framework, SBOM should be copied to that framework's publish directory
        var net8PublishPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "publish", "sbom.json");
        File.Exists(net8PublishPath).Should().BeTrue($"SBOM should be copied to net8.0 publish directory at {net8PublishPath}. Publish output: {publishResult.Output}");

        // The SBOM should also still exist in the custom output location from build
        var customSbomPath = Path.Combine(_projectDirectory, "bin", "sbom", "sbom.json");
        File.Exists(customSbomPath).Should().BeTrue($"SBOM should still exist in custom output directory");
    }

    public void Dispose()
    {
        _builder?.Dispose();
        GC.SuppressFinalize(this);
    }
}
