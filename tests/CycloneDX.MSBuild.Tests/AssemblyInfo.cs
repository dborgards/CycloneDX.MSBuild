using Xunit;

// Disable test parallelization to prevent file locking issues
// Multiple test classes build the same Integration.Tests projects (e.g., SimpleProject.csproj)
// Running these builds in parallel causes CS2012 errors (cannot access file because it is being used)
[assembly: CollectionBehavior(DisableTestParallelization = true)]
