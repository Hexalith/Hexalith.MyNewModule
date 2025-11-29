// <copyright file="MyNewModuleEnabledOnSummaryProjectionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Hexalith.MyNewModules.ProjectionHandlers.Summaries;

using System.Diagnostics.CodeAnalysis;

using Hexalith.Application.Projections;
using Hexalith.MyNewModules.Events.MyNewModules;
using Hexalith.MyNewModules.Requests.MyNewModule;

/// <summary>
/// Handles the projection update when a warehouse is enabled.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MyNewModuleEnabledOnSummaryProjectionHandler"/> class.
/// </remarks>
/// <param name="factory">The projection factory.</param>
public class MyNewModuleEnabledOnSummaryProjectionHandler(IProjectionFactory<MyNewModuleSummaryViewModel> factory)
    : MyNewModuleSummaryProjectionHandler<MyNewModuleEnabled>(factory)
{
    /// <summary>
    /// Applies the event to the summary projection.
    /// </summary>
    /// <param name="baseEvent">The event to apply.</param>
    /// <param name="summary">The current summary projection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated summary projection.</returns>
    protected override Task<MyNewModuleSummaryViewModel?> ApplyEventAsync([NotNull] MyNewModuleEnabled baseEvent, MyNewModuleSummaryViewModel? summary, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return summary == null
            ? Task.FromResult<MyNewModuleSummaryViewModel?>(null)
            : Task.FromResult<MyNewModuleSummaryViewModel?>(summary with { Disabled = false });
    }
}