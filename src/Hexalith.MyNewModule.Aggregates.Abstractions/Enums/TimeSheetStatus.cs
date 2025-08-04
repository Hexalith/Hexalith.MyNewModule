// <copyright file="TimeSheetStatus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Aggregates.Enums;

/// <summary>
/// Represents the status of a timesheet.
/// </summary>
public enum TimeSheetStatus
{
    /// <summary>
    /// The timesheet is in draft status.
    /// </summary>
    Draft = 0,

    /// <summary>
    /// The timesheet is submitted for approval.
    /// </summary>
    Submitted = 1,

    /// <summary>
    /// The timesheet is approved.
    /// </summary>
    Approved = 2,

    /// <summary>
    /// The timesheet is rejected.
    /// </summary>
    Rejected = 3,
}