# Hexalith.MyNewModules.UI.Pages

This project contains Blazor pages and view models for the MyNewModule bounded context.

## Overview

UI Pages provides:
- Routable Blazor pages
- Page-specific view models
- Edit view models with validation
- Page navigation and routing

## Directory Structure

```
Hexalith.MyNewModules.UI.Pages/
├── Modules/
│   └── MyNewModulesMenu.cs
├── MyNewModule/
│   ├── MyNewModuleDetails.razor
│   ├── MyNewModuleEditValidation.cs
│   ├── MyNewModuleEditViewModel.cs
│   ├── MyNewModuleIndex.razor
│   └── Labels.cs
├── Home.razor
├── _Imports.razor
├── wwwroot/
│   └── (static assets)
└── Hexalith.MyNewModules.UI.Pages.csproj
```

## Pages

### Home Page

**Route:** `/MyNewModule`

The module's landing page:

```razor
@page "/MyNewModule"
@attribute [AllowAnonymous]

<FluentLabel Typo="Typography.PageTitle">MyNewModule</FluentLabel>
```

### Index Page (List View)

**Route:** `/MyNewModule/MyNewModule`

Displays a list of all modules:

```razor
@page "/MyNewModule/MyNewModule"
@rendermode InteractiveAuto

<HexEntityIndexPage 
    OnLoadData="LoadSummaries"
    OnImport="ImportAsync"
    OnExport="ExportAsync"
    OnDatabaseSynchronize="SynchronizeDatabaseAsync"
    AddPagePath="/MyNewModule/Add/MyNewModule"
    Title="@Labels.ListTitle">
    <MyNewModuleSummaryGrid 
        Items="_summariesQuery" 
        EntityDetailsPath="/MyNewModule/MyNewModule" 
        OnDisabledChanged="OnDisabledChangedAsync" />
</HexEntityIndexPage>
```

**Features:**
- Data grid with sorting/filtering
- Add new item button
- Import/Export functionality
- Database synchronization
- Disable/Enable toggle

### Details Page (Add/Edit)

**Routes:**
- `/MyNewModule/Add/MyNewModule` - Add new
- `/MyNewModule/MyNewModule/{Id}` - Edit existing

```razor
@page "/MyNewModule/Add/MyNewModule"
@page "/MyNewModule/MyNewModule/{Id}"

<HexEntityDetailsPage 
    AddTitle="@Labels.AddTitle"
    EditTitle="@Labels.Title"
    ViewModel="_data"
    EntityId="@Id"
    IndexPath="/MyNewModule/MyNewModule"
    ValidationResult="_validationResult"
    OnSave="OnSave"
    OnLoadData="OnLoadDataAsync">
</HexEntityDetailsPage>
```

**Features:**
- Add or edit mode
- Form validation
- Save functionality
- Navigation back to list

## View Models

### MyNewModuleEditViewModel

Edit view model for the details page:

```csharp
public sealed class MyNewModuleEditViewModel : IIdDescription, IEntityViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Comments { get; set; }
    public bool Disabled { get; set; }
    
    public MyNewModuleDetailsViewModel Original { get; }
    
    public bool HasChanges => 
        Id != Original.Id ||
        DescriptionChanged ||
        Disabled != Original.Disabled;
        
    public bool DescriptionChanged => 
        Comments != Original.Comments || 
        Name != Original.Name;

    internal async Task SaveAsync(
        ClaimsPrincipal user, 
        ICommandService commandService, 
        bool create, 
        CancellationToken cancellationToken)
    {
        if (create)
        {
            await commandService.SubmitCommandAsync(user, 
                new AddMyNewModule(Id, Name, Comments), 
                cancellationToken);
        }
        else
        {
            if (DescriptionChanged)
            {
                await commandService.SubmitCommandAsync(user, 
                    new ChangeMyNewModuleDescription(Id, Name, Comments), 
                    cancellationToken);
            }
            // Handle enable/disable changes...
        }
    }
}
```

### MyNewModuleEditValidation

FluentValidation validator:

```csharp
public class MyNewModuleEditValidation : AbstractValidator<MyNewModuleEditViewModel>
{
    public MyNewModuleEditValidation()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("ID is required");
            
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Name is required and must be less than 100 characters");
    }
}
```

## Menu Configuration

**MyNewModulesMenu.cs:**

```csharp
public static class MyNewModulesMenu
{
    public static MenuItem Menu => new MenuItem
    {
        Id = "mynewmodules",
        Name = MyNewModulesMenuResources.MyNewModulesMenuItem,
        Path = "/MyNewModule",
        Icon = Icons.Regular.Size20.Apps,
        Children = [
            new MenuItem
            {
                Id = "mynewmodule",
                Name = MyNewModulesMenuResources.MyNewModuleMenuItem,
                Path = "/MyNewModule/MyNewModule"
            }
        ]
    };
}
```

## Labels and Localization

**Labels.cs:**

```csharp
internal static class Labels
{
    public static string Title => MyNewModuleResources.Title;
    public static string AddTitle => MyNewModuleResources.AddTitle;
    public static string ListTitle => MyNewModuleResources.ListTitle;
}
```

Labels are sourced from localization resources for i18n support.

## Global Imports

**_Imports.razor:**

```razor
@using System.Security.Claims
@using Microsoft.AspNetCore.Authorization
@using Microsoft.AspNetCore.Components.Authorization
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.FluentUI.AspNetCore.Components
@using FluentValidation.Results
@using Hexalith.Application.Commands
@using Hexalith.Application.Modules.Applications
@using Hexalith.Application.Requests
@using Hexalith.MyNewModules
@using Hexalith.MyNewModules.Commands.MyNewModule
@using Hexalith.MyNewModules.Localizations
@using Hexalith.MyNewModules.Requests.MyNewModule
@using Hexalith.MyNewModules.UI.Components.MyNewModule
@using Hexalith.MyNewModules.UI.Pages.MyNewModule
@using Hexalith.UI.Components
@using Hexalith.UI.Components.Pages

@inject NavigationManager NavigationManager
@inject IHexalithApplication Application
@inject ICommandService CommandService
@inject IRequestService RequestService
```

## Dependencies

- `Hexalith.UI.Components` - Base UI components
- `Hexalith.MyNewModules.UI.Components` - Module-specific components
- `Hexalith.MyNewModules.Commands` - Command definitions
- `Hexalith.MyNewModules.Requests` - Request/view model definitions
- `Hexalith.MyNewModules.Localizations` - Localization resources
- `FluentValidation` - Form validation
- `Microsoft.FluentUI.AspNetCore.Components` - Fluent UI

## Page Flow

```
                    ┌────────────────────┐
                    │    Home Page       │
                    │   /MyNewModule     │
                    └────────────────────┘
                             │
                             ▼
                    ┌────────────────────┐
                    │   Index Page       │
                    │ /MyNewModule/      │
                    │   MyNewModule      │
                    └────────────────────┘
                      │             │
          ┌───────────┘             └───────────┐
          ▼                                     ▼
┌──────────────────┐               ┌──────────────────┐
│    Add Page      │               │   Edit Page      │
│ /MyNewModule/    │               │ /MyNewModule/    │
│   Add/MyNewModule│               │ MyNewModule/{Id} │
└──────────────────┘               └──────────────────┘
```

## Best Practices

1. **Use render modes appropriately** - Choose based on interactivity needs
2. **Validate on client and server** - Don't trust client validation alone
3. **Handle loading states** - Show feedback during async operations
4. **Handle errors gracefully** - Display user-friendly error messages
5. **Use localization** - Support multiple languages
6. **Follow naming conventions** - Consistent page and route naming


