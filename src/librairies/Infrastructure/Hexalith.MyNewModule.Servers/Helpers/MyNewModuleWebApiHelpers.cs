// <copyright file="MyNewModuleWebApiHelpers.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.MyNewModule.Servers.Helpers;

using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.MyNewModule.Aggregates;
using Hexalith.MyNewModule.Helpers;
using Hexalith.MyNewModule.Requests.MyNewModule;

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