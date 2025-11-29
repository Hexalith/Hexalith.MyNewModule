// <copyright file="MyNewModuleRequest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Requests.MyNewModules;

using System.Runtime.Serialization;

using Hexalith.MyNewModule.Aggregates;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a base class for document commands.
/// </summary>
/// <param name="Id">The aggregate ID of the document command.</param>
[PolymorphicSerialization]
public abstract partial record MyNewModuleRequest([property: DataMember(Order = 1)] string Id)
{
    /// <summary>
    /// Gets the aggregate ID of the document command.
    /// </summary>
    public string AggregateId => Id;

    /// <summary>
    /// Gets the aggregate name of the document command.
    /// </summary>
    public static string AggregateName => MyNewModuleDomainHelper.MyNewModuleAggregateName;
}