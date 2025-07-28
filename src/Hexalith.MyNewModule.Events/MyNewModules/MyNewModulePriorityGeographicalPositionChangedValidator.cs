// <copyright file="MyNewModulePriorityGeographicalPositionChangedValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Events.MyNewModules;

using System.Diagnostics.CodeAnalysis;

using FluentValidation;

using Hexalith.MyNewModule.Aggregates.Validators;

using Microsoft.Extensions.Localization;

using Labels = Localizations.MyNewModule;

/// <summary>
/// Validator for the <see cref="MyNewModulePriorityGeographicalPositionChanged"/> event.
/// </summary>
public class MyNewModulePriorityGeographicalPositionChangedValidator : AbstractValidator<MyNewModulePriorityGeographicalPositionChanged>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyNewModulePriorityGeographicalPositionChangedValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public MyNewModulePriorityGeographicalPositionChangedValidator([NotNull] IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);

        // Position validation: when provided, it must be valid
        _ = RuleFor(x => x.Position)
            .SetValidator(new GeographicPositionValidator(localizer)!)
            .When(x => x.Position is not null)
            .WithMessage(localizer[nameof(Labels.GeographicalPositionMustBeValid)]);

        // Temperature validation: must not be null when position is provided
        _ = RuleFor(x => x.Temperature)
            .NotNull()
            .When(x => x.Position is not null)
            .WithMessage(localizer[nameof(Labels.TemperatureMustBeNotNull)]);
    }
}