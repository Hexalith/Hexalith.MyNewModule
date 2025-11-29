// <copyright file="HexalithMyNewModulesWebServerApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace HexalithApp.WebServer;

using System;
using System.Collections.Generic;

using Hexalith.Application.Modules.Applications;
using Hexalith.MyNewModules;
using Hexalith.MyNewModules.WebServer.Modules;
using Hexalith.UI.WebServer;

using HexalithApp.WebApp;

/// <summary>
/// Represents a server application.
/// </summary>
public class HexalithMyNewModulesWebServerApplication : HexalithWebServerApplication
{
    /// <inheritdoc/>
    public override string Id => $"{HexalithMyNewModulesInformation.Id}.{ApplicationType}";

    /// <inheritdoc/>
    public override string Name => $"{HexalithMyNewModulesInformation.Name} {ApplicationType}";

    /// <inheritdoc/>
    public override string ShortName => HexalithMyNewModulesInformation.ShortName;

    /// <inheritdoc/>
    public override Type WebAppApplicationType => typeof(HexalithMyNewModulesWebAppApplication);

    /// <inheritdoc/>
    public override IEnumerable<Type> WebServerModules => [
        typeof(HexalithUIComponentsWebServerModule),
        typeof(HexalithMyNewModuleWebServerModule)];
}