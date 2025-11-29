// <copyright file="MyNewModulesMenu.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.MyNewModules.UI.Pages.Modules;

using Hexalith.UI.Components;
using Hexalith.UI.Components.Icons;

using Labels = Localizations.MyNewModulesMenu;

/// <summary>
/// Represents the MyNewModule menu.
/// </summary>
public static class MyNewModulesMenu
{
    /// <summary>
    /// Gets the menu information.
    /// </summary>
    public static MenuItemInformation Menu => new(
                    Labels.MyNewModulesMenuItem,
                    string.Empty,
                    new IconInformation("MyNewModuleDatabase", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                    true,
                    10,
                    MyNewModuleRoles.Reader,
                    [
                        new MenuItemInformation(
                            Labels.MyNewModuleMenuItem,
                            "MyNewModule",
                            new IconInformation("MyNewModuleBulletListMultiple", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                            false,
                            30,
                            MyNewModuleRoles.Reader,
                            []),
                    ]);

    private static string IconLibraryName
        => typeof(MyNewModulesMenu).Assembly?.FullName
            ?? throw new InvalidOperationException("Menu Assembly not found");
}