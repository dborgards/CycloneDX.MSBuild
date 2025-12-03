using BenchmarkDotNet.Running;
using CycloneDX.MSBuild.Benchmarks;

Console.WriteLine("CycloneDX.MSBuild Performance Benchmarks");
Console.WriteLine("=========================================");
Console.WriteLine();
Console.WriteLine("This benchmark measures the performance impact of SBOM generation.");
Console.WriteLine("Results will be saved to the BenchmarkDotNet.Artifacts directory.");
Console.WriteLine();

var summary = BenchmarkRunner.Run<SbomGenerationBenchmarks>();

Console.WriteLine();
Console.WriteLine("Benchmark complete! Check the results in BenchmarkDotNet.Artifacts/");
