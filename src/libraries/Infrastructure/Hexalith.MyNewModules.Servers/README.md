# Hexalith.MyNewModules.Servers

This project provides shared server utilities and services for the MyNewModule bounded context.

## Overview

The Servers project contains:
- Shared helper classes for server projects
- Common server-side service implementations
- Actor factory registrations
- Server configuration utilities

## Directory Structure

```
Hexalith.MyNewModules.Servers/
├── Helpers/
│   └── MyNewModuleServerHelper.cs
├── Services/
│   └── (shared services)
└── Hexalith.MyNewModules.Servers.csproj
```

## Helper Classes

### MyNewModuleServerHelper

Provides extension methods for server-side service registration:

```csharp
public static class MyNewModuleServerHelper
{
    /// <summary>
    /// Adds MyNewModule projection actor factories to the service collection.
    /// </summary>
    public static IServiceCollection AddMyNewModuleProjectionActorFactories(
        this IServiceCollection services)
    {
        // Register actor factories for projections
        services.AddSingleton<IProjectionActorFactory<MyNewModule>, 
            ProjectionActorFactory<MyNewModule>>();
        services.AddSingleton<IProjectionActorFactory<MyNewModuleSummaryViewModel>, 
            ProjectionActorFactory<MyNewModuleSummaryViewModel>>();
        services.AddSingleton<IProjectionActorFactory<MyNewModuleDetailsViewModel>, 
            ProjectionActorFactory<MyNewModuleDetailsViewModel>>();
            
        return services;
    }
}
```

## Actor Factory Registration

Actor factories are registered for:

| Projection Type | Purpose |
|-----------------|---------|
| `MyNewModule` | Aggregate snapshots |
| `MyNewModuleSummaryViewModel` | List/grid displays |
| `MyNewModuleDetailsViewModel` | Detail views |

## Usage

### In API Server

```csharp
public static void AddServices(IServiceCollection services, IConfiguration configuration)
{
    // ... other registrations

    // Add projection actor factories
    services.AddMyNewModuleProjectionActorFactories();
}
```

### In Web Server

```csharp
public static void AddServices(IServiceCollection services, IConfiguration configuration)
{
    // ... other registrations

    // Add projection actor factories (if needed)
    services.AddMyNewModuleProjectionActorFactories();
}
```

## Dependencies

- `Hexalith.Infrastructure.DaprRuntime` - Dapr integration
- `Hexalith.MyNewModules.Projections` - Projection definitions
- `Hexalith.MyNewModules.Requests` - View model definitions

## Design Guidelines

This project should contain:
- ✅ Shared server utilities
- ✅ Actor registration helpers
- ✅ Common service implementations
- ❌ Business logic (belongs in Application layer)
- ❌ UI components (belongs in Presentation layer)
- ❌ Domain code (belongs in Domain layer)
