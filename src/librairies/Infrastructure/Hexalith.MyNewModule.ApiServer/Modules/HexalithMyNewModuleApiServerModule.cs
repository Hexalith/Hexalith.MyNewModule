// <copyright file="HexalithMyNewModuleApiServerModule.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.MyNewModule.ApiServer.Modules;

using System.Collections.Generic;

using Dapr.Actors.Runtime;

using Hexalith.Application.Modules.Modules;
using Hexalith.Application.Services;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.CosmosDb.Configurations;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.MyNewModule.Abstractions;
using Hexalith.MyNewModule.Aggregates;
using Hexalith.MyNewModule.ApiServer.Controllers;
using Hexalith.MyNewModule.Commands.Extensions;
using Hexalith.MyNewModule.Events.Extensions;
using Hexalith.MyNewModule.Helpers;
using Hexalith.MyNewModule.Requests.Extensions;
using Hexalith.MyNewModule.Requests.MyNewModule;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// The MyNewModule construction site client module.
/// </summary>
public sealed class HexalithMyNewModuleApiServerModule : IApiServerApplicationModule, IMyNewModuleModule
{
    /// <inheritdoc/>
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => MyNewModuleModulePolicies.AuthorizationPolicies;

    /// <inheritdoc/>
    public IEnumerable<string> Dependencies => [];

    /// <inheritdoc/>
    public string Description => "Hexalith MyNewModule API Server module";

    /// <inheritdoc/>
    public string Id => "Hexalith.MyNewModule.ApiServer";

    /// <inheritdoc/>
    public string Name => "Hexalith MyNewModule API Server";

    /// <inheritdoc/>
    public int OrderWeight => 0;

    /// <inheritdoc/>
    public string Version => "1.0";

    /// <inheritdoc/>
    string IApplicationModule.Path => Path;

    private static string Path => nameof(MyNewModule);

    /// <summary>
    /// Adds services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        _ = services
            .ConfigureSettings<CosmosDbSettings>(configuration);

        HexalithMyNewModuleEventsSerialization.RegisterPolymorphicMappers();
        HexalithMyNewModuleCommandsSerialization.RegisterPolymorphicMappers();
        HexalithMyNewModuleRequestsSerialization.RegisterPolymorphicMappers();

        // Add application module
        services.TryAddSingleton<IMyNewModuleModule, HexalithMyNewModuleApiServerModule>();

        // Add command handlers
        _ = services.AddMyNewModule();

        _ = services
         .AddControllers()
         .AddApplicationPart(typeof(MyNewModuleIntegrationEventsController).Assembly);
    }

    /// <summary>
    /// Registers the actors associated with the module.
    /// </summary>
    /// <param name="actorCollection">The actor collection.</param>
    /// <exception cref="ArgumentNullException">Thrown when actorCollection is null.</exception>
    /// <exception cref="ArgumentException">Thrown when actorCollection is not an ActorRegistrationCollection.</exception>
    public static void RegisterActors(object actorCollection)
    {
        ArgumentNullException.ThrowIfNull(actorCollection);
        if (actorCollection is not ActorRegistrationCollection actorRegistrations)
        {
            throw new ArgumentException($"{nameof(RegisterActors)} parameter must be an {nameof(ActorRegistrationCollection)}. Actual type : {actorCollection.GetType().Name}.", nameof(actorCollection));
        }

        actorRegistrations.RegisterActor<DomainAggregateActor>(MyNewModuleDomainHelper.MyNewModuleAggregateName.ToAggregateActorName());
        actorRegistrations.RegisterProjectionActor<MyNewModule>();
        actorRegistrations.RegisterProjectionActor<MyNewModuleSummaryViewModel>();
        actorRegistrations.RegisterProjectionActor<MyNewModuleDetailsViewModel>();
        actorRegistrations.RegisterActor<SequentialStringListActor>(IIdCollectionFactory.GetAggregateCollectionName(MyNewModuleDomainHelper.MyNewModuleAggregateName));
    }

    /// <inheritdoc/>
    public void UseModule(object application)
    {
    }
}