## [1.3.1](https://github.com/dborgards/CycloneDX.MSBuild/compare/v1.3.0...v1.3.1) (2025-12-10)

### 🐛 Bug Fixes

* disable SBOM generation for CycloneDX.MSBuild project ([8d47fd5](https://github.com/dborgards/CycloneDX.MSBuild/commit/8d47fd50aabf0e57965750da9ae5e38d589f4591))
* prevent file locking during parallel transitive builds ([3e14db6](https://github.com/dborgards/CycloneDX.MSBuild/commit/3e14db68edb537be1dbb45619d55673f0cc665ba))

## [1.3.0](https://github.com/dborgards/CycloneDX.MSBuild/compare/v1.2.1...v1.3.0) (2025-12-03)

### ✨ Features

* implement comprehensive project improvements ([f77d85a](https://github.com/dborgards/CycloneDX.MSBuild/commit/f77d85abead52af2c2e8bc680b69f93c0819bc19))

### 🐛 Bug Fixes

* add detailed diagnostics to failing tests ([bbad9d3](https://github.com/dborgards/CycloneDX.MSBuild/commit/bbad9d30417a9d822927a6e1a1408ed993b1c5e4))
* add detailed SBOM structure validation diagnostics ([1cd0d92](https://github.com/dborgards/CycloneDX.MSBuild/commit/1cd0d92e4331bf742e72b7fd0b4bd170b2c79dc0))
* add GetTargetPath target to MultiTargetProject for tooling compatibility ([a5ce1e9](https://github.com/dborgards/CycloneDX.MSBuild/commit/a5ce1e939a1e4c4bfa3ed0db4e2a290636060f8a))
* add Integration.Tests projects as build dependencies ([9d6276f](https://github.com/dborgards/CycloneDX.MSBuild/commit/9d6276f5e52f4a4b355e0ada390b43190f6a67f1))
* add new test projects to solution file ([39f24f5](https://github.com/dborgards/CycloneDX.MSBuild/commit/39f24f57428d0bfaa2137867259c6d69bd8d41bb))
* add project dependencies and prevent parallel builds for benchmarks and integration tests ([86e07f3](https://github.com/dborgards/CycloneDX.MSBuild/commit/86e07f330699e9548f90eba77584861d164a94ec))
* code formatting - add braces to single-line if statements ([cacc977](https://github.com/dborgards/CycloneDX.MSBuild/commit/cacc977374ad109f27cd9bec8ea1db7c9ed0da91))
* disable package generation in test project references ([78e2723](https://github.com/dborgards/CycloneDX.MSBuild/commit/78e27234532d80b496a3f4c502c22750a4354184))
* disable parallel builds at solution level with -m:1 ([7be6b47](https://github.com/dborgards/CycloneDX.MSBuild/commit/7be6b4744e7d5125477030ff9aec051d7cc69336)), closes [#1](https://github.com/dborgards/CycloneDX.MSBuild/issues/1) [#2](https://github.com/dborgards/CycloneDX.MSBuild/issues/2)
* disable parallel builds for all Integration.Tests projects ([c367c05](https://github.com/dborgards/CycloneDX.MSBuild/commit/c367c0519f3c077d4504317e83db897f1df0521e))
* enable SBOM copy to publish directory for multi-target inner builds ([4a26668](https://github.com/dborgards/CycloneDX.MSBuild/commit/4a2666844ef1687078ba892fad13ccad99055b68))
* enhance test diagnostics to identify missing SBOM fields ([7ebf344](https://github.com/dborgards/CycloneDX.MSBuild/commit/7ebf344e1fdde97a364e02d179626518b88042fc))
* improve test reliability and publish SBOM functionality ([8c27290](https://github.com/dborgards/CycloneDX.MSBuild/commit/8c27290d4a444158df0ef8edb8eeec418a178847))
* improve version field detection regex to handle any field order ([6a9f7af](https://github.com/dborgards/CycloneDX.MSBuild/commit/6a9f7af23f45a2eb4a73d34cf2434991237eff68))
* isolate project outputs and prevent file locking during builds ([0cba86c](https://github.com/dborgards/CycloneDX.MSBuild/commit/0cba86c08b4af03d4ff0c399fcf11ff80c4088e6))
* pass Configuration property to all ProjectReferences ([ed9b2bd](https://github.com/dborgards/CycloneDX.MSBuild/commit/ed9b2bd978d6f439ce4fb19418f7dd61d0f5bd2b))
* prevent parallel builds of MultiTargetProject inner frameworks ([03598ed](https://github.com/dborgards/CycloneDX.MSBuild/commit/03598ed3b39dc82488f34f0571d43a72a0c8e387))
* prevent parallel builds of shared CycloneDX.MSBuild dependency ([74cadd1](https://github.com/dborgards/CycloneDX.MSBuild/commit/74cadd1b09b713acbcbb413a74c12f0ef357edaf))
* properly calculate CycloneDxOutputPath in targets instead of props ([e3f723b](https://github.com/dborgards/CycloneDX.MSBuild/commit/e3f723b4c43e1f7197eb77a1c62f4b1417f2ac02))
* remove Integration.Tests projects from solution to prevent parallel builds ([c83a316](https://github.com/dborgards/CycloneDX.MSBuild/commit/c83a3164e5abe526169c28753dab9e630f9136c6))
* remove invalid dotnet_diagnostic properties from MSBuild file ([d66e5e2](https://github.com/dborgards/CycloneDX.MSBuild/commit/d66e5e2265bb4301583eef4fa23ebece2caa05b9))
* remove invalid return statement from MSBuild inline task ([2cc6d4d](https://github.com/dborgards/CycloneDX.MSBuild/commit/2cc6d4dcc31dbb2a38a749873888955ffd36ad07))
* rename XUnit collection classes to avoid CA1711 warning ([c2d5d48](https://github.com/dborgards/CycloneDX.MSBuild/commit/c2d5d485ca58e6f8950c2938f0d5e07f264eb5d0))
* replace System.Text.Json with regex-based approach for MSBuild task compatibility ([04e50b0](https://github.com/dborgards/CycloneDX.MSBuild/commit/04e50b0319c3c85b51c0f31c404f1e79f319549a))
* serialize integration tests to prevent file locking ([0a392a9](https://github.com/dborgards/CycloneDX.MSBuild/commit/0a392a93510e38428318e4dd18e4e64970bed1a8))
* specify target framework for multi-target project publish test ([9e1d02f](https://github.com/dborgards/CycloneDX.MSBuild/commit/9e1d02fe6e3554a104d6cce4849bee85aa0cb437))
* suppress code analysis warnings for test projects ([92dd8b0](https://github.com/dborgards/CycloneDX.MSBuild/commit/92dd8b0d4368b1b137730d0d90ca1a4dbd82aef9))

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
