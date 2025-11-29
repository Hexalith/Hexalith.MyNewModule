// <copyright file="HexalithMyNewModuleWebAppModule.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.MyNewModule.WebApp.Modules;

using System.Collections.Generic;
using System.Reflection;

using Hexalith.Application.Modules.Modules;
using Hexalith.MyNewModule.Abstractions;
using Hexalith.MyNewModule.Commands.Extensions;
using Hexalith.MyNewModule.Events.Extensions;
using Hexalith.MyNewModule.Helpers;
using Hexalith.MyNewModule.Requests.Extensions;
using Hexalith.MyNewModule.UI.Pages.Modules;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// The MyNewModule construction site client module.
/// </summary>
public class HexalithMyNewModuleWebAppModule : IWebAppApplicationModule, IMyNewModuleModule
{
    /// <inheritdoc/>
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => MyNewModuleModulePolicies.AuthorizationPolicies;

    /// <inheritdoc/>
    public IEnumerable<string> Dependencies => [];

    /// <inheritdoc/>
    public string Description => "MyNewModule client module";

    /// <inheritdoc/>
    public string Id => "Hexalith.MyNewModule.Client";

    /// <inheritdoc/>
    public string Name => "MyNewModule client";

    /// <inheritdoc/>
    public int OrderWeight => 0;

    /// <inheritdoc/>
    public string Path => nameof(MyNewModule);

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
        HexalithMyNewModuleEventsSerialization.RegisterPolymorphicMappers();
        HexalithMyNewModuleCommandsSerialization.RegisterPolymorphicMappers();
        HexalithMyNewModuleRequestsSerialization.RegisterPolymorphicMappers();

        // Add application module
        services.TryAddSingleton<IMyNewModuleModule, HexalithMyNewModuleWebAppModule>();

        _ = services
            .AddMyNewModuleQueryServices()
            .AddTransient(_ => MyNewModuleMenu.Menu);
    }

    /// <inheritdoc/>
    public void UseModule(object application)
    {
    }
}