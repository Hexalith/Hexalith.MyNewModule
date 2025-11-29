// <copyright file="DocumentMenu.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.UI.Pages.Modules;

using Hexalith.MyNewModule.Application;
using Hexalith.UI.Components;
using Hexalith.UI.Components.Icons;

using Labels = Resources.DocumentMenu;

/// <summary>
/// Represents the Document menu.
/// </summary>
public static class MyNewModuleMenu
{
    /// <summary>
    /// Gets the menu information.
    /// </summary>
    public static MenuItemInformation Menu => new(
                    Labels.DocumentMenuItem,
                    string.Empty,
                    new IconInformation("DocumentDatabase", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                    true,
                    10,
                    DocumentRoles.Reader,
                    [
                        new MenuItemInformation(
                            Labels.DocumentMenuItem,
                            "MyNewModule",
                            new IconInformation("DocumentBulletListMultiple", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                            false,
                            30,
                            DocumentRoles.Reader,
                            []),
                        new MenuItemInformation(
                            Labels.UploadMenuItem,
                            "MyNewModule/Upload/Document",
                            new IconInformation("ArrowUpload", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                            false,
                            20,
                            DocumentRoles.Contributor,
                            []),
                        new MenuItemInformation(
                            Labels.SetupMenuItem,
                            null,
                            new IconInformation("AppsSettings", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                            false,
                            10,
                            DocumentRoles.Owner,
                            [
                                new MenuItemInformation(
                                    Labels.DocumentMenuItem,
                                    "MyNewModule/Document",
                                    new IconInformation("Document", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                                    false,
                                    110,
                                    DocumentRoles.Owner,
                                    []),
                                new MenuItemInformation(
                                    Labels.DataManagementMenuItem,
                                    "MyNewModule/DataManagement",
                                    new IconInformation("DatabaseMultiple", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                                    false,
                                    100,
                                    DocumentRoles.Owner,
                                    []),
                                new MenuItemInformation(
                                    Labels.MyNewModuletorageMenuItem,
                                    "MyNewModule/MyNewModuletorage",
                                    new IconInformation("DocumentBulletListCube", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                                    false,
                                    90,
                                    DocumentRoles.Owner,
                                    []),
                                new MenuItemInformation(
                                    Labels.DocumentTypeMenuItem,
                                    "MyNewModule/DocumentType",
                                    new IconInformation("BookQuestionMarkRtl", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                                    false,
                                    80,
                                    DocumentRoles.Owner,
                                    []),
                                new MenuItemInformation(
                                    Labels.FileTypeMenuItem,
                                    "MyNewModule/FileType",
                                    new IconInformation("DocumentData", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                                    false,
                                    70,
                                    DocumentRoles.Owner,
                                    []),
                                new MenuItemInformation(
                                    Labels.FileTextExtractionTypeMenuItem,
                                    "MyNewModule/DocumentInformationExtraction",
                                    new IconInformation("ScanType", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                                    false,
                                    60,
                                    DocumentRoles.Owner,
                                    []),
                                new MenuItemInformation(
                                    Labels.DocumentContainerTypeMenuItem,
                                    "MyNewModule/DocumentContainer",
                                    new IconInformation("DocumentFolder", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                                    false,
                                    50,
                                    DocumentRoles.Owner,
                                    []),
                                ]),
                    ]);

    private static string IconLibraryName
        => typeof(MyNewModuleMenu).Assembly?.FullName
            ?? throw new InvalidOperationException("Menu Assembly not found");
}