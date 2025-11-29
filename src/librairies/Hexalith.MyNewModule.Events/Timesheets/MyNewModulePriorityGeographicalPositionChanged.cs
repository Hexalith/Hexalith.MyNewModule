// <copyright file="MyNewModulePrioritySubmissionPeriod Changed.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Events.MyNewModules;

using System.Runtime.Serialization;

using Hexalith.MyNewModule.Aggregates.Enums;
using Hexalith.MyNewModule.Aggregates.ValueObjects;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when a mynewmodule priority weight is changed.
/// </summary>
/// <param name="Id">The identifier of the mynewmodule.</param>
/// <param name="SubmissionPeriod ">The new geographical position of the mynewmodule.</param>
/// <param name="Temperature">The new temperature of the mynewmodule.</param>
[PolymorphicSerialization]
public partial record MyNewModulePrioritySubmissionPeriod Changed(
    string Id,
    [property: DataMember(Order = 2)] SubmissionPeriod ? SubmissionPeriod ,
    [property: DataMember(Order = 3)] TimeSheetStatus? Temperature)
    : MyNewModuleEvent(Id);