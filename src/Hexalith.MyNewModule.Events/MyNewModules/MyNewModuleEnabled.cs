// <copyright file="MyNewModuleEnabled.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Events.MyNewModules;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when a mynewmodule is enabled.
/// </summary>
/// <param name="Id">The identifier of the mynewmodule.</param>
[PolymorphicSerialization]
public partial record MyNewModuleEnabled(string Id) : MyNewModuleEvent(Id);