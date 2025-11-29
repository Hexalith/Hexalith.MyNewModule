// <copyright file="MyNewModulePolicies.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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