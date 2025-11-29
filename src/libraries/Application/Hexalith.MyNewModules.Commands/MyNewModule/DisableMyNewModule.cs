// <copyright file="DisableMyNewModule.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModules.Commands.MyNewModule;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command raised when a mynewmodule is disabled.
/// </summary>
/// <param name="Id">The identifier of the mynewmodule.</param>
[PolymorphicSerialization]
public partial record DisableMyNewModule(string Id) : MyNewModuleCommand(Id);