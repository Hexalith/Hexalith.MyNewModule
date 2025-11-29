// <copyright file="MyNewModuleStatus.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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