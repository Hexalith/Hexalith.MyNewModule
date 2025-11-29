// <copyright file="GetMyNewModuleDetails.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace Hexalith.MyNewModules.Requests.MyNewModule;

using System.Runtime.Serialization;

using Hexalith.MyNewModules.Requests.MyNewModules;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a request to get the details of a warehouse by its ID.
/// </summary>
/// <param name="Id">The ID of the warehouse.</param>
/// <param name="Result">The warehouse details view model result.</param>
[PolymorphicSerialization]
public partial record GetMyNewModuleDetails(string Id, [property: DataMember(Order = 2)] MyNewModuleDetailsViewModel? Result = null)
    : MyNewModuleRequest(Id);