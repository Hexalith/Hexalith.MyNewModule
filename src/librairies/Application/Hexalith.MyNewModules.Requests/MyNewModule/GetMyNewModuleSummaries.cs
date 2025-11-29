// <copyright file="GetMyNewModuleSummaries.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Manhole.Requests.MyNewModules;

using System.Runtime.Serialization;

using Hexalith.Application.Requests;
using Hexalith.MyNewModules.Aggregates;
using Hexalith.MyNewModules.Requests.MyNewModule;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a request to get warehouse summaries with pagination.
/// </summary>
/// <param name="Skip">The number of warehouse summaries to skip.</param>
/// <param name="Take">The number of warehouse summaries to take.</param>
/// <param name="Search">Optional search term to filter results.</param>
/// <param name="Ids">The list of warehouse IDs to search by.</param>
/// <param name="Results">The list of warehouse summaries.</param>
[PolymorphicSerialization]
public partial record GetMyNewModuleSummaries(
    [property: DataMember(Order = 1)] int Skip,
    [property: DataMember(Order = 2)] int Take,
    [property: DataMember(Order = 3)] string? Search,
    [property: DataMember(Order = 4)] IEnumerable<string> Ids,
    [property: DataMember(Order = 5)] IEnumerable<MyNewModuleSummaryViewModel> Results) : ISearchChunkableRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyNewModuleSummaries"/> class.
    /// </summary>
    public GetMyNewModuleSummaries()
        : this(0, 0, null, [], [])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyNewModuleSummaries"/> class with specified skip, take, and search values.
    /// </summary>
    /// <param name="skip">Number of records to skip for pagination.</param>
    /// <param name="take">Maximum number of records to return.</param>
    /// <param name="search">Optional search term to filter results.</param>
    public GetMyNewModuleSummaries(int skip, int take, string? search = null)
        : this(skip, take, search, [], [])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyNewModuleSummaries"/> class with specified warehouse IDs.
    /// </summary>
    /// <param name="ids">The list of warehouse IDs to search by.</param>
    public GetMyNewModuleSummaries(IEnumerable<string> ids)
        : this(0, 0, null, ids, [])
    {
    }

    /// <summary>
    /// Gets the aggregate ID.
    /// </summary>
    public static string AggregateId => MyNewModuleDomainHelper.MyNewModuleAggregateName;

    /// <summary>
    /// Gets the aggregate name.
    /// </summary>
    public static string AggregateName => MyNewModuleDomainHelper.MyNewModuleAggregateName;

    /// <inheritdoc/>
    IEnumerable<object>? ICollectionRequest.Results => Results;

    /// <inheritdoc/>
    public IChunkableRequest CreateNextChunkRequest() => this with { Skip = Skip + Take, Results = [] };

    /// <inheritdoc/>
    public ICollectionRequest CreateResults(IEnumerable<object> results)
        => this with { Results = (IEnumerable<MyNewModuleSummaryViewModel>)results };
}