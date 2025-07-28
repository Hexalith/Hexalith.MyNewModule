// <copyright file="GeographicalPosition.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Aggregates.ValueObjects;

using System.Runtime.Serialization;

/// <summary>
/// Represents a geographical position with latitude and longitude.
/// </summary>
/// <param name="Latitude">The latitude of the geographic position.</param>
/// <param name="Longitude">The longitude of the geographic position.</param>
[DataContract]
public record GeographicalPosition(
    [property: DataMember(Order = 1)]
    decimal Latitude,
    [property: DataMember(Order = 2)]
    decimal Longitude)
{
    /// <summary>
    /// Gets the maximum value for latitude.
    /// </summary>
    public static decimal MinLatitude => -90.0m;

    /// <summary>
    /// Gets the minimum value for latitude.
    /// </summary>
    public static decimal MaxLatitude => 90.0m;

    /// <summary>
    /// Gets the maximum value for longitude.
    /// </summary>
    public static decimal MinLongitude => -180.0m;

    /// <summary>
    /// Gets the minimum value for longitude.
    /// </summary>
    public static decimal MaxLongitude => 180.0m;
}