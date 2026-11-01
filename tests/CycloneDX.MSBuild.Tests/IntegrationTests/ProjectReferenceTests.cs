using CycloneDX.MSBuild.Tests.TestHelpers;
using FluentAssertions;
using Xunit;

namespace CycloneDX.MSBuild.Tests.IntegrationTests;

/// <summary>
/// Tests for CycloneDxIncludeProjectReferences and CycloneDxRecursive
/// (https://github.com/dborgards/CycloneDX.MSBuild/issues/104).
/// ProjectReferenceProject references ReferencedLibrary, a netstandard2.0 project.
/// </summary>
[Collection("ProjectReferenceProject")]
public class ProjectReferenceTests : IDisposable
{
    private readonly string _projectDirectory;
    private readonly ProjectBuilder _builder;

    public ProjectReferenceTests()
    {
        var currentDir = Directory.GetCurrentDirectory();
        _projectDirectory = Path.Combine(currentDir, "..", "..", "..", "..", "Integration.Tests", "ProjectReferenceProject");
        _builder = new ProjectBuilder(Path.Combine(_projectDirectory, "ProjectReferenceProject.csproj"));
        _builder.Clean();
    }

    [Fact]
    public void Build_ByDefault_ShouldNotListOrScanProjectReferences()
    {
        // Act
        var result = _builder.Build("Debug");

        // Assert
        result.Success.Should().BeTrue($"build should succeed. Output: {result.Output}\nError: {result.Error}");

        using var sbom = SbomHelper.ReadJsonSbom(GetSbomPath());
        SbomHelper.ContainsPackage(sbom, "Newtonsoft.Json").Should().BeTrue("direct package references are always listed");
        SbomHelper.ContainsPackage(sbom, "ReferencedLibrary").Should().BeFalse(
            "project references are not listed as components by default");
        SbomHelper.ContainsPackage(sbom, "NETStandard.Library").Should().BeFalse(
            "packages that only referenced projects use are not scanned by default");
    }

    [Fact]
    public void Build_WithIncludeProjectReferences_ShouldListReferencedProjectAsComponent()
    {
        // Arrange
        var properties = new Dictionary<string, string>
        {
            ["CycloneDxIncludeProjectReferences"] = "true"
        };

        // Act
        var result = _builder.Build("Debug", properties);

        // Assert
        result.Success.Should().BeTrue($"build should succeed. Output: {result.Output}\nError: {result.Error}");

        using var sbom = SbomHelper.ReadJsonSbom(GetSbomPath());
        var component = SbomHelper.GetComponentByName(sbom, "ReferencedLibrary");
        component.Should().NotBeNull("the referenced project should be listed as a component");
        component!.Value.GetProperty("version").GetString().Should().Be("2.0.0");
    }

    [Fact]
    public void Build_WithRecursive_ShouldScanPackagesOfReferencedProjects()
    {
        // Arrange
        var properties = new Dictionary<string, string>
        {
            ["CycloneDxRecursive"] = "true"
        };

        // Act
        var result = _builder.Build("Debug", properties);

        // Assert
        result.Success.Should().BeTrue($"build should succeed. Output: {result.Output}\nError: {result.Error}");

        using var sbom = SbomHelper.ReadJsonSbom(GetSbomPath());
        SbomHelper.ContainsPackage(sbom, "NETStandard.Library").Should().BeTrue(
            "the recursive scan should pick up the implicit package of the netstandard2.0 project reference");
        SbomHelper.ContainsPackage(sbom, "ReferencedLibrary").Should().BeFalse(
            "recursive scanning alone does not list the project reference itself");
    }

    private string GetSbomPath()
    {
        var sbomPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "sbom.json");
        File.Exists(sbomPath).Should().BeTrue($"SBOM should be generated at {sbomPath}");
        return sbomPath;
    }

    public void Dispose()
    {
        _builder?.Dispose();
        GC.SuppressFinalize(this);
    }
}
