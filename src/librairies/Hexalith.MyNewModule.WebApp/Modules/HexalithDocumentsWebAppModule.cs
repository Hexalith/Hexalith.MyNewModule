// <copyright file="HexalithDocumentsWebAppModule.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.WebApp.Modules;

using System.Collections.Generic;
using System.Reflection;

using Hexalith.Application.Modules.Modules;
using Hexalith.Documents.Abstractions.Extensions;
using Hexalith.Documents.Application.Documents;
using Hexalith.Documents.Application.Helpers;
using Hexalith.Documents.Commands.Extensions;
using Hexalith.Documents.Projections.Helpers;
using Hexalith.Documents.Requests.Extensions;
using Hexalith.Documents.UI.Pages.Modules;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// The document construction site client module.
/// </summary>
public class HexalithDocumentsWebAppModule : IWebAppApplicationModule, IDocumentModule
{
    /// <inheritdoc/>
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => DocumentModulePolicies.AuthorizationPolicies;

    /// <inheritdoc/>
    public IEnumerable<string> Dependencies => [];

    /// <inheritdoc/>
    public string Description => "Document client module";

    /// <inheritdoc/>
    public string Id => "Hexalith.Document.Client";

    /// <inheritdoc/>
    public string Name => "Document client";

    /// <inheritdoc/>
    public int OrderWeight => 0;

    /// <inheritdoc/>
    public string Path => nameof(Documents);

    /// <inheritdoc/>
    public IEnumerable<Assembly> PresentationAssemblies => [
        GetType().Assembly,
        typeof(UI.Components._Imports).Assembly,
        typeof(UI.Pages._Imports).Assembly,
    ];

    /// <inheritdoc/>
    public string Version => "1.0";

    /// <summary>
    /// Adds services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void AddServices(IServiceCollection services)
    {
        HexalithDocumentsAbstractionsSerialization.RegisterPolymorphicMappers();
        HexalithDocumentsCommandsSerialization.RegisterPolymorphicMappers();
        HexalithDocumentsRequestsSerialization.RegisterPolymorphicMappers();

        // Add application module
        services.TryAddSingleton<IDocumentModule, HexalithDocumentsWebAppModule>();

        _ = services
            .AddDocumentsQueryServices()
            .AddTransient(_ => DocumentMenu.Menu);
    }

    /// <inheritdoc/>
    public void UseModule(object application)
    {
    }
}