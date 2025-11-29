// <copyright file="MyNewModuleStatus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModules.Aggregates.Enums;

/// <summary>
/// Represents the status of MyNewModule.
/// </summary>
public enum MyNewModuleStatus
{
    /// <summary>
    /// The MyNewModule is in draft status.
    /// </summary>
    Draft = 0,

    /// <summary>
    /// The MyNewModule is submitted for approval.
    /// </summary>
    Submitted = 1,

    /// <summary>
    /// The MyNewModule is approved.
    /// </summary>
    Approved = 2,

    /// <summary>
    /// The MyNewModule is rejected.
    /// </summary>
    Rejected = 3,
}