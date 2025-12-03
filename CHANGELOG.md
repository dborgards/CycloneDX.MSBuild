## [1.2.1](https://github.com/dborgards/CycloneDX.MSBuild/compare/v1.2.0...v1.2.1) (2025-12-03)

### 🐛 Bug Fixes

* ensure SBOM version field is present in generated SBOMs ([6046907](https://github.com/dborgards/CycloneDX.MSBuild/commit/60469071bd6fe1d5d74a4d1e2ee4547a4dd86019))

### ♻️ Code Refactoring

* optimize SBOM version field validation - parse JSON once ([1a07aab](https://github.com/dborgards/CycloneDX.MSBuild/commit/1a07aab19ca6738d59f9ea76b7ced43ed02ae4d0))

## [1.2.0](https://github.com/dborgards/CycloneDX.MSBuild/compare/v1.1.0...v1.2.0) (2025-12-01)

### ✨ Features

* add automatic SBOM metadata generation from assembly properties ([e32ba42](https://github.com/dborgards/CycloneDX.MSBuild/commit/e32ba424e8b8c03d9d2711005aac4b4ce7a2327b))
* **msbuild:** improve SBOM generation and XML handling ([5ca72e3](https://github.com/dborgards/CycloneDX.MSBuild/commit/5ca72e35316647ef3e43d7a0050c42faa70d0842))

### ♻️ Code Refactoring

* **build:** simplify CycloneDX MSBuild targets ([87cd7f0](https://github.com/dborgards/CycloneDX.MSBuild/commit/87cd7f01bacd7ab5d0f2bade3066a47957f26384))
* **msbuild:** simplify CycloneDX target handling ([7b8ebc8](https://github.com/dborgards/CycloneDX.MSBuild/commit/7b8ebc87a30fff69c0e1606ed633fa7e4b3e0433))

## [1.1.0](https://github.com/dborgards/CycloneDX.MSBuild/compare/v1.0.2...v1.1.0) (2025-12-01)

### ✨ Features

* add support for SBOM generation during dotnet publish ([8b05e28](https://github.com/dborgards/CycloneDX.MSBuild/commit/8b05e28392fe911f4c011fc73ea2447d4da9cd80))

## [1.0.2](https://github.com/dborgards/CycloneDX.MSBuild/compare/v1.0.1...v1.0.2) (2025-12-01)

### 🐛 Bug Fixes

* remove MinVer to allow semantic-release version control ([8fbafe6](https://github.com/dborgards/CycloneDX.MSBuild/commit/8fbafe607956d4f3b3a51b6e26730e4edc0c6195))

## [1.0.1](https://github.com/dborgards/CycloneDX.MSBuild/compare/v1.0.0...v1.0.1) (2025-12-01)

### 🐛 Bug Fixes

* correct NUGET_API_KEY syntax in semantic-release config ([254236c](https://github.com/dborgards/CycloneDX.MSBuild/commit/254236c6148c6151dde5c3180f67b5fdf1cd0f61))

## 1.0.0 (2025-12-01)

### ✨ Features

* add comprehensive CI/CD pipeline with automated NuGet publishing ([30b324d](https://github.com/dborgards/CycloneDX.MSBuild/commit/30b324d706a33efd79e89accd95a31c3c36c10f1))
* Add support for importing SBOM metadata templates ([82572e7](https://github.com/dborgards/CycloneDX.MSBuild/commit/82572e7ba0e59d5dc93a811968583b53774c6d86))
* Change default SBOM filename from bom to sbom ([09928c4](https://github.com/dborgards/CycloneDX.MSBuild/commit/09928c434df7768e5b3d36e982e59c45bd2b1afb))
* implement automated semantic versioning with MinVer and semantic-release ([8441d66](https://github.com/dborgards/CycloneDX.MSBuild/commit/8441d66e9c831644ebbeeca97bc64f2b44a07ade))
* Implement CycloneDX.MSBuild NuGet package ([f5792f6](https://github.com/dborgards/CycloneDX.MSBuild/commit/f5792f602b992e54e08b7472388e7f0b535e66dd))

### 🐛 Bug Fixes

* Add explicit imports for local testing with ProjectReference ([c2b046f](https://github.com/dborgards/CycloneDX.MSBuild/commit/c2b046fc571f7d825105e4c28835e67d94a56285))
* Add fallback values for format variables ([ac5f6ca](https://github.com/dborgards/CycloneDX.MSBuild/commit/ac5f6cad900e1da89cace0a0011bdd06112e0918))
* Add file extension to SBOM filename ([2072c4e](https://github.com/dborgards/CycloneDX.MSBuild/commit/2072c4e1fd684dc74a5d69a47ceafc464651b7ab))
* add package-lock.json for reproducible npm ci builds ([a61d0be](https://github.com/dborgards/CycloneDX.MSBuild/commit/a61d0be672c6c6a682318f2fb5f1b028e9ce4ddd))
* Correct CycloneDX tool parameters and update configuration ([3ae48ea](https://github.com/dborgards/CycloneDX.MSBuild/commit/3ae48ea59b070e827fe17984744b93d49d0911bc))
* Correct LangVersion and conditional feature enablement ([10198e6](https://github.com/dborgards/CycloneDX.MSBuild/commit/10198e6f7727bb64c0f7145f1a42072859efaf1c))
* Correct license references to MIT ([ca6972e](https://github.com/dborgards/CycloneDX.MSBuild/commit/ca6972e0ce082e50ea2bd30f5c097e49abb48252))
* Escape backslash in TrimEnd function ([1777d76](https://github.com/dborgards/CycloneDX.MSBuild/commit/1777d767490315ee6b6eca4a42c2187098a0e24c))
* improve CI/CD workflow quality gates and naming accuracy ([c5e499b](https://github.com/dborgards/CycloneDX.MSBuild/commit/c5e499b5b30aac1ddb6c8b2e02893cb83d7edc7a))
* Remove trailing slashes from output directory path ([60b0491](https://github.com/dborgards/CycloneDX.MSBuild/commit/60b0491ccc2f01fdeeeb4ccc76ca4ab89c1e8985))
* Set CycloneDxOutputDirectory default at build time ([0cc96e4](https://github.com/dborgards/CycloneDX.MSBuild/commit/0cc96e4694c01803890c01eba43fdfd6eb8b41d1))
* update repository URL to point to fork ([0aacc62](https://github.com/dborgards/CycloneDX.MSBuild/commit/0aacc6239dc801c85243f796da21debb8f2e2df5))

### 📚 Documentation

* Update README to reflect actual configuration options ([5a28122](https://github.com/dborgards/CycloneDX.MSBuild/commit/5a281227e1bc90f0a491f161a037182dfb05ffc6))

## 1.0.0 (2025-12-01)

### ✨ Features

* add comprehensive CI/CD pipeline with automated NuGet publishing ([30b324d](https://github.com/dborgards/CycloneDX.MSBuild/commit/30b324d706a33efd79e89accd95a31c3c36c10f1))
* Add support for importing SBOM metadata templates ([82572e7](https://github.com/dborgards/CycloneDX.MSBuild/commit/82572e7ba0e59d5dc93a811968583b53774c6d86))
* Change default SBOM filename from bom to sbom ([09928c4](https://github.com/dborgards/CycloneDX.MSBuild/commit/09928c434df7768e5b3d36e982e59c45bd2b1afb))
* implement automated semantic versioning with MinVer and semantic-release ([8441d66](https://github.com/dborgards/CycloneDX.MSBuild/commit/8441d66e9c831644ebbeeca97bc64f2b44a07ade))
* Implement CycloneDX.MSBuild NuGet package ([f5792f6](https://github.com/dborgards/CycloneDX.MSBuild/commit/f5792f602b992e54e08b7472388e7f0b535e66dd))

### 🐛 Bug Fixes

* Add explicit imports for local testing with ProjectReference ([c2b046f](https://github.com/dborgards/CycloneDX.MSBuild/commit/c2b046fc571f7d825105e4c28835e67d94a56285))
* Add fallback values for format variables ([ac5f6ca](https://github.com/dborgards/CycloneDX.MSBuild/commit/ac5f6cad900e1da89cace0a0011bdd06112e0918))
* Add file extension to SBOM filename ([2072c4e](https://github.com/dborgards/CycloneDX.MSBuild/commit/2072c4e1fd684dc74a5d69a47ceafc464651b7ab))
* add package-lock.json for reproducible npm ci builds ([a61d0be](https://github.com/dborgards/CycloneDX.MSBuild/commit/a61d0be672c6c6a682318f2fb5f1b028e9ce4ddd))
* Correct CycloneDX tool parameters and update configuration ([3ae48ea](https://github.com/dborgards/CycloneDX.MSBuild/commit/3ae48ea59b070e827fe17984744b93d49d0911bc))
* Correct LangVersion and conditional feature enablement ([10198e6](https://github.com/dborgards/CycloneDX.MSBuild/commit/10198e6f7727bb64c0f7145f1a42072859efaf1c))
* Correct license references to MIT ([ca6972e](https://github.com/dborgards/CycloneDX.MSBuild/commit/ca6972e0ce082e50ea2bd30f5c097e49abb48252))
* Escape backslash in TrimEnd function ([1777d76](https://github.com/dborgards/CycloneDX.MSBuild/commit/1777d767490315ee6b6eca4a42c2187098a0e24c))
* improve CI/CD workflow quality gates and naming accuracy ([c5e499b](https://github.com/dborgards/CycloneDX.MSBuild/commit/c5e499b5b30aac1ddb6c8b2e02893cb83d7edc7a))
* Remove trailing slashes from output directory path ([60b0491](https://github.com/dborgards/CycloneDX.MSBuild/commit/60b0491ccc2f01fdeeeb4ccc76ca4ab89c1e8985))
* Set CycloneDxOutputDirectory default at build time ([0cc96e4](https://github.com/dborgards/CycloneDX.MSBuild/commit/0cc96e4694c01803890c01eba43fdfd6eb8b41d1))
* update repository URL to point to fork ([0aacc62](https://github.com/dborgards/CycloneDX.MSBuild/commit/0aacc6239dc801c85243f796da21debb8f2e2df5))

### 📚 Documentation

* Update README to reflect actual configuration options ([5a28122](https://github.com/dborgards/CycloneDX.MSBuild/commit/5a281227e1bc90f0a491f161a037182dfb05ffc6))
