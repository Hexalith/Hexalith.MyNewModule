// <copyright file="MyNewModuleEditViewModel.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModules.UI.Pages.MyNewModule;

using System.Security.Claims;

using Hexalith.Application.Commands;
using Hexalith.Domains.ValueObjects;
using Hexalith.MyNewModules.Commands.MyNewModule;
using Hexalith.MyNewModules.Requests.MyNewModule;
using Hexalith.UI.Components;

/// <summary>
/// ViewModel for editing file types.
/// </summary>
internal sealed class MyNewModuleEditViewModel : IIdDescription, IEntityViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyNewModuleEditViewModel"/> class.
    /// </summary>
    /// <param name="details">The details of the file type.</param>
    public MyNewModuleEditViewModel(MyNewModuleDetailsViewModel details)
    {
        ArgumentNullException.ThrowIfNull(details);
        Id = details.Id;
        Original = details;
        Name = details.Name;
        Comments = details.Comments;
        Disabled = details.Disabled;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MyNewModuleEditViewModel"/> class.
    /// </summary>
    public MyNewModuleEditViewModel()
    : this(new MyNewModuleDetailsViewModel(
            string.Empty,
            string.Empty,
            string.Empty,
            false))
    {
    }

    /// <summary>
    /// Gets or sets the description of the file type.
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    /// Gets a value indicating whether the description has changed.
    /// </summary>
    public bool DescriptionChanged => Comments != Original.Comments || Name != Original.Name;

    /// <summary>
    /// Gets or sets a value indicating whether the file type is disabled.
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets a value indicating whether there are changes in the file type details.
    /// </summary>
    public bool HasChanges =>
        Id != Original.Id ||
        DescriptionChanged ||
        Disabled != Original.Disabled;

    /// <summary>
    /// Gets or sets the ID of the file type.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the file type.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets the original details of the file type.
    /// </summary>
    public MyNewModuleDetailsViewModel Original { get; }

    /// <inheritdoc/>
    string IIdDescription.Description => Name;

    /// <summary>
    /// Saves the file type details asynchronously.
    /// </summary>
    /// <param name="user">The user performing the save operation.</param>
    /// <param name="commandService">The command service to submit commands.</param>
    /// <param name="create">A value indicating whether to create a new file type.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    internal async Task SaveAsync(ClaimsPrincipal user, ICommandService commandService, bool create, CancellationToken cancellationToken)
    {
        MyNewModuleCommand myNewModuleCommand;
        if (create)
        {
            myNewModuleCommand = new AddMyNewModule(
                        Id!,
                        Name!,
                        Comments);
            await commandService.SubmitCommandAsync(user, myNewModuleCommand, cancellationToken).ConfigureAwait(false);
            return;
        }

        if (DescriptionChanged)
        {
            myNewModuleCommand = new ChangeMyNewModuleDescription(
            Id!,
            Name!,
            Comments);
            await commandService.SubmitCommandAsync(user, myNewModuleCommand, cancellationToken).ConfigureAwait(false);
        }

        if (Disabled != Original.Disabled && Disabled)
        {
            myNewModuleCommand = new DisableMyNewModule(Id);
            await commandService.SubmitCommandAsync(user, myNewModuleCommand, cancellationToken).ConfigureAwait(false);
        }

        if (Disabled != Original.Disabled && !Disabled)
        {
            myNewModuleCommand = new EnableMyNewModule(Id);
            await commandService.SubmitCommandAsync(user, myNewModuleCommand, cancellationToken).ConfigureAwait(false);
        }
    }
}