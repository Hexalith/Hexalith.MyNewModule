# Hexalith.MyNewModules.Projections

This project contains the projection handlers and request handlers for maintaining read models in the MyNewModule bounded context.

## Overview

Projections are responsible for:
1. Processing domain events to update read models
2. Maintaining denormalized views optimized for queries
3. Handling request queries against the read models

## Architecture

```
┌───────────────┐    ┌────────────────────┐    ┌──────────────┐
│ Domain Events │───▶│ Projection Handler │───▶│  Read Model  │
│               │    │                    │    │  (Database)  │
└───────────────┘    └────────────────────┘    └──────────────┘
                                                      │
                                                      ▼
┌───────────────┐    ┌────────────────────┐    ┌──────────────┐
│   Requests    │───▶│  Request Handler   │◀──│  View Model  │
│   (Queries)   │    │                    │    │   (Result)   │
└───────────────┘    └────────────────────┘    └──────────────┘
```

## Projection Handlers

### Directory Structure

```
ProjectionHandlers/
├── Details/
│   ├── MyNewModuleAddedOnDetailsProjectionHandler.cs
│   ├── MyNewModuleDescriptionChangedOnDetailsProjectionHandler.cs
│   ├── MyNewModuleDisabledOnDetailsProjectionHandler.cs
│   └── MyNewModuleEnabledOnDetailsProjectionHandler.cs
│
└── Summaries/
    ├── MyNewModuleAddedOnSummaryProjectionHandler.cs
    ├── MyNewModuleDescriptionChangedOnSummaryProjectionHandler.cs
    ├── MyNewModuleDisabledOnSummaryProjectionHandler.cs
    ├── MyNewModuleEnabledOnSummaryProjectionHandler.cs
    └── MyNewModuleSnapshotOnSummaryProjectionHandler.cs
```

### Registration

```csharp
public static IServiceCollection AddMyNewModuleProjectionHandlers(this IServiceCollection services)
    => services
        // Collection projections
        .AddScoped<IProjectionUpdateHandler<MyNewModuleAdded>, 
            IdsCollectionProjectionHandler<MyNewModuleAdded>>()
        
        // Summary projections
        .AddScoped<IProjectionUpdateHandler<MyNewModuleAdded>, 
            MyNewModuleAddedOnSummaryProjectionHandler>()
        .AddScoped<IProjectionUpdateHandler<MyNewModuleDescriptionChanged>, 
            MyNewModuleDescriptionChangedOnSummaryProjectionHandler>()
        .AddScoped<IProjectionUpdateHandler<MyNewModuleDisabled>, 
            MyNewModuleDisabledOnSummaryProjectionHandler>()
        .AddScoped<IProjectionUpdateHandler<MyNewModuleEnabled>, 
            MyNewModuleEnabledOnSummaryProjectionHandler>()
        .AddScoped<IProjectionUpdateHandler<SnapshotEvent>, 
            MyNewModuleSnapshotOnSummaryProjectionHandler>()
        
        // Details projections
        .AddScoped<IProjectionUpdateHandler<MyNewModuleAdded>, 
            MyNewModuleAddedOnDetailsProjectionHandler>()
        // ... more handlers
```

### Handler Example

```csharp
public class MyNewModuleAddedOnSummaryProjectionHandler : 
    IProjectionUpdateHandler<MyNewModuleAdded>
{
    public async Task<IEnumerable<object>> HandleAsync(
        MyNewModuleAdded @event, 
        CancellationToken cancellationToken)
    {
        var summary = new MyNewModuleSummaryViewModel(
            @event.Id,
            @event.Name,
            false);
            
        // Return projection updates
        return [new AddProjection<MyNewModuleSummaryViewModel>(summary)];
    }
}
```

## Request Handlers

### Available Handlers

| Handler | Request | Description |
|---------|---------|-------------|
| `GetMyNewModuleDetailsHandler` | `GetMyNewModuleDetails` | Retrieves full module details |
| `GetFilteredCollectionHandler` | `GetMyNewModuleSummaries` | Retrieves paginated summaries |
| `GetAggregateIdsRequestHandler` | `GetMyNewModuleIds` | Retrieves all module IDs |
| `GetExportsRequestHandler` | `GetMyNewModuleExports` | Exports full aggregate data |

### Request Handler Example

```csharp
public class GetMyNewModuleDetailsHandler : IRequestHandler<GetMyNewModuleDetails>
{
    private readonly IProjectionStore<MyNewModuleDetailsViewModel> _store;

    public GetMyNewModuleDetailsHandler(
        IProjectionStore<MyNewModuleDetailsViewModel> store)
    {
        _store = store;
    }

    public async Task<RequestResult<MyNewModuleDetailsViewModel>> HandleAsync(
        GetMyNewModuleDetails request, 
        CancellationToken cancellationToken)
    {
        var details = await _store.GetAsync(request.Id, cancellationToken);
        return details is null 
            ? RequestResult<MyNewModuleDetailsViewModel>.NotFound(request.Id)
            : RequestResult<MyNewModuleDetailsViewModel>.Success(details);
    }
}
```

## Helper Classes

### MyNewModuleProjectionHelper

Provides extension methods for service registration:

```csharp
public static class MyNewModuleProjectionHelper
{
    // Register aggregate providers
    public static IServiceCollection AddMyNewModuleAggregateProviders(
        this IServiceCollection services);
    
    // Register projection handlers
    public static IServiceCollection AddMyNewModuleProjectionHandlers(
        this IServiceCollection services);
    
    // Register projections and request handlers
    public static IServiceCollection AddMyNewModuleProjections(
        this IServiceCollection services);
    
    // Register query services
    public static IServiceCollection AddMyNewModuleQueryServices(
        this IServiceCollection services);
    
    // Register request handlers
    public static IServiceCollection AddMyNewModuleRequestHandlers(
        this IServiceCollection services);
}
```

## Services

### MyNewModulesQuickStartData

Provides sample data for development and demos:

```csharp
public static class MyNewModulesQuickStartData
{
    public static IEnumerable<object> Data => [
        new AddMyNewModule("sample-001", "Sample Module 1", "First sample"),
        new AddMyNewModule("sample-002", "Sample Module 2", "Second sample")
    ];
}
```

## Dependencies

- `Hexalith.Application.Projections` - Projection abstractions
- `Hexalith.Application.Requests` - Request handling
- `Hexalith.MyNewModules.Events` - Domain events
- `Hexalith.MyNewModules.Requests` - Request/view model definitions

## Dapr Actor Integration

Projections are executed via Dapr actors for:
- Reliable event processing
- State persistence
- Scalability

Actor registration:

```csharp
actorRegistrations.RegisterProjectionActor<MyNewModule>();
actorRegistrations.RegisterProjectionActor<MyNewModuleSummaryViewModel>();
actorRegistrations.RegisterProjectionActor<MyNewModuleDetailsViewModel>();
```

## Best Practices

1. **Keep projections simple** - One projection per read model per event
2. **Handle all relevant events** - Ensure read models stay consistent
3. **Support snapshot events** - For rebuilding projections
4. **Use async/await** - For I/O operations
5. **Log projection failures** - For debugging and monitoring
