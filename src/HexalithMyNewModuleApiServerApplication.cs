// <copyright file="HexalithMyNewModuleApiServerApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace HexalithApp.ApiServer;

using System;
using System.Collections.Generic;

using Hexalith.Application.Modules.Applications;
using Hexalith.MyNewModules.ApiServer.Modules;
using Hexalith.MyNewModules.Application;
using Hexalith.Security.ApiServer;
using Hexalith.UI.ApiServer;

/// <summary>
/// Represents a server application.
/// </summary>
public class HexalithMyNewModuleApiServerApplication : HexalithApiServerApplication
{
    /// <inheritdoc/>
    public override IEnumerable<Type> ApiServerModules => [
        typeof(HexalithUIComponentsApiServerModule),
        typeof(HexalithMyNewModuleApiServerModule),
        typeof(HexalithSecurityApiServerModule)];

    /// <inheritdoc/>
    public override string Id => $"{HexalithMyNewModuleApplicationInformation.Id}.{ApplicationType}";

    /// <inheritdoc/>
    public override string Name => $"{HexalithMyNewModuleApplicationInformation.Name} {ApplicationType}";

    /// <inheritdoc/>
    public override string ShortName => HexalithMyNewModuleApplicationInformation.ShortName;
}