// <copyright file="MyNewModuleSummaryProjectionHandler{TMyNewModuleEvent}.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Hexalith.MyNewModule.ProjectionHandlers.Summaries;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projections;
using Hexalith.MyNewModule.Events.MyNewModules;
using Hexalith.MyNewModule.Requests.MyNewModule;

/// <summary>
/// Abstract base class for handling updates to MyNewModule projections based on events.
/// </summary>
/// <typeparam name="TMyNewModuleEvent">The type of the warehouse event.</typeparam>
/// <param name="factory">The actor projection factory.</param>
public abstract class MyNewModuleSummaryProjectionHandler<TMyNewModuleEvent>(IProjectionFactory<MyNewModuleSummaryViewModel> factory)
    : KeyValueProjectionUpdateEventHandlerBase<TMyNewModuleEvent, MyNewModuleSummaryViewModel>(factory)
    where TMyNewModuleEvent : MyNewModuleEvent
{
    /// <inheritdoc/>
    public override async Task ApplyAsync([NotNull] TMyNewModuleEvent baseEvent, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        ArgumentNullException.ThrowIfNull(metadata);

        MyNewModuleSummaryViewModel? currentValue = await GetProjectionAsync(metadata.AggregateGlobalId, cancellationToken)
            .ConfigureAwait(false);

        MyNewModuleSummaryViewModel? newValue = await ApplyEventAsync(
                baseEvent,
                currentValue,
                cancellationToken)
            .ConfigureAwait(false);
        if (newValue == null)
        {
            return;
        }

        await SaveProjectionAsync(metadata.AggregateGlobalId, newValue, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Applies the event to the warehouse summary view model.
    /// </summary>
    /// <param name="baseEvent">The warehouse event.</param>
    /// <param name="summary">The existing warehouse summary view model, if any.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated warehouse summary view model.</returns>
    protected abstract Task<MyNewModuleSummaryViewModel?> ApplyEventAsync(TMyNewModuleEvent baseEvent, MyNewModuleSummaryViewModel? summary, CancellationToken cancellationToken);
}