// <copyright file="MyNewModuleProjectionHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Hexalith.MyNewModule.Helpers;

using Hexalith.Application.Projections;
using Hexalith.Application.Requests;
using Hexalith.Domain.Events;
using Hexalith.MyNewModule.Aggregates;
using Hexalith.MyNewModule.Events.MyNewModules;
using Hexalith.MyNewModule.ProjectionHandlers.Details;
using Hexalith.MyNewModule.ProjectionHandlers.Summaries;
using Hexalith.MyNewModule.RequestHandlers;
using Hexalith.MyNewModule.Requests.MyNewModule;

using Manhole.Requests.MyNewModules;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Helper class for adding mynewmodule projections to the service collection.
/// </summary>
public static class MyNewModuleProjectionHelper
{
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