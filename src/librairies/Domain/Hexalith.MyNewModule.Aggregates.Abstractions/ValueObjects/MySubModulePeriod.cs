// <copyright file="MySubModulePeriod.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.MyNewModule.Aggregates.ValueObjects;

using System.Runtime.Serialization;

/// <summary>
/// Represents a MyNewModule period with a start and end date.
/// </summary>
/// <param name="StartDate">The start date of the MyNewModule period.</param>
/// <param name="EndDate">The end date of the MyNewModule period.</param>
[DataContract]
public record MySubModulePeriod(
    [property: DataMember(Order = 1)]
    DateOnly StartDate,
    [property: DataMember(Order = 2)]
    DateOnly EndDate);