using Xunit;

namespace CycloneDX.MSBuild.Tests.IntegrationTests;

/// <summary>
/// XUnit collection definitions to control test parallelization.
/// Tests in the same collection run sequentially to avoid file locking issues.
/// </summary>

// SimpleProject tests - serialize to avoid file locking on SimpleProject.dll
[CollectionDefinition("SimpleProject")]
public class SimpleProjectTestFixture
{
}

// MultiTargetProject tests - serialize to avoid file locking
[CollectionDefinition("MultiTargetProject")]
public class MultiTargetProjectTestFixture
{
}

// DisabledProject tests - serialize to avoid file locking
[CollectionDefinition("DisabledProject")]
public class DisabledProjectTestFixture
{
}

// Configuration tests can run in parallel with project-specific tests
// but serialize among themselves
[CollectionDefinition("Configuration")]
public class ConfigurationTestFixture
{
}
