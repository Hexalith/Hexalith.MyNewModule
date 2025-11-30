# CLAUDE.md

AI assistant guidance for **Hexalith.MyNewModule** - a DDD/CQRS/Event Sourcing module template.

## Critical Rules (Always Follow)

### File Header (Required on ALL .cs files)
```csharp
// <copyright file="{FileName}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
```

### C# Style Requirements
- **Primary constructors** - Use for classes and records; NEVER duplicate properties in body
- **File-scoped namespaces** - Always use `namespace X;` not `namespace X { }`
- **XML documentation** - Required on all public/protected/internal members
- **Record params** - Document with `<param>` tags, not `<property>`

### Record Documentation Pattern
```csharp
/// <summary>
/// Represents a domain entity.
/// </summary>
/// <param name="Id">The unique identifier.</param>
/// <param name="Name">The display name.</param>
[DataContract]
public sealed record MyEntity(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name);
```

## Tech Stack
- **.NET 10** / **C# 13** (use latest features)
- **Blazor InteractiveAuto** (SSR + WebAssembly)
- **Dapr** / **.NET Aspire** / **Azure Cosmos DB**
- **xUnit + Shouldly + Moq** (testing)

## Architecture Layers

| Layer | Projects | Purpose |
|-------|----------|---------|
| **Domain** | `*.Aggregates`, `*.Events`, `*.Localizations` | Core business logic |
| **Application** | `*.Commands`, `*.Requests`, `*.Projections` | CQRS handlers |
| **Infrastructure** | `*.ApiServer`, `*.WebServer`, `*.WebApp` | Technical implementation |
| **Presentation** | `*.UI.Components`, `*.UI.Pages` | Blazor UI |

## Code Patterns

### Domain Event
```csharp
[PolymorphicSerialization]
public partial record MyNewModuleAdded(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : MyNewModuleEvent(Id);
```

### Command (same pattern as Event)
```csharp
[PolymorphicSerialization]
public abstract partial record MyNewModuleCommand(string Id)
{
    public string AggregateId => Id;
    public static string AggregateName => MyNewModuleDomainHelper.MyNewModuleAggregateName;
}
```

### Aggregate Apply Pattern
```csharp
public ApplyResult Apply([NotNull] object domainEvent)
{
    ArgumentNullException.ThrowIfNull(domainEvent);
    return domainEvent switch
    {
        MyNewModuleAdded e => ApplyEvent(e),
        MyNewModuleDescriptionChanged e => ApplyEvent(e),
        MyNewModuleEvent => ApplyResult.NotImplemented(this),
        _ => ApplyResult.InvalidEvent(this, domainEvent),
    };
}

private ApplyResult ApplyEvent(MyNewModuleAdded e) => !(this as IDomainAggregate).IsInitialized()
    ? ApplyResult.Success(new MyNewModule(e), [e])
    : ApplyResult.Error(this, "The MyNewModule already exists.");
```

### Request with Result
```csharp
[PolymorphicSerialization]
public partial record GetMyNewModuleDetails(
    string Id,
    [property: DataMember(Order = 2)] MyNewModuleDetailsViewModel? Result = null)
    : MyNewModuleRequest(Id), IRequest
{
    object? IRequest.Result => Result;
}
```

## Naming Conventions

| Type | Pattern | Example |
|------|---------|---------|
| Events | `{Entity}{PastTenseVerb}` | `MyNewModuleAdded`, `MyNewModuleDisabled` |
| Commands | `{Verb}{Entity}` | `AddMyNewModule`, `DisableMyNewModule` |
| Requests | `Get{Entity}{Details\|Summaries}` | `GetMyNewModuleDetails` |
| Handlers | `{Command\|Request}Handler` | `AddMyNewModuleHandler` |
| Private fields | `_camelCase` | `_repository` |

## Key Attributes

- `[PolymorphicSerialization]` - Required on events, commands, requests (enables JSON polymorphism)
- `[DataContract]` - On aggregates and DTOs
- `[DataMember(Order = N)]` - Property serialization order (start at 1 for Id, increment)

## Entity Creation Order

1. **Events** → `src/libraries/Domain/Hexalith.MyNewModules.Events/{Entity}/`
2. **Aggregate** → `src/libraries/Domain/Hexalith.MyNewModules.Aggregates/`
3. **Commands** → `src/libraries/Application/Hexalith.MyNewModules.Commands/{Entity}/`
4. **Requests** → `src/libraries/Application/Hexalith.MyNewModules.Requests/{Entity}/`
5. **Projections** → `src/libraries/Application/Hexalith.MyNewModules.Projections/`
6. **UI Components** → `src/libraries/Presentation/Hexalith.MyNewModules.UI.Components/`
7. **UI Pages** → `src/libraries/Presentation/Hexalith.MyNewModules.UI.Pages/`

## Testing

```csharp
[Fact]
public void Apply_MyNewModuleAdded_ShouldInitializeAggregate()
{
    // Arrange
    var aggregate = new MyNewModule();
    var added = new MyNewModuleAdded("test-id", "Test Name", null);

    // Act
    var result = aggregate.Apply(added);

    // Assert
    result.Succeeded.ShouldBeTrue();
    result.Aggregate.ShouldBeOfType<MyNewModule>()
        .Id.ShouldBe("test-id");
}
```

Test naming: `{Method}_{Scenario}_{ExpectedResult}`

## Commands

```bash
dotnet build                    # Build solution
dotnet test                     # Run tests
cd AspireHost && dotnet run     # Start with Aspire orchestration
./initialize.ps1 -PackageName "YourModule"  # Initialize from template
```

## Do Not Modify

- `DOCUMENTATION.ai.md` - AI prompts
- `Hexalith.Builds/` - Build configs (submodule)
- `HexalithApp/` - Base framework (submodule)

## Hexalith Package Version

Current: **1.71.1** (defined in `Directory.Packages.props`)
