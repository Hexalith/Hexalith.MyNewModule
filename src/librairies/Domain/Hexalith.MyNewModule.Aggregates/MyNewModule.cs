// <copyright file="MyNewModule.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.MyNewModule.Aggregates;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using Hexalith.Domains;
using Hexalith.Domains.Results;
using Hexalith.MyNewModule.Events.MyNewModules;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a mynewmodule.
/// </summary>
/// <param name="Id">The mynewmodule identifier.</param>
/// <param name="Name">The mynewmodule name.</param>
/// <param name="Comments">The mynewmodule description.</param>
/// <param name="Disabled">The mynewmodule disabled status.</param>
[PolymorphicSerialization]
public sealed record MyNewModule(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments,
    [property: DataMember(Order = 7)] bool Disabled) : IDomainAggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyNewModule"/> class.
    /// </summary>
    public MyNewModule()
        : this(string.Empty, string.Empty, null, false)
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
                MyNewModuleEvent => ApplyResult.NotImplemented(this),
                _ => ApplyResult.InvalidEvent(this, domainEvent),
            };
        }
    }

    private ApplyResult ApplyEvent(MyNewModuleAdded e) => !(this as IDomainAggregate).IsInitialized()
        ? ApplyResult.Success(new MyNewModule(e), [e])
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
}