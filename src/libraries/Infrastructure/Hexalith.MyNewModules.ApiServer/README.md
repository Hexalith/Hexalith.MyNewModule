# Hexalith.MyNewModules.ApiServer

This project provides the REST API server infrastructure for the MyNewModule bounded context.

## Overview

The API Server module handles:
- REST API endpoints via controllers
- Dapr actor registration for event sourcing
- Integration event processing
- Service composition for the API layer

## Directory Structure

```
Hexalith.MyNewModules.ApiServer/
├── Controllers/
│   ├── MyNewModuleEventsBusTopicAttribute.cs
│   └── MyNewModuleIntegrationEventsController.cs
├── Modules/
│   └── HexalithMyNewModulesApiServerModule.cs
├── Properties/
│   └── launchSettings.json
└── Hexalith.MyNewModules.ApiServer.csproj
```

## Module Definition

### HexalithMyNewModulesApiServerModule

The main module class implementing `IApiServerApplicationModule`:

```csharp
public sealed class HexalithMyNewModulesApiServerModule : 
    IApiServerApplicationModule, IMyNewModuleModule
{
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies 
        => MyNewModuleModulePolicies.AuthorizationPolicies;
    
    public IEnumerable<string> Dependencies => [];
    public string Description => "Hexalith MyNewModule API Server module";
    public string Id => "Hexalith.MyNewModules.ApiServer";
    public string Name => "Hexalith MyNewModule API Server";
    public int OrderWeight => 0;
    public string Version => "1.0";
}
```

### Service Registration

```csharp
public static void AddServices(IServiceCollection services, IConfiguration configuration)
{
    // Configure settings
    services.ConfigureSettings<CosmosDbSettings>(configuration);

    // Register serialization mappers
    HexalithMyNewModulesEventsSerialization.RegisterPolymorphicMappers();
    HexalithMyNewModulesCommandsSerialization.RegisterPolymorphicMappers();
    HexalithMyNewModulesRequestsSerialization.RegisterPolymorphicMappers();

    // Add application module
    services.TryAddSingleton<IMyNewModuleModule, HexalithMyNewModulesApiServerModule>();

    // Add command handlers
    services.AddMyNewModule();

    // Add projection handlers and actor factories
    services.AddMyNewModuleProjectionActorFactories();

    // Add controllers
    services.AddControllers()
        .AddApplicationPart(typeof(MyNewModuleIntegrationEventsController).Assembly);
}
```

### Actor Registration

```csharp
public static void RegisterActors(object actorCollection)
{
    var actorRegistrations = (ActorRegistrationCollection)actorCollection;
    
    // Domain aggregate actor
    actorRegistrations.RegisterActor<DomainAggregateActor>(
        MyNewModuleDomainHelper.MyNewModuleAggregateName.ToAggregateActorName());
    
    // Projection actors
    actorRegistrations.RegisterProjectionActor<MyNewModule>();
    actorRegistrations.RegisterProjectionActor<MyNewModuleSummaryViewModel>();
    actorRegistrations.RegisterProjectionActor<MyNewModuleDetailsViewModel>();
    
    // ID collection actor
    actorRegistrations.RegisterActor<SequentialStringListActor>(
        IIdCollectionFactory.GetAggregateCollectionName(
            MyNewModuleDomainHelper.MyNewModuleAggregateName));
}
```

## Controllers

### MyNewModuleIntegrationEventsController

Handles integration events from the message bus:

```csharp
[ApiController]
[Route("api/v1/[controller]")]
public class MyNewModuleIntegrationEventsController : ControllerBase
{
    [HttpPost]
    [MyNewModuleEventsBusTopic]
    public async Task<IActionResult> HandleIntegrationEvent(
        [FromBody] CloudEvent cloudEvent,
        CancellationToken cancellationToken)
    {
        // Process integration event
    }
}
```

### MyNewModuleEventsBusTopicAttribute

Custom attribute for Dapr pub/sub subscription:

```csharp
[AttributeUsage(AttributeTargets.Method)]
public class MyNewModuleEventsBusTopicAttribute : TopicAttribute
{
    public MyNewModuleEventsBusTopicAttribute() 
        : base("pubsub", "mynewmodule-events")
    {
    }
}
```

## Dapr Integration

### Actors

| Actor Type | Purpose | Naming |
|------------|---------|--------|
| `DomainAggregateActor` | Event sourcing for aggregates | `{AggregateName}Aggregate` |
| `ProjectionActor<T>` | Read model maintenance | `{TypeName}Projection` |
| `SequentialStringListActor` | ID collection management | `{AggregateName}Collection` |

### State Stores

- Event store for domain events
- Projection store for read models

### Pub/Sub

- Topic: `mynewmodule-events`
- Used for integration events

## Configuration

### appsettings.json

```json
{
  "CosmosDb": {
    "ConnectionString": "your-connection-string",
    "DatabaseName": "MyNewModules"
  },
  "Dapr": {
    "AppId": "mynewmodules-api",
    "HttpPort": 3500,
    "GrpcPort": 50001
  }
}
```

## Dependencies

- `Hexalith.Infrastructure.DaprRuntime` - Dapr integration
- `Hexalith.Infrastructure.CosmosDb.Configurations` - CosmosDB settings
- `Hexalith.UI.ApiServer` - API server framework
- `Hexalith.MyNewModules` - Application layer

## API Endpoints

The module exposes endpoints under `/api/v1/mynewmodule/`:

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/v1/commands` | Submit commands |
| POST | `/api/v1/requests` | Submit queries |
| POST | `/api/v1/integration-events` | Handle integration events |

## Security

API endpoints are secured using:
- JWT authentication
- Role-based authorization policies
- HTTPS enforcement

## Best Practices

1. **Use Dapr abstractions** - Don't couple directly to infrastructure
2. **Register all actors** - Ensure actors are properly registered
3. **Handle errors gracefully** - Return appropriate HTTP status codes
4. **Log requests** - Use structured logging
5. **Validate input** - Use model validation
