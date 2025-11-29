// <copyright file="GetMyNewModuleIdDescription.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Hexalith.MyNewModules.Requests.MyNewModule;

using System.Runtime.Serialization;

using Hexalith.Domains.ValueObjects;
using Hexalith.MyNewModules.Requests.MyNewModules;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a request to get the identifier and description of a warehouse by its ID.
/// </summary>
/// <param name="Id">The ID of the warehouse.</param>
/// <param name="Result">The result containing the identifier and description.</param>
[PolymorphicSerialization]
public partial record GetMyNewModuleIdDescription(string Id, [property: DataMember(Order = 2)] IdDescription? Result = null)
    : MyNewModuleRequest(Id);