// <copyright file="MyNewModuleHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Helpers;

using Hexalith.MyNewModule.CommandHandlers;
using Hexalith.MyNewModule.EventHandlers;
using Hexalith.MyNewModule.Projections.Helpers;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class for adding MyNewModule projections to the service collection.
/// </summary>
public static class MyNewModuleHelper
{
    /// <summary>
    /// Adds the MyNewModule module to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyNewModule(this IServiceCollection services)
    {
        _ = services.AddMyToDoCommandHandlers();
        _ = services.AddMyToDoAggregateProviders();
        _ = services.AddMyToDoValidators();
        return services;
    }
}