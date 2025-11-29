// <copyright file="AddMyNewModule.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Hexalith.MyNewModules.Commands.MyNewModule;

using System.Runtime.Serialization;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command raised when a new mynewmodule is added.
/// </summary>
/// <param name="Id">The identifier of the mynewmodule.</param>
/// <param name="Name">The name of the mynewmodule.</param>
/// <param name="Comments">Optional comments about the mynewmodule.</param>
/// <param name="PriorityWeight">The priority weight of the mynewmodule.</param>
[PolymorphicSerialization]
public partial record AddMyNewModule(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments,
    [property: DataMember(Order = 4)] decimal PriorityWeight)
    : MyNewModuleCommand(Id);