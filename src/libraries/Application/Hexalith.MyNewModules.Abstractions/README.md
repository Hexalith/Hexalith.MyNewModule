# Hexalith.MyNewModules.Abstractions

This project contains the interfaces, contracts, and shared definitions for the MyNewModule application layer.

## Overview

The abstractions project defines the contracts that other projects depend on, enabling:
- Loose coupling between components
- Easy mocking for unit tests
- Clear API boundaries

## Contents

### Module Information

**HexalithMyNewModulesInformation.cs**

Static class providing module identification:

```csharp
public static class HexalithMyNewModulesInformation
{
    public static string Id => "Hexalith.MyNewModule";
    public static string Name => "Hexalith MyNewModule";
    public static string ShortName => "MyNewModule";
}
```

### Module Interface

**IMyNewModuleModule.cs**

Interface marking a module as a MyNewModule module:

```csharp
public interface IMyNewModuleModule : IApplicationModule;
```

This interface is implemented by:
- `HexalithMyNewModulesApiServerModule`
- `HexalithMyNewModulesWebAppModule`
- `HexalithMyNewModulesWebServerModule`

### Service Interface

**IMyNewModuleService.cs**

Service contract for MyNewModule operations:

```csharp
public interface IMyNewModuleService
{
    Task DoSomethingAsync(CancellationToken cancellationToken);
}
```

Extend this interface as needed for your module's specific services.

### Security

**MyNewModuleRoles.cs**

Defines security roles:

```csharp
public static class MyNewModuleRoles
{
    public const string Owner = nameof(MyNewModules) + nameof(Owner);
    public const string Contributor = nameof(MyNewModules) + nameof(Contributor);
    public const string Reader = nameof(MyNewModules) + nameof(Reader);
}
```

| Role | Constant Value | Description |
|------|---------------|-------------|
| Owner | `MyNewModulesOwner` | Full administrative access |
| Contributor | `MyNewModulesContributor` | Create, read, update access |
| Reader | `MyNewModulesReader` | Read-only access |

**MyNewModulePolicies.cs**

Defines authorization policies:

```csharp
public static class MyNewModulePolicies
{
    public const string Owner = MyNewModuleRoles.Owner;
    public const string Contributor = MyNewModuleRoles.Contributor;
    public const string Reader = MyNewModuleRoles.Reader;
}
```

## Dependencies

This project has minimal dependencies:
- `Hexalith.Application.Modules.Abstractions` - Module interfaces

## Usage

### Referencing in Other Projects

```xml
<ItemGroup>
  <ProjectReference Include="..\Hexalith.MyNewModules.Abstractions" />
</ItemGroup>
```

### Using in Code

```csharp
using Hexalith.MyNewModules;

// Get module information
var moduleId = HexalithMyNewModulesInformation.Id;
var moduleName = HexalithMyNewModulesInformation.Name;

// Check user roles
if (user.IsInRole(MyNewModuleRoles.Owner))
{
    // Allow administrative operations
}

// Apply authorization policy
[Authorize(Policy = MyNewModulePolicies.Contributor)]
public class MyController : ControllerBase
{
    // Controller actions
}
```

### Implementing the Service Interface

```csharp
public class MyNewModuleService : IMyNewModuleService
{
    public async Task DoSomethingAsync(CancellationToken cancellationToken)
    {
        // Implementation
    }
}

// Register in DI
services.AddScoped<IMyNewModuleService, MyNewModuleService>();
```

## Design Guidelines

1. **Keep abstractions minimal** - Only include what's needed
2. **Avoid implementation details** - No concrete classes
3. **Use interfaces for services** - Enable dependency injection
4. **Constants for magic strings** - Centralize role/policy names
5. **No external dependencies** - Keep the project lightweight
