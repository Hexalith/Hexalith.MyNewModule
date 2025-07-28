// <copyright file="GeographicalPosition.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Aggregates.ValueObjects;

/// <summary>
/// Represents a geographical position with latitude and longitude.
/// </summary>
/// <param name="Latitude">The latitude of the geographic position.</param>
/// <param name="Longitude">The longitude of the geographic position.</param>
public record GeographicalPosition(decimal Latitude, decimal Longitude);