# Contributing to CycloneDX.MSBuild

Thank you for your interest in contributing to CycloneDX.MSBuild! This document provides guidelines and instructions for contributing.

## Code of Conduct

This project adheres to the CycloneDX community standards. By participating, you are expected to uphold professional and respectful communication.

## How to Contribute

### Reporting Issues

- Use the GitHub issue tracker
- Search existing issues before creating a new one
- Provide detailed reproduction steps
- Include environment details (OS, .NET version, MSBuild version)

### Suggesting Features

- Open a GitHub issue with the label "enhancement"
- Clearly describe the use case and benefits
- Consider backward compatibility

### Pull Requests

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/your-feature`)
3. Make your changes following our coding standards
4. Add tests for new functionality
5. Update documentation
6. Commit with clear messages
7. Push to your fork
8. Open a pull request

## Development Guidelines

### Security by Design

All contributions must follow these security principles:

- **No Elevated Permissions**: Code runs in build context only
- **Input Validation**: Validate all MSBuild properties
- **Fail-Safe Defaults**: Default to safe behavior
- **No Arbitrary Code Execution**: Only execute vetted tools
- **Dependency Pinning**: Use explicit versions

### Clean Code Principles

- **Separation of Concerns**: Keep configuration (.props) and logic (.targets) separate
- **Single Responsibility**: Each target does one thing
- **DRY**: Don't repeat yourself
- **Meaningful Names**: Use clear, descriptive names
- **Documentation**: Comment complex logic

### MSBuild Best Practices

- Use conditions to avoid unnecessary execution
- Provide clear messages for errors and warnings
- Use appropriate message importance levels
- Handle multi-targeting scenarios
- Test with various project types

### Code Style

- Follow existing patterns in the codebase
- Use XML formatting for .props and .targets files
- Indent with 2 spaces
- Keep lines under 120 characters

## Testing

### Manual Testing

Test your changes with:

1. Simple single-target projects
2. Multi-targeting projects
3. Projects with disabled SBOM generation
4. Custom configuration scenarios

### Test Projects

Use the integration test projects:

```bash
# Build test projects
dotnet build tests/Integration.Tests/SimpleProject/SimpleProject.csproj
dotnet build tests/Integration.Tests/MultiTargetProject/MultiTargetProject.csproj
dotnet build tests/Integration.Tests/DisabledProject/DisabledProject.csproj

# Verify SBOM generation
ls tests/Integration.Tests/SimpleProject/bin/Debug/net8.0/bom.json
```

## Documentation

Update documentation when:

- Adding new features
- Changing configuration options
- Modifying behavior
- Adding examples

Documentation locations:
- `README.md` - User-facing documentation
- XML comments in `.props` and `.targets` - Property descriptions
- `CONTRIBUTING.md` - Development guidelines

## Release Process

1. Update version in `CycloneDX.MSBuild.csproj`
2. Update `CHANGELOG.md` (if present)
3. Update `README.md` version references
4. Create a pull request
5. After merge, maintainers will create a release tag

## Questions?

- Open a GitHub discussion
- Check existing documentation
- Review similar issues

## License

By contributing, you agree that your contributions will be licensed under the MIT License.
