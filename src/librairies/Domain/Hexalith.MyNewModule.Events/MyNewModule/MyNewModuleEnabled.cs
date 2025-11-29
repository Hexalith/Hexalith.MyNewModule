// <copyright file="MyNewModuleEnabled.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Hexalith.MyNewModule.Events.MyNewModules;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when a mynewmodule is enabled.
/// </summary>
/// <param name="Id">The identifier of the mynewmodule.</param>
[PolymorphicSerialization]
public partial record MyNewModuleEnabled(string Id) : MyNewModuleEvent(Id);