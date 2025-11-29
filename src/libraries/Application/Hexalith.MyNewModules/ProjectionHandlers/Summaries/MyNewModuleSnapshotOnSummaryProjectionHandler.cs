// <copyright file="MyNewModuleSnapshotOnSummaryProjectionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Hexalith.MyNewModules.ProjectionHandlers.Summaries;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projections;
using Hexalith.Domain.Events;
using Hexalith.MyNewModules.Aggregates;
using Hexalith.MyNewModules.Requests.MyNewModule;

/// <summary>
/// Handles the projection updates for warehouse snapshots on summary.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class MyNewModuleSnapshotOnSummaryProjectionHandler(IProjectionFactory<MyNewModuleSummaryViewModel> factory)
    : IProjectionUpdateHandler<SnapshotEvent>
{
    /// <inheritdoc/>
    public async Task ApplyAsync(SnapshotEvent baseEvent, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        ArgumentNullException.ThrowIfNull(metadata);
        if (baseEvent?.AggregateName != MyNewModuleDomainHelper.MyNewModuleAggregateName)
        {
            return;
        }

        MyNewModuleSummaryViewModel? currentValue = await factory
            .GetStateAsync(metadata.AggregateGlobalId, cancellationToken)
            .ConfigureAwait(false);

        MyNewModule warehouse = baseEvent.GetAggregate<MyNewModule>();
        MyNewModuleSummaryViewModel newValue = new(warehouse.Id, warehouse.Name, warehouse.Disabled);
        if (currentValue is not null && currentValue == newValue)
        {
            return;
        }

        await factory
            .SetStateAsync(
                metadata.AggregateGlobalId,
                newValue,
                cancellationToken)
            .ConfigureAwait(false);
    }
}