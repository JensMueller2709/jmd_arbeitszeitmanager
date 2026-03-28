# JMD Arbeitszeitmanager .NET 10.0 Upgrade Tasks

## Overview

This document tracks the execution of the JMD Arbeitszeitmanager solution upgrade from .NET Core 3.1 / .NET Standard 2.0 to .NET 10.0. All 3 projects will be upgraded simultaneously in a single atomic operation, followed by testing and validation.

**Progress**: 0/4 tasks complete (0%) ![0%](https://progress-bar.xyz/0)

---

## Tasks

### [▶] TASK-001: Verify prerequisites
**References**: Plan §Phase 0

- [▶] (1) Verify .NET 10.0 SDK installed per Plan §Prerequisites
- [ ] (2) .NET 10.0 SDK meets minimum requirements (**Verify**)
- [ ] (3) Check for global.json constraints (if file exists)
- [ ] (4) No version constraints blocking .NET 10.0 (**Verify**)

---

### [ ] TASK-002: Atomic framework and dependency upgrade
**References**: Plan §Phase 1, Plan §Project-by-Project Plans, Plan §Package Update Reference

- [ ] (1) Update target framework in JMD_Arbeitszeitmanager\JMD_Arbeitszeitmanager.csproj to net10.0-windows
- [ ] (2) Update target framework in JMD_ArbeitszeitmanagerTests\JMD_ArbeitszeitmanagerTests.csproj to net10.0
- [ ] (3) JMD_Arbeitszeitmanager.Core remains netstandard2.0 (no change) (**Verify**)
- [ ] (4) Update Newtonsoft.Json to 13.0.4 in JMD_Arbeitszeitmanager.Core\JMD_Arbeitszeitmanager.Core.csproj
- [ ] (5) Update Microsoft.Extensions.Hosting to 10.0.5 in JMD_Arbeitszeitmanager\JMD_Arbeitszeitmanager.csproj
- [ ] (6) All target frameworks and package versions updated (**Verify**)
- [ ] (7) Restore all dependencies
- [ ] (8) All dependencies restored successfully (**Verify**)
- [ ] (9) Build solution and fix all compilation errors per Plan §Breaking Changes Catalog
- [ ] (10) Solution builds with 0 errors (**Verify**)

---

### [ ] TASK-003: Run full test suite and validate upgrade
**References**: Plan §Phase 2, Plan §Testing & Validation Strategy

- [ ] (1) Run tests in JMD_ArbeitszeitmanagerTests project
- [ ] (2) Fix any test failures (reference Plan §Breaking Changes Catalog for framework and package changes)
- [ ] (3) Re-run tests after fixes
- [ ] (4) All tests pass with 0 failures (**Verify**)

---

### [ ] TASK-004: Final commit
**References**: Plan §Source Control Strategy

- [ ] (1) Commit all changes with message: "Upgrade to .NET 10.0 - Updated all projects to .NET 10.0, upgraded Newtonsoft.Json to 13.0.4 (security fix), upgraded Microsoft.Extensions.Hosting to 10.0.5, all tests passing"

---
