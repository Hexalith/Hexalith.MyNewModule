// <copyright file="ChangeMyNewModuleSubmissionPeriod Validator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Commands.Timesheets;

using System.Diagnostics.CodeAnalysis;

using FluentValidation;

using Hexalith.MyNewModule.Aggregates.Validators;
using Hexalith.MyNewModule.Commands.MyNewModules;

using Microsoft.Extensions.Localization;

using Labels = Localizations.MyNewModule;

/// <summary>
/// Validator for the <see cref="ChangeMyNewModuleSubmissionPeriod "/> event.
/// </summary>
public class ChangeMyNewModuleSubmissionPeriod Validator : AbstractValidator<ChangeMyNewModuleSubmissionPeriod >
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeMyNewModuleSubmissionPeriod Validator"/> class.
    /// </summary>
    /// <param name="localizer">The localizer for validation messages.</param>
    public ChangeMyNewModuleSubmissionPeriod Validator([NotNull] IStringLocalizer<Labels> localizer)
    {
        ArgumentNullException.ThrowIfNull(localizer);

        // SubmissionPeriod  validation: when provided, it must be valid
        _ = RuleFor(x => x.SubmissionPeriod )
            .SetValidator(new GeographicSubmissionPeriod Validator(localizer)!)
            .When(x => x.SubmissionPeriod  is not null)
            .WithMessage(localizer[nameof(Labels.SubmissionPeriod MustBeValid)]);
    }
}