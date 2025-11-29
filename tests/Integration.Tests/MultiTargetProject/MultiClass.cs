namespace MultiTargetProject;

/// <summary>
/// Multi-target test class for SBOM generation testing.
/// </summary>
public class MultiTargetClass
{
    public string GetTargetFramework()
    {
#if NET8_0
        return "net8.0";
#elif NET6_0
        return "net6.0";
#elif NETSTANDARD2_0
        return "netstandard2.0";
#else
        return "unknown";
#endif
    }
}
