// <copyright file="HexalithDocumentsWebServerModule.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.WebServer.Modules;

using System.Collections.Generic;
using System.Reflection;

using Dapr.Actors.Runtime;

using Hexalith.Application.Modules.Modules;
using Hexalith.Documents.Abstractions.Extensions;
using Hexalith.Documents.Application.Documents;
using Hexalith.Documents.Application.Helpers;
using Hexalith.Documents.Commands.Extensions;
using Hexalith.Documents.Projections.Helpers;
using Hexalith.Documents.Requests.Extensions;
using Hexalith.Documents.Servers.Helpers;
using Hexalith.Documents.UI.Pages.Modules;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.CosmosDb.Configurations;
using Hexalith.MyNewModule.WebServer.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// The document construction site client module.
/// </summary>
public sealed class HexalithDocumentsWebServerModule : IWebServerApplicationModule, IDocumentModule
{
    /// <inheritdoc/>
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => DocumentModulePolicies.AuthorizationPolicies;

    /// <inheritdoc/>
    public IEnumerable<string> Dependencies => [];

    /// <inheritdoc/>
    public string Description => "Document server module";

    /// <inheritdoc/>
    public string Id => "Document.Server";

    /// <inheritdoc/>
    public string Name => "Hexalith Document server";

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

    private static string Path => nameof(Documents);

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
            .AddDocumentStorage()
            .AddDocumentsCommandHandlers()
            .AddDocumentEventValidators()
            .AddDocumentsProjectionActorFactories()
            .AddDocumentsRequestHandlers()
            .AddDocumentProjections();

        HexalithDocumentsAbstractionsSerialization.RegisterPolymorphicMappers();
        HexalithDocumentsCommandsSerialization.RegisterPolymorphicMappers();
        HexalithDocumentsRequestsSerialization.RegisterPolymorphicMappers();

        // Add application module
        services.TryAddSingleton<IDocumentModule, HexalithDocumentsWebServerModule>();

        _ = services.AddTransient(_ => DocumentMenu.Menu);
        _ = services.AddControllers().AddApplicationPart(typeof(DocumentFilesController).Assembly);
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