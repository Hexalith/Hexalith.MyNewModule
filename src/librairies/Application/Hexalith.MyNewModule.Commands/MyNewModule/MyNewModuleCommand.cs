// <copyright file="MyNewModuleCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Hexalith.MyNewModule.Commands.MyNewModules;

using Hexalith.MyNewModule.Aggregates;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Base command for mynewmodule operations.
/// </summary>
/// <param name="Id">The identifier of the mynewmodule.</param>
[PolymorphicSerialization]
public abstract partial record MyNewModuleCommand(string Id)
{
    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    public string AggregateId => Id;

    /// <summary>
    /// Gets the aggregate name.
    /// </summary>
    public static string AggregateName => MyNewModuleDomainHelper.MyNewModuleAggregateName;
}