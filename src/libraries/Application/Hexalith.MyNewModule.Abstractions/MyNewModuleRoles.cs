// <copyright file="MyNewModuleRoles.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule;

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