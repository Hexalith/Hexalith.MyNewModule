// <copyright file="ChangeMyNewModuleDescription.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModules.Commands.MyNewModule;

using System.Runtime.Serialization;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command raised when a mynewmodule description is changed.
/// </summary>
/// <param name="Id">The identifier of the mynewmodule.</param>
/// <param name="Name">The name of the mynewmodule.</param>
/// <param name="Comments">Optional comments about the mynewmodule.</param>
[PolymorphicSerialization]
public partial record ChangeMyNewModuleDescription(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : MyNewModuleCommand(Id);