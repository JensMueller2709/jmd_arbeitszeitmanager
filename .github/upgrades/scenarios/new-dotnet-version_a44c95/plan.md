# .NET 10.0 Upgrade Plan

## Table of Contents

1. [Executive Summary](#executive-summary)
2. [Migration Strategy](#migration-strategy)
3. [Detailed Dependency Analysis](#detailed-dependency-analysis)
4. [Project-by-Project Plans](#project-by-project-plans)
   - [JMD_Arbeitszeitmanager.Core](#jmd_arbeitszeitmanagercore)
   - [JMD_Arbeitszeitmanager](#jmd_arbeitszeitmanager)
   - [JMD_ArbeitszeitmanagerTests](#jmd_arbeitszeitmanagertests)
5. [Package Update Reference](#package-update-reference)
6. [Breaking Changes Catalog](#breaking-changes-catalog)
7. [Risk Management](#risk-management)
8. [Testing & Validation Strategy](#testing--validation-strategy)
9. [Complexity & Effort Assessment](#complexity--effort-assessment)
10. [Source Control Strategy](#source-control-strategy)
11. [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Description

Upgrade JMD Arbeitszeitmanager solution from .NET Core 3.1 / .NET Standard 2.0 to .NET 10.0 (Long Term Support).

### Scope

**3 Projects Affected:**
- **JMD_Arbeitszeitmanager.Core** (netstandard2.0 → remains netstandard2.0)
- **JMD_Arbeitszeitmanager** (netcoreapp3.1 → net10.0-windows)
- **JMD_ArbeitszeitmanagerTests** (netcoreapp3.1 → net10.0)

**Current State:**
- Total Lines of Code: 3,618
- All projects are SDK-style
- Clear dependency chain with no circular dependencies
- 2 NuGet packages require updates
- 1 security vulnerability identified (Newtonsoft.Json)

**Target State:**
- All projects targeting .NET 10.0 or compatible framework
- All packages updated to secure, compatible versions
- Zero security vulnerabilities
- Solution builds and all tests pass

### Selected Strategy

**All-At-Once Strategy** - All projects upgraded simultaneously in single coordinated operation.

**Rationale:**
- Small solution (3 projects)
- All currently on modern .NET (netcoreapp3.1 / netstandard2.0)
- Simple dependency structure (depth: 3)
- Low complexity across all projects
- Only 2 package updates required
- Clean SDK-style projects
- Good test coverage (dedicated test project)

### Complexity Assessment

**Discovered Metrics:**
- Projects: 3
- Dependency Depth: 3 levels
- Total LOC: 3,618
- Package Updates: 2
- Security Issues: 1 (critical - Newtonsoft.Json)
- API Compatibility: 100% compatible (1,023/1,023 APIs)
- Breaking Changes: 0 identified

**Classification: Simple Solution**

All projects rated as Low difficulty with clear upgrade paths and no blocking issues identified.

### Critical Issues

**🔴 Security Vulnerability:**
- **Newtonsoft.Json 12.0.3** in JMD_Arbeitszeitmanager.Core contains security vulnerabilities
- **Resolution:** Upgrade to 13.0.4 immediately as part of this migration

**Package Updates Required:**
- Microsoft.Extensions.Hosting: 5.0.0 → 10.0.5
- Newtonsoft.Json: 12.0.3 → 13.0.4

### Recommended Approach

Execute all project upgrades, package updates, and compilation fixes as a single atomic operation, followed by comprehensive testing. This minimizes complexity and delivers fastest time to completion.

### Expected Iterations

- **Phase 1:** Discovery & Classification (Complete)
- **Phase 2:** Foundation sections (3 iterations)
- **Phase 3:** Project details (2 batch iterations)
- **Total:** ~6 iterations

---

## Migration Strategy

### Approach Selection

**Selected: All-At-Once Strategy**

All projects will be upgraded simultaneously in a single coordinated operation.

### Justification

**Why All-At-Once is ideal for this solution:**

1. **Small Scale**
   - Only 3 projects
   - Total 3,618 LOC
   - Manageable complexity

2. **Modern Foundation**
   - All projects already on modern .NET (netcoreapp3.1 / netstandard2.0)
   - All SDK-style projects
   - No legacy .NET Framework migration needed

3. **Simple Dependencies**
   - Linear dependency chain
   - No circular dependencies
   - Clear upgrade path

4. **Low Package Complexity**
   - Only 2 package updates required
   - Both have clear upgrade paths
   - No deprecated packages requiring replacement

5. **Good Test Coverage**
   - Dedicated test project exists
   - Can validate entire solution post-upgrade

6. **Minimal Risk**
   - All projects rated "Low" difficulty
   - 100% API compatibility (1,023/1,023 APIs compatible)
   - No breaking changes identified in assessment

### All-At-Once Strategy Rationale

The All-At-Once approach provides:
- **Fastest completion** - Single upgrade cycle
- **No intermediate states** - Solution always in consistent state
- **Simplified coordination** - All changes happen together
- **Reduced overhead** - No multi-targeting complexity
- **Atomic validation** - Test entire solution in one pass

### Dependency-Based Ordering Rationale

While all projects upgrade simultaneously, understanding the dependency order matters for:
- **Troubleshooting** - Errors in foundation projects must be fixed first
- **Testing strategy** - Test from bottom-up (Core → App → Tests)
- **Risk mitigation** - Foundation changes impact all dependants

**Logical order for issue resolution:**
1. JMD_Arbeitszeitmanager.Core (foundation)
2. JMD_Arbeitszeitmanager (application)
3. JMD_ArbeitszeitmanagerTests (tests)

### Execution Approach

**Single Atomic Operation:**

All project files and package references will be updated together, followed by:
1. Dependency restoration
2. Solution build
3. Compilation error fixes (if any)
4. Full test suite execution

**No Intermediate States:** Unlike incremental migration, there are no phases where some projects are on old frameworks while others are upgraded. This eliminates multi-targeting complexity.

### Phase Definitions

Although this is an atomic upgrade, we organize work into logical phases for clarity:

**Phase 0: Preparation (if required)**
- Verify .NET 10.0 SDK installed
- Check for global.json constraints

**Phase 1: Atomic Upgrade**
- Update all TargetFramework properties
- Update all PackageReference versions
- Restore dependencies
- Build solution and fix compilation errors
- Rebuild and verify

**Phase 2: Test Validation**
- Execute all test projects
- Address test failures
- Verify application functionality

**Phase 3: Final Verification**
- Confirm zero build warnings
- Validate no security vulnerabilities remain
- Performance smoke test

---

## Detailed Dependency Analysis

### Dependency Graph Summary

The solution has a clean, linear dependency structure:

```
JMD_ArbeitszeitmanagerTests (netcoreapp3.1)
    └─→ JMD_Arbeitszeitmanager (netcoreapp3.1)
            └─→ JMD_Arbeitszeitmanager.Core (netstandard2.0)
```

**Characteristics:**
- **No circular dependencies** - Clean upgrade path
- **Maximum depth: 3 levels** - Simple structure
- **Single dependency chain** - No complex cross-dependencies
- **All SDK-style projects** - Modern project format

### Project Groupings by Migration Phase

Since this is an **All-At-Once Strategy**, all projects will be upgraded simultaneously. However, understanding the dependency order is important for troubleshooting:

**Foundation Layer (No Dependencies):**
- JMD_Arbeitszeitmanager.Core
  - Current: netstandard2.0
  - Target: netstandard2.0 (remains unchanged - already compatible)
  - Type: Class Library
  - LOC: 202
  - Dependencies: 0

**Application Layer (Depends on Foundation):**
- JMD_Arbeitszeitmanager
  - Current: netcoreapp3.1
  - Target: net10.0-windows
  - Type: WPF Application
  - LOC: 3,227
  - Dependencies: 1 (Core)

**Test Layer (Depends on Application):**
- JMD_ArbeitszeitmanagerTests
  - Current: netcoreapp3.1
  - Target: net10.0
  - Type: Test Project
  - LOC: 189
  - Dependencies: 1 (Main Application)

### Critical Path Identification

The **critical path** for this upgrade is:

1. **Core Library** - Must remain compatible with all dependants
2. **Main Application** - Core business logic and UI
3. **Test Project** - Validates entire solution

**Key Consideration:** Since JMD_Arbeitszeitmanager.Core remains on netstandard2.0, it maintains broad compatibility. This simplifies the upgrade as we don't need to worry about cross-targeting issues.

### Circular Dependency Analysis

**Status:** ✅ No circular dependencies detected

This clean structure allows for straightforward atomic upgrade without multi-targeting complexity.

---

## Project-by-Project Plans

### JMD_Arbeitszeitmanager.Core

**Current State:**
- Target Framework: netstandard2.0
- SDK-style: True
- Project Type: ClassLibrary
- Dependencies: 0
- Dependants: 1 (JMD_Arbeitszeitmanager)
- Files: 7
- LOC: 202
- Risk Level: Low

**Target State:**
- Target Framework: netstandard2.0 (unchanged - already compatible)
- Packages: Newtonsoft.Json upgraded to 13.0.4

#### Migration Steps

**1. Prerequisites**
- No framework change required (netstandard2.0 is compatible with .NET 10.0)
- Ensure project builds successfully before package updates

**2. Technology/Framework Update**
- **Action:** None required
- **Rationale:** netstandard2.0 is compatible with .NET 10.0 and maintains broad compatibility

**3. Package/Module/Dependency Updates**

| Package Name | Current Version | Target Version | Reason |
|--------------|----------------|----------------|---------|
| Newtonsoft.Json | 12.0.3 | 13.0.4 | **🔴 CRITICAL: Security vulnerabilities** |
| NETStandard.Library | 2.0.3 | 2.0.3 | ✅ No update required (compatible) |

**Update Details:**

**File:** `JMD_Arbeitszeitmanager.Core\JMD_Arbeitszeitmanager.Core.csproj`

**Change:**
```xml
<PackageReference Include="Newtonsoft.Json" Version="13.0.4" />
```

**4. Expected Breaking Changes**

**Newtonsoft.Json 12.0.3 → 13.0.4:**
- Generally backward compatible for most scenarios
- Possible changes:
  - Stricter validation of JSON input
  - Performance improvements may alter timing-sensitive code
  - Bug fixes may change behavior that was previously incorrect

**Areas to Review:**
- Custom JsonConverter implementations
- JsonSerializerSettings configurations
- Deserialization of complex types
- Date/time handling
- Null value handling

**5. Code Modifications**

**Expected changes:** None anticipated based on assessment (100% API compatibility)

**Areas requiring review if compilation errors occur:**
- Custom JSON serialization logic
- Type converters
- Serialization attributes

**6. Testing Strategy**

**Unit Tests:**
- Verify JSON serialization/deserialization for all model classes
- Test custom converters (if any)
- Validate null handling behavior

**Integration Tests:**
- Ensure data persistence works correctly
- Verify API contracts remain stable

**Manual Validation:**
- Review any configuration files using JSON
- Test edge cases for complex type serialization

**7. Validation Checklist**

- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] No security vulnerabilities remain (verify with security audit)
- [ ] JSON serialization tests pass
- [ ] Dependent projects (JMD_Arbeitszeitmanager) still build correctly
- [ ] No performance degradation in serialization operations

---

### JMD_Arbeitszeitmanager

**Current State:**
- Target Framework: netcoreapp3.1
- SDK-style: True
- Project Type: WPF Application
- Dependencies: 1 (Core)
- Dependants: 1 (Tests)
- Files: 45
- LOC: 3,227
- Risk Level: Low

**Target State:**
- Target Framework: net10.0-windows
- Packages: Microsoft.Extensions.Hosting upgraded to 10.0.5

#### Migration Steps

**1. Prerequisites**
- JMD_Arbeitszeitmanager.Core must build successfully (dependency)
- Verify .NET 10.0 SDK installed

**2. Technology/Framework Update**

**File:** `JMD_Arbeitszeitmanager\JMD_Arbeitszeitmanager.csproj`

**Change TargetFramework:**
```xml
<TargetFramework>net10.0-windows</TargetFramework>
```

**Note:** The `-windows` suffix is required for WPF applications to access Windows-specific APIs.

**3. Package/Module/Dependency Updates**

| Package Name | Current Version | Target Version | Reason |
|--------------|----------------|----------------|---------|
| Microsoft.Extensions.Hosting | 5.0.0 | 10.0.5 | Framework compatibility and improvements |
| MahApps.Metro | 2.4.3 | 2.4.3 | ✅ No update required (compatible) |
| MySql.Data | 8.0.23 | 8.0.23 | ✅ No update required (compatible) |

**Update Details:**

**File:** `JMD_Arbeitszeitmanager\JMD_Arbeitszeitmanager.csproj`

**Change:**
```xml
<PackageReference Include="Microsoft.Extensions.Hosting" Version="10.0.5" />
```

**4. Expected Breaking Changes**

**Framework: netcoreapp3.1 → net10.0-windows**

**WPF-Specific Changes:**
- WPF rendering engine improvements (generally backward compatible)
- Default theme/styling behaviors (minimal changes expected)
- Window management APIs (stable across versions)

**Microsoft.Extensions.Hosting 5.0.0 → 10.0.5:**
- Host builder patterns remain stable
- Service registration APIs unchanged
- Configuration system enhancements (backward compatible)
- Logging APIs stable

**Potential Areas of Impact:**
- Generic host configuration in `Program.cs` or `App.xaml.cs`
- Dependency injection container registration
- Application lifecycle event handlers
- Logging configuration

**5. Code Modifications**

**Expected changes:** None anticipated based on assessment (100% API compatibility)

**Areas requiring review if compilation errors occur:**

**Program.cs / App.xaml.cs:**
- Host builder initialization
- Service registration patterns
- Configuration loading

**Example of modern hosting pattern (if updating):**
```csharp
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<MainWindow>();
// ... service registrations
var host = builder.Build();
```

**Dependency Injection:**
- Verify service lifetimes (Singleton, Scoped, Transient)
- Check constructor injection in ViewModels/Services

**Configuration:**
- Verify appsettings.json loading
- Check environment-specific configuration

**6. Testing Strategy**

**Build Validation:**
- Clean build after framework update
- Resolve any compilation errors

**Functional Tests:**
- Application starts successfully
- Main window renders correctly
- All WPF controls function as expected
- Data binding works correctly

**Integration Tests:**
- Dependency injection resolves services correctly
- Database connection works (MySql.Data)
- Configuration values load correctly
- Logging writes to expected targets

**UI/Visual Tests:**
- MahApps.Metro theming applies correctly
- All dialogs and windows display properly
- No visual regressions

**Manual Validation:**
- Test critical user workflows
- Verify all menu items and buttons function
- Check for any performance changes

**7. Validation Checklist**

- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] Application starts successfully
- [ ] Main UI renders correctly
- [ ] All WPF controls functional
- [ ] MahApps.Metro theme applies correctly
- [ ] Database connectivity works (MySQL)
- [ ] Dependency injection resolves services
- [ ] Configuration loads correctly
- [ ] Logging functions properly
- [ ] No performance degradation
- [ ] All critical workflows tested

---

### JMD_ArbeitszeitmanagerTests

**Current State:**
- Target Framework: netcoreapp3.1
- SDK-style: True
- Project Type: Test Project (DotNetCoreApp)
- Dependencies: 1 (Main Application)
- Dependants: 0
- Files: 6
- LOC: 189
- Risk Level: Low

**Target State:**
- Target Framework: net10.0
- Packages: No updates required (all compatible)

#### Migration Steps

**1. Prerequisites**
- JMD_Arbeitszeitmanager must build successfully (dependency)
- JMD_Arbeitszeitmanager.Core must build successfully (transitive dependency)

**2. Technology/Framework Update**

**File:** `JMD_ArbeitszeitmanagerTests\JMD_ArbeitszeitmanagerTests.csproj`

**Change TargetFramework:**
```xml
<TargetFramework>net10.0</TargetFramework>
```

**Note:** Test projects use `net10.0` (not `net10.0-windows`) as they don't require Windows-specific APIs directly.

**3. Package/Module/Dependency Updates**

| Package Name | Current Version | Target Version | Reason |
|--------------|----------------|----------------|---------|
| Microsoft.NET.Test.Sdk | 16.5.0 | 16.5.0 | ✅ Compatible (consider updating to latest for better tooling) |
| MSTest.TestAdapter | 2.1.0 | 2.1.0 | ✅ Compatible |
| MSTest.TestFramework | 2.1.0 | 2.1.0 | ✅ Compatible |
| coverlet.collector | 1.2.0 | 1.2.0 | ✅ Compatible |

**No package updates required** - all test packages are compatible with .NET 10.0.

**Optional Enhancement (not required for this migration):**
- Consider updating Microsoft.NET.Test.Sdk to latest version for improved test discovery and execution

**4. Expected Breaking Changes**

**Framework: netcoreapp3.1 → net10.0**

**MSTest Framework:**
- No breaking changes expected
- Test discovery mechanism stable
- Assert APIs unchanged
- TestContext functionality preserved

**Test Execution:**
- Test adapter remains compatible
- Code coverage collection continues working
- Test result reporting unchanged

**5. Code Modifications**

**Expected changes:** None anticipated

**Areas requiring review if test failures occur:**
- Tests depending on .NET 3.1-specific behavior
- Tests with timing assumptions (performance improvements may affect timing)
- Tests mocking framework-specific types

**6. Testing Strategy**

**Test Execution:**
- Run full test suite after framework update
- Verify all tests discovered correctly
- Check test execution time (should improve on .NET 10.0)

**Test Categories to Validate:**
- Unit tests
- Integration tests (if any)
- Data access tests
- UI tests (if any)

**Code Coverage:**
- Verify code coverage collection still works
- Compare coverage metrics pre/post upgrade

**7. Validation Checklist**

- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] All tests discovered by test runner
- [ ] All existing tests pass
- [ ] No skipped tests (unless intentional)
- [ ] Code coverage collection works
- [ ] Test execution completes successfully
- [ ] Test results report correctly
- [ ] No test performance degradation

---

## Package Update Reference

### Overview

Total packages in solution: 9  
Packages requiring updates: 2  
Packages compatible as-is: 7

### Common Package Updates (Affecting Multiple Projects)

No packages are shared across multiple projects in this solution.

### Project-Specific Package Updates

#### JMD_Arbeitszeitmanager.Core

| Package | Current | Target | Update Reason | Priority |
|---------|---------|--------|---------------|----------|
| Newtonsoft.Json | 12.0.3 | 13.0.4 | **🔴 CRITICAL: Security vulnerabilities** | Immediate |
| NETStandard.Library | 2.0.3 | 2.0.3 | ✅ Compatible - no update needed | N/A |

#### JMD_Arbeitszeitmanager

| Package | Current | Target | Update Reason | Priority |
|---------|---------|--------|---------------|----------|
| Microsoft.Extensions.Hosting | 5.0.0 | 10.0.5 | Framework alignment and improvements | High |
| MahApps.Metro | 2.4.3 | 2.4.3 | ✅ Compatible - no update needed | N/A |
| MySql.Data | 8.0.23 | 8.0.23 | ✅ Compatible - no update needed | N/A |

#### JMD_ArbeitszeitmanagerTests

| Package | Current | Target | Update Reason | Priority |
|---------|---------|--------|---------------|----------|
| Microsoft.NET.Test.Sdk | 16.5.0 | 16.5.0 | ✅ Compatible - no update needed | N/A |
| MSTest.TestAdapter | 2.1.0 | 2.1.0 | ✅ Compatible - no update needed | N/A |
| MSTest.TestFramework | 2.1.0 | 2.1.0 | ✅ Compatible - no update needed | N/A |
| coverlet.collector | 1.2.0 | 1.2.0 | ✅ Compatible - no update needed | N/A |

### Update Sequence

Since this is an **All-At-Once Strategy**, all package updates occur simultaneously. However, if troubleshooting is needed, address packages in this priority order:

1. **Critical Security Fixes** (Newtonsoft.Json) - Must be resolved first
2. **Framework-Related Updates** (Microsoft.Extensions.Hosting) - Core functionality
3. **All Other Packages** - No issues expected

### Package Update Commands

**Option 1: Manual .csproj Edits (Recommended for this migration)**
Edit the .csproj files directly to update `Version` attributes.

**Option 2: dotnet CLI**
```bash
# From solution root
cd JMD_Arbeitszeitmanager.Core
dotnet add package Newtonsoft.Json --version 13.0.4

cd ..\JMD_Arbeitszeitmanager
dotnet add package Microsoft.Extensions.Hosting --version 10.0.5
```

**Option 3: NuGet Package Manager (Visual Studio)**
- Right-click project → Manage NuGet Packages
- Update tab → Select packages → Choose specific versions

### Verification After Updates

After all package updates:
```bash
dotnet restore
dotnet build
```

Expected outcome: Clean build with no errors or warnings.

---

## Breaking Changes Catalog

### Framework Breaking Changes

#### .NET Core 3.1 → .NET 10.0

**Good News:** The assessment shows **100% API compatibility** (1,023/1,023 APIs analyzed).

**General Areas to Monitor:**

1. **Runtime Behavior Changes**
   - Garbage collection improvements (may affect timing)
   - JIT compilation optimizations (performance characteristics)
   - Globalization behavior refinements

2. **ASP.NET Core Changes** (Not applicable - this is a WPF app)

3. **WPF Changes**
   - Rendering engine improvements (generally backward compatible)
   - High DPI handling enhancements
   - Touch and stylus input improvements

**Expected Impact: Minimal to None**

The large version jump (3.1 → 10.0) sounds risky, but Microsoft maintains strong backward compatibility for WPF applications. Most breaking changes affect ASP.NET Core, not desktop applications.

### Package Breaking Changes

#### Newtonsoft.Json 12.0.3 → 13.0.4

**Scope:** Generally backward compatible

**Potential Changes:**
- **Stricter JSON validation** - Malformed JSON may now throw where it previously succeeded
- **Improved null handling** - Edge cases with null values may behave differently
- **Performance optimizations** - Faster serialization may expose timing-dependent bugs
- **Bug fixes** - Previous incorrect behavior now fixed

**Review Required:**
- Custom `JsonConverter` implementations
- `JsonSerializerSettings` configurations
- Deserialization of complex/nested types
- Date/time serialization formats

**Mitigation:**
- Run comprehensive tests of JSON serialization/deserialization
- Review any custom converters against v13 documentation
- Test edge cases (nulls, empty arrays, special characters)

#### Microsoft.Extensions.Hosting 5.0.0 → 10.0.5

**Scope:** Backward compatible with minor API additions

**Potential Changes:**
- **New overloads** - Additional configuration methods available
- **Improved logging** - Better structured logging support
- **Enhanced configuration** - New configuration sources and patterns
- **Performance improvements** - Faster startup and lower memory usage

**Review Required:**
- Host builder initialization patterns
- Service registration order (if order-dependent)
- Logging configuration
- Application lifetime event handlers

**Mitigation:**
- Verify application starts successfully
- Test dependency injection resolution
- Check logging output format/destination
- Validate configuration loading

### Code Patterns to Review

Based on the project types and packages, review these patterns if compilation or runtime issues occur:

#### 1. JSON Serialization Patterns
```csharp
// Review custom converters
public class CustomConverter : JsonConverter { ... }

// Review serializer settings
var settings = new JsonSerializerSettings {
    NullValueHandling = NullValueHandling.Ignore,
    // ... other settings
};
```

#### 2. Host Builder Patterns
```csharp
// Review host initialization
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) => {
        // Service registrations
    })
    .Build();
```

#### 3. Dependency Injection
```csharp
// Review service lifetimes
services.AddSingleton<IService, ServiceImpl>();
services.AddScoped<IScopedService, ScopedServiceImpl>();
services.AddTransient<ITransientService, TransientServiceImpl>();
```

#### 4. WPF Application Lifecycle
```csharp
// Review App.xaml.cs patterns
protected override void OnStartup(StartupEventArgs e) {
    base.OnStartup(e);
    // Initialization logic
}
```

### Breaking Changes by Severity

| Severity | Count | Description |
|----------|-------|-------------|
| 🔴 Critical (Binary Incompatible) | 0 | APIs removed or signatures changed |
| 🟡 Warning (Source Incompatible) | 0 | Recompilation may reveal conflicts |
| 🔵 Behavioral | 0 | Runtime behavior changes |
| ✅ Compatible | 1,023 | APIs work as before |

**Conclusion:** This upgrade has exceptionally low breaking change risk. No breaking changes were identified in the assessment.

---

## Risk Management

### High-Risk Changes

| Project | Risk Level | Description | Mitigation |
|---------|-----------|-------------|------------|
| JMD_Arbeitszeitmanager.Core | 🟡 Medium | Security vulnerability in Newtonsoft.Json 12.0.3 | Upgrade to 13.0.4 immediately; verify JSON serialization behavior post-upgrade |
| JMD_Arbeitszeitmanager | 🟢 Low | Major version jump (3.1 → 10.0) for WPF app | WPF APIs stable across versions; verify UI rendering and hosting configuration |
| JMD_ArbeitszeitmanagerTests | 🟢 Low | Test framework compatibility | MSTest fully compatible; verify test discovery |

### Security Vulnerabilities

**🔴 CRITICAL - Immediate Action Required:**

| Package | Current Version | CVE/Issue | Remediation | Projects Affected |
|---------|----------------|-----------|-------------|-------------------|
| Newtonsoft.Json | 12.0.3 | Security vulnerabilities (specific CVEs not detailed in assessment) | Upgrade to 13.0.4 | JMD_Arbeitszeitmanager.Core |

**Impact:** This vulnerability exists in the Core library, which is referenced by all other projects. Upgrading addresses the security issue for the entire solution.

**Verification:** After upgrade, run security audit tools to confirm no vulnerabilities remain.

### Contingency Plans

**Blocking Issue: Build Failures After Framework Update**
- **Alternative:** Roll back TargetFramework changes for affected project
- **Investigation:** Check specific error messages against Microsoft breaking changes documentation
- **Resolution:** Apply targeted fixes based on breaking change guidance

**Blocking Issue: Newtonsoft.Json 13.0.4 Breaking Changes**
- **Alternative:** Review JSON serialization settings and custom converters
- **Investigation:** Check Newtonsoft.Json release notes for 12.0 → 13.0 breaking changes
- **Resolution:** Update serialization code if necessary; consider migration to System.Text.Json (long-term)

**Blocking Issue: WPF Rendering Issues on .NET 10.0**
- **Alternative:** Test on multiple machines to rule out environment-specific issues
- **Investigation:** Check for deprecated WPF APIs or changed default behaviors
- **Resolution:** Update XAML or code-behind based on WPF migration guidance

**Performance Degradation**
- **Detection:** Run performance smoke tests comparing .NET 3.1 vs .NET 10.0
- **Investigation:** Profile hot paths; check for removed performance optimizations
- **Resolution:** Apply .NET 10.0 performance best practices

### Risk Mitigation Strategies

1. **Incremental Testing**
   - Build after each logical change group
   - Don't accumulate multiple failures
   - Fix compilation errors before proceeding to tests

2. **Package Update Validation**
   - Verify package compatibility before updating
   - Check release notes for breaking changes
   - Test JSON serialization specifically for Newtonsoft.Json

3. **Backup & Rollback**
   - Work on dedicated branch (upgrade-to-NET10-1)
   - Commit before major changes
   - Can revert to last known-good state

4. **Test-Driven Validation**
   - Run existing tests immediately after upgrade
   - Tests act as regression detection
   - Fix failures before declaring success

### All-At-Once Strategy Risk Factors

**Concentrated Risk:**
- All projects change simultaneously
- Single build failure can block entire solution
- Requires fixing all issues before solution compiles

**Mitigation:**
- Assessment shows 100% API compatibility
- No breaking changes identified
- Small solution size limits blast radius
- Good test coverage enables quick validation

---

## Testing & Validation Strategy

### Multi-Level Testing Approach

Testing follows the All-At-Once strategy with comprehensive validation after the atomic upgrade.

### Phase 0: Pre-Upgrade Baseline

**Before making any changes:**

1. **Establish Baseline**
   - Document current test pass rate
   - Capture build output (warnings, errors)
   - Record application startup time
   - Note any known issues

2. **Baseline Test Execution**
   ```bash
   dotnet test JMD_ArbeitszeitmanagerTests\JMD_ArbeitszeitmanagerTests.csproj
   ```
   - Document: X tests passed, Y tests failed, Z tests skipped
   - Save test results for comparison

### Phase 1: Per-Project Validation (During Atomic Upgrade)

#### After Framework Updates + Package Updates + Build

**Build Validation:**
```bash
dotnet restore
dotnet build --configuration Release
```

**Success Criteria:**
- [ ] Solution restores without errors
- [ ] All 3 projects build successfully
- [ ] Zero build errors
- [ ] Zero build warnings (or document expected warnings)

**If Build Fails:**
- Fix errors in dependency order: Core → App → Tests
- Do not proceed to testing until clean build achieved

#### Immediate Smoke Tests

**JMD_Arbeitszeitmanager.Core:**
- [ ] Project builds independently
- [ ] No missing dependencies
- [ ] NuGet package Newtonsoft.Json 13.0.4 restored correctly

**JMD_Arbeitszeitmanager:**
- [ ] Project builds independently
- [ ] References to Core project resolve correctly
- [ ] NuGet package Microsoft.Extensions.Hosting 10.0.5 restored correctly
- [ ] Application starts (does not crash on startup)

**JMD_ArbeitszeitmanagerTests:**
- [ ] Project builds independently
- [ ] Test discovery succeeds
- [ ] Test runner recognizes all tests

### Phase 2: Comprehensive Testing

#### Unit Test Execution

**Run full test suite:**
```bash
dotnet test JMD_ArbeitszeitmanagerTests\JMD_ArbeitszeitmanagerTests.csproj --logger "console;verbosity=detailed"
```

**Success Criteria:**
- [ ] All tests discovered (compare to baseline)
- [ ] Pass rate ≥ baseline pass rate
- [ ] No new test failures
- [ ] Test execution completes without crashes
- [ ] Code coverage ≥ baseline (if measured)

**If Tests Fail:**
- Analyze failure patterns
- Check for Newtonsoft.Json serialization issues
- Verify dependency injection resolution
- Review host builder initialization
- Compare error messages to breaking changes catalog

#### Integration Testing

**Database Connectivity (MySQL):**
- [ ] Connection string resolves correctly
- [ ] Database queries execute successfully
- [ ] Data retrieval works as expected
- [ ] Transactions complete successfully

**JSON Serialization:**
- [ ] Model serialization produces correct JSON
- [ ] Deserialization reconstructs objects correctly
- [ ] Null handling behaves as expected
- [ ] Date/time formats preserved

**Dependency Injection:**
- [ ] All services resolve from container
- [ ] Service lifetimes correct (Singleton, Scoped, Transient)
- [ ] No circular dependency errors
- [ ] Hosted services start correctly

#### Application Testing (Manual)

**Startup Validation:**
- [ ] Application launches without errors
- [ ] Main window displays correctly
- [ ] MahApps.Metro theme applies properly
- [ ] No console errors or exceptions

**Core Functionality:**
- [ ] All menu items accessible
- [ ] All buttons and controls functional
- [ ] Data loads from database
- [ ] User interactions respond correctly

**UI/Visual Validation:**
- [ ] Layout renders correctly
- [ ] Fonts and colors as expected
- [ ] Icons and images display
- [ ] No visual regressions
- [ ] High DPI scaling works (if applicable)

**Critical User Workflows:**
- [ ] [Specific workflow 1 - define based on application]
- [ ] [Specific workflow 2 - define based on application]
- [ ] [Specific workflow 3 - define based on application]

### Phase 3: Security & Quality Validation

#### Security Audit

**Vulnerability Scan:**
```bash
dotnet list package --vulnerable --include-transitive
```

**Success Criteria:**
- [ ] Zero vulnerabilities reported
- [ ] Newtonsoft.Json 13.0.4 shows no vulnerabilities
- [ ] No new vulnerable dependencies introduced

#### Performance Validation

**Startup Performance:**
- [ ] Application startup time ≤ baseline
- [ ] Memory usage at startup ≤ baseline + 10%

**Runtime Performance:**
- [ ] UI responsiveness maintained
- [ ] Database query performance stable
- [ ] JSON serialization performance ≥ baseline (should improve)

**Benchmark (Optional):**
```csharp
// Run performance-critical operations and compare timings
```

#### Build Quality Checks

**Warnings Review:**
```bash
dotnet build --configuration Release /warnaserror
```
- [ ] Address any new warnings
- [ ] No obsolete API usage warnings
- [ ] No nullable reference warnings (if enabled)

**Code Analysis (Optional):**
```bash
dotnet build /p:EnableNETAnalyzers=true /p:AnalysisLevel=latest
```

### Testing Checklist Summary

#### Critical (Must Pass)
- [ ] Solution builds without errors
- [ ] All unit tests pass
- [ ] Application starts successfully
- [ ] No security vulnerabilities
- [ ] Core functionality works

#### Important (Should Pass)
- [ ] Zero build warnings
- [ ] All integration tests pass
- [ ] UI renders correctly
- [ ] Performance ≥ baseline

#### Nice to Have
- [ ] Code analysis clean
- [ ] Performance improvements observed
- [ ] Updated test tooling utilized

### Rollback Criteria

**If any of these occur, consider rollback:**
- Critical tests fail with no clear resolution path
- Application crashes on startup consistently
- Data corruption or loss observed
- Security vulnerabilities cannot be resolved
- Performance degradation >25%

**Rollback Process:**
```bash
git checkout upgrade-dotNet10  # Return to source branch
```

### Test Execution Timeline

**Estimated testing duration:**
- Build validation: Immediate
- Unit tests: Minutes (automated)
- Integration tests: 15-30 minutes
- Manual application testing: 30-60 minutes
- Security audit: 5 minutes
- Performance validation: 15-30 minutes

**Total testing time: 1-2 hours**

---

## Complexity & Effort Assessment

### Per-Project Complexity

| Project | Complexity | Framework Change | Package Updates | Risk Factors | Dependencies |
|---------|-----------|------------------|-----------------|--------------|--------------|
| JMD_Arbeitszeitmanager.Core | 🟢 Low | None (stays netstandard2.0) | 1 (Newtonsoft.Json) | Security vulnerability | 0 |
| JMD_Arbeitszeitmanager | 🟡 Medium | netcoreapp3.1 → net10.0-windows | 1 (Extensions.Hosting) | Major version jump, WPF | 1 |
| JMD_ArbeitszeitmanagerTests | 🟢 Low | netcoreapp3.1 → net10.0 | 0 | None | 1 |

### Phase Complexity Assessment

**Phase 0: Preparation**
- Complexity: 🟢 Low
- Prerequisites: Verify .NET 10.0 SDK installed

**Phase 1: Atomic Upgrade**
- Complexity: 🟡 Medium
- Scope: All projects + all packages simultaneously
- Expected challenges:
  - WPF configuration changes (if any)
  - Hosting service registration updates (if any)
  - JSON serialization behavior validation
- Dependency ordering: Core → App → Tests (for troubleshooting)

**Phase 2: Test Validation**
- Complexity: 🟢 Low
- Scope: Run existing test suite
- Expected challenges: Minimal (MSTest fully compatible)

**Phase 3: Final Verification**
- Complexity: 🟢 Low
- Scope: Build validation, security audit, smoke test

### Resource Requirements

**Technical Skills Required:**
- .NET 10.0 knowledge (APIs and behavior changes)
- WPF application development experience
- NuGet package management
- MSTest testing framework
- Git/source control proficiency

**Parallel Work Capacity:**
Since this is an All-At-Once strategy with atomic upgrade, parallelization is limited:
- **Single developer sufficient** - Changes are coordinated
- **No parallel tracks** - All projects upgrade together
- **Sequential troubleshooting** - Fix errors in dependency order

### Relative Complexity Ratings

**Overall Solution Complexity: 🟢 Low-Medium**

**Factors reducing complexity:**
- Small solution (3 projects, 3,618 LOC)
- Modern starting point (netcoreapp3.1)
- All SDK-style projects
- No legacy .NET Framework
- Clean dependency structure
- Good test coverage
- Only 2 package updates
- 100% API compatibility

**Factors increasing complexity:**
- Major version jump (3.1 → 10.0)
- Security vulnerability requiring immediate fix
- WPF application (UI validation needed)
- Hosting services configuration may need updates

**Net Assessment:** This is a straightforward upgrade with low overall risk. The major version jump is mitigated by excellent API compatibility and modern starting point.

---

## Source Control Strategy

### Branching Strategy

**Source Branch:** `upgrade-dotNet10`  
**Upgrade Branch:** `upgrade-to-NET10-1`  
**Target Branch:** `upgrade-dotNet10` (merge back after validation)

**Branch Structure:**
```
upgrade-dotNet10 (source)
    └─→ upgrade-to-NET10-1 (work branch)
            └─→ upgrade-dotNet10 (merge after completion)
```

### All-At-Once Commit Strategy

Since this is an All-At-Once upgrade, prefer a **single consolidated commit** for the atomic upgrade phase:

**Option 1: Single Commit (Recommended)**
```
✅ Upgrade to .NET 10.0

- Updated all projects to .NET 10.0
- JMD_Arbeitszeitmanager.Core: Upgraded Newtonsoft.Json 12.0.3 → 13.0.4 (security fix)
- JMD_Arbeitszeitmanager: Updated to net10.0-windows, upgraded Microsoft.Extensions.Hosting 5.0.0 → 10.0.5
- JMD_ArbeitszeitmanagerTests: Updated to net10.0
- All tests passing
- Zero security vulnerabilities
```

**Rationale:** 
- All changes are interdependent
- Single atomic upgrade is easier to review
- Clean revert point if issues arise
- Simplifies PR review process

**Option 2: Phased Commits (Alternative)**
If you prefer more granular commits:

```
1. "Update project target frameworks to .NET 10.0"
2. "Update package versions (Newtonsoft.Json, Microsoft.Extensions.Hosting)"
3. "Fix compilation errors (if any)"
4. "Verify all tests passing"
```

### Commit Message Format

**Use conventional commit format:**

```
type(scope): subject

body

footer
```

**Example:**
```
feat(upgrade): Upgrade solution to .NET 10.0

- All projects migrated from .NET Core 3.1 to .NET 10.0
- Updated Newtonsoft.Json to 13.0.4 (addresses security vulnerabilities)
- Updated Microsoft.Extensions.Hosting to 10.0.5
- All 3 projects building successfully
- All tests passing

BREAKING CHANGE: Requires .NET 10.0 SDK to build
Closes #[issue-number]
```

### Checkpoints & Commits

**Mandatory Checkpoints:**

1. **Before Starting** (Baseline)
   - Already on `upgrade-to-NET10-1` branch
   - Clean working directory

2. **After Atomic Upgrade** (if compilation succeeds)
   ```bash
   git add .
   git commit -m "feat(upgrade): Upgrade to .NET 10.0"
   ```

3. **After Test Validation** (if tests pass)
   - Update commit or add tag:
   ```bash
   git tag -a v10.0-validated -m "Validated .NET 10.0 upgrade"
   ```

4. **After Final Review** (ready to merge)
   - Push to remote:
   ```bash
   git push origin upgrade-to-NET10-1
   ```

### Pull Request Strategy

**PR Title:**
```
Upgrade solution to .NET 10.0 (LTS)
```

**PR Description Template:**
```markdown
## Summary
Upgrades JMD Arbeitszeitmanager solution from .NET Core 3.1 to .NET 10.0 (LTS).

## Changes
- **JMD_Arbeitszeitmanager.Core**: Upgraded Newtonsoft.Json 12.0.3 → 13.0.4 (🔴 security fix)
- **JMD_Arbeitszeitmanager**: Updated to net10.0-windows, upgraded Microsoft.Extensions.Hosting 5.0.0 → 10.0.5
- **JMD_ArbeitszeitmanagerTests**: Updated to net10.0

## Migration Strategy
All-At-Once Strategy - atomic upgrade of all projects simultaneously.

## Testing
- ✅ All projects build without errors
- ✅ All unit tests pass (X/X passing)
- ✅ Application starts successfully
- ✅ Core functionality validated
- ✅ Security vulnerabilities resolved
- ✅ No performance degradation

## Breaking Changes
None identified. 100% API compatibility maintained.

## Rollback Plan
Revert this PR if issues arise. Clean revert point available.

## Checklist
- [ ] Code review completed
- [ ] All tests passing
- [ ] No build warnings
- [ ] Security audit clean
- [ ] Documentation updated (if needed)
```

### Review & Merge Process

**PR Requirements:**
- [ ] All automated tests pass
- [ ] Build succeeds on CI/CD (if configured)
- [ ] Code review approved by [reviewer]
- [ ] No merge conflicts
- [ ] Branch up to date with source

**Merge Method:**
- **Recommended:** Squash and merge (creates single commit in history)
- **Alternative:** Merge commit (preserves all commits)

**Post-Merge:**
```bash
git checkout upgrade-dotNet10
git pull origin upgrade-dotNet10
git branch -d upgrade-to-NET10-1  # Clean up local branch
```

### Rollback Strategy

**If issues discovered after merge:**

**Option 1: Revert Commit**
```bash
git revert [commit-hash]
git push origin upgrade-dotNet10
```

**Option 2: Branch Rollback**
```bash
git checkout upgrade-dotNet10
git reset --hard [pre-upgrade-commit-hash]
git push --force origin upgrade-dotNet10  # Use with caution
```

**Option 3: New Branch from Previous State**
```bash
git checkout [pre-upgrade-commit-hash]
git checkout -b hotfix/revert-dotnet10-upgrade
# Continue work from stable state
```

### Continuous Integration Considerations

**If CI/CD pipeline exists:**
- Ensure .NET 10.0 SDK available on build agents
- Update pipeline configuration if needed
- Verify all automated tests run successfully
- Check deployment scripts for framework version references

**Pipeline validation checklist:**
- [ ] Build agent has .NET 10.0 SDK
- [ ] Restore succeeds
- [ ] Build succeeds
- [ ] Tests execute
- [ ] Artifacts generated correctly

---

## Success Criteria

### Technical Criteria

#### Build Success
- [ ] **All projects build without errors**
  - JMD_Arbeitszeitmanager.Core compiles successfully
  - JMD_Arbeitszeitmanager compiles successfully
  - JMD_ArbeitszeitmanagerTests compiles successfully

- [ ] **Zero build warnings** (or all warnings documented and justified)

- [ ] **Dependency restoration succeeds**
  ```bash
  dotnet restore
  # Expected: Success, no errors
  ```

#### Framework Migration
- [ ] **All projects target correct frameworks**
  - JMD_Arbeitszeitmanager.Core: netstandard2.0 ✅ (unchanged)
  - JMD_Arbeitszeitmanager: net10.0-windows ✅
  - JMD_ArbeitszeitmanagerTests: net10.0 ✅

- [ ] **No framework compatibility issues**

#### Package Updates
- [ ] **All required packages updated**
  - Newtonsoft.Json: 13.0.4 ✅
  - Microsoft.Extensions.Hosting: 10.0.5 ✅

- [ ] **All packages restore correctly**

- [ ] **No package dependency conflicts**

#### Security
- [ ] **Zero security vulnerabilities**
  ```bash
  dotnet list package --vulnerable --include-transitive
  # Expected: No vulnerable packages found
  ```

- [ ] **Newtonsoft.Json security issue resolved**
  - Version 13.0.4 or higher installed
  - No vulnerabilities in transitive dependencies

#### Testing
- [ ] **All unit tests pass**
  ```bash
  dotnet test
  # Expected: 100% pass rate (or equal to baseline)
  ```

- [ ] **Test discovery succeeds**
  - All tests found by test runner
  - No missing or skipped tests (unless intentional)

- [ ] **Integration tests pass** (if applicable)

#### Application Functionality
- [ ] **Application starts successfully**
  - No startup crashes
  - No initialization errors
  - Main window displays

- [ ] **Core functionality validated**
  - User can interact with application
  - Database connectivity works (MySQL)
  - Data loads and displays correctly
  - User workflows complete successfully

- [ ] **UI renders correctly**
  - MahApps.Metro theme applies
  - All controls visible and functional
  - No visual regressions
  - Layout preserved

### Quality Criteria

#### Code Quality
- [ ] **No code quality degradation**
  - No new compiler warnings
  - No new code analysis issues
  - Maintainability preserved

- [ ] **API usage up-to-date**
  - No obsolete API warnings
  - Using current best practices
  - No deprecated patterns

#### Test Coverage
- [ ] **Test coverage maintained or improved**
  - Code coverage ≥ baseline percentage
  - Critical paths covered
  - New code tested (if any added)

#### Performance
- [ ] **No performance degradation**
  - Application startup time ≤ baseline
  - UI responsiveness maintained
  - Database query performance stable
  - Memory usage ≤ baseline + 10%

- [ ] **Performance improvements observed** (expected on .NET 10.0)
  - Faster JIT compilation
  - Improved garbage collection
  - Better throughput

#### Documentation
- [ ] **Upgrade documented**
  - Migration notes captured
  - Breaking changes documented (if any)
  - Known issues noted

- [ ] **Dependencies documented**
  - Package versions recorded
  - Target frameworks documented
  - Prerequisites listed

### Process Criteria

#### Migration Strategy
- [ ] **All-At-Once strategy followed**
  - All projects upgraded simultaneously
  - Single atomic operation completed
  - No intermediate multi-targeting states

- [ ] **Dependency order respected**
  - Issues resolved in correct order (Core → App → Tests)
  - No dependency conflicts

#### Source Control
- [ ] **Work on correct branch**
  - Changes made on `upgrade-to-NET10-1`
  - Clean commit history
  - Meaningful commit messages

- [ ] **Changes committed appropriately**
  - Logical commit boundaries
  - Atomic commits where possible
  - Clear commit descriptions

- [ ] **Ready for merge**
  - Branch up-to-date with source
  - No merge conflicts
  - PR created and reviewed

#### Validation
- [ ] **Multi-level testing completed**
  - Build validation ✅
  - Unit tests ✅
  - Integration tests ✅
  - Manual application testing ✅
  - Security audit ✅
  - Performance validation ✅

- [ ] **All checklists completed**
  - Per-project validation checklists complete
  - Testing checklist complete
  - Source control checklist complete

### Acceptance Criteria Summary

**The upgrade is considered successful when:**

1. ✅ **All 3 projects target .NET 10.0** (or compatible framework)
2. ✅ **All 2 required package updates applied**
3. ✅ **Solution builds with zero errors and zero warnings**
4. ✅ **All tests pass** (100% pass rate or equal to baseline)
5. ✅ **Zero security vulnerabilities remain**
6. ✅ **Application runs correctly** (starts, functions, no crashes)
7. ✅ **No performance degradation**
8. ✅ **Changes committed and ready for review**

### Definition of Done

**This migration is DONE when:**

- All technical criteria met ✅
- All quality criteria met ✅
- All process criteria met ✅
- PR approved and merged ✅
- Solution deployed/usable on .NET 10.0 ✅

**Final Verification Command:**
```bash
# From solution root
dotnet restore
dotnet build --configuration Release
dotnet test --configuration Release
dotnet list package --vulnerable --include-transitive

# Expected output:
# - Restore: Success
# - Build: Success, 0 Error(s), 0 Warning(s)
# - Test: All tests passed
# - Vulnerabilities: No vulnerable packages
```

---

## Appendix

### Useful Commands Reference

**Build & Test:**
```bash
dotnet restore
dotnet build
dotnet build --configuration Release
dotnet test
dotnet test --logger "console;verbosity=detailed"
dotnet clean
```

**Package Management:**
```bash
dotnet list package
dotnet list package --outdated
dotnet list package --vulnerable
dotnet list package --vulnerable --include-transitive
dotnet add package [PackageName] --version [Version]
```

**Project Information:**
```bash
dotnet --info
dotnet --list-sdks
dotnet --list-runtimes
```

**Performance:**
```bash
dotnet build --configuration Release /p:UseSharedCompilation=false
dotnet run --configuration Release
```

### Additional Resources

- [.NET 10.0 Release Notes](https://github.com/dotnet/core/blob/main/release-notes/10.0/README.md)
- [Breaking Changes in .NET 10.0](https://docs.microsoft.com/en-us/dotnet/core/compatibility/10.0)
- [Newtonsoft.Json Documentation](https://www.newtonsoft.com/json/help/html/Introduction.htm)
- [Microsoft.Extensions.Hosting Documentation](https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host)
- [WPF .NET 10.0 Migration Guide](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/migration/)

### Contact & Support

For issues or questions during migration:
- Review this plan document
- Check assessment.md for detailed analysis
- Consult Microsoft documentation
- Review package-specific release notes
