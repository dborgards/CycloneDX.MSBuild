using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Diagnostics;

namespace CycloneDX.MSBuild.Benchmarks;

/// <summary>
/// Benchmarks for SBOM generation performance.
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class SbomGenerationBenchmarks
{
    private string _simpleProjectPath = string.Empty;
    private string _multiTargetProjectPath = string.Empty;

    [GlobalSetup]
    public void Setup()
    {
        // Find the test projects
        var currentDir = Directory.GetCurrentDirectory();
        var baseDir = Path.Combine(currentDir, "..", "..", "..", "..", "Integration.Tests");

        _simpleProjectPath = Path.Combine(baseDir, "SimpleProject", "SimpleProject.csproj");
        _multiTargetProjectPath = Path.Combine(baseDir, "MultiTargetProject", "MultiTargetProject.csproj");

        // Clean up before benchmarking
        CleanProject(_simpleProjectPath);
        CleanProject(_multiTargetProjectPath);
    }

    [Benchmark(Description = "Build SimpleProject with SBOM generation")]
    public void BuildSimpleProjectWithSbom()
    {
        RunDotnetCommand("build", _simpleProjectPath, new Dictionary<string, string>
        {
            ["GenerateCycloneDxSbom"] = "true"
        });
    }

    [Benchmark(Description = "Build SimpleProject without SBOM")]
    public void BuildSimpleProjectWithoutSbom()
    {
        RunDotnetCommand("build", _simpleProjectPath, new Dictionary<string, string>
        {
            ["GenerateCycloneDxSbom"] = "false"
        });
    }

    [Benchmark(Description = "Build MultiTargetProject with SBOM")]
    public void BuildMultiTargetProjectWithSbom()
    {
        RunDotnetCommand("build", _multiTargetProjectPath, new Dictionary<string, string>
        {
            ["GenerateCycloneDxSbom"] = "true"
        });
    }

    [Benchmark(Description = "Build with incremental SBOM (first build)")]
    public void BuildWithIncrementalSbomFirstBuild()
    {
        // Clean first to ensure fresh build
        CleanProject(_simpleProjectPath);

        RunDotnetCommand("build", _simpleProjectPath, new Dictionary<string, string>
        {
            ["GenerateCycloneDxSbom"] = "true",
            ["CycloneDxEnableIncrementalBuild"] = "true"
        });
    }

    [Benchmark(Description = "Build with incremental SBOM (cached)")]
    public void BuildWithIncrementalSbomCached()
    {
        // Don't clean - use cached SBOM
        RunDotnetCommand("build", _simpleProjectPath, new Dictionary<string, string>
        {
            ["GenerateCycloneDxSbom"] = "true",
            ["CycloneDxEnableIncrementalBuild"] = "true"
        });
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        // Clean between iterations
        CleanProject(_simpleProjectPath);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        CleanProject(_simpleProjectPath);
        CleanProject(_multiTargetProjectPath);
    }

    private void CleanProject(string projectPath)
    {
        RunDotnetCommand("clean", projectPath);
    }

    private void RunDotnetCommand(string command, string projectPath, Dictionary<string, string>? properties = null)
    {
        var arguments = $"{command} \"{projectPath}\" --verbosity quiet";

        if (properties != null)
        {
            foreach (var (key, value) in properties)
            {
                arguments += $" -p:{key}={value}";
            }
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = arguments,
            WorkingDirectory = Path.GetDirectoryName(projectPath) ?? string.Empty,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = startInfo };
        process.Start();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            var error = process.StandardError.ReadToEnd();
            throw new Exception($"Command failed: dotnet {arguments}\nError: {error}");
        }
    }
}
