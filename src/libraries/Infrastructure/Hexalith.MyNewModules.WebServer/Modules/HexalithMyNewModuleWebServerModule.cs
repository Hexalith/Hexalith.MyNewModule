// <copyright file="HexalithMyNewModuleWebServerModule.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModules.WebServer.Modules;

using System.Collections.Generic;
using System.Reflection;

using Dapr.Actors.Runtime;

using Hexalith.Application.Modules.Modules;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.CosmosDb.Configurations;
using Hexalith.MyNewModules.Commands.Extensions;
using Hexalith.MyNewModules.Events.Extensions;
using Hexalith.MyNewModules.Helpers;
using Hexalith.MyNewModules.Requests.Extensions;
using Hexalith.MyNewModules.Servers.Helpers;
using Hexalith.MyNewModules.UI.Pages.Modules;
using Hexalith.MyNewModules.WebServer.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// The MyNewModules construction site client module.
/// </summary>
public sealed class HexalithMyNewModuleWebServerModule : IWebServerApplicationModule, IMyNewModuleModule
{
    /// <inheritdoc/>
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => MyNewModuleModulePolicies.AuthorizationPolicies;

    /// <inheritdoc/>
    public IEnumerable<string> Dependencies => [];

    /// <inheritdoc/>
    public string Description => "MyNewModules server module";

    /// <inheritdoc/>
    public string Id => "MyNewModules.Server";

    /// <inheritdoc/>
    public string Name => "Hexalith MyNewModules server";

    /// <inheritdoc/>
    public int OrderWeight => 0;

    /// <inheritdoc/>
    public IEnumerable<Assembly> PresentationAssemblies => [
        GetType().Assembly,
        typeof(UI.Components._Imports).Assembly,
        typeof(UI.Pages._Imports).Assembly,
    ];

    /// <inheritdoc/>
    public string Version => "1.0";

    /// <inheritdoc/>
    string IApplicationModule.Path => Path;

    private static string Path => nameof(MyNewModules);

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

        _ = services
            .AddMyNewModuleCommandHandlers()
            .AddMyNewModuleEventValidators()
            .AddMyNewModuleProjectionActorFactories()
            .AddMyNewModuleRequestHandlers()
            .AddMyNewModuleProjections();

        HexalithMyNewModulesEventsSerialization.RegisterPolymorphicMappers();
        HexalithMyNewModulesCommandsSerialization.RegisterPolymorphicMappers();
        HexalithMyNewModulesRequestsSerialization.RegisterPolymorphicMappers();

        // Add application module
        services.TryAddSingleton<IMyNewModuleModule, HexalithMyNewModuleWebServerModule>();

        _ = services.AddTransient(_ => MyNewModulesMenu.Menu);
        _ = services.AddControllers().AddApplicationPart(typeof(MyNewModulesController).Assembly);
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
        if (actorCollection is not ActorRegistrationCollection)
        {
            throw new ArgumentException($"{nameof(RegisterActors)} parameter must be an {nameof(ActorRegistrationCollection)}. Actual type : {actorCollection.GetType().Name}.", nameof(actorCollection));
        }
    }

    /// <inheritdoc/>
    public void UseModule(object application)
    {
    }

    /// <inheritdoc/>
    public void UseSecurity(object application)
    {
    }
}