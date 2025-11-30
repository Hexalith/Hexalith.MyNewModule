// <copyright file="HexalithMyNewModulesApiServerModule.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModules.ApiServer.Modules;

using System.Collections.Generic;

using Dapr.Actors.Runtime;

using Hexalith.Application.Modules.Modules;
using Hexalith.Application.Services;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.CosmosDb.Configurations;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.MyNewModules;
using Hexalith.MyNewModules.Aggregates;
using Hexalith.MyNewModules.ApiServer.Controllers;
using Hexalith.MyNewModules.Commands.Extensions;
using Hexalith.MyNewModules.Events.Extensions;
using Hexalith.MyNewModules.Helpers;
using Hexalith.MyNewModules.Requests.Extensions;
using Hexalith.MyNewModules.Requests.MyNewModule;
using Hexalith.MyNewModules.Servers.Helpers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// The MyNewModule construction site client module.
/// </summary>
public sealed class HexalithMyNewModulesApiServerModule : IApiServerApplicationModule, IMyNewModuleModule
{
    /// <inheritdoc/>
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => MyNewModuleModulePolicies.AuthorizationPolicies;

    /// <inheritdoc/>
    public IEnumerable<string> Dependencies => [];

    /// <inheritdoc/>
    public string Description => "Hexalith MyNewModule API Server module";

    /// <inheritdoc/>
    public string Id => "Hexalith.MyNewModules.ApiServer";

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

        HexalithMyNewModulesEventsSerialization.RegisterPolymorphicMappers();
        HexalithMyNewModulesCommandsSerialization.RegisterPolymorphicMappers();
        HexalithMyNewModulesRequestsSerialization.RegisterPolymorphicMappers();

        // Add application module
        services.TryAddSingleton<IMyNewModuleModule, HexalithMyNewModulesApiServerModule>();

        // Add command handlers
        _ = services.AddMyNewModule();

        // Add projection handlers and actor factories for event processing
        _ = services.AddMyNewModuleProjectionActorFactories();

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