// <copyright file="MyNewModuleModulePolicies.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModules.Helpers;

using System.Collections.Generic;

using Hexalith.Application;
using Hexalith.MyNewModules;

using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Provides authorization policies for the MyNewModule module.
/// </summary>
public static class MyNewModuleModulePolicies
{
    /// <summary>
    /// Gets the authorization policies for the MyNewModule module.
    /// </summary>
    public static IDictionary<string, AuthorizationPolicy> AuthorizationPolicies =>
    new Dictionary<string, AuthorizationPolicy>
    {
        {
            MyNewModulePolicies.Owner, new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(ApplicationRoles.GlobalAdministrator, MyNewModuleRoles.Owner)
                .Build()
        },
        {
            MyNewModulePolicies.Contributor, new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(ApplicationRoles.GlobalAdministrator, MyNewModuleRoles.Owner, MyNewModuleRoles.Contributor)
                .Build()
        },
        {
            MyNewModulePolicies.Reader, new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(ApplicationRoles.GlobalAdministrator, MyNewModuleRoles.Owner, MyNewModuleRoles.Contributor, MyNewModuleRoles.Reader)
                .Build()
        },
    };

    /// <summary>
    /// Adds the MyNewModule module policies to the specified authorization options.
    /// </summary>
    /// <param name="options">The authorization options to add the policies to.</param>
    /// <returns>The updated authorization options.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the options parameter is null.</exception>
    public static AuthorizationOptions AddMyNewModuleAuthorizationPolicies(this AuthorizationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        foreach (KeyValuePair<string, AuthorizationPolicy> policy in AuthorizationPolicies)
        {
            options.AddPolicy(policy.Key, policy.Value);
        }

        return options;
    }
}