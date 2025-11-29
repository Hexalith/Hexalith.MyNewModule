// <copyright file="IMyNewModuleService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.MyNewModules;

using System.Threading.Tasks;

/// <summary>
/// Interface for MyNewModule upload service.
/// </summary>
public interface IMyNewModuleService
{
    /// <summary>
    /// Performs an asynchronous operation related to MyNewModule.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DoSomethingAsync(CancellationToken cancellationToken);
}