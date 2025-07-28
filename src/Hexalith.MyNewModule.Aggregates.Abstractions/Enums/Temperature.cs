// <copyright file="Temperature.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Aggregates.Enums;

/// <summary>
/// Represents temperature levels from very cold to very hot.
/// </summary>
public enum Temperature
{
    /// <summary>
    /// Very cold temperature.
    /// </summary>
    VeryCold = 0,

    /// <summary>
    /// Cold temperature.
    /// </summary>
    Cold = 1,

    /// <summary>
    /// Cool temperature.
    /// </summary>
    Cool = 2,

    /// <summary>
    /// Warm temperature.
    /// </summary>
    Warm = 3,

    /// <summary>
    /// Hot temperature.
    /// </summary>
    Hot = 4,

    /// <summary>
    /// Very hot temperature.
    /// </summary>
    VeryHot = 5,
}