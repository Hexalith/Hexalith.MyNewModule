# Hexalith.MyNewModules.Events

This project contains the domain event definitions for the MyNewModule bounded context.

## Overview

Domain events represent facts that have happened in the domain. They are immutable records of state changes and form the foundation of event sourcing.

## Event Hierarchy

```
MyNewModuleEvent (abstract base)
├── MyNewModuleAdded
├── MyNewModuleDescriptionChanged
├── MyNewModuleDisabled
└── MyNewModuleEnabled
```

## Events

### MyNewModuleEvent (Base Event)

All MyNewModule events inherit from this abstract base record:

```csharp
[PolymorphicSerialization]
public abstract partial record MyNewModuleEvent(string Id)
{
    public string AggregateId => Id;
    public static string AggregateName => MyNewModuleDomainHelper.MyNewModuleAggregateName;
}
```

### MyNewModuleAdded

Raised when a new module is created.

```csharp
[PolymorphicSerialization]
public partial record MyNewModuleAdded(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : MyNewModuleEvent(Id);
```

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `string` | Unique identifier for the module |
| `Name` | `string` | Display name of the module |
| `Comments` | `string?` | Optional description or comments |

### MyNewModuleDescriptionChanged

Raised when the module's name or description is updated.

```csharp
[PolymorphicSerialization]
public partial record MyNewModuleDescriptionChanged(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : MyNewModuleEvent(Id);
```

### MyNewModuleDisabled

Raised when a module is disabled.

```csharp
[PolymorphicSerialization]
public partial record MyNewModuleDisabled(string Id) : MyNewModuleEvent(Id);
```

### MyNewModuleEnabled

Raised when a disabled module is re-enabled.

```csharp
[PolymorphicSerialization]
public partial record MyNewModuleEnabled(string Id) : MyNewModuleEvent(Id);
```

## Polymorphic Serialization

Events use the `[PolymorphicSerialization]` attribute for JSON serialization with type discrimination. This enables:

- Type-safe deserialization
- Event versioning support
- Cross-service compatibility

### Registration

Events must be registered in the serialization helper:

```csharp
public static class HexalithMyNewModulesEventsSerialization
{
    public static void RegisterPolymorphicMappers()
    {
        // Auto-generated registration code
    }
}
```

## Event Versioning

When modifying events, follow these guidelines:

1. **Never remove properties** - Mark as obsolete instead
2. **Add new properties with defaults** - Ensure backward compatibility
3. **Use DataMember Order** - Maintain serialization order
4. **Document changes** - Keep a changelog

## Dependencies

- `Hexalith.PolymorphicSerializations` - Serialization support
- `Hexalith.MyNewModules.Aggregates.Abstractions` - Domain helpers

## Usage Example

```csharp
// Create an event
var added = new MyNewModuleAdded(
    Id: "mod-001",
    Name: "Sample Module",
    Comments: "This is a sample module"
);

// Access aggregate information
Console.WriteLine(added.AggregateId);   // "mod-001"
Console.WriteLine(MyNewModuleEvent.AggregateName); // "MyNewModule"

// Serialize to JSON
var json = JsonSerializer.Serialize(added);
// Deserialize back
var deserialized = JsonSerializer.Deserialize<MyNewModuleEvent>(json);
```

## Best Practices

1. **Keep events simple** - Only include data that changed
2. **Use meaningful names** - Events should read like past-tense sentences
3. **Include all required data** - Events should be self-contained
4. **Avoid business logic** - Events are data, not behavior
