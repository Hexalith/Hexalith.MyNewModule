// <copyright file="MyNewModuleProjectionHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Hexalith.MyNewModules.Helpers;

using FluentValidation;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Commands;
using Hexalith.Application.Projections;
using Hexalith.Application.Requests;
using Hexalith.Domain.Events;
using Hexalith.MyNewModules.Aggregates;
using Hexalith.MyNewModules.Commands.MyNewModule;
using Hexalith.MyNewModules.Events.MyNewModules;
using Hexalith.MyNewModules.ProjectionHandlers.Details;
using Hexalith.MyNewModules.ProjectionHandlers.Summaries;
using Hexalith.MyNewModules.RequestHandlers;
using Hexalith.MyNewModules.Requests.MyNewModule;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Helper class for adding mynewmodule projections to the service collection.
/// </summary>
public static class MyNewModuleProjectionHelper
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

    /// <summary>
    /// Adds the mynewmodule event validators to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyNewModuleEventValidators(this IServiceCollection services)
            => services.AddTransient<IValidator<AddMyNewModule>, AddMyNewModuleValidator>();

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