# Security Policy

## Supported Versions

We release security updates for the following versions of CycloneDX.MSBuild:

| Version | Supported          |
| ------- | ------------------ |
| 1.0.x   | :white_check_mark: |
| < 1.0   | :x:                |

## Reporting a Vulnerability

We take the security of CycloneDX.MSBuild seriously. If you believe you have found a security vulnerability, please report it to us as described below.

### Where to Report

**Please do not report security vulnerabilities through public GitHub issues.**

Instead, please report security vulnerabilities by:

1. **Opening a private security advisory** on GitHub:
   - Navigate to the repository's Security tab
   - Click "Report a vulnerability"
   - Fill out the advisory form with details

2. **Or email the maintainers directly** if you prefer:
   - Include "SECURITY" in the subject line
   - Provide a detailed description of the vulnerability
   - Include steps to reproduce if possible

### What to Include

Please include the following information in your report:

- Type of vulnerability (e.g., command injection, information disclosure, etc.)
- Full paths of source file(s) related to the vulnerability
- Location of the affected source code (tag/branch/commit or direct URL)
- Any special configuration required to reproduce the issue
- Step-by-step instructions to reproduce the issue
- Proof-of-concept or exploit code (if possible)
- Impact of the issue, including how an attacker might exploit it

### Response Timeline

- **Initial Response**: We will acknowledge receipt of your vulnerability report within 48 hours
- **Status Updates**: We will provide regular updates (at least every 5 business days) on our progress
- **Resolution Timeline**: We aim to release a fix within 30 days of initial report for critical vulnerabilities
- **Disclosure**: We will coordinate with you on the disclosure timeline

### What to Expect

1. **Acknowledgment**: We'll confirm receipt and begin investigation
2. **Assessment**: We'll assess the vulnerability and determine severity
3. **Fix Development**: We'll develop and test a fix
4. **Security Advisory**: We'll publish a security advisory (with credit to reporter if desired)
5. **Release**: We'll release a patched version
6. **Notification**: We'll notify users through GitHub releases and security advisories

## Security Update Policy

### Critical Vulnerabilities

- **Definition**: Vulnerabilities that allow arbitrary code execution, privilege escalation, or unauthorized access
- **Response Time**: Fix within 7 days
- **Notification**: Immediate security advisory and emergency release

### High Severity Vulnerabilities

- **Definition**: Vulnerabilities that could lead to information disclosure or denial of service
- **Response Time**: Fix within 30 days
- **Notification**: Security advisory and patch release

### Medium/Low Severity Vulnerabilities

- **Definition**: Vulnerabilities with limited impact or require specific conditions
- **Response Time**: Fix in next regular release (typically within 90 days)
- **Notification**: Mentioned in changelog and release notes

## Security Best Practices

When using CycloneDX.MSBuild, we recommend:

1. **Keep Updated**: Always use the latest version to receive security patches
2. **Review Generated SBOMs**: Ensure SBOMs don't inadvertently expose sensitive information
3. **Secure GitHub Tokens**: If using GitHub license resolution, protect your tokens:
   - Use read-only tokens with minimal scopes
   - Never commit tokens to source control
   - Use environment variables or secure secret management
4. **Validate Tool Installation**: Ensure the CycloneDX tool is downloaded from official sources
5. **Network Security**: Be aware that GitHub license resolution requires network access
6. **Build Environment**: Run builds in isolated, secure environments
7. **Dependency Scanning**: Regularly scan your dependencies for known vulnerabilities

## Security Design Principles

CycloneDX.MSBuild is designed with security in mind:

1. **No Elevated Privileges**: Runs in standard build context without requiring admin rights
2. **Minimal Network Access**: Only makes network calls when GitHub license resolution is explicitly enabled
3. **No Code Execution**: Only invokes the official CycloneDX .NET tool via dotnet tool
4. **Read-Only Operations**: Only reads project files and dependency information
5. **Fail-Safe Defaults**: SBOM generation failures don't break builds by default
6. **No Telemetry**: Doesn't collect or transmit usage data
7. **Transparent Operation**: All operations are logged and visible in build output

## Known Security Considerations

### GitHub API Token Usage

If you enable GitHub license resolution (`CycloneDxEnableGitHubLicenses=true`):

- **Rate Limiting**: Unauthenticated requests are limited to 60/hour
- **Token Security**: Store tokens securely (use secrets, not plain text)
- **Minimal Permissions**: Use tokens with read-only repository access
- **Token Exposure**: Ensure build logs don't expose tokens

### SBOM Content

Generated SBOMs may contain:

- Dependency names and versions (generally public information)
- License information
- Package URLs and repository locations
- Timestamps and serial numbers

**Recommendation**: Review SBOMs before public distribution to ensure no sensitive information is included.

### Tool Installation

The package automatically installs the CycloneDX .NET tool:

- Downloaded from official NuGet.org source
- Version pinned by default for reproducibility
- Uses standard dotnet tool installation mechanism

## Security Champions

This project follows security best practices including:

- ✅ Dependency vulnerability scanning (via GitHub Dependabot)
- ✅ Automated security updates
- ✅ Code review requirements for all changes
- ✅ Conventional commits for clear change tracking
- ✅ Semantic versioning for predictable updates
- ✅ Automated release process reducing human error
- ✅ Read-only operations in build context

## Credits

We gratefully acknowledge security researchers who help improve CycloneDX.MSBuild. Unless you prefer otherwise, we will publicly credit you in:

- The security advisory
- The release notes for the fix
- This SECURITY.md file (Hall of Fame section below)

### Hall of Fame

*No security vulnerabilities have been reported yet.*

## Questions?

If you have questions about this security policy, please open a discussion in the GitHub repository.

---

*Last updated: December 1, 2025*
