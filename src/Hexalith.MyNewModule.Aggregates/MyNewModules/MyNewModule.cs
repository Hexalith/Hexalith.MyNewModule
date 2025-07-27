﻿// <copyright file="MyNewModule.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.MyNewModules;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using Hexalith.Domains;
using Hexalith.Domains.Results;
using Hexalith.MyNewModule.Events;

using Manhole.Events.MyNewModules;

/// <summary>
/// Represents a mynewmodule.
/// </summary>
/// <param name="Id">The mynewmodule identifier.</param>
/// <param name="Name">The mynewmodule name.</param>
/// <param name="Comments">The mynewmodule description.</param>
/// <param name="PriorityWeight">The mynewmodule priority weight.</param>
/// <param name="Disabled">The mynewmodule disabled status.</param>
[DataContract]
public sealed record MyNewModule(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments,
    [property: DataMember(Order = 4)] decimal PriorityWeight,
    [property: DataMember(Order = 5)] bool Disabled) : IDomainAggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyNewModule"/> class.
    /// </summary>
    public MyNewModule()
        : this(string.Empty, string.Empty, string.Empty, 0, false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MyNewModule"/> class with the specified event.
    /// </summary>
    /// <param name="added">The event that adds a mynewmodule.</param>
    public MyNewModule(MyNewModuleAdded added)
        : this(
            (added ?? throw new ArgumentNullException(nameof(added))).Id,
            added.Name,
            added.Comments,
            added.PriorityWeight,
            false)
    {
    }

    /// <inheritdoc/>
    public string AggregateId => Id;

    /// <inheritdoc/>
    public string AggregateName => MyNewModuleDomainHelper.MyNewModuleAggregateName;

    /// <inheritdoc/>
    public ApplyResult Apply([NotNull] object domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        if (domainEvent is MyNewModuleEvent && domainEvent is not MyNewModuleEnabled or MyNewModuleDisabled && Disabled)
        {
            return ApplyResult.NotEnabled(this);
        }
        else if (!(this as IDomainAggregate).IsInitialized() && domainEvent is not MyNewModuleAdded)
        {
            return ApplyResult.NotInitialized(this);
        }
        else
        {
            return domainEvent switch
            {
                MyNewModuleAdded e => ApplyEvent(e),
                MyNewModuleDescriptionChanged e => ApplyEvent(e),
                MyNewModuleDisabled e => ApplyEvent(e),
                MyNewModuleEnabled e => ApplyEvent(e),
                MyNewModulePriorityWeightChanged e => ApplyEvent(e),
                MyNewModuleEvent => ApplyResult.NotImplemented(this),
                _ => ApplyResult.InvalidEvent(this, domainEvent),
            };
        }
    }

    private ApplyResult ApplyEvent(MyNewModuleAdded e) => !(this as IDomainAggregate).IsInitialized()
        ? ApplyResult.Success(new MyNewModule(e), [e])
        : ApplyResult.Error(this, "The mynewmodule already exists.");

    private ApplyResult ApplyEvent(MyNewModuleDescriptionChanged e) => Comments == e.Comments && Name == e.Name
            ? ApplyResult.Error(this, "The mynewmodule name and description is already set to the specified value.")
            : ApplyResult.Success(this with { Comments = e.Comments, Name = e.Name }, [e]);

    private ApplyResult ApplyEvent(MyNewModuleDisabled e) => Disabled
            ? ApplyResult.Error(this, "The mynewmodule is already disabled.")
            : ApplyResult.Success(this with { Disabled = true }, [e]);

    private ApplyResult ApplyEvent(MyNewModuleEnabled e) => Disabled
            ? ApplyResult.Success(this with { Disabled = false }, [e])
            : ApplyResult.Error(this, "The mynewmodule is already enabled.");

    private ApplyResult ApplyEvent(MyNewModulePriorityWeightChanged e) => PriorityWeight == e.PriorityWeight
            ? ApplyResult.Error(this, "The mynewmodule priority weight is already set to the specified value.")
            : ApplyResult.Success(this with { PriorityWeight = e.PriorityWeight }, [e]);
}