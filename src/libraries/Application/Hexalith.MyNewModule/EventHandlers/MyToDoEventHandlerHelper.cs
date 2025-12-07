// <copyright file="MyToDoEventHandlerHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.EventHandlers;

using FluentValidation;

using Hexalith.MyNewModule.Commands.MyToDo;
using Hexalith.MyNewModule.Events.MyToDo;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class for adding mytodo event handlers to the service collection.
/// </summary>
public static class MyToDoEventHandlerHelper
{
    /// <summary>
    /// Adds the mytodo command validators to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyToDoCommandValidators(this IServiceCollection services)
    {
        _ = services.AddTransient<IValidator<AddMyToDo>, AddMyToDoValidator>();
        _ = services.AddTransient<IValidator<ChangeMyToDoDescription>, ChangeMyToDoDescriptionValidator>();
        _ = services.AddTransient<IValidator<DisableMyToDo>, DisableMyToDoValidator>();
        _ = services.AddTransient<IValidator<EnableMyToDo>, EnableMyToDoValidator>();
        return services;
    }

    /// <summary>
    /// Adds the mytodo event validators to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyToDoEventValidators(this IServiceCollection services)
    {
        _ = services.AddTransient<IValidator<MyToDoAdded>, MyToDoAddedValidator>();
        _ = services.AddTransient<IValidator<MyToDoDescriptionChanged>, MyToDoDescriptionChangedValidator>();
        _ = services.AddTransient<IValidator<MyToDoDisabled>, MyToDoDisabledValidator>();
        _ = services.AddTransient<IValidator<MyToDoEnabled>, MyToDoEnabledValidator>();
        return services;
    }

    /// <summary>
    /// Adds all mytodo validators (commands and events) to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyToDoValidators(this IServiceCollection services)
        => services
            .AddMyToDoCommandValidators()
            .AddMyToDoEventValidators();
}