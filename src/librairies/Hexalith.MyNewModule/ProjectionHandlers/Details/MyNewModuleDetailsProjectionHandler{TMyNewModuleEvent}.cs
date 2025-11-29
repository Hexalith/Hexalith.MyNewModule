// <copyright file="MyNewModuleDetailsProjectionHandler{TMyNewModuleEvent}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.ProjectionHandlers.Details;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projections;
using Hexalith.MyNewModule.Events.MyNewModules;
using Hexalith.MyNewModule.Requests.Timesheets;

/// <summary>
/// Abstract base class for handling updates to MyNewModule projections based on events.
/// </summary>
/// <typeparam name="TMyNewModuleEvent">The type of the warehouse event.</typeparam>
/// <param name="factory">The actor projection factory.</param>
public abstract class MyNewModuleDetailsProjectionHandler<TMyNewModuleEvent>(IProjectionFactory<MyNewModuleDetailsViewModel> factory)
    : KeyValueProjectionUpdateEventHandlerBase<TMyNewModuleEvent, MyNewModuleDetailsViewModel>(factory)
    where TMyNewModuleEvent : MyNewModuleEvent
{
    /// <inheritdoc/>
    public override async Task ApplyAsync([NotNull] TMyNewModuleEvent baseEvent, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        ArgumentNullException.ThrowIfNull(metadata);

        MyNewModuleDetailsViewModel? currentValue = await GetProjectionAsync(metadata.AggregateGlobalId, cancellationToken)
            .ConfigureAwait(false);

        MyNewModuleDetailsViewModel? newValue = await ApplyEventAsync(
                baseEvent,
                currentValue,
                cancellationToken)
            .ConfigureAwait(false);
        if (newValue == null || newValue == currentValue)
        {
            return;
        }

        await SaveProjectionAsync(metadata.AggregateGlobalId, newValue, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Applies the event to the warehouse summary view model.
    /// </summary>
    /// <param name="baseEvent">The warehouse event.</param>
    /// <param name="model">The current warehouse detail view model.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated warehouse summary view model.</returns>
    protected abstract Task<MyNewModuleDetailsViewModel?> ApplyEventAsync(TMyNewModuleEvent baseEvent, MyNewModuleDetailsViewModel? model, CancellationToken cancellationToken);
}