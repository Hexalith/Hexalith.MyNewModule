// <copyright file="ChangeMyNewModulePriorityWeight.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Commands.MyNewModules;

using System.Runtime.Serialization;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command raised when a mynewmodule priority weight is changed.
/// </summary>
/// <param name="Id">The identifier of the mynewmodule.</param>
/// <param name="PriorityWeight">The priority weight of the mynewmodule.</param>
[PolymorphicSerialization]
public partial record ChangeMyNewModulePriorityWeight(
    string Id,
    [property: DataMember(Order = 2)] decimal PriorityWeight)
    : MyNewModuleCommand(Id);