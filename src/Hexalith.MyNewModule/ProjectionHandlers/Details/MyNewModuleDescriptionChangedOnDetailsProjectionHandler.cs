// <copyright file="MyNewModuleDescriptionChangedOnDetailsProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.ProjectionHandlers.Details;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Projections;
using Hexalith.MyNewModule.Events.MyNewModules;
using Hexalith.MyNewModule.Requests.MyNewModules;

/// <summary>
/// Handles the projection update when a warehouse description is changed.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MyNewModuleDescriptionChangedOnDetailsProjectionHandler"/> class.
/// </remarks>
/// <param name="factory">The projection factory.</param>
public class MyNewModuleDescriptionChangedOnDetailsProjectionHandler(IProjectionFactory<MyNewModuleDetailsViewModel> factory)
    : MyNewModuleDetailsProjectionHandler<MyNewModuleDescriptionChanged>(factory)
{
    /// <inheritdoc/>
    protected override Task<MyNewModuleDetailsViewModel?> ApplyEventAsync([NotNull] MyNewModuleDescriptionChanged baseEvent, MyNewModuleDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return model == null
            ? Task.FromResult<MyNewModuleDetailsViewModel?>(new MyNewModuleDetailsViewModel(
                baseEvent.Id,
                baseEvent.Name,
                baseEvent.Comments,
                0,
                false))
            : Task.FromResult<MyNewModuleDetailsViewModel?>(model with { Name = baseEvent.Name, Comments = baseEvent.Comments });
    }
}