// <copyright file="Warehouse.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Manhole.Domain.Warehouses;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using Hexalith.Domains;
using Hexalith.Domains.Results;
using Hexalith.Domains.ValueObjects;

using Manhole.Domain.ValueObjects;
using Manhole.Events.Warehouses;

/// <summary>
/// Represents a warehouse.
/// </summary>
/// <param name="Id">The warehouse identifier.</param>
/// <param name="Name">The warehouse name.</param>
/// <param name="Comments">The warehouse description.</param>
/// <param name="PriorityWeight">The warehouse priority weight.</param>
/// <param name="Disabled">The warehouse disabled status.</param>
[DataContract]
public sealed record Warehouse(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments,
    [property: DataMember(Order = 4)] decimal PriorityWeight,
    [property: DataMember(Order = 5)] bool Disabled) : IDomainAggregate, IIdDescriptionPriority
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Warehouse"/> class.
    /// </summary>
    public Warehouse()
        : this(string.Empty, string.Empty, string.Empty, 0, false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Warehouse"/> class with the specified event.
    /// </summary>
    /// <param name="added">The event that adds a warehouse.</param>
    public Warehouse(WarehouseAdded added)
        : this(
            (added ?? throw new ArgumentNullException(nameof(added))).Id,
            added.Name,
            added.Comments,
            added.PriorityWeight,
            false)
    {
    }

    /// <inheritdoc/>
    string IIdDescription.Description => Name;

    /// <inheritdoc/>
    public string AggregateId => Id;

    /// <inheritdoc/>
    public string AggregateName => ManholeDomainHelper.WarehouseAggregateName;

    /// <inheritdoc/>
    public ApplyResult Apply([NotNull] object domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        if (domainEvent is WarehouseEvent && domainEvent is not WarehouseEnabled or WarehouseDisabled && Disabled)
        {
            return ApplyResult.Error(this, "Cannot change a disabled warehouse.");
        }
        else if (!(this as IDomainAggregate).IsInitialized() && domainEvent is not WarehouseAdded)
        {
            return ApplyResult.Error(this, "Cannot apply changes to an uninitialized warehouse.");
        }
        else
        {
            return domainEvent switch
            {
                WarehouseAdded e => ApplyEvent(e),
                WarehouseDescriptionChanged e => ApplyEvent(e),
                WarehouseDisabled e => ApplyEvent(e),
                WarehouseEnabled e => ApplyEvent(e),
                WarehousePriorityWeightChanged e => ApplyEvent(e),
                WarehouseEvent => ApplyResult.NotImplemented(this),
                _ => ApplyResult.InvalidEvent(this, domainEvent),
            };
        }
    }

    private ApplyResult ApplyEvent(WarehouseAdded e) => !(this as IDomainAggregate).IsInitialized()
        ? ApplyResult.Success(new Warehouse(e), [e])
        : ApplyResult.Error(this, "The warehouse already exists.");

    private ApplyResult ApplyEvent(WarehouseDescriptionChanged e) => Comments == e.Comments && Name == e.Name
            ? ApplyResult.Error(this, "The warehouse name and description is already set to the specified value.")
            : ApplyResult.Success(this with { Comments = e.Comments, Name = e.Name }, [e]);

    private ApplyResult ApplyEvent(WarehouseDisabled e) => Disabled
            ? ApplyResult.Error(this, "The warehouse is already disabled.")
            : ApplyResult.Success(this with { Disabled = true }, [e]);

    private ApplyResult ApplyEvent(WarehouseEnabled e) => Disabled
            ? ApplyResult.Success(this with { Disabled = false }, [e])
            : ApplyResult.Error(this, "The warehouse is already enabled.");

    private ApplyResult ApplyEvent(WarehousePriorityWeightChanged e) => PriorityWeight == e.PriorityWeight
            ? ApplyResult.Error(this, "The warehouse priority weight is already set to the specified value.")
            : ApplyResult.Success(this with { PriorityWeight = e.PriorityWeight }, [e]);
}