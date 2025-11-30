// <copyright file="HexalithMyNewModulesWebAppModule.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModules.WebApp.Modules;

using System.Collections.Generic;
using System.Reflection;

using Hexalith.Application.Modules.Modules;
using Hexalith.MyNewModules;
using Hexalith.MyNewModules.Commands.Extensions;
using Hexalith.MyNewModules.Events.Extensions;
using Hexalith.MyNewModules.Helpers;
using Hexalith.MyNewModules.Projections.Helpers;
using Hexalith.MyNewModules.Requests.Extensions;
using Hexalith.MyNewModules.UI.Pages.Modules;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// The MyNewModule construction site client module.
/// </summary>
public class HexalithMyNewModulesWebAppModule : IWebAppApplicationModule, IMyNewModuleModule
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
    public string Path => nameof(MyNewModules);

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
        HexalithMyNewModulesEventsSerialization.RegisterPolymorphicMappers();
        HexalithMyNewModulesCommandsSerialization.RegisterPolymorphicMappers();
        HexalithMyNewModulesRequestsSerialization.RegisterPolymorphicMappers();

        // Add application module
        services.TryAddSingleton<IMyNewModuleModule, HexalithMyNewModulesWebAppModule>();

        _ = services
            .AddMyNewModuleQueryServices()
            .AddTransient(_ => MyNewModulesMenu.Menu);
    }

    /// <inheritdoc/>
    public void UseModule(object application)
    {
    }
}