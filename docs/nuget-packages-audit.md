# NuGet packages audit

**Date:** 2026-04-21  
**Scope:** Entire repository (`Directory.Packages.props` + all `*.csproj` files).

## Method

The solution uses [central package management](https://learn.microsoft.com/nuget/consume-packages/central-package-management) (`ManagePackageVersionsCentrally` in `Directory.Packages.props`).

A package is treated as **not used** when it appears as `<PackageVersion>` in `Directory.Packages.props` but **no** project contains `<PackageReference Include="…">` for that package id.

Packages referenced only by other packages (transitive dependencies) are not listed in `Directory.Packages.props` unless pinned there; those are out of scope for this list.

## Packages referenced by at least one project

| Package id | Project(s) |
|------------|------------|
| `coverlet.collector` | `Musicality.Application.Tests` |
| `Meziantou.Xunit.v3.ParallelTestFramework` | `Musicality.Application.Tests` |
| `Microsoft.AspNetCore.OpenApi` | `Musicality` |
| `Microsoft.Extensions.Http.Polly` | `Musicality.Pipelines` |
| `Microsoft.NET.Test.Sdk` | `Musicality.Application.Tests` |
| `MiniExcel` | `Musicality.Infrastructure` |
| `Polly` | `Musicality.Pipelines` |
| `Polly.Contrib.WaitAndRetry` | `Musicality.Pipelines` |
| `Telegram.Bot` | `Musicality.Pipelines` |
| `xunit.runner.visualstudio` | `Musicality.Application.Tests` |
| `xunit.v3` | `Musicality.Application.Tests` |
| `YoutubeExplode` | `Musicality.Common` |
| `YoutubeExplode.Converter` | `Musicality.Common` |

**Projects with no direct `PackageReference`:** `Musicality.Application`, `Musicality.Domain`, `Musicality.Persistence` (dependencies come from project references and the shared framework only).

## Packages in `Directory.Packages.props` with no project reference (unused)

These version pins are not consumed by any `.csproj` in the repository. They can be removed from `Directory.Packages.props` unless you plan to add them to a project soon, or you rely on this file as a template for future work.

1. `Asp.Versioning.Mvc`
2. `Asp.Versioning.Mvc.ApiExplorer`
3. `AspNetCore.HealthChecks.CosmosDb`
4. `AspNetCore.HealthChecks.UI.Client`
5. `AspNetCore.HealthChecks.UI.Core`
6. `Azure.Identity`
7. `Bogus`
8. `FluentAssertions`
9. `Microsoft.ApplicationInsights`
10. `Microsoft.ApplicationInsights.AspNetCore`
11. `Microsoft.AspNetCore.Authentication.JwtBearer`
12. `Microsoft.AspNetCore.Authentication.OpenIdConnect`
13. `Microsoft.AspNetCore.Mvc.NewtonsoftJson`
14. `Microsoft.AspNetCore.Mvc.Testing`
15. `Microsoft.Azure.Cosmos`
16. `Microsoft.EntityFrameworkCore`
17. `Microsoft.EntityFrameworkCore.Cosmos`
18. `Microsoft.Extensions.Caching.Abstractions`
19. `Microsoft.Extensions.Caching.Memory`
20. `Microsoft.Extensions.Caching.StackExchangeRedis`
21. `Microsoft.Extensions.Configuration`
22. `Microsoft.Extensions.Configuration.Abstractions`
23. `Microsoft.Extensions.Configuration.Binder`
24. `Microsoft.Extensions.DependencyInjection`
25. `Microsoft.Extensions.DependencyInjection.Abstractions`
26. `Microsoft.Extensions.Hosting.Abstractions`
27. `Microsoft.Extensions.Http.Resilience`
28. `Microsoft.Extensions.Localization`
29. `Microsoft.Extensions.Localization.Abstractions`
30. `Microsoft.Extensions.Options`
31. `Microsoft.Extensions.Options.ConfigurationExtensions`
32. `Microsoft.Identity.Web`
33. `Microsoft.IdentityModel.Tokens`
34. `Microsoft.VisualStudio.Azure.Containers.Tools.Targets`
35. `Newtonsoft.Json`
36. `NSubstitute`
37. `Quartz`
38. `Quartz.Extensions.DependencyInjection`
39. `Quartz.Extensions.Hosting`
40. `ScottPlot`
41. `Scrutor`
42. `Serilog`
43. `Serilog.AspNetCore`
44. `Serilog.Sinks.ApplicationInsights`
45. `Serilog.Sinks.Async`
46. `SkiaSharp.NativeAssets.Linux.NoDependencies`
47. `Swashbuckle.AspNetCore`
48. `System.IdentityModel.Tokens.Jwt`

**Count:** 48 unused central package entries.

## Note on `Musicality` and transitive packages

The `Musicality` web project references `Microsoft.AspNetCore.OpenApi` directly and uses `Polly`, `Polly.Contrib.WaitAndRetry`, and `Telegram.Bot` in `Program.cs` while those packages are declared on `Musicality.Pipelines`. The solution builds successfully; those types resolve via the project reference graph. For clearer ownership and tooling (e.g. analyzers, vulnerability reports per project), you may still choose to add explicit `PackageReference` entries on `Musicality` for packages its code uses directly.
