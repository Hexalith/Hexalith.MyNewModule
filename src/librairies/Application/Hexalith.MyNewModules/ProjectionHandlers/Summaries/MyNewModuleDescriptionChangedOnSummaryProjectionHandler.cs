// <copyright file="MyNewModuleDescriptionChangedOnSummaryProjectionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Hexalith.MyNewModules.ProjectionHandlers.Summaries;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Projections;
using Hexalith.MyNewModules.Events.MyNewModules;
using Hexalith.MyNewModules.Requests.MyNewModule;

/// <summary>
/// Handles the projection update when a warehouse description is changed.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MyNewModuleDescriptionChangedOnSummaryProjectionHandler"/> class.
/// </remarks>
/// <param name="factory">The projection factory.</param>
public class MyNewModuleDescriptionChangedOnSummaryProjectionHandler(IProjectionFactory<MyNewModuleSummaryViewModel> factory)
    : MyNewModuleSummaryProjectionHandler<MyNewModuleDescriptionChanged>(factory)
{
    /// <inheritdoc/>
    protected override Task<MyNewModuleSummaryViewModel?> ApplyEventAsync([NotNull] MyNewModuleDescriptionChanged baseEvent, MyNewModuleSummaryViewModel? summary, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return summary == null
            ? Task.FromResult<MyNewModuleSummaryViewModel?>(null)
            : Task.FromResult<MyNewModuleSummaryViewModel?>(summary with { Name = baseEvent.Name });
    }
}