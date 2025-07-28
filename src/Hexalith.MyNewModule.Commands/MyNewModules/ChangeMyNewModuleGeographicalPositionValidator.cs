// <copyright file="ChangeMyNewModuleGeographicalPositionValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Commands.MyNewModules;

using System.Diagnostics.CodeAnalysis;

using FluentValidation;

using Hexalith.MyNewModule.Aggregates.Validators;

using Microsoft.Extensions.Localization;

using Labels = Localizations.MyNewModule;

/// <summary>
/// Validator for the <see cref="ChangeMyNewModuleGeographicalPosition"/> event.
/// </summary>
public class ChangeMyNewModuleGeographicalPositionValidator : AbstractValidator<ChangeMyNewModuleGeographicalPosition>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeMyNewModuleGeographicalPositionValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public ChangeMyNewModuleGeographicalPositionValidator([NotNull] IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);

        // Position validation: when provided, it must be valid
        _ = RuleFor(x => x.Position)
            .SetValidator(new GeographicPositionValidator(localizer)!)
            .When(x => x.Position is not null)
            .WithMessage(localizer[nameof(Labels.GeographicalPositionMustBeValid)]);
    }
}