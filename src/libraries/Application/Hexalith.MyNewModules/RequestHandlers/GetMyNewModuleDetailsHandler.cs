// <copyright file="GetMyNewModuleDetailsHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModules.RequestHandlers;

using System;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projections;
using Hexalith.Application.Requests;
using Hexalith.MyNewModules.Requests.MyNewModule;

/// <summary>
/// Handler for getting mynewmodule details.
/// </summary>
public class GetMyNewModuleDetailsHandler : RequestHandlerBase<GetMyNewModuleDetails>
{
    private readonly IProjectionFactory<MyNewModuleDetailsViewModel> _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyNewModuleDetailsHandler"/> class.
    /// </summary>
    /// <param name="factory">The projection mynewmodule.</param>
    /// <exception cref="ArgumentNullException">Thrown when projectionMyNewModule is null.</exception>
    public GetMyNewModuleDetailsHandler(IProjectionFactory<MyNewModuleDetailsViewModel> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
    }

    /// <inheritdoc/>
    public override async Task<GetMyNewModuleDetails> ExecuteAsync(GetMyNewModuleDetails request, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(metadata);

        return request with
        {
            Result = await _factory
                .GetStateAsync(metadata.AggregateGlobalId, cancellationToken)
                .ConfigureAwait(false)
                    ?? throw new InvalidOperationException($"MyNewModule type {metadata.AggregateGlobalId} not found."),
        };
    }
}