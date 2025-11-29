// <copyright file="MyNewModuleRoles.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.MyNewModule.Abstractions;

/// <summary>
/// Defines the roles for MyNewModule security within the application.
/// </summary>
public static class MyNewModuleRoles
{
    /// <summary>
    /// Role for users who can contribute to MyNewModule.
    /// </summary>
    public const string Contributor = nameof(MyNewModule) + nameof(Contributor);

    /// <summary>
    /// Role for users who own MyNewModule.
    /// </summary>
    public const string Owner = nameof(MyNewModule) + nameof(Owner);

    /// <summary>
    /// Role for users who can read MyNewModule.
    /// </summary>
    public const string Reader = nameof(MyNewModule) + nameof(Reader);
}