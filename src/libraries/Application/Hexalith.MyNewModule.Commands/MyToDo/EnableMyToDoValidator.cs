// <copyright file="EnableMyToDoValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Commands.MyToDo;

using FluentValidation;

using Microsoft.Extensions.Localization;

using Labels = Localizations.MyToDo;

/// <summary>
/// Validator for EnableMyToDo command.
/// </summary>
public class EnableMyToDoValidator : AbstractValidator<EnableMyToDo>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnableMyToDoValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public EnableMyToDoValidator(IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);
        _ = RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer[Labels.IdRequired]);
    }
}
