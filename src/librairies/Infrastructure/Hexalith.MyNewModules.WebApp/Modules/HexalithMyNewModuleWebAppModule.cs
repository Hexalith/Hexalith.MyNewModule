// <copyright file="HexalithMyNewModuleWebAppModule.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.MyNewModules.WebApp.Modules;

using System.Collections.Generic;
using System.Reflection;

using Hexalith.Application.Modules.Modules;
using Hexalith.MyNewModules;
using Hexalith.MyNewModules.Commands.Extensions;
using Hexalith.MyNewModules.Events.Extensions;
using Hexalith.MyNewModules.Helpers;
using Hexalith.MyNewModules.Requests.Extensions;
using Hexalith.MyNewModules.UI.Pages.Modules;

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
    public string Id => "Hexalith.MyNewModules.Client";

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