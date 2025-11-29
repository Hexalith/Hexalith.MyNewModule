// <copyright file="MyNewModuleDetailsViewModel.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModules.Requests.MyNewModule;

using System.Runtime.Serialization;

using Hexalith.Domains.ValueObjects;

/// <summary>
/// Represents the details of a warehouse.
/// </summary>
/// <param name="Id">The warehouse identifier.</param>
/// <param name="Name">The warehouse name.</param>
/// <param name="Comments">The warehouse description.</param>
/// <param name="Disabled">The warehouse disabled status.</param>
[DataContract]
public sealed record MyNewModuleDetailsViewModel(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments,
    [property: DataMember(Order = 5)] bool Disabled) : IIdDescription
{
    /// <inheritdoc/>
    string IIdDescription.Description => Name;
}