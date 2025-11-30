# Hexalith.MyNewModules.Requests

This project contains the request (query) definitions and view models for the MyNewModule bounded context.

## Overview

Requests represent queries for data retrieval in the CQRS pattern. They are handled separately from commands and typically read from optimized read models (projections).

## Request Types

### Query Requests

| Request | Description | Returns |
|---------|-------------|---------|
| `GetMyNewModuleDetails` | Get full details of a single module | `MyNewModuleDetailsViewModel` |
| `GetMyNewModuleSummaries` | Get paginated list of module summaries | `IEnumerable<MyNewModuleSummaryViewModel>` |
| `GetMyNewModuleIds` | Get list of all module IDs | `IEnumerable<string>` |
| `GetMyNewModuleExports` | Export full module data | `IEnumerable<MyNewModule>` |
| `GetMyNewModuleIdDescription` | Get ID/Description pairs for dropdowns | `IEnumerable<IdDescription>` |

## Request Definitions

### MyNewModuleRequest (Base)

```csharp
[PolymorphicSerialization]
public abstract partial record MyNewModuleRequest
{
    public static string AggregateName => MyNewModuleDomainHelper.MyNewModuleAggregateName;
}
```

### GetMyNewModuleDetails

```csharp
[PolymorphicSerialization]
public partial record GetMyNewModuleDetails(string Id) : MyNewModuleRequest;
```

### GetMyNewModuleSummaries

```csharp
[PolymorphicSerialization]
public partial record GetMyNewModuleSummaries() : MyNewModuleRequest;
```

## View Models

### MyNewModuleDetailsViewModel

Full details view model for detail pages:

```csharp
[DataContract]
public sealed record MyNewModuleDetailsViewModel(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments,
    [property: DataMember(Order = 5)] bool Disabled) : IIdDescription
{
    public static MyNewModuleDetailsViewModel Empty 
        => new(string.Empty, string.Empty, string.Empty, false);
        
    public static MyNewModuleDetailsViewModel Create(string? id)
        => new(
            string.IsNullOrWhiteSpace(id) ? UniqueIdHelper.GenerateUniqueStringId() : id, 
            string.Empty, 
            string.Empty, 
            false);
}
```

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `string` | Unique identifier |
| `Name` | `string` | Display name |
| `Comments` | `string?` | Optional description |
| `Disabled` | `bool` | Whether the module is disabled |

### MyNewModuleSummaryViewModel

Lightweight view model for list/grid displays:

```csharp
[DataContract]
public sealed record MyNewModuleSummaryViewModel(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] bool Disabled) : IIdDescription;
```

## Request Handlers

Request handlers are registered in the Projections project:

```csharp
public static IServiceCollection AddMyNewModuleRequestHandlers(this IServiceCollection services)
{
    services.TryAddScoped<IRequestHandler<GetMyNewModuleDetails>, GetMyNewModuleDetailsHandler>();
    services.TryAddScoped<IRequestHandler<GetMyNewModuleSummaries>, 
        GetFilteredCollectionHandler<GetMyNewModuleSummaries, MyNewModuleSummaryViewModel>>();
    services.TryAddScoped<IRequestHandler<GetMyNewModuleIds>, 
        GetAggregateIdsRequestHandler<GetMyNewModuleIds>>();
    return services;
}
```

## Request Flow

```
┌─────────────┐    ┌─────────────────┐    ┌──────────────┐
│   Request   │───▶│ Request Handler │───▶│  Read Model  │
│   (Query)   │    │   (Processor)   │    │ (Projection) │
└─────────────┘    └─────────────────┘    └──────────────┘
                           │
                           ▼
                    ┌──────────────┐
                    │  View Model  │
                    │   (Result)   │
                    └──────────────┘
```

## Dependencies

- `Hexalith.PolymorphicSerializations` - Serialization support
- `Hexalith.Domains.ValueObjects` - Common value objects

## Usage Example

```csharp
// Get single module details
var request = new GetMyNewModuleDetails("mod-001");
var response = await requestService.SubmitAsync(user, request, cancellationToken);
var details = response.Result;

// Get all summaries
var summariesRequest = new GetMyNewModuleSummaries();
var summariesResponse = await requestService.SubmitAsync(user, summariesRequest, cancellationToken);
var summaries = summariesResponse.Results;

// Get IDs for dropdown
var idsRequest = new GetMyNewModuleIds();
var idsResponse = await requestService.SubmitAsync(user, idsRequest, cancellationToken);
var ids = idsResponse.Results;
```

## Best Practices

1. **Keep view models lean** - Only include data needed by the UI
2. **Use appropriate request types** - Details vs Summary vs Export
3. **Support pagination** - For large datasets
4. **Implement caching** - For frequently accessed data
5. **Handle not found** - Return appropriate responses for missing data
