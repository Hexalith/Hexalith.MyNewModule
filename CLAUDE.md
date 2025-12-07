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
public partial record MyToDoAdded(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : MyToDoEvent(Id);
```

### Command (same pattern as Event)

```csharp
[PolymorphicSerialization]
public abstract partial record MyToDoCommand(string Id)
{
    public string AggregateId => Id;
    public static string AggregateName => MyToDoDomainHelper.MyToDoAggregateName;
}
```

### Aggregate Apply Pattern

```csharp
public ApplyResult Apply([NotNull] object domainEvent)
{
    ArgumentNullException.ThrowIfNull(domainEvent);
    return domainEvent switch
    {
        MyToDoAdded e => ApplyEvent(e),
        MyToDoDescriptionChanged e => ApplyEvent(e),
        MyToDoEvent => ApplyResult.NotImplemented(this),
        _ => ApplyResult.InvalidEvent(this, domainEvent),
    };
}

private ApplyResult ApplyEvent(MyToDoAdded e) => !(this as IDomainAggregate).IsInitialized()
    ? ApplyResult.Success(new MyToDo(e), [e])
    : ApplyResult.Error(this, "The MyToDo already exists.");
```

### Request with Result

```csharp
[PolymorphicSerialization]
public partial record GetMyToDoDetails(
    string Id,
    [property: DataMember(Order = 2)] MyToDoDetailsViewModel? Result = null)
    : MyToDoRequest(Id), IRequest
{
    object? IRequest.Result => Result;
}
```

### Module Security
```csharp
public static class MyNewModuleRoles
{
    /// <summary>
    /// Role for users who can contribute to MyNewModule but can't manage it.
    /// </summary>
    public const string Contributor = nameof(MyNewModule) + nameof(Contributor);

    /// <summary>
    /// Role for users who own MyNewModule. They can manage it.
    /// </summary>
    public const string Owner = nameof(MyNewModule) + nameof(Owner);

    /// <summary>
    /// Role for users who can only read MyNewModule.
    /// </summary>
    public const string Reader = nameof(MyNewModule) + nameof(Reader);
}
public static class MyNewModulePolicies
{
    public const string Contributor = MyNewModuleRoles.Contributor;
    public const string Owner = MyNewModuleRoles.Owner;
    public const string Reader = MyNewModuleRoles.Reader;
}
public static class MyNewModuleModulePolicies
{
    public static IDictionary<string, AuthorizationPolicy> AuthorizationPolicies =>
    new Dictionary<string, AuthorizationPolicy>
    {
        {
            MyNewModulePolicies.Owner, new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(ApplicationRoles.GlobalAdministrator, MyNewModuleRoles.Owner)
                .Build()
        },
        {
            MyNewModulePolicies.Contributor, new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(ApplicationRoles.GlobalAdministrator, MyNewModuleRoles.Owner, MyNewModuleRoles.Contributor)
                .Build()
        },
        {
            MyNewModulePolicies.Reader, new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(ApplicationRoles.GlobalAdministrator, MyNewModuleRoles.Owner, MyNewModuleRoles.Contributor, MyNewModuleRoles.Reader)
                .Build()
        },
    };
```

## Naming Conventions

| Type | Pattern | Example |
|------|---------|---------|
| Events | `{Entity}{PastTenseVerb}` | `MyToDoAdded`, `MyToDoDisabled` |
| Commands | `{Verb}{Entity}` | `AddMyToDo`, `DisableMyToDo` |
| Requests | `Get{Entity}{Details\|Summaries}` | `GetMyToDoDetails` |
| Handlers | `{Command\|Request}Handler` | `AddMyToDoHandler` |
| Private fields | `_camelCase` | `_repository` |

## Key Attributes

- `[PolymorphicSerialization]` - Required on events, commands, requests (enables JSON polymorphism)
- `[DataContract]` - On aggregates and DTOs
- `[DataMember(Order = N)]` - Property serialization order (start at 1 for Id, increment)

## Entity Creation Order

1. **Events** → `src/libraries/Domain/Hexalith.MyNewModule.Events/{Entity}/`
2. **Aggregate** → `src/libraries/Domain/Hexalith.MyNewModule.Aggregates/`
3. **Commands** → `src/libraries/Application/Hexalith.MyNewModule.Commands/{Entity}/`
4. **Requests** → `src/libraries/Application/Hexalith.MyNewModule.Requests/{Entity}/`
5. **Projections** → `src/libraries/Application/Hexalith.MyNewModule.Projections/`
6. **UI Components** → `src/libraries/Presentation/Hexalith.MyNewModule.UI.Components/`
7. **UI Pages** → `src/libraries/Presentation/Hexalith.MyNewModule.UI.Pages/`

## Testing

```csharp
[Fact]
public void Apply_MyToDoAdded_ShouldInitializeAggregate()
{
    // Arrange
    var aggregate = new MyToDo();
    var added = new MyToDoAdded("test-id", "Test Name", null);

    // Act
    var result = aggregate.Apply(added);

    // Assert
    result.Succeeded.ShouldBeTrue();
    result.Aggregate.ShouldBeOfType<MyToDo>()
        .Id.ShouldBe("test-id");
}
```

Test naming: `{Method}_{Scenario}_{ExpectedResult}`

## Do Not Modify

- `Hexalith.Builds/` - Build configs (submodule)
- `HexalithApp/` - Base framework (submodule)

