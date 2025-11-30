# Hexalith.MyNewModules.UI.Components

This project contains reusable Blazor UI components for the MyNewModule bounded context.

## Overview

UI Components provides:
- Reusable Blazor components
- Input fields and forms
- Data grids and tables
- Common UI patterns

## Directory Structure

```
Hexalith.MyNewModules.UI.Components/
├── Common/
│   └── (shared components)
├── MyNewModule/
│   ├── _Imports.razor
│   ├── MyNewModuleIdField.razor
│   └── MyNewModuleSummaryGrid.razor
├── wwwroot/
│   └── (static assets)
├── _Imports.razor
└── Hexalith.MyNewModules.UI.Components.csproj
```

## Components

### MyNewModuleIdField

An input field component for selecting or entering a MyNewModule ID:

```razor
<MyNewModuleIdField @bind-Value="@SelectedId" />
```

**Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| `Value` | `string?` | The selected ID value |
| `ValueChanged` | `EventCallback<string?>` | Callback when value changes |
| `Disabled` | `bool` | Whether the field is disabled |
| `Required` | `bool` | Whether the field is required |

### MyNewModuleSummaryGrid

A data grid component for displaying module summaries:

```razor
<MyNewModuleSummaryGrid 
    Items="@_summariesQuery" 
    EntityDetailsPath="/MyNewModule/MyNewModule" 
    OnDisabledChanged="OnDisabledChangedAsync" />
```

**Parameters:**
| Parameter | Type | Description |
|-----------|------|-------------|
| `Items` | `IQueryable<MyNewModuleSummaryViewModel>?` | Data source |
| `EntityDetailsPath` | `string` | Path to detail page |
| `OnDisabledChanged` | `EventCallback<string>` | Callback when disable toggled |

**Features:**
- Sortable columns
- Filterable data
- Pagination
- Row selection
- Action buttons

## Global Imports

**_Imports.razor:**

```razor
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.FluentUI.AspNetCore.Components
@using Hexalith.MyNewModules.Requests.MyNewModule
@using Hexalith.UI.Components
```

## Styling

Components use the Fluent UI design system:
- Consistent with Microsoft design language
- Accessible by default
- Responsive design
- Theme support (light/dark)

### Custom Styles

Custom styles can be added in `wwwroot/`:

```css
/* Custom component styles */
.mynewmodule-grid {
    /* Custom grid styles */
}
```

## Component Guidelines

### Creating New Components

1. Create a new `.razor` file in the appropriate folder
2. Add necessary `@using` directives
3. Define parameters using `[Parameter]`
4. Implement component logic in `@code` block
5. Add XML documentation

**Example:**

```razor
@* MyNewComponent.razor *@

<div class="mynewmodule-component">
    <FluentLabel>@Label</FluentLabel>
    <FluentTextField @bind-Value="@Value" />
</div>

@code {
    /// <summary>
    /// The label text.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// The input value.
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// Callback when value changes.
    /// </summary>
    [Parameter]
    public EventCallback<string?> ValueChanged { get; set; }
}
```

## Dependencies

- `Microsoft.FluentUI.AspNetCore.Components` - Fluent UI
- `Hexalith.UI.Components` - Base component library
- `Hexalith.MyNewModules.Requests` - View models

## Usage in Pages

```razor
@page "/MyNewModule/Example"

<MyNewModuleIdField @bind-Value="@_selectedId" />

<MyNewModuleSummaryGrid 
    Items="@_items"
    EntityDetailsPath="/MyNewModule/MyNewModule" />

@code {
    private string? _selectedId;
    private IQueryable<MyNewModuleSummaryViewModel>? _items;
}
```

## Best Practices

1. **Keep components focused** - Single responsibility
2. **Use parameters** - Make components configurable
3. **Support two-way binding** - Use `EventCallback` for value changes
4. **Handle null values** - Graceful handling of missing data
5. **Add documentation** - XML comments for IntelliSense
6. **Use semantic HTML** - Accessibility first


