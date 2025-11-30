# Hexalith.MyNewModules.Localizations

This project contains localization resource files for the MyNewModule bounded context.

## Overview

The Localizations project provides:
- Resource files for multiple languages
- Strongly-typed resource accessors
- Labels and messages for UI
- Menu item labels

## Directory Structure

```
Hexalith.MyNewModules.Localizations/
├── MyNewModule.resx                 # English (default)
├── MyNewModule.fr.resx              # French
├── MyNewModule.Designer.cs          # Auto-generated accessor
├── MyNewModulesMenu.resx            # Menu labels (English)
├── MyNewModulesMenu.fr.resx         # Menu labels (French)
├── MyNewModulesMenu.Designer.cs     # Auto-generated accessor
└── Hexalith.MyNewModules.Localizations.csproj
```

## Resource Files

### MyNewModule Resources

Labels and messages for the module:

| Key | English | French |
|-----|---------|--------|
| `Title` | Module | Module |
| `AddTitle` | Add Module | Ajouter un Module |
| `ListTitle` | Modules | Modules |
| `IdLabel` | ID | Identifiant |
| `NameLabel` | Name | Nom |
| `CommentsLabel` | Comments | Commentaires |
| `DisabledLabel` | Disabled | Désactivé |

### MyNewModulesMenu Resources

Menu labels:

| Key | English | French |
|-----|---------|--------|
| `MyNewModulesMenuItem` | New Module | Nouveau Module |
| `MyNewModuleMenuItem` | Module | Module |

## Generated Accessors

### MyNewModule Class

```csharp
public class MyNewModule
{
    public static string Title => ResourceManager.GetString("Title", Culture);
    public static string AddTitle => ResourceManager.GetString("AddTitle", Culture);
    public static string ListTitle => ResourceManager.GetString("ListTitle", Culture);
    // ... more properties
}
```

### MyNewModulesMenu Class

```csharp
public class MyNewModulesMenu
{
    public static string MyNewModulesMenuItem => 
        ResourceManager.GetString("MyNewModulesMenuItem", Culture);
    public static string MyNewModuleMenuItem => 
        ResourceManager.GetString("MyNewModuleMenuItem", Culture);
}
```

## Adding New Resources

### 1. Add to Default Resource File

Edit `MyNewModule.resx`:

```xml
<data name="NewLabel" xml:space="preserve">
  <value>New Value</value>
</data>
```

### 2. Add Translations

Edit `MyNewModule.fr.resx`:

```xml
<data name="NewLabel" xml:space="preserve">
  <value>Nouvelle Valeur</value>
</data>
```

### 3. Regenerate Designer File

The designer file is auto-generated when you save the `.resx` file in Visual Studio.

## Adding New Languages

1. Copy the default `.resx` file
2. Rename with culture code: `MyNewModule.{culture}.resx`
3. Translate all values
4. Rebuild the project

**Culture codes:**
- `fr` - French
- `de` - German
- `es` - Spanish
- `it` - Italian
- `pt` - Portuguese
- `ja` - Japanese
- `zh` - Chinese

## Usage in Code

### In C# Code

```csharp
using Hexalith.MyNewModules.Localizations;

var title = MyNewModule.Title;
var menuItem = MyNewModulesMenu.MyNewModulesMenuItem;
```

### In Blazor Pages

```razor
@using Hexalith.MyNewModules.Localizations

<FluentLabel>@MyNewModule.Title</FluentLabel>
```

### In Labels Class

```csharp
internal static class Labels
{
    public static string Title => MyNewModule.Title;
    public static string AddTitle => MyNewModule.AddTitle;
    public static string ListTitle => MyNewModule.ListTitle;
}
```

## Setting Culture

### In Blazor Server

```csharp
// Startup.cs
services.AddLocalization(options => options.ResourcesPath = "Resources");
services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "fr" };
    options.SetDefaultCulture("en")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
});
```

### In Components

```razor
@inject IStringLocalizer<MyNewModule> Localizer

<FluentLabel>@Localizer["Title"]</FluentLabel>
```

## Best Practices

1. **Use meaningful keys** - Descriptive and consistent
2. **Keep values short** - UI space is limited
3. **Provide context** - Add comments for translators
4. **Test all cultures** - Verify translations display correctly
5. **Handle missing** - Provide fallback to default culture
6. **Format strings** - Use placeholders: `"Hello, {0}!"`
