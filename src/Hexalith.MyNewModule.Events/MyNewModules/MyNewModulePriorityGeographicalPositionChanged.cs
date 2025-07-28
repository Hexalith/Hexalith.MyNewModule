// <copyright file="MyNewModulePriorityGeographicalPositionChanged.cs" company="ITANEO">
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
/// <param name="Position">The new geographical position of the mynewmodule.</param>
/// <param name="Temperature">The new temperature of the mynewmodule.</param>
[PolymorphicSerialization]
public partial record MyNewModulePriorityGeographicalPositionChanged(
    string Id,
    [property: DataMember(Order = 2)] GeographicalPosition? Position,
    [property: DataMember(Order = 3)] Temperature? Temperature)
    : MyNewModuleEvent(Id);