// <copyright file="MyNewModuleAddedOnSummaryProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.ProjectionHandlers.Summaries;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Projections;
using Hexalith.MyNewModule.Events.MyNewModules;
using Hexalith.MyNewModule.Requests.MyNewModule;

/// <summary>
/// Handles the projection update when a warehouse is added.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class MyNewModuleAddedOnSummaryProjectionHandler(IProjectionFactory<MyNewModuleSummaryViewModel> factory)
    : MyNewModuleSummaryProjectionHandler<MyNewModuleAdded>(factory)
{
    /// <inheritdoc/>
    protected override Task<MyNewModuleSummaryViewModel?> ApplyEventAsync([NotNull] MyNewModuleAdded baseEvent, MyNewModuleSummaryViewModel? summary, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return Task.FromResult<MyNewModuleSummaryViewModel?>(new MyNewModuleSummaryViewModel(baseEvent.Id, baseEvent.Name, false));
    }
}