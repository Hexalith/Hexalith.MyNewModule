// <copyright file="EnableMyNewModule.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Hexalith.MyNewModule.Commands.MyNewModules;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command raised when a mynewmodule is enabled.
/// </summary>
/// <param name="Id">The identifier of the mynewmodule.</param>
[PolymorphicSerialization]
public partial record EnableMyNewModule(string Id) : MyNewModuleCommand(Id);