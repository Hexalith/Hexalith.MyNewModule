// <copyright file="MyNewModulePrioritySubmissionPeriod ChangedValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Events.Timesheets;

using System.Diagnostics.CodeAnalysis;

using FluentValidation;

using Hexalith.MyNewModule.Aggregates.Validators;
using Hexalith.MyNewModule.Events.MyNewModules;

using Microsoft.Extensions.Localization;

using Labels = Localizations.MyNewModule;

/// <summary>
/// Validator for the <see cref="MyNewModulePrioritySubmissionPeriod Changed"/> event.
/// </summary>
public class MyNewModulePrioritySubmissionPeriod ChangedValidator : AbstractValidator<MyNewModulePrioritySubmissionPeriod Changed>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyNewModulePrioritySubmissionPeriod ChangedValidator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public MyNewModulePrioritySubmissionPeriod ChangedValidator([NotNull] IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);

        // SubmissionPeriod  validation: when provided, it must be valid
        _ = RuleFor(x => x.SubmissionPeriod )
            .SetValidator(new GeographicSubmissionPeriod Validator(localizer)!)
            .When(x => x.SubmissionPeriod  is not null)
            .WithMessage(localizer[nameof(Labels.SubmissionPeriod MustBeValid)]);

        // Temperature validation: must not be null when position is provided
        _ = RuleFor(x => x.Temperature)
            .NotNull()
            .When(x => x.SubmissionPeriod  is not null)
            .WithMessage(localizer[nameof(Labels.TemperatureMustBeNotNull)]);
    }
}