// <copyright file="MyNewModuleEventHandlerHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModules.EventHandlers;

using FluentValidation;

using Hexalith.MyNewModules.Commands.MyNewModule;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class for adding mynewmodule event handlers to the service collection.
/// </summary>
public static class MyNewModuleEventHandlerHelper
{
    /// <summary>
    /// Adds the mynewmodule event validators to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyNewModuleEventValidators(this IServiceCollection services)
            => services.AddTransient<IValidator<AddMyNewModule>, AddMyNewModuleValidator>();
}