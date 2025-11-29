// <copyright file="MyNewModuleAdded.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Hexalith.MyNewModules.Events.MyNewModules;

using System.Runtime.Serialization;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when a new mynewmodule is added.
/// </summary>
/// <param name="Id">The identifier of the mynewmodule.</param>
/// <param name="Name">The name of the mynewmodule.</param>
/// <param name="Comments">Optional comments about the mynewmodule.</param>
[PolymorphicSerialization]
public partial record MyNewModuleAdded(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : MyNewModuleEvent(Id)
;