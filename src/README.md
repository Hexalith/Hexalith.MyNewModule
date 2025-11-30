# Source Code Directory

This directory contains the main source code for the Hexalith MyNewModule project.

## Directory Structure

```
src/
├── HexalithMyNewModulesApiServerApplication.cs  # API Server application definition
├── HexalithMyNewModulesWebAppApplication.cs     # WebAssembly client application definition
├── HexalithMyNewModulesWebServerApplication.cs  # Web Server application definition
│
└── libraries/                                    # NuGet package projects
    ├── Application/                              # Application layer (CQRS)
    ├── Domain/                                   # Domain layer (DDD)
    ├── Infrastructure/                           # Infrastructure layer
    └── Presentation/                             # Presentation layer (Blazor)
```

## Application Entry Points

### API Server Application

The `HexalithMyNewModulesApiServerApplication.cs` defines the API server configuration:

```csharp
public class HexalithMyNewModulesApiServerApplication : HexalithApiServerApplication
{
    public override IEnumerable<Type> ApiServerModules => [
        typeof(HexalithUIComponentsApiServerModule),
        typeof(HexalithSecurityApiServerModule),
        typeof(HexalithMyNewModulesApiServerModule)
    ];
    
    public override string Id => $"{HexalithMyNewModulesInformation.Id}.{ApplicationType}";
    public override string Name => $"{HexalithMyNewModulesInformation.Name} {ApplicationType}";
}
```

### Web Server Application

The `HexalithMyNewModulesWebServerApplication.cs` defines the server-side rendered web application:

```csharp
public class HexalithMyNewModulesWebServerApplication : HexalithWebServerApplication
{
    public override Type WebAppApplicationType => typeof(HexalithMyNewModulesWebAppApplication);
    
    public override IEnumerable<Type> WebServerModules => [
        typeof(HexalithUIComponentsWebServerModule),
        typeof(HexalithSecurityWebServerModule),
        typeof(HexalithMyNewModuleWebServerModule)
    ];
}
```

### WebApp Application

The `HexalithMyNewModulesWebAppApplication.cs` defines the WebAssembly client application for interactive client-side rendering.

## Layer Dependencies

```
Presentation Layer
       ↓
Infrastructure Layer
       ↓
Application Layer
       ↓
Domain Layer
```

Dependencies flow downward only. Upper layers depend on lower layers, but never the reverse.

## Building

From this directory:

```bash
# Restore dependencies
dotnet restore

# Build all projects
dotnet build

# Build specific project
dotnet build libraries/Application/Hexalith.MyNewModules/
```

## Adding New Projects

When adding new projects to the `libraries/` directory:

1. Create the project in the appropriate layer folder
2. Follow the naming convention: `Hexalith.MyNewModules.{Purpose}`
3. Add project reference to the solution file
4. Update `Directory.Build.props` if needed
5. Add package reference to `Directory.Packages.props` if using new NuGet packages
