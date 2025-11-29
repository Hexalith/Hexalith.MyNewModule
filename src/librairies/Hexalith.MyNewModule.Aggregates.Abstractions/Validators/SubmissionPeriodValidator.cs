// <copyright file="SubmissionPeriodValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Aggregates.Validators;

using FluentValidation;

using Hexalith.MyNewModule.Aggregates.ValueObjects;

using Microsoft.Extensions.Localization;

using Labels = Localizations.Timesheets;

/// <summary>
/// Validator for geographical positions.
/// </summary>
public class SubmissionPeriodValidator : AbstractValidator<SubmissionPeriod>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SubmissionPeriodValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public SubmissionPeriodValidator(IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);
        _ = RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage(localizer[Labels.SubmissionPeriodStartDateRequired]);
        _ = RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage(localizer[Labels.SubmissionPeriodEndDateRequired])
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage(localizer[Labels.SubmissionPeriodEndDateAfterStartDate]);
    }
}