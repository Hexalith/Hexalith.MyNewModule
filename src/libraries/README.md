# Libraries (NuGet Packages)

This directory contains all the library projects that are packaged and distributed as NuGet packages.

## Architecture Overview

The libraries follow a clean architecture approach with four main layers:

```
┌─────────────────────────────────────────────────────────┐
│                   Presentation                          │
│  (Hexalith.MyNewModules.UI.Components)                 │
│  (Hexalith.MyNewModules.UI.Pages)                      │
└─────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────┐
│                   Infrastructure                        │
│  (Hexalith.MyNewModules.ApiServer)                     │
│  (Hexalith.MyNewModules.WebServer)                     │
│  (Hexalith.MyNewModules.WebApp)                        │
│  (Hexalith.MyNewModules.Servers)                       │
└─────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────┐
│                    Application                          │
│  (Hexalith.MyNewModules)                               │
│  (Hexalith.MyNewModules.Abstractions)                  │
│  (Hexalith.MyNewModules.Commands)                      │
│  (Hexalith.MyNewModules.Requests)                      │
│  (Hexalith.MyNewModules.Projections)                   │
└─────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────┐
│                      Domain                             │
│  (Hexalith.MyNewModules.Aggregates)                    │
│  (Hexalith.MyNewModules.Aggregates.Abstractions)       │
│  (Hexalith.MyNewModules.Events)                        │
│  (Hexalith.MyNewModules.Localizations)                 │
└─────────────────────────────────────────────────────────┘
```

## Layer Descriptions

### Domain Layer (`Domain/`)

The domain layer contains the core business logic and is completely independent of any external framework.

| Project | Description |
|---------|-------------|
| `Hexalith.MyNewModules.Aggregates` | Domain aggregate implementations with event sourcing |
| `Hexalith.MyNewModules.Aggregates.Abstractions` | Domain helpers, enums, and value objects |
| `Hexalith.MyNewModules.Events` | Domain event definitions |
| `Hexalith.MyNewModules.Localizations` | Localization resource files |

### Application Layer (`Application/`)

The application layer implements use cases and orchestrates domain operations.

| Project | Description |
|---------|-------------|
| `Hexalith.MyNewModules` | Main application logic, command handlers, event handlers |
| `Hexalith.MyNewModules.Abstractions` | Module interfaces, policies, roles, service contracts |
| `Hexalith.MyNewModules.Commands` | Command definitions for write operations |
| `Hexalith.MyNewModules.Requests` | Query/request definitions and view models |
| `Hexalith.MyNewModules.Projections` | Projection handlers for updating read models |

### Infrastructure Layer (`Infrastructure/`)

The infrastructure layer provides technical implementations for hosting and integration.

| Project | Description |
|---------|-------------|
| `Hexalith.MyNewModules.ApiServer` | REST API controllers, Dapr actor registration |
| `Hexalith.MyNewModules.WebServer` | Server-side Blazor hosting |
| `Hexalith.MyNewModules.WebApp` | WebAssembly client module |
| `Hexalith.MyNewModules.Servers` | Shared server utilities |

### Presentation Layer (`Presentation/`)

The presentation layer contains all UI-related code.

| Project | Description |
|---------|-------------|
| `Hexalith.MyNewModules.UI.Components` | Reusable Blazor components |
| `Hexalith.MyNewModules.UI.Pages` | Blazor pages and view models |

## Package Dependencies

### Domain Layer Dependencies

```
Hexalith.MyNewModules.Aggregates
├── Hexalith.MyNewModules.Aggregates.Abstractions
└── Hexalith.MyNewModules.Events

Hexalith.MyNewModules.Events
└── Hexalith.MyNewModules.Aggregates.Abstractions

Hexalith.MyNewModules.Localizations
└── (no internal dependencies)
```

### Application Layer Dependencies

```
Hexalith.MyNewModules
├── Hexalith.MyNewModules.Abstractions
├── Hexalith.MyNewModules.Aggregates
├── Hexalith.MyNewModules.Commands
├── Hexalith.MyNewModules.Events
├── Hexalith.MyNewModules.Projections
└── Hexalith.MyNewModules.Requests

Hexalith.MyNewModules.Commands
├── Hexalith.MyNewModules.Aggregates.Abstractions
└── Hexalith.MyNewModules.Abstractions

Hexalith.MyNewModules.Requests
├── Hexalith.MyNewModules.Aggregates.Abstractions
└── Hexalith.MyNewModules.Abstractions
```

## Building Packages

### Build All Libraries

```bash
cd src/libraries
dotnet build
```

### Create NuGet Packages

```bash
dotnet pack --configuration Release
```

### Local Development

During local development, project references are used instead of package references. This is controlled by the `UseProjectReference` property in `Directory.Build.props`.

## Adding New Libraries

1. Create a new folder in the appropriate layer directory
2. Add a `.csproj` file following the naming convention
3. Add to the solution file
4. Set up project references following the dependency graph
5. Create a README.md for the new project

### Project Template

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <!-- Add package references -->
  </ItemGroup>
  <ItemGroup>
    <!-- Add project references -->
  </ItemGroup>
</Project>
```

## Code Organization Guidelines

1. **One concept per file**: Each class, record, or interface should be in its own file
2. **Consistent naming**: File names should match the type name they contain
3. **Folder structure**: Organize by feature, not by type
4. **README per project**: Each project should have its own README.md
