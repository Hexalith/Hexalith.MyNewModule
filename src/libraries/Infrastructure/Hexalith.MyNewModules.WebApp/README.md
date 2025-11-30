# Hexalith.MyNewModules.WebApp

This project provides the WebAssembly (WASM) client module for the MyNewModule bounded context.

## Overview

The WebApp module enables:
- Client-side Blazor WebAssembly execution
- Interactive client-side rendering (CSR)
- Client-side service registration
- UI component assembly references

## Directory Structure

```
Hexalith.MyNewModules.WebApp/
├── Modules/
│   └── HexalithMyNewModulesWebAppModule.cs
├── Properties/
│   └── launchSettings.json
├── Services/
│   └── (client-side services)
└── Hexalith.MyNewModules.WebApp.csproj
```

## Module Definition

### HexalithMyNewModulesWebAppModule

The main module class implementing `IWebAppApplicationModule`:

```csharp
public class HexalithMyNewModulesWebAppModule : IWebAppApplicationModule, IMyNewModuleModule
{
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies 
        => MyNewModuleModulePolicies.AuthorizationPolicies;
    
    public IEnumerable<string> Dependencies => [];
    public string Description => "MyNewModule client module";
    public string Id => "Hexalith.MyNewModules.Client";
    public string Name => "MyNewModule client";
    public int OrderWeight => 0;
    public string Path => nameof(MyNewModules);
    public string Version => "1.0";

    public IEnumerable<Assembly> PresentationAssemblies => [
        GetType().Assembly,
        typeof(UI.Components._Imports).Assembly,
        typeof(UI.Pages._Imports).Assembly,
    ];
}
```

### Service Registration

```csharp
public static void AddServices(IServiceCollection services)
{
    // Register serialization mappers
    HexalithMyNewModulesEventsSerialization.RegisterPolymorphicMappers();
    HexalithMyNewModulesCommandsSerialization.RegisterPolymorphicMappers();
    HexalithMyNewModulesRequestsSerialization.RegisterPolymorphicMappers();

    // Add application module
    services.TryAddSingleton<IMyNewModuleModule, HexalithMyNewModulesWebAppModule>();

    // Add query services
    services.AddMyNewModuleQueryServices();
    
    // Add menu configuration
    services.AddTransient(_ => MyNewModulesMenu.Menu);
}
```

## Presentation Assemblies

The module references presentation assemblies for Blazor component routing:

| Assembly | Purpose |
|----------|---------|
| `HexalithMyNewModulesWebAppModule` | Module assembly itself |
| `UI.Components._Imports` | Reusable UI components |
| `UI.Pages._Imports` | Blazor pages and routes |

## Render Modes

The WebApp module supports:
- **InteractiveWebAssembly** - Full client-side interactivity
- **InteractiveAuto** - Automatic selection based on capabilities

### Using Render Modes in Pages

```razor
@page "/MyNewModule/MyNewModule"
@rendermode InteractiveAuto

<MyComponent />
```

## Client-Side Services

Services registered in the WebApp are available for:
- HTTP requests to the API server
- Client-side command/request submission
- Local state management

### Service Communication

```
┌─────────────────┐    HTTP    ┌─────────────────┐
│   Blazor WASM   │◄─────────►│   API Server    │
│    (Client)     │            │    (Backend)    │
└─────────────────┘            └─────────────────┘
```

## Menu Integration

The module registers its menu items:

```csharp
services.AddTransient(_ => MyNewModulesMenu.Menu);
```

Menu items appear in the application's navigation based on the registered `MenuItem` objects.

## Dependencies

- `Hexalith.Infrastructure.ClientAppOnWasm` - WASM client infrastructure
- `Hexalith.MyNewModules.Projections.Helpers` - Query services
- `Hexalith.MyNewModules.UI.Pages` - Blazor pages
- `Hexalith.MyNewModules.UI.Components` - Blazor components

## Configuration

### launchSettings.json

```json
{
  "profiles": {
    "WebAssembly": {
      "commandName": "Project",
      "launchBrowser": true,
      "inspectUri": "{wsProtocol}://{url.hostname}:{url.port}/_framework/debug/ws-proxy?browser={browserInspectUri}"
    }
  }
}
```

## Integration with WebServer

The WebApp module is referenced by the WebServer for:
1. Server-side prerendering
2. Component assembly discovery
3. Route registration

```csharp
// In WebServer
public override Type WebAppApplicationType => typeof(HexalithMyNewModulesWebAppApplication);
```

## Best Practices

1. **Minimize bundle size** - Only include necessary code
2. **Use lazy loading** - For large components
3. **Handle offline scenarios** - Graceful degradation
4. **Optimize API calls** - Batch requests when possible
5. **Client-side caching** - Reduce API round trips
