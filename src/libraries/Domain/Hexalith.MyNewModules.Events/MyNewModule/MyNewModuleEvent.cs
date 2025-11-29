// <copyright file="MyNewModuleEvent.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModules.Events.MyNewModules;

using Hexalith.MyNewModules.Aggregates;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Base event for mynewmodule operations.
/// </summary>
/// <param name="Id">The identifier of the mynewmodule.</param>
[PolymorphicSerialization]
public abstract partial record MyNewModuleEvent(string Id)
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