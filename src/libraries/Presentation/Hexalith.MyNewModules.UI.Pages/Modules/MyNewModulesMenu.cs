// <copyright file="MyNewModulesMenu.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModules.UI.Pages.Modules;

using Hexalith.MyNewModules;

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
                    new IconInformation("Apps", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                    true,
                    10,
                    MyNewModuleRoles.Reader,
                    [
                        new MenuItemInformation(
                            Labels.MyNewModuleMenuItem,
                            "MyNewModule/MyNewModule",
                            new IconInformation("AppsAddIn", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                            false,
                            30,
                            MyNewModuleRoles.Reader,
                            []),
                    ]);

    private static string IconLibraryName
        => typeof(MyNewModulesMenu).Assembly?.FullName
            ?? throw new InvalidOperationException("Menu Assembly not found");
}