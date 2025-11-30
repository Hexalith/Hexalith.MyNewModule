// <copyright file="MyNewModuleAddedOnDetailsProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModules.Projections.ProjectionHandlers.Details;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Projections;
using Hexalith.MyNewModules.Events.MyNewModules;
using Hexalith.MyNewModules.Requests.MyNewModule;

/// <summary>
/// Handles the projection update when a warehouse is added.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class MyNewModuleAddedOnDetailsProjectionHandler(IProjectionFactory<MyNewModuleDetailsViewModel> factory)
    : MyNewModuleDetailsProjectionHandler<MyNewModuleAdded>(factory)
{
    /// <inheritdoc/>
    protected override Task<MyNewModuleDetailsViewModel?> ApplyEventAsync([NotNull] MyNewModuleAdded baseEvent, MyNewModuleDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return Task.FromResult<MyNewModuleDetailsViewModel?>(new MyNewModuleDetailsViewModel(
            baseEvent.Id,
            baseEvent.Name,
            baseEvent.Comments,
            false));
    }
}