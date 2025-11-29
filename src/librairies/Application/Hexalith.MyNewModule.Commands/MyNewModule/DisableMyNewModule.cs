// <copyright file="DisableMyNewModule.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Hexalith.MyNewModule.Commands.MyNewModules;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command raised when a mynewmodule is disabled.
/// </summary>
/// <param name="Id">The identifier of the mynewmodule.</param>
[PolymorphicSerialization]
public partial record DisableMyNewModule(string Id) : MyNewModuleCommand(Id);