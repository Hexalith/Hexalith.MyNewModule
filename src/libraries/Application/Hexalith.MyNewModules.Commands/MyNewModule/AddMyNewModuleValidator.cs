// <copyright file="AddMyNewModuleValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModules.Commands.MyNewModule;

using FluentValidation;

using Microsoft.Extensions.Localization;

using Labels = Localizations.MyNewModule;

/// <summary>
/// Validator for MyNewModule.
/// </summary>
public class AddMyNewModuleValidator : AbstractValidator<AddMyNewModule>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddMyNewModuleValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public AddMyNewModuleValidator(IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);
        _ = RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localizer[Labels.NameRequired]);
        _ = RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer[Labels.IdRequired]);
    }
}