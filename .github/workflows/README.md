# CI/CD Workflows

This directory contains GitHub Actions workflows for automated build, test, and deployment.

## Workflows

### 1. CI Workflow (`ci.yml`)

**Trigger:** Runs on all pushes (except main/master) and pull requests

**Jobs:**

#### Build and Test
- Runs on multiple platforms (Ubuntu, Windows, macOS)
- Tests .NET compatibility with versions 6.0.x and 8.0.x
- Builds the main solution and all test projects
- Validates NuGet package creation
- Uploads build artifacts for inspection

#### Code Quality Checks
- Verifies code formatting standards
- Validates NuGet package metadata
- Ensures package quality requirements

#### Dependency Security Check
- Scans for vulnerable dependencies
- Checks for deprecated packages
- Provides early warning for security issues

### 2. Release Workflow (`release.yml`)

**Trigger:** Runs on pushes to main/master/beta/alpha branches

**Process:**
1. **Build & Test** - Compiles the project and runs all tests
2. **Package Validation** - Validates the NuGet package structure
3. **Semantic Versioning** - Analyzes commits using conventional commits
4. **Version Bump** - Automatically determines the next version
5. **Create Release** - Creates GitHub release with release notes
6. **Publish to NuGet.org** - Publishes package after successful tests

**Required Secrets:**
- `GITHUB_TOKEN` - Automatically provided by GitHub Actions
- `NUGET_API_KEY` - Must be configured in repository secrets

## Setting up NuGet Publishing

### 1. Get a NuGet API Key

1. Go to [NuGet.org](https://www.nuget.org)
2. Sign in to your account
3. Navigate to **API Keys** under your account settings
4. Click **Create** and configure:
   - **Key Name:** GitHub Actions - CycloneDX.MSBuild
   - **Glob Pattern:** CycloneDX.MSBuild
   - **Select Scopes:** Push new packages and package versions
   - **Expiration:** Choose appropriate duration

### 2. Add the API Key to GitHub Secrets

1. Go to your repository on GitHub
2. Navigate to **Settings** → **Secrets and variables** → **Actions**
3. Click **New repository secret**
4. Name: `NUGET_API_KEY`
5. Value: Paste your NuGet API key
6. Click **Add secret**

## Semantic Versioning

This project uses [semantic-release](https://github.com/semantic-release/semantic-release) with [conventional commits](https://www.conventionalcommits.org/).

### Commit Message Format

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types and Version Bumps

| Commit Type | Version Bump | Example |
|-------------|--------------|---------|
| `feat:` | Minor | `feat: add XML format support` |
| `fix:` | Patch | `fix: resolve null reference in generator` |
| `perf:` | Patch | `perf: optimize SBOM generation speed` |
| `refactor:` | Patch | `refactor: simplify configuration loading` |
| `docs:` | Patch | `docs: update installation instructions` |
| `BREAKING CHANGE:` | Major | `feat!: remove deprecated parameters` |

### Examples

**Feature (Minor version bump):**
```
feat: add support for custom SBOM templates

Implement ability to import and use custom SBOM metadata templates
during package generation.
```

**Bug Fix (Patch version bump):**
```
fix: resolve crash when output directory doesn't exist

Create output directory automatically if it doesn't exist instead
of throwing an exception.
```

**Breaking Change (Major version bump):**
```
feat!: change default output format to JSON

BREAKING CHANGE: The default SBOM output format is now JSON instead
of XML. Users who depend on XML format must explicitly configure it.
```

## Workflow Status Badges

Add these badges to your README.md:

```markdown
[![CI](https://github.com/YOUR_USERNAME/CycloneDX.MSBuild/actions/workflows/ci.yml/badge.svg)](https://github.com/YOUR_USERNAME/CycloneDX.MSBuild/actions/workflows/ci.yml)
[![Release](https://github.com/YOUR_USERNAME/CycloneDX.MSBuild/actions/workflows/release.yml/badge.svg)](https://github.com/YOUR_USERNAME/CycloneDX.MSBuild/actions/workflows/release.yml)
[![NuGet](https://img.shields.io/nuget/v/CycloneDX.MSBuild.svg)](https://www.nuget.org/packages/CycloneDX.MSBuild/)
```

## Testing the Pipeline

### Test the CI Workflow
1. Create a feature branch: `git checkout -b feature/test-ci`
2. Make a change and commit: `git commit -m "test: verify CI pipeline"`
3. Push the branch: `git push origin feature/test-ci`
4. Open a pull request and verify all checks pass

### Test the Release Workflow
1. Merge a commit to main with a conventional commit message
2. The workflow will automatically:
   - Determine the version based on commit messages
   - Create a Git tag
   - Generate release notes
   - Publish to NuGet.org
   - Create a GitHub release

## Troubleshooting

### NuGet Publishing Fails

**Error:** `401 Unauthorized`
- Check that `NUGET_API_KEY` secret is correctly configured
- Verify the API key hasn't expired
- Ensure the API key has "Push" permissions

**Error:** `409 Conflict` or "Package version already exists"
- This version has already been published
- Semantic-release handles this automatically with `--skip-duplicate`

### Build Fails on Specific Platform

- Check the CI workflow logs for the failing platform
- Test locally using the same .NET version
- Consider if the issue is platform-specific

### Tests Fail in CI but Pass Locally

- Ensure all dependencies are committed
- Check for environment-specific assumptions
- Verify file paths use correct separators

## Manual Publishing (Emergency)

If automated publishing fails, you can publish manually:

```bash
# Build and pack
dotnet pack src/CycloneDX.MSBuild/CycloneDX.MSBuild.csproj -c Release -p:Version=1.0.0

# Push to NuGet.org
dotnet nuget push ./artifacts/*.nupkg \
  --source https://api.nuget.org/v3/index.json \
  --api-key YOUR_NUGET_API_KEY
```

⚠️ **Note:** Manual publishing should be avoided as it bypasses version control and release automation.
