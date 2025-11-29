// <copyright file="MyNewModulePolicies.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModules;

/// <summary>
/// Defines the policies for MyNewModule security within the application.
/// </summary>
public static class MyNewModulePolicies
{
    /// <summary>
    /// Policy for users who can contribute to MyNewModule.
    /// </summary>
    public const string Contributor = MyNewModuleRoles.Contributor;

    /// <summary>
    /// Policy for users who own MyNewModule.
    /// </summary>
    public const string Owner = MyNewModuleRoles.Owner;

    /// <summary>
    /// Policy for users who can read MyNewModule.
    /// </summary>
    public const string Reader = MyNewModuleRoles.Reader;
}