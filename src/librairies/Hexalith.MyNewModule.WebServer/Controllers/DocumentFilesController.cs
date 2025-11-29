// <copyright file="DocumentFilesController.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.WebServer.Controllers;

using Hexalith.Application.Requests;
using Hexalith.Application.Services;
using Hexalith.MyNewModule.Application.Services;
using Hexalith.MyNewModule.DocumentContainers;
using Hexalith.MyNewModule.MyNewModule;
using Hexalith.MyNewModule.MyNewModuletorages;
using Hexalith.MyNewModule.Requests.DocumentContainers;
using Hexalith.MyNewModule.Requests.MyNewModule;
using Hexalith.MyNewModule.Requests.MyNewModuletorages;
using Hexalith.MyNewModule.ValueObjects;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller for handling document file operations.
/// </summary>
[ApiController]
[Route("MyNewModule")]
[Authorize]
public class DocumentFilesController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IReadableFileProvider _readableFileProvider;
    private readonly IRequestService _requestService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentFilesController"/> class.
    /// </summary>
    /// <param name="readableFileProvider">The readable file provider.</param>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <param name="requestService">The request service.</param>
    public DocumentFilesController(
        IReadableFileProvider readableFileProvider,
        IHttpContextAccessor httpContextAccessor,
        IRequestService requestService)
    {
        ArgumentNullException.ThrowIfNull(requestService);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(readableFileProvider);
        _readableFileProvider = readableFileProvider;
        _httpContextAccessor = httpContextAccessor;
        _requestService = requestService;
    }

    /// <summary>
    /// Downloads the file with the specified document ID.
    /// </summary>
    /// <param name="documentId">The document ID.</param>
    /// <returns>The file to download.</returns>
    [HttpGet("download/{documentId}")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Avoid on async disposable")]
    public async Task<IActionResult> DownloadFileAsync(string documentId)
    {
        System.Security.Claims.ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            return Unauthorized();
        }

        DocumentDetailsViewModel? document = (await _requestService
            .SubmitAsync(user, new GetDocumentDetails(documentId), CancellationToken.None)
            .ConfigureAwait(false))?.Result;

        if (document == null)
        {
            return NotFound("Document not found.");
        }

        if (document.Files == null)
        {
            return NotFound("Document does not contain any files.");
        }

        if (string.IsNullOrWhiteSpace(document.Description.DocumentContainerId))
        {
            return BadRequest("Document storage container ID is not defined. A valid container ID is required to retrieve the document file.");
        }

        DocumentContainerDetailsViewModel? container = (await _requestService
            .SubmitAsync(user, new GetDocumentContainerDetails(document.Description.DocumentContainerId), CancellationToken.None)
            .ConfigureAwait(false))?.Result;

        if (container == null)
        {
            return NotFound($"Document container with ID {document.Description.DocumentContainerId} not found.");
        }

        if (string.IsNullOrWhiteSpace(container.MyNewModuletorageId))
        {
            return BadRequest($"Document container with ID {document.Description.DocumentContainerId} storage is not defined.");
        }

        MyNewModuletorageDetailsViewModel? storage = (await _requestService
            .SubmitAsync(user, new GetMyNewModuletorageDetails(container.MyNewModuletorageId), CancellationToken.None)
            .ConfigureAwait(false))?.Result;

        if (storage == null)
        {
            return NotFound($"Document storage with ID {container.MyNewModuletorageId} not found. Check container {document.Description.DocumentContainerId} configuration.");
        }

        FileDescription fileDescription = document.Files.First();
        IReadableFile file = await _readableFileProvider
            .OpenFileAsync(
                storage.StorageType,
                storage.ConnectionString,
                Path.Combine(container.Path, document.Id),
                fileDescription.Name,
                CancellationToken.None)
            .ConfigureAwait(false);

        return File(file.Stream, fileDescription.ContentType, fileDescription.Name);
    }

    /// <summary>
    /// Downloads the file using an access key.
    /// </summary>
    /// <param name="partitionId">The partition ID.</param>
    /// <param name="documentId">The document ID.</param>
    /// <param name="key">The access key.</param>
    /// <param name="aggregateService">The aggregate service.</param>
    /// <param name="timeProvider">The time provider.</param>
    /// <returns>The file to download.</returns>
    [AllowAnonymous]
    [HttpGet("download/{partitionId}/{documentId}/{key}")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "Avoid on async disposable")]
    public async Task<IActionResult> DownloadFileAsync(
        string partitionId,
        string documentId,
        string key,
        [FromServices] IAggregateService aggregateService,
        [FromServices] TimeProvider timeProvider)
    {
        if (string.IsNullOrWhiteSpace(partitionId))
        {
            return BadRequest("Partition identifier is required.");
        }

        if (string.IsNullOrWhiteSpace(documentId))
        {
            return BadRequest("Document identifier is required.");
        }

        if (string.IsNullOrWhiteSpace(key))
        {
            return BadRequest("Document access key is required.");
        }

        Document? document = await aggregateService
            .FindAsync(new Document() with { Id = documentId }, partitionId, CancellationToken.None)
            .ConfigureAwait(false);

        if (document is null)
        {
            return NotFound("Document not found.");
        }

        if (!document.AccessKeys.Any(p => p.Key == key && p.ValidUntil > timeProvider.GetUtcNow()))
        {
            return Unauthorized("Access key is invalid or expired.");
        }

        if (!document.Files.Any())
        {
            return NotFound("Document does not contain any files.");
        }

        if (string.IsNullOrWhiteSpace(document.Description.DocumentContainerId))
        {
            return NotFound("The document container ID is not defined for this document. A valid container ID is required to retrieve the document file.");
        }

        DocumentContainer? container = await aggregateService
            .FindAsync(new DocumentContainer() with { Id = document.Description.DocumentContainerId }, partitionId, CancellationToken.None)
            .ConfigureAwait(false);

        if (container is null)
        {
            return NotFound($"Document container {document.Description.DocumentContainerId} not found.");
        }

        if (string.IsNullOrWhiteSpace(container.MyNewModuletorageId))
        {
            return NotFound("The document storage ID is not defined for this document. A valid storage ID is required to retrieve the document file.");
        }

        MyNewModuletorage? storage = await aggregateService
            .FindAsync(new MyNewModuletorage() with { Id = container.MyNewModuletorageId }, partitionId, CancellationToken.None)
            .ConfigureAwait(false);

        if (storage is null)
        {
            return NotFound($"Document storage {container.MyNewModuletorageId} not found.");
        }

        FileDescription fileDescription = document.Files.First();
        IReadableFile file = await _readableFileProvider
            .OpenFileAsync(
                storage.StorageType,
                storage.ConnectionString,
                Path.Combine(container.Path, document.Id),
                fileDescription.Name,
                CancellationToken.None)
            .ConfigureAwait(false);

        return File(file.Stream, fileDescription.ContentType, fileDescription.Name);
    }
}