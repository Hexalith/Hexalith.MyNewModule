// <copyright file="MyNewModuleEnabledOnDetailsProjectionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Hexalith.MyNewModules.ProjectionHandlers.Details;

using System.Diagnostics.CodeAnalysis;

using Hexalith.Application.Projections;
using Hexalith.MyNewModules.Events.MyNewModules;
using Hexalith.MyNewModules.Requests.MyNewModule;

/// <summary>
/// Handles the projection update when a warehouse is enabled.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MyNewModuleEnabledOnDetailsProjectionHandler"/> class.
/// </remarks>
/// <param name="factory">The projection factory.</param>
public class MyNewModuleEnabledOnDetailsProjectionHandler(IProjectionFactory<MyNewModuleDetailsViewModel> factory)
    : MyNewModuleDetailsProjectionHandler<MyNewModuleEnabled>(factory)
{
    /// <inheritdoc/>
    protected override Task<MyNewModuleDetailsViewModel?> ApplyEventAsync([NotNull] MyNewModuleEnabled baseEvent, MyNewModuleDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return model?.Disabled == true
            ? Task.FromResult<MyNewModuleDetailsViewModel?>(model with { Disabled = false })
            : Task.FromResult<MyNewModuleDetailsViewModel?>(null);
    }
}