// <copyright file="MyNewModulePriorityWeightChangedOnDetailsProjectionHandler.cs" company="ITANEO">
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
/// Handles the projection update when a MyNewModuleFileToTextConverterChanged event is received.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MyNewModulePriorityWeightChangedOnDetailsProjectionHandler"/> class.
/// </remarks>
/// <param name="factory">The projection factory.</param>
public class MyNewModulePriorityWeightChangedOnDetailsProjectionHandler(IProjectionFactory<MyNewModuleDetailsViewModel> factory) : MyNewModuleDetailsProjectionHandler<MyNewModulePriorityWeightChanged>(factory)
{
    /// <inheritdoc/>
    protected override Task<MyNewModuleDetailsViewModel?> ApplyEventAsync([NotNull] MyNewModulePriorityWeightChanged baseEvent, MyNewModuleDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return model == null
            ? Task.FromResult<MyNewModuleDetailsViewModel?>(null)
            : Task.FromResult<MyNewModuleDetailsViewModel?>(model with { PriorityWeight = baseEvent.PriorityWeight });
    }
}