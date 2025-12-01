using System.Text.Json;
using System.Xml.Linq;

namespace CycloneDX.MSBuild.Tests.TestHelpers;

/// <summary>
/// Helper class for working with SBOM files.
/// </summary>
public static class SbomHelper
{
    /// <summary>
    /// Reads and parses a JSON SBOM file.
    /// </summary>
    public static JsonDocument ReadJsonSbom(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"SBOM file not found: {path}");

        var content = File.ReadAllText(path);
        return JsonDocument.Parse(content);
    }

    /// <summary>
    /// Reads and parses an XML SBOM file.
    /// </summary>
    public static XDocument ReadXmlSbom(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"SBOM file not found: {path}");

        return XDocument.Load(path);
    }

    /// <summary>
    /// Gets the SBOM spec version from a JSON SBOM.
    /// </summary>
    public static string? GetSpecVersion(JsonDocument sbom)
    {
        return sbom.RootElement.GetProperty("specVersion").GetString();
    }

    /// <summary>
    /// Gets the SBOM serial number from a JSON SBOM.
    /// </summary>
    public static string? GetSerialNumber(JsonDocument sbom)
    {
        return sbom.RootElement.GetProperty("serialNumber").GetString();
    }

    /// <summary>
    /// Gets all components from a JSON SBOM.
    /// </summary>
    public static JsonElement.ArrayEnumerator GetComponents(JsonDocument sbom)
    {
        if (sbom.RootElement.TryGetProperty("components", out var components))
        {
            return components.EnumerateArray();
        }
        return default;
    }

    /// <summary>
    /// Gets metadata component from a JSON SBOM.
    /// </summary>
    public static JsonElement? GetMetadataComponent(JsonDocument sbom)
    {
        if (sbom.RootElement.TryGetProperty("metadata", out var metadata) &&
            metadata.TryGetProperty("component", out var component))
        {
            return component;
        }
        return null;
    }

    /// <summary>
    /// Gets the count of components in a JSON SBOM.
    /// </summary>
    public static int GetComponentCount(JsonDocument sbom)
    {
        if (sbom.RootElement.TryGetProperty("components", out var components))
        {
            return components.GetArrayLength();
        }
        return 0;
    }

    /// <summary>
    /// Checks if a specific package is present in the SBOM components.
    /// </summary>
    public static bool ContainsPackage(JsonDocument sbom, string packageName)
    {
        foreach (var component in GetComponents(sbom))
        {
            if (component.TryGetProperty("name", out var name) &&
                name.GetString()?.Equals(packageName, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Gets a component by name from the SBOM.
    /// </summary>
    public static JsonElement? GetComponentByName(JsonDocument sbom, string packageName)
    {
        foreach (var component in GetComponents(sbom))
        {
            if (component.TryGetProperty("name", out var name) &&
                name.GetString()?.Equals(packageName, StringComparison.OrdinalIgnoreCase) == true)
            {
                return component;
            }
        }
        return null;
    }

    /// <summary>
    /// Validates basic SBOM structure (has required fields).
    /// </summary>
    public static bool IsValidBasicStructure(JsonDocument sbom)
    {
        try
        {
            // Required fields per CycloneDX spec
            var hasBomFormat = sbom.RootElement.TryGetProperty("bomFormat", out _);
            var hasSpecVersion = sbom.RootElement.TryGetProperty("specVersion", out _);
            var hasVersion = sbom.RootElement.TryGetProperty("version", out _);

            return hasBomFormat && hasSpecVersion && hasVersion;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the BOM format from a JSON SBOM.
    /// </summary>
    public static string? GetBomFormat(JsonDocument sbom)
    {
        if (sbom.RootElement.TryGetProperty("bomFormat", out var format))
        {
            return format.GetString();
        }
        return null;
    }

    /// <summary>
    /// Gets the version number from a JSON SBOM.
    /// </summary>
    public static int? GetVersion(JsonDocument sbom)
    {
        if (sbom.RootElement.TryGetProperty("version", out var version))
        {
            return version.GetInt32();
        }
        return null;
    }
}
