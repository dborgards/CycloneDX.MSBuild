using CycloneDX.MSBuild.Tests.TestHelpers;
using FluentAssertions;
using Xunit;

namespace CycloneDX.MSBuild.Tests.IntegrationTests;

/// <summary>
/// Regression tests for https://github.com/dborgards/CycloneDX.MSBuild/issues/68 - the SBOM
/// must reflect the packages the build actually resolved, including conditional
/// PackageReference items that depend on properties passed on the command line.
/// </summary>
[Collection("ConditionalDependencyProject")]
public class ConditionalDependencyTests : IDisposable
{
    private readonly string _projectDirectory;
    private readonly ProjectBuilder _builder;

    public ConditionalDependencyTests()
    {
        var currentDir = Directory.GetCurrentDirectory();
        _projectDirectory = Path.Combine(currentDir, "..", "..", "..", "..", "Integration.Tests", "ConditionalDependencyProject");
        _builder = new ProjectBuilder(Path.Combine(_projectDirectory, "ConditionalDependencyProject.csproj"));
        _builder.Clean();
    }

    [Theory]
    [InlineData("true", "Serilog", "Newtonsoft.Json")]
    [InlineData("false", "Newtonsoft.Json", "Serilog")]
    public void Build_WithConditionalPackageReference_ShouldRecordResolvedPackage(
        string useSerilog, string expectedPackage, string unexpectedPackage)
    {
        // Arrange
        var properties = new Dictionary<string, string> { ["UseSerilog"] = useSerilog };

        // Act
        var result = _builder.Build("Debug", properties);

        // Assert
        result.Success.Should().BeTrue($"build should succeed. Output: {result.Output}\nError: {result.Error}");

        var sbomPath = Path.Combine(_projectDirectory, "bin", "Debug", "net8.0", "sbom.json");
        File.Exists(sbomPath).Should().BeTrue($"SBOM should be generated at {sbomPath}");

        using var sbom = SbomHelper.ReadJsonSbom(sbomPath);
        SbomHelper.ContainsPackage(sbom, expectedPackage).Should().BeTrue(
            $"SBOM should contain {expectedPackage} when UseSerilog={useSerilog}");
        SbomHelper.ContainsPackage(sbom, unexpectedPackage).Should().BeFalse(
            $"SBOM should not contain {unexpectedPackage} when UseSerilog={useSerilog}");
    }

    public void Dispose()
    {
        _builder?.Dispose();
        GC.SuppressFinalize(this);
    }
}
