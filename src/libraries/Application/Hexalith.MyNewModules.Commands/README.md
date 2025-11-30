# Hexalith.MyNewModules.Commands

This project contains the command definitions for the MyNewModule bounded context.

## Overview

Commands represent the intent to change the system state. They are part of the CQRS (Command Query Responsibility Segregation) pattern implementation.

## Command Hierarchy

```
MyNewModuleCommand (abstract base)
├── AddMyNewModule
├── ChangeMyNewModuleDescription
├── DisableMyNewModule
└── EnableMyNewModule
```

## Commands

### MyNewModuleCommand (Base Command)

All MyNewModule commands inherit from this abstract base record:

```csharp
[PolymorphicSerialization]
public abstract partial record MyNewModuleCommand(string Id)
{
    public string AggregateId => Id;
    public static string AggregateName => MyNewModuleDomainHelper.MyNewModuleAggregateName;
}
```

### AddMyNewModule

Command to create a new module.

```csharp
[PolymorphicSerialization]
public partial record AddMyNewModule(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : MyNewModuleCommand(Id);
```

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Id` | `string` | Yes | Unique identifier for the module |
| `Name` | `string` | Yes | Display name of the module |
| `Comments` | `string?` | No | Optional description |

### ChangeMyNewModuleDescription

Command to update the module's name and/or description.

```csharp
[PolymorphicSerialization]
public partial record ChangeMyNewModuleDescription(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : MyNewModuleCommand(Id);
```

### DisableMyNewModule

Command to disable a module.

```csharp
[PolymorphicSerialization]
public partial record DisableMyNewModule(string Id) : MyNewModuleCommand(Id);
```

### EnableMyNewModule

Command to enable a previously disabled module.

```csharp
[PolymorphicSerialization]
public partial record EnableMyNewModule(string Id) : MyNewModuleCommand(Id);
```

## Command Validators

**Location**: `MyNewModule/AddMyNewModuleValidator.cs`

Commands are validated using FluentValidation:

```csharp
public class AddMyNewModuleValidator : AbstractValidator<AddMyNewModule>
{
    public AddMyNewModuleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Module ID is required");
            
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Name must be between 1 and 100 characters");
    }
}
```

## Command Handlers

Command handlers are responsible for:

1. Validating the command
2. Loading the aggregate from the event store
3. Executing business logic
4. Producing domain events
5. Persisting the events

Handlers are registered in the `Application` layer:

```csharp
services.AddMyNewModuleCommandHandlers();
```

## Command Flow

```
┌─────────────┐    ┌────────────────┐    ┌───────────────┐
│   Command   │───▶│ Command Handler │───▶│   Aggregate   │
│  (Intent)   │    │  (Orchestrator) │    │ (Apply Logic) │
└─────────────┘    └────────────────┘    └───────────────┘
                           │                      │
                           ▼                      ▼
                    ┌────────────┐         ┌───────────┐
                    │ Validation │         │  Events   │
                    └────────────┘         └───────────┘
                                                 │
                                                 ▼
                                          ┌───────────┐
                                          │Event Store│
                                          └───────────┘
```

## Dependencies

- `Hexalith.PolymorphicSerializations` - Serialization support
- `Hexalith.MyNewModules.Aggregates.Abstractions` - Domain helpers
- `FluentValidation` - Command validation

## Usage Example

```csharp
// Create a command
var command = new AddMyNewModule(
    Id: "mod-001",
    Name: "Sample Module",
    Comments: "This is a sample module"
);

// Submit via command service
await commandService.SubmitCommandAsync(user, command, cancellationToken);

// Or submit multiple commands
await commandService.SubmitCommandsAsync(user, [
    new AddMyNewModule("mod-001", "Module 1", null),
    new AddMyNewModule("mod-002", "Module 2", null)
], cancellationToken);
```

## Best Practices

1. **Commands are imperative** - Name them as actions (Add, Change, Disable)
2. **Include validation** - Fail fast with clear error messages
3. **Keep commands focused** - One intent per command
4. **Immutable data** - Use records for immutability
5. **Idempotency** - Design handlers to be idempotent when possible
