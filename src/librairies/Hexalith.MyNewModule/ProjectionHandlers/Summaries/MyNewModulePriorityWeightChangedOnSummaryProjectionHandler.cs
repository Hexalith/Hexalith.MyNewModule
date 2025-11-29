// <copyright file="MyNewModulePriorityWeightChangedOnSummaryProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.ProjectionHandlers.Summaries;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Projections;
using Hexalith.MyNewModule.Events.MyNewModules;
using Hexalith.MyNewModule.Requests.Timesheets;

/// <summary>
/// Handles the projection update when the priority weight of a warehouse changes.
/// </summary>
/// <param name="factory">The projection factory for <see cref="MyNewModuleSummaryViewModel"/>.</param>
public class MyNewModulePriorityWeightChangedOnSummaryProjectionHandler(IProjectionFactory<MyNewModuleSummaryViewModel> factory)
    : MyNewModuleSummaryProjectionHandler<MyNewModulePriorityWeightChanged>(factory)
{
    /// <summary>
    /// Applies the <see cref="MyNewModulePriorityWeightChanged"/> event to the <see cref="MyNewModuleSummaryViewModel"/>.
    /// </summary>
    /// <param name="baseEvent">The event containing the new priority weight.</param>
    /// <param name="summary">The current summary view model.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated summary view model or null if the summary is null.</returns>
    protected override Task<MyNewModuleSummaryViewModel?> ApplyEventAsync([NotNull] MyNewModulePriorityWeightChanged baseEvent, MyNewModuleSummaryViewModel? summary, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return summary == null
            ? Task.FromResult<MyNewModuleSummaryViewModel?>(null)
            : Task.FromResult<MyNewModuleSummaryViewModel?>(summary with { PriorityWeight = baseEvent.PriorityWeight });
    }
}