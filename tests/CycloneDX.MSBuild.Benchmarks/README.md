# CycloneDX.MSBuild Performance Benchmarks

This project contains performance benchmarks for CycloneDX.MSBuild using BenchmarkDotNet.

## Running Benchmarks

```bash
cd tests/CycloneDX.MSBuild.Benchmarks
dotnet run -c Release
```

## What's Measured

The benchmarks measure:

1. **Build time with SBOM generation** - Full build + SBOM generation
2. **Build time without SBOM generation** - Baseline (no SBOM)
3. **Multi-target project builds** - SBOM generation for projects with multiple target frameworks
4. **Incremental build performance** - First build vs cached builds with incremental SBOM enabled

## Interpreting Results

BenchmarkDotNet will output:

- **Mean**: Average execution time
- **Error**: Half of 99.9% confidence interval
- **StdDev**: Standard deviation of all measurements
- **Gen0/Gen1/Gen2**: Garbage collection statistics
- **Allocated**: Total memory allocated

### Expected Results

Typical overhead for SBOM generation:

- **Simple projects**: 2-5 seconds
- **Multi-target projects**: 3-7 seconds (still single SBOM)
- **Incremental builds (cached)**: < 100ms (SBOM reused)

The overhead comes from:
1. Tool invocation (~1s)
2. Dependency analysis (~1-2s)
3. SBOM file generation (~0.5-1s)
4. File I/O (~0.5s)

## Continuous Performance Monitoring

For CI/CD integration, consider:

1. **Baseline tracking**: Store benchmark results and track over time
2. **Regression detection**: Fail builds if performance degrades > 10%
3. **Performance budgets**: Set maximum acceptable SBOM generation time

### Example: GitHub Actions Integration

```yaml
- name: Run benchmarks
  run: dotnet run -c Release --project tests/CycloneDX.MSBuild.Benchmarks/

- name: Store baseline
  uses: benchmark-action/github-action-benchmark@v1
  with:
    tool: 'benchmarkdotnet'
    output-file-path: tests/CycloneDX.MSBuild.Benchmarks/BenchmarkDotNet.Artifacts/results/
    alert-threshold: '110%'
    fail-on-alert: true
```

## Optimization Tips

If SBOM generation is too slow:

1. **Enable incremental builds**:
   ```xml
   <CycloneDxEnableIncrementalBuild>true</CycloneDxEnableIncrementalBuild>
   ```

2. **Disable for Debug builds**:
   ```xml
   <PropertyGroup Condition="'$(Configuration)' == 'Debug'">
     <GenerateCycloneDxSbom>false</GenerateCycloneDxSbom>
   </PropertyGroup>
   ```

3. **Exclude test projects**:
   ```xml
   <CycloneDxExcludeTestProjects>true</CycloneDxExcludeTestProjects>
   ```

4. **Use custom output directory** (avoid file locking):
   ```xml
   <CycloneDxOutputDirectory>$(MSBuildProjectDirectory)/sbom/</CycloneDxOutputDirectory>
   ```

## Custom Benchmarks

To add more benchmarks, edit `SbomGenerationBenchmarks.cs` and add methods with the `[Benchmark]` attribute:

```csharp
[Benchmark(Description = "Your custom benchmark")]
public void YourBenchmark()
{
    // Your benchmark code
}
```

## Benchmark Configuration

The benchmarks use:

- **MemoryDiagnoser**: Tracks memory allocations
- **Orderer**: Orders results from fastest to slowest
- **RankColumn**: Shows relative performance ranking

Customize in `SbomGenerationBenchmarks.cs` by modifying class attributes.

## Troubleshooting

### Benchmark Takes Too Long

- Reduce iterations: Add `[SimpleJob(launchCount: 1, warmupCount: 3, iterationCount: 5)]`
- Quick mode: `dotnet run -c Release -- --filter * --job Dry`

### Projects Not Found

- Ensure integration test projects exist in `tests/Integration.Tests/`
- Check paths in `Setup()` method

### Build Failures During Benchmark

- Check that test projects build successfully outside benchmarks
- Ensure CycloneDX.MSBuild package is installed in test projects

## Results Archive

Store benchmark results for historical analysis:

```bash
# After running benchmarks
cp -r BenchmarkDotNet.Artifacts/results benchmark-results-$(date +%Y%m%d)
```

## CI/CD Integration

For automated performance tracking:

1. Run benchmarks on every release
2. Store results as build artifacts
3. Compare with baseline
4. Alert on regressions

See `.github/workflows/` for example integration.

---

*Last updated: December 1, 2025*
