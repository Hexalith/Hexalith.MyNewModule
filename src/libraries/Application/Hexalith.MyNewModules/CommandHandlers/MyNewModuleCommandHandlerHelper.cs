// <copyright file="MyNewModuleCommandHandlerHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModules.CommandHandlers;

using Hexalith.Application.Commands;
using Hexalith.MyNewModules.Aggregates;
using Hexalith.MyNewModules.Commands.MyNewModule;
using Hexalith.MyNewModules.Events.MyNewModules;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class for adding mynewmodule command handlers to the service collection.
/// </summary>
public static class MyNewModuleCommandHandlerHelper
{
    /// <summary>
    /// Adds the mynewmodule command handlers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyNewModuleCommandHandlers(this IServiceCollection services)
    {
        _ = services.TryAddSimpleInitializationCommandHandler<AddMyNewModule>(
            c => new MyNewModuleAdded(
                c.Id,
                c.Name,
                c.Comments),
            ev => new MyNewModule((MyNewModuleAdded)ev));
        return services;
    }
}