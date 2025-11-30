# Hexalith.MyNewModules.Aggregates

This project contains the domain aggregate implementations for the MyNewModule bounded context.

## Overview

Aggregates are the primary domain objects that encapsulate business rules and enforce invariants. They are the consistency boundary for transactions and the unit of persistence in event sourcing.

## Aggregate Pattern

### MyNewModule Aggregate

The `MyNewModule` aggregate is implemented as a C# record with immutable properties:

```csharp
[DataContract]
public sealed record MyNewModule(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments,
    [property: DataMember(Order = 7)] bool Disabled) : IDomainAggregate
{
    // Implementation
}
```

### Key Features

| Feature | Description |
|---------|-------------|
| **Immutability** | Uses C# records for immutable state |
| **Event Sourcing** | State changes are captured as events |
| **Apply Method** | Handles domain events to produce new state |
| **Validation** | Business rules are enforced in event handlers |

## Event Handling

The aggregate handles the following events:

| Event | Description | Behavior |
|-------|-------------|----------|
| `MyNewModuleAdded` | Initial creation | Initializes aggregate from event data |
| `MyNewModuleDescriptionChanged` | Update name/comments | Returns new aggregate with updated fields |
| `MyNewModuleDisabled` | Disable module | Sets `Disabled` to `true` |
| `MyNewModuleEnabled` | Enable module | Sets `Disabled` to `false` |

### Apply Method

```csharp
public ApplyResult Apply([NotNull] object domainEvent)
{
    return domainEvent switch
    {
        MyNewModuleAdded e => ApplyEvent(e),
        MyNewModuleDescriptionChanged e => ApplyEvent(e),
        MyNewModuleDisabled e => ApplyEvent(e),
        MyNewModuleEnabled e => ApplyEvent(e),
        MyNewModuleEvent => ApplyResult.NotImplemented(this),
        _ => ApplyResult.InvalidEvent(this, domainEvent),
    };
}
```

### Business Rules

1. **Cannot apply events to disabled aggregate** (except Enable/Disable events)
2. **Cannot apply events to uninitialized aggregate** (except Added event)
3. **Cannot add an already existing aggregate**
4. **Cannot disable an already disabled aggregate**
5. **Cannot enable an already enabled aggregate**

## Validators

**Location**: `Validators/`

Validators ensure that events and commands meet business requirements before being applied.

```csharp
public class MyNewModuleAddedValidator : AbstractValidator<MyNewModuleAdded>
{
    public MyNewModuleAddedValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
```

## Dependencies

- `Hexalith.Domains` - Base domain abstractions
- `Hexalith.MyNewModules.Events` - Domain events
- `Hexalith.MyNewModules.Aggregates.Abstractions` - Domain helpers

## Usage Example

```csharp
// Create new aggregate from Added event
var addedEvent = new MyNewModuleAdded("mod-001", "My Module", "Description");
var aggregate = new MyNewModule();
var result = aggregate.Apply(addedEvent);

if (result.Succeeded)
{
    var newAggregate = result.Aggregate as MyNewModule;
    // newAggregate.Id == "mod-001"
    // newAggregate.Name == "My Module"
}

// Apply subsequent events
var changeEvent = new MyNewModuleDescriptionChanged("mod-001", "Updated Name", "New Description");
var updateResult = newAggregate.Apply(changeEvent);
```

## Testing

Tests for this project should verify:

1. Event application produces correct state
2. Business rules are enforced
3. Invalid operations return appropriate errors
4. Validators reject invalid data
