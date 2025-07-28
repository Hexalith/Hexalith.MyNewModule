// <copyright file="GeographicPositionValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Aggregates.Validators;

using FluentValidation;

using Hexalith.MyNewModule.Aggregates.ValueObjects;

using Microsoft.Extensions.Localization;

using Labels = Localizations.MyNewModule;

/// <summary>
/// Validator for geographical positions.
/// </summary>
public class GeographicPositionValidator : AbstractValidator<GeographicalPosition>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GeographicPositionValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public GeographicPositionValidator(IStringLocalizer<Labels> localizer)
    {
        _ = RuleFor(x => x.Latitude)
            .NotNull()
            .WithMessage(localizer[nameof(Labels.LatitudeMustBeNotNull)])
            .InclusiveBetween(GeographicalPosition.MinLatitude, GeographicalPosition.MaxLatitude)
            .WithMessage(localizer[nameof(Labels.LatitudeMustBeInRange), GeographicalPosition.MinLatitude, GeographicalPosition.MaxLatitude]);
        _ = RuleFor(x => x.Longitude)
            .NotNull()
            .WithMessage(localizer[nameof(Labels.LongitudeMustBeNotNull)])
            .InclusiveBetween(GeographicalPosition.MinLongitude, GeographicalPosition.MaxLongitude)
            .WithMessage(localizer[nameof(Labels.LongitudeMustBeInRange), GeographicalPosition.MinLongitude, GeographicalPosition.MaxLongitude]);
    }
}