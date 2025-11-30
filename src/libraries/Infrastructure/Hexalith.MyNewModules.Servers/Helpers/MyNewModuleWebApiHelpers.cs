// <copyright file="MyNewModuleWebApiHelpers.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModules.Servers.Helpers;

using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.MyNewModules.Aggregates;
using Hexalith.MyNewModules.Projections.Helpers;
using Hexalith.MyNewModules.Requests.MyNewModule;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class MyNewModuleWebApiHelpers.
/// </summary>
public static class MyNewModuleWebApiHelpers
{
    /// <summary>
    /// Adds the MyNewModule projection actor factories.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
    public static IServiceCollection AddMyNewModuleProjectionActorFactories(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        _ = services.AddMyNewModuleProjections();
        _ = services.AddActorProjectionFactory<MyNewModuleSummaryViewModel>();
        _ = services.AddActorProjectionFactory<MyNewModuleDetailsViewModel>();
        _ = services.AddActorProjectionFactory<MyNewModule>();
        return services;
    }
}