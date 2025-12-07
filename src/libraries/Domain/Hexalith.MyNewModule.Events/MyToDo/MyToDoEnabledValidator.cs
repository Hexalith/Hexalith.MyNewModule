// <copyright file="MyToDoEnabledValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Events.MyToDo;

using FluentValidation;

using Microsoft.Extensions.Localization;

using Labels = Localizations.MyToDo;

/// <summary>
/// Validator for MyToDoEnabled event.
/// </summary>
public class MyToDoEnabledValidator : AbstractValidator<MyToDoEnabled>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyToDoEnabledValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public MyToDoEnabledValidator(IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);
        _ = RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer[Labels.IdRequired]);
    }
}
