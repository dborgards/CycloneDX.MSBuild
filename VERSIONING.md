# Versioning & Release Process

This project uses automated semantic versioning based on [Conventional Commits](https://www.conventionalcommits.org/).

## 🔄 How It Works

### MinVer
[MinVer](https://github.com/adamralph/minver) automatically determines the version during build based on Git tags and commit history.

- **No manual version bumping required**
- Version is calculated from Git tags (e.g., `1.2.3`)
- Pre-release versions include commit information (e.g., `1.2.3-alpha.0.1+abc123`)

### semantic-release
[semantic-release](https://semantic-release.gitbook.io/) automates the entire release process:

1. **Analyzes commits** since the last release
2. **Determines version bump** based on commit types
3. **Generates CHANGELOG.md**
4. **Creates Git tag**
5. **Builds and publishes NuGet package**
6. **Creates GitHub Release** with release notes

## 📝 Commit Message Format

Follow the [Conventional Commits](https://www.conventionalcommits.org/) specification:

```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

### Types and Their Impact

| Type | Description | Version Bump | Example |
|------|-------------|--------------|---------|
| `feat` | New feature | **Minor** (0.x.0) | `feat: add XML output format support` |
| `fix` | Bug fix | **Patch** (0.0.x) | `fix: correct SBOM filename extension` |
| `perf` | Performance improvement | **Patch** (0.0.x) | `perf: optimize dependency scanning` |
| `docs` | Documentation only | **Patch** (0.0.x) | `docs: update configuration examples` |
| `refactor` | Code refactoring | **Patch** (0.0.x) | `refactor: simplify target execution` |
| `test` | Test changes | **None** | `test: add multi-target integration test` |
| `build` | Build system changes | **None** | `build: update SDK version` |
| `ci` | CI/CD changes | **None** | `ci: add release workflow` |
| `chore` | Maintenance tasks | **None** | `chore: update dependencies` |
| `revert` | Revert previous commit | **Patch** (0.0.x) | `revert: undo feature X` |

### Breaking Changes

Add `BREAKING CHANGE:` in the footer or append `!` after the type to trigger a **Major** version bump (x.0.0):

```bash
# Option 1: With exclamation mark
feat!: redesign configuration API

# Option 2: With footer
feat: redesign configuration API

BREAKING CHANGE: Configuration property names have changed
```

## 🚀 Release Process

### Automatic Releases

Releases are triggered automatically when commits are pushed to these branches:

- **`main`** or **`master`**: Production releases (e.g., `1.2.3`)
- **`beta`**: Beta pre-releases (e.g., `1.2.3-beta.1`)
- **`alpha`**: Alpha pre-releases (e.g., `1.2.3-alpha.1`)

### Release Workflow

When you push to `main`:

1. GitHub Actions workflow starts
2. semantic-release analyzes commits
3. If releasable changes found:
   - Version is bumped
   - CHANGELOG.md is updated
   - Git tag is created
   - NuGet package is built
   - Package is published to NuGet.org (if `NUGET_API_KEY` is configured)
   - GitHub Release is created
   - CHANGELOG.md is committed back

### Manual Testing

To test the version locally:

```bash
# Install MinVer
dotnet restore

# Build and see the calculated version
dotnet build -c Release
dotnet pack -c Release

# Check the generated package version
ls -lh artifacts/
```

## 🔐 GitHub Secrets

For the full release workflow to work, configure these secrets in your GitHub repository:

### Required
- **`GITHUB_TOKEN`**: Automatically provided by GitHub Actions

### Optional
- **`NUGET_API_KEY`**: For publishing to NuGet.org
  - Get your API key from https://www.nuget.org/account/apikeys
  - Add to repository secrets at `Settings > Secrets and variables > Actions`

## 📋 Examples

### Feature Release (Minor Version)

```bash
git commit -m "feat: add support for CycloneDX 1.6 specification"
git push origin main
```
→ Version: `1.1.0` → `1.2.0`

### Bug Fix Release (Patch Version)

```bash
git commit -m "fix: resolve path encoding issue on Windows"
git push origin main
```
→ Version: `1.2.0` → `1.2.1`

### Breaking Change Release (Major Version)

```bash
git commit -m "feat!: change default output format to XML

BREAKING CHANGE: The default output format has changed from JSON to XML.
Use CycloneDxOutputFormat=json to maintain previous behavior."
git push origin main
```
→ Version: `1.2.1` → `2.0.0`

### Pre-release (Beta)

```bash
git checkout -b beta
git commit -m "feat: experimental feature X"
git push origin beta
```
→ Version: `1.2.1` → `1.3.0-beta.1`

## 🛠️ Troubleshooting

### No release created

Check that:
- You're pushing to `main`, `master`, `beta`, or `alpha` branch
- Commits follow Conventional Commits format
- There are releasable changes (not just `chore`, `ci`, `test`, `build`)
- GitHub Actions workflow completed successfully

### Version mismatch

MinVer calculates version from Git tags. Ensure:
- You have fetched all tags: `git fetch --tags`
- Tags follow semantic versioning: `v1.2.3` or `1.2.3`
- No duplicate or conflicting tags exist

### Commit validation fails

Use commitlint to validate locally:

```bash
npm install
echo "feat: my new feature" | npx commitlint
```

## 📚 References

- [Conventional Commits](https://www.conventionalcommits.org/)
- [semantic-release Documentation](https://semantic-release.gitbook.io/)
- [MinVer Documentation](https://github.com/adamralph/minver)
- [Keep a Changelog](https://keepachangelog.com/)
- [Semantic Versioning](https://semver.org/)

## 🤝 Contributing

When contributing, ensure your commits follow the Conventional Commits format. The CI will validate commit messages automatically.

For more details, see [CONTRIBUTING.md](CONTRIBUTING.md).
