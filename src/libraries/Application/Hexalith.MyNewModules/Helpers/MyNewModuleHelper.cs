// <copyright file="MyNewModuleHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModules.Helpers;

using Hexalith.MyNewModules.CommandHandlers;
using Hexalith.MyNewModules.EventHandlers;
using Hexalith.MyNewModules.Projections.Helpers;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class for adding mynewmodule projections to the service collection.
/// </summary>
public static class MyNewModuleHelper
{
    /// <summary>
    /// Adds the mynewmodule module to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyNewModule(this IServiceCollection services)
    {
        _ = services.AddMyNewModuleCommandHandlers();
        _ = services.AddMyNewModuleAggregateProviders();
        _ = services.AddMyNewModuleEventValidators();
        return services;
    }
}