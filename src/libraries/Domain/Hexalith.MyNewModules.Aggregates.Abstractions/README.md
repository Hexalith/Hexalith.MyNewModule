# Hexalith.MyNewModules.Aggregates.Abstractions

This project contains domain helper classes, enums, and value objects for the MyNewModule bounded context.

## Overview

The Aggregates Abstractions project provides:
- Domain helper constants
- Enumeration types
- Value objects
- Shared domain definitions

## Directory Structure

```
Hexalith.MyNewModules.Aggregates.Abstractions/
├── Enums/
│   └── (enumeration types)
├── ValueObjects/
│   └── (value object definitions)
├── MyNewModuleDomainHelper.cs
└── Hexalith.MyNewModules.Aggregates.Abstractions.csproj
```

## Domain Helper

### MyNewModuleDomainHelper

Provides constant values for domain operations:

```csharp
public static class MyNewModuleDomainHelper
{
    /// <summary>
    /// The name of the MyNewModule aggregate.
    /// </summary>
    public const string MyNewModuleAggregateName = "MyNewModule";
}
```

**Usage:**

```csharp
// Get aggregate name for routing
var aggregateName = MyNewModuleDomainHelper.MyNewModuleAggregateName;

// Use in event base class
public static string AggregateName => MyNewModuleDomainHelper.MyNewModuleAggregateName;

// Use in actor registration
actorRegistrations.RegisterActor<DomainAggregateActor>(
    MyNewModuleDomainHelper.MyNewModuleAggregateName.ToAggregateActorName());
```

## Enums

Add enumeration types for domain concepts:

```csharp
/// <summary>
/// Represents the status of a module.
/// </summary>
public enum MyNewModuleStatus
{
    /// <summary>
    /// Module is active and available.
    /// </summary>
    Active = 0,
    
    /// <summary>
    /// Module is temporarily suspended.
    /// </summary>
    Suspended = 1,
    
    /// <summary>
    /// Module has been archived.
    /// </summary>
    Archived = 2
}
```

## Value Objects

Value objects are immutable types with no identity:

```csharp
/// <summary>
/// Represents a module configuration value.
/// </summary>
[DataContract]
public sealed record ModuleConfiguration(
    [property: DataMember(Order = 1)] string Key,
    [property: DataMember(Order = 2)] string Value,
    [property: DataMember(Order = 3)] bool IsRequired)
{
    /// <summary>
    /// Creates an empty configuration.
    /// </summary>
    public static ModuleConfiguration Empty => new(string.Empty, string.Empty, false);
}
```

**Value Object Guidelines:**

1. **Immutability** - Use records or readonly properties
2. **Equality by value** - Not by reference
3. **No identity** - No unique identifier
4. **Self-validating** - Validate in constructor
5. **Side-effect free** - No external state changes

## Dependencies

This project has minimal dependencies:
- `System.Runtime.Serialization` - For `[DataContract]` attributes

## Usage

### In Events

```csharp
public abstract partial record MyNewModuleEvent(string Id)
{
    public static string AggregateName => MyNewModuleDomainHelper.MyNewModuleAggregateName;
}
```

### In Commands

```csharp
public abstract partial record MyNewModuleCommand(string Id)
{
    public static string AggregateName => MyNewModuleDomainHelper.MyNewModuleAggregateName;
}
```

### In Aggregates

```csharp
public sealed record MyNewModule(...) : IDomainAggregate
{
    public string AggregateName => MyNewModuleDomainHelper.MyNewModuleAggregateName;
}
```

## Adding New Types

### Adding an Enum

1. Create a new file in `Enums/`
2. Document with XML comments
3. Use meaningful values

### Adding a Value Object

1. Create a new file in `ValueObjects/`
2. Use C# records for immutability
3. Add `[DataContract]` for serialization
4. Implement validation if needed
5. Add factory methods for common cases

## Best Practices

1. **Keep types simple** - Single responsibility
2. **Use constants** - Avoid magic strings
3. **Document everything** - XML comments for all public types
4. **Consider serialization** - Use appropriate attributes
5. **No business logic** - Keep in aggregates
