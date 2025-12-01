using System.Diagnostics;
using System.Text;

namespace CycloneDX.MSBuild.Tests.TestHelpers;

/// <summary>
/// Helper class for building test projects and capturing output.
/// </summary>
public class ProjectBuilder : IDisposable
{
    private readonly string _projectPath;
    private readonly string _workingDirectory;

    public ProjectBuilder(string projectPath)
    {
        _projectPath = projectPath;
        _workingDirectory = Path.GetDirectoryName(projectPath) ?? throw new InvalidOperationException("Invalid project path");
    }

    public BuildResult Build(string? configuration = null, Dictionary<string, string>? properties = null)
    {
        return ExecuteDotnetCommand("build", configuration, properties);
    }

    public BuildResult Pack(string? configuration = null, Dictionary<string, string>? properties = null)
    {
        return ExecuteDotnetCommand("pack", configuration, properties);
    }

    public BuildResult Publish(string? configuration = null, Dictionary<string, string>? properties = null)
    {
        return ExecuteDotnetCommand("publish", configuration, properties);
    }

    public BuildResult Clean(string? configuration = null)
    {
        return ExecuteDotnetCommand("clean", configuration);
    }

    private BuildResult ExecuteDotnetCommand(string command, string? configuration = null, Dictionary<string, string>? properties = null)
    {
        var arguments = new StringBuilder(command);
        arguments.Append($" \"{_projectPath}\"");

        if (!string.IsNullOrEmpty(configuration))
        {
            arguments.Append($" --configuration {configuration}");
        }

        if (properties != null)
        {
            foreach (var (key, value) in properties)
            {
                arguments.Append($" -p:{key}={value}");
            }
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = arguments.ToString(),
            WorkingDirectory = _workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = startInfo };
        var output = new StringBuilder();
        var error = new StringBuilder();

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
                output.AppendLine(e.Data);
        };

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
                error.AppendLine(e.Data);
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        return new BuildResult
        {
            ExitCode = process.ExitCode,
            Output = output.ToString(),
            Error = error.ToString(),
            Success = process.ExitCode == 0
        };
    }

    public void Dispose()
    {
        // Cleanup if needed
        GC.SuppressFinalize(this);
    }
}

public class BuildResult
{
    public int ExitCode { get; init; }
    public string Output { get; init; } = string.Empty;
    public string Error { get; init; } = string.Empty;
    public bool Success { get; init; }
}
