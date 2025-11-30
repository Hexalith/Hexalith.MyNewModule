# Hexalith.MyNewModules

This is the main application project that orchestrates the MyNewModule domain logic, including command handling, event processing, and service integration.

## Overview

This project serves as the composition root for the application layer, bringing together:
- Command handlers
- Event handlers
- Aggregate providers
- Service registrations

## Directory Structure

```
Hexalith.MyNewModules/
├── CommandHandlers/
│   └── MyNewModuleCommandHandlerHelper.cs
├── EventHandlers/
│   └── MyNewModuleEventHandlerHelper.cs
├── Helpers/
│   ├── MyNewModuleHelper.cs
│   └── MyNewModuleModulePolicies.cs
└── Hexalith.MyNewModules.csproj
```

## Key Components

### MyNewModuleHelper

Main entry point for service registration:

```csharp
public static class MyNewModuleHelper
{
    public static IServiceCollection AddMyNewModule(this IServiceCollection services)
    {
        _ = services.AddMyNewModuleCommandHandlers();
        _ = services.AddMyNewModuleAggregateProviders();
        _ = services.AddMyNewModuleEventValidators();
        return services;
    }
}
```

### MyNewModuleCommandHandlerHelper

Registers command handlers:

```csharp
public static class MyNewModuleCommandHandlerHelper
{
    public static IServiceCollection AddMyNewModuleCommandHandlers(
        this IServiceCollection services)
    {
        // Register command handlers for each command type
        services.AddScoped<ICommandHandler<AddMyNewModule>, AddMyNewModuleHandler>();
        services.AddScoped<ICommandHandler<ChangeMyNewModuleDescription>, 
            ChangeMyNewModuleDescriptionHandler>();
        // ... more handlers
        return services;
    }
}
```

### MyNewModuleEventHandlerHelper

Registers event validators:

```csharp
public static class MyNewModuleEventHandlerHelper
{
    public static IServiceCollection AddMyNewModuleEventValidators(
        this IServiceCollection services)
    {
        // Register validators for domain events
        return services;
    }
}
```

### MyNewModuleModulePolicies

Defines authorization policies for the module:

```csharp
public static class MyNewModuleModulePolicies
{
    public static IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => 
        new Dictionary<string, AuthorizationPolicy>
        {
            [MyNewModulePolicies.Owner] = new AuthorizationPolicyBuilder()
                .RequireRole(MyNewModuleRoles.Owner)
                .Build(),
            [MyNewModulePolicies.Contributor] = new AuthorizationPolicyBuilder()
                .RequireRole(MyNewModuleRoles.Owner, MyNewModuleRoles.Contributor)
                .Build(),
            [MyNewModulePolicies.Reader] = new AuthorizationPolicyBuilder()
                .RequireRole(MyNewModuleRoles.Owner, MyNewModuleRoles.Contributor, 
                    MyNewModuleRoles.Reader)
                .Build()
        };
}
```

## Dependencies

This project references all other MyNewModules projects:

```xml
<ItemGroup>
  <ProjectReference Include="..\Hexalith.MyNewModules.Abstractions" />
  <ProjectReference Include="..\Hexalith.MyNewModules.Commands" />
  <ProjectReference Include="..\Hexalith.MyNewModules.Projections" />
  <ProjectReference Include="..\Hexalith.MyNewModules.Requests" />
  <ProjectReference Include="..\..\Domain\Hexalith.MyNewModules.Aggregates" />
  <ProjectReference Include="..\..\Domain\Hexalith.MyNewModules.Events" />
</ItemGroup>
```

External packages:
- `Hexalith.Application` - Application framework
- `Hexalith.Domains.Abstractions` - Domain abstractions
- `Hexalith.PolymorphicSerializations` - Serialization support
- `Microsoft.AspNetCore.Authorization` - Authorization

## Usage

### In API Server

```csharp
public static void AddServices(IServiceCollection services, IConfiguration configuration)
{
    // Register serialization mappers
    HexalithMyNewModulesEventsSerialization.RegisterPolymorphicMappers();
    HexalithMyNewModulesCommandsSerialization.RegisterPolymorphicMappers();
    HexalithMyNewModulesRequestsSerialization.RegisterPolymorphicMappers();

    // Add module services
    services.AddMyNewModule();
    
    // Add projection handlers
    services.AddMyNewModuleProjectionActorFactories();
}
```

### In Web App

```csharp
public static void AddServices(IServiceCollection services)
{
    // Register serialization mappers
    HexalithMyNewModulesEventsSerialization.RegisterPolymorphicMappers();
    HexalithMyNewModulesCommandsSerialization.RegisterPolymorphicMappers();
    HexalithMyNewModulesRequestsSerialization.RegisterPolymorphicMappers();

    // Add query services
    services.AddMyNewModuleQueryServices();
}
```

## Authorization

The module implements role-based access control:

| Role | Permissions |
|------|-------------|
| Owner | Full access (CRUD + Admin) |
| Contributor | Create, Read, Update |
| Reader | Read only |

## Best Practices

1. **Register all services** - Ensure all handlers are properly registered
2. **Use dependency injection** - Follow DI patterns for testability
3. **Validate early** - Use validators in command handlers
4. **Handle exceptions** - Implement proper error handling
5. **Log operations** - Use logging for debugging and auditing
