// <copyright file="MyNewModuleRoles.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.MyNewModules;

/// <summary>
/// Defines the roles for MyNewModule security within the application.
/// </summary>
public static class MyNewModuleRoles
{
    /// <summary>
    /// Role for users who can contribute to MyNewModule.
    /// </summary>
    public const string Contributor = nameof(MyNewModules) + nameof(Contributor);

    /// <summary>
    /// Role for users who own MyNewModule.
    /// </summary>
    public const string Owner = nameof(MyNewModules) + nameof(Owner);

    /// <summary>
    /// Role for users who can read MyNewModule.
    /// </summary>
    public const string Reader = nameof(MyNewModules) + nameof(Reader);
}