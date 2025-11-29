// <copyright file="MyNewModuleEditViewModel.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModules.UI.Pages.MyNewModule;

using System.Linq;
using System.Security.Claims;

using Hexalith.Application.Commands;
using Hexalith.Domains.ValueObjects;
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
    public MyNewModuleEditViewModel(FileTypeDetailsViewModel details)
    {
        ArgumentNullException.ThrowIfNull(details);
        Id = details.Id;
        Original = details;
        Name = details.Name;
        Comments = details.Comments;
        Disabled = details.Disabled;
        ContentType = details.ContentType;
        FileExtension = details.FileExtension;
        FileToTextConverter = details.FileToTextConverter;
        OtherContentTypes = [.. details.OtherContentTypes];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MyNewModuleEditViewModel"/> class.
    /// </summary>
    public MyNewModuleEditViewModel()
    : this(new FileTypeDetailsViewModel(
            string.Empty,
            string.Empty,
            string.Empty,
            [],
            string.Empty,
            null,
            null,
            false))
    {
    }

    /// <summary>
    /// Gets or sets the description of the file type.
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    /// Gets or sets the content type of the file.
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// Gets a value indicating whether the description has changed.
    /// </summary>
    public bool DescriptionChanged => Comments != Original.Comments || Name != Original.Name;

    /// <summary>
    /// Gets or sets a value indicating whether the file type is disabled.
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the file extension of the file type.
    /// </summary>
    public string FileExtension { get; set; }

    /// <summary>
    /// Gets or sets the file to text converter.
    /// </summary>
    public string? FileToTextConverter { get; set; }

    /// <summary>
    /// Gets a value indicating whether the file to text converter has changed.
    /// </summary>
    public bool FileToTextConverterChanged => FileToTextConverter != Original.FileToTextConverter;

    /// <summary>
    /// Gets a value indicating whether there are changes in the file type details.
    /// </summary>
    public bool HasChanges =>
        Id != Original.Id ||
        DescriptionChanged ||
        FileToTextConverterChanged ||
        OtherContentTypesChanged ||
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
    public FileTypeDetailsViewModel Original { get; }

    /// <summary>
    /// Gets or sets the targets associated with the file type.
    /// </summary>
    public ICollection<string> OtherContentTypes { get; set; }

    /// <summary>
    /// Gets a value indicating whether the targets have changed.
    /// </summary>
    public bool OtherContentTypesChanged => !OtherContentTypes.SequenceEqual(Original.OtherContentTypes);

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
        FileTypeCommand fileTypeCommand;
        if (create)
        {
            fileTypeCommand = new AddFileType(
                        Id!,
                        Name!,
                        ContentType,
                        OtherContentTypes,
                        FileExtension,
                        Comments,
                        FileToTextConverter);
            await commandService.SubmitCommandAsync(user, fileTypeCommand, cancellationToken).ConfigureAwait(false);
            return;
        }

        if (DescriptionChanged)
        {
            fileTypeCommand = new ChangeFileTypeDescription(
            Id!,
            Name!,
            Comments);
            await commandService.SubmitCommandAsync(user, fileTypeCommand, cancellationToken).ConfigureAwait(false);
        }

        if (ContentType != Original.ContentType)
        {
            fileTypeCommand = new ChangeFileTypeContentType(Id!, ContentType);
            await commandService.SubmitCommandAsync(user, fileTypeCommand, cancellationToken).ConfigureAwait(false);
        }

        if (FileExtension != Original.FileExtension)
        {
            fileTypeCommand = new ChangeFileTypeFileExtension(Id!, FileExtension);
            await commandService.SubmitCommandAsync(user, fileTypeCommand, cancellationToken).ConfigureAwait(false);
        }

        if (FileToTextConverterChanged)
        {
            fileTypeCommand = new ChangeFileTypeFileToTextConverter(
            Id!,
            FileToTextConverter);
            await commandService.SubmitCommandAsync(user, fileTypeCommand, cancellationToken).ConfigureAwait(false);
        }

        if (Disabled != Original.Disabled && Disabled)
        {
            fileTypeCommand = new DisableFileType(Id);
            await commandService.SubmitCommandAsync(user, fileTypeCommand, cancellationToken).ConfigureAwait(false);
        }

        if (Disabled != Original.Disabled && !Disabled)
        {
            fileTypeCommand = new EnableFileType(Id);
            await commandService.SubmitCommandAsync(user, fileTypeCommand, cancellationToken).ConfigureAwait(false);
        }

        foreach (string? target in OtherContentTypes.Where(target => !Original.OtherContentTypes.Contains(target)))
        {
            // for each content type in other content types, add it if it does not exist
            fileTypeCommand = new AddFileTypeOtherContentType(Id, target);
            await commandService.SubmitCommandAsync(user, fileTypeCommand, cancellationToken).ConfigureAwait(false);
        }

        foreach (string? target in Original.OtherContentTypes.Where(target => !OtherContentTypes.Contains(target)))
        {
            // for each content type in Original.OtherContentTypes, remove it if it does not exist
            fileTypeCommand = new RemoveFileTypeOtherContentType(Id, target);
            await commandService.SubmitCommandAsync(user, fileTypeCommand, cancellationToken).ConfigureAwait(false);
        }
    }
}