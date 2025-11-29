// <copyright file="MyNewModuleDisabledOnDetailsProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.ProjectionHandlers.Details;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Projections;
using Hexalith.MyNewModule.Events.MyNewModules;
using Hexalith.MyNewModule.Requests.MyNewModule;

/// <summary>
/// Handles the projection update when a warehouse is disabled.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MyNewModuleDisabledOnDetailsProjectionHandler"/> class.
/// </remarks>
/// <param name="factory">The projection factory.</param>
public class MyNewModuleDisabledOnDetailsProjectionHandler(IProjectionFactory<MyNewModuleDetailsViewModel> factory)
    : MyNewModuleDetailsProjectionHandler<MyNewModuleDisabled>(factory)
{
    /// <inheritdoc/>
    protected override Task<MyNewModuleDetailsViewModel?> ApplyEventAsync([NotNull] MyNewModuleDisabled baseEvent, MyNewModuleDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return model?.Disabled == false
            ? Task.FromResult<MyNewModuleDetailsViewModel?>(model with { Disabled = true })
            : Task.FromResult<MyNewModuleDetailsViewModel?>(null);
    }
}