// <copyright file="GetMyNewModuleExports.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Hexalith.MyNewModules.Requests.MyNewModule;

using System.Runtime.Serialization;

using Hexalith.Application.Requests;
using Hexalith.MyNewModules.Aggregates;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a request to get MyNewModule summaries with pagination.
/// </summary>
/// <param name="Skip">The number of MyNewModule summaries to skip.</param>
/// <param name="Take">The number of MyNewModule summaries to take.</param>
/// <param name="Results">The list of MyNewModule summaries.</param>
[PolymorphicSerialization]
public partial record GetMyNewModuleExports(
    [property: DataMember(Order = 1)] int Skip,
    [property: DataMember(Order = 2)] int Take,
    [property: DataMember(Order = 3)] IEnumerable<MyNewModule> Results)
    : IChunkableRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyNewModuleExports"/> class.
    /// </summary>
    public GetMyNewModuleExports()
        : this(0, 0, [])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyNewModuleExports"/> class with specified skip and take values.
    /// </summary>
    /// <param name="skip">The number of MyNewModule summaries to skip.</param>
    /// <param name="take">The number of MyNewModule summaries to take.</param>
    public GetMyNewModuleExports(int skip, int take)
        : this(skip, take, [])
    {
    }

    /// <summary>
    /// Gets the aggregate ID of the MyNewModule command.
    /// </summary>
    public static string AggregateId => MyNewModuleDomainHelper.MyNewModuleAggregateName;

    /// <summary>
    /// Gets the aggregate name of the MyNewModule command.
    /// </summary>
    public static string AggregateName => MyNewModuleDomainHelper.MyNewModuleAggregateName;

    /// <inheritdoc/>
    IEnumerable<object>? ICollectionRequest.Results => Results;

    /// <inheritdoc/>
    public IChunkableRequest CreateNextChunkRequest()
        => ((IChunkableRequest)this).HasNextChunk
            ? this with { Skip = Skip + Take, Results = [] }
            : throw new InvalidRequestChunkException();

    /// <inheritdoc/>
    public ICollectionRequest CreateResults(IEnumerable<object> results) => this with { Results = (IEnumerable<MyNewModule>)results };
}