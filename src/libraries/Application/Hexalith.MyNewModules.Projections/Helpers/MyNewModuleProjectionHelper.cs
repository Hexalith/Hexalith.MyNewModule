// <copyright file="MyNewModuleProjectionHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModules.Projections.Helpers;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Projections;
using Hexalith.Application.Requests;
using Hexalith.Domain.Events;
using Hexalith.MyNewModules.Aggregates;
using Hexalith.MyNewModules.Events.MyNewModules;
using Hexalith.MyNewModules.Projections.ProjectionHandlers.Details;
using Hexalith.MyNewModules.Projections.ProjectionHandlers.Summaries;
using Hexalith.MyNewModules.Projections.RequestHandlers;
using Hexalith.MyNewModules.Requests.MyNewModule;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Helper class for adding mynewmodule projections to the service collection.
/// </summary>
public static class MyNewModuleProjectionHelper
{
    /// <summary>
    /// Adds the mynewmodule aggregate providers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyNewModuleAggregateProviders(this IServiceCollection services)
    {
        _ = services
            .AddSingleton<IDomainAggregateProvider, DomainAggregateProvider<MyNewModule>>();
        return services;
    }

    /// <summary>
    /// Adds the mynewmodule projections to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyNewModuleProjectionHandlers(this IServiceCollection services)
        => services

            // Collection projections
            .AddScoped<IProjectionUpdateHandler<MyNewModuleAdded>, IdsCollectionProjectionHandler<MyNewModuleAdded>>()

            // Summary projections
            .AddScoped<IProjectionUpdateHandler<MyNewModuleAdded>, MyNewModuleAddedOnSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<MyNewModuleDescriptionChanged>, MyNewModuleDescriptionChangedOnSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<MyNewModuleDisabled>, MyNewModuleDisabledOnSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<MyNewModuleEnabled>, MyNewModuleEnabledOnSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<SnapshotEvent>, MyNewModuleSnapshotOnSummaryProjectionHandler>()

            // Details
            .AddScoped<IProjectionUpdateHandler<MyNewModuleAdded>, MyNewModuleAddedOnDetailsProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<MyNewModuleDescriptionChanged>, MyNewModuleDescriptionChangedOnDetailsProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<MyNewModuleDisabled>, MyNewModuleDisabledOnDetailsProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<MyNewModuleEnabled>, MyNewModuleEnabledOnDetailsProjectionHandler>();

    /// <summary>
    /// Adds the mynewmodule projections and request handlers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyNewModuleProjections(this IServiceCollection services)
        => services
        .AddMyNewModuleProjectionHandlers()
        .AddMyNewModuleRequestHandlers();

    /// <summary>
    /// Adds the mynewmodule query services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyNewModuleQueryServices(this IServiceCollection services)
    => services;

    /// <summary>
    /// Adds the mynewmodule request handlers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyNewModuleRequestHandlers(this IServiceCollection services)
    {
        services.TryAddScoped<IRequestHandler<GetMyNewModuleDetails>, GetMyNewModuleDetailsHandler>();
        services.TryAddScoped<IRequestHandler<GetMyNewModuleSummaries>, GetFilteredCollectionHandler<GetMyNewModuleSummaries, MyNewModuleSummaryViewModel>>();
        services.TryAddScoped<IRequestHandler<GetMyNewModuleIds>, GetAggregateIdsRequestHandler<GetMyNewModuleIds>>();
        services.TryAddScoped<IRequestHandler<GetMyNewModuleExports>, GetExportsRequestHandler<GetMyNewModuleExports, MyNewModule>>();
        return services;
    }
}