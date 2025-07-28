// <copyright file="MyNewModuleSummaryViewModel.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Requests.MyNewModules;

using System.Runtime.Serialization;

using Hexalith.Domains.ValueObjects;

/// <summary>
/// Represents a summary view of a warehouse with essential information.
/// </summary>
/// <param name="Id">The unique identifier of the warehouse.</param>
/// <param name="Name">The name of the warehouse.</param>
/// <param name="PriorityWeight">The priority weight for sorting or ordering.</param>
/// <param name="Disabled">Indicates whether the warehouse is disabled.</param>
[DataContract]
public sealed record MyNewModuleSummaryViewModel(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] decimal PriorityWeight,
    [property: DataMember(Order = 3)] bool Disabled) : IIdDescription
{
    /// <inheritdoc/>
    string IIdDescription.Description => Name;
}