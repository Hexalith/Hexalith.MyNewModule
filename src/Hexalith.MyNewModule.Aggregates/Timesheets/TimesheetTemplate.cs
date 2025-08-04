// <copyright file="Timesheet.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Aggregates.Timesheets;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using Hexalith.Domains;
using Hexalith.Domains.Results;
using Hexalith.MyNewModule.Aggregates;
using Hexalith.MyNewModule.Aggregates.Enums;
using Hexalith.MyNewModule.Aggregates.ValueObjects;
using Hexalith.MyNewModule.Events.MyNewModules;

/// <summary>
/// Represents a mynewmodule.
/// </summary>
/// <param name="Id">The mynewmodule identifier.</param>
/// <param name="Name">The mynewmodule name.</param>
/// <param name="Comments">The mynewmodule description.</param>
/// <param name="WorkerId">The mynewmodule priority weight.</param>
/// <param name="SubmissionPeriod ">The mynewmodule geographical position.</param>
/// <param name="Status"> The mynewmodule temperature.</param>
/// <param name="Disabled">The mynewmodule disabled status.</param>
[DataContract]
public sealed record Timesheet(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments,
    [property: DataMember(Order = 4)] string WorkerId,
    [property: DataMember(Order = 5)] SubmissionPeriod SubmissionPeriod,
    [property: DataMember(Order = 6)] TimeSheetStatus Status,
    [property: DataMember(Order = 7)] bool Disabled) : IDomainAggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Timesheet"/> class.
    /// </summary>
    public Timesheet()
        : this(string.Empty, string.Empty, string.Empty, 0, null, null, false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Timesheet"/> class with the specified event.
    /// </summary>
    /// <param name="added">The event that adds a mynewmodule.</param>
    public Timesheet(MyNewModuleAdded added)
        : this(
            (added ?? throw new ArgumentNullException(nameof(added))).Id,
            added.Name,
            added.Comments,
            added.PriorityWeight,
            null,
            null,
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
                MyNewModulePrioritySubmissionPeriod Changed e => ApplyEvent(e),
                MyNewModuleEvent => ApplyResult.NotImplemented(this),
                _ => ApplyResult.InvalidEvent(this, domainEvent),
            };
        }
    }

    private ApplyResult ApplyEvent(MyNewModuleAdded e) => !(this as IDomainAggregate).IsInitialized()
        ? ApplyResult.Success(new Timesheet(e), [e])
        : ApplyResult.Error(this, "The MyNewModule already exists.");

    private ApplyResult ApplyEvent(MyNewModuleDescriptionChanged e) => Comments == e.Comments && Name == e.Name
            ? ApplyResult.Error(this, "The MyNewModule name and description are already set to the specified values.")
            : ApplyResult.Success(this with { Comments = e.Comments, Name = e.Name }, [e]);

    private ApplyResult ApplyEvent(MyNewModuleDisabled e) => Disabled
            ? ApplyResult.Error(this, "The MyNewModule is already disabled.")
            : ApplyResult.Success(this with { Disabled = true }, [e]);

    private ApplyResult ApplyEvent(MyNewModuleEnabled e) => Disabled
            ? ApplyResult.Success(this with { Disabled = false }, [e])
            : ApplyResult.Error(this, "The MyNewModule is already enabled.");

    private ApplyResult ApplyEvent(MyNewModulePriorityWeightChanged e) => WorkerId == e.PriorityWeight
            ? ApplyResult.Error(this, "The MyNewModule priority weight is already set to the specified value.")
            : ApplyResult.Success(this with { WorkerId = e.PriorityWeight }, [e]);

    private ApplyResult ApplyEvent(MyNewModulePrioritySubmissionPeriod Changed e) => SubmissionPeriod == e.SubmissionPeriod && Status == e.Temperature
            ? ApplyResult.Error(this, "The MyNewModule geographical position and temperature is already set to the specified value.")
            : ApplyResult.Success(this with { SubmissionPeriod = e.SubmissionPeriod, Status = Status }, [e]);
}