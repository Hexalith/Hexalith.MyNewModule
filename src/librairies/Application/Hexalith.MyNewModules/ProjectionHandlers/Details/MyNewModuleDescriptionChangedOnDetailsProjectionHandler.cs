// <copyright file="MyNewModuleDescriptionChangedOnDetailsProjectionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Hexalith.MyNewModules.ProjectionHandlers.Details;

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
                false))
            : Task.FromResult<MyNewModuleDetailsViewModel?>(model with { Name = baseEvent.Name, Comments = baseEvent.Comments });
    }
}