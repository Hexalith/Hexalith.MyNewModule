// <copyright file="DocumentUploadService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Servers.Services;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Services;
using Hexalith.Application.Sessions.Models;
using Hexalith.Application.Sessions.Services;
using Hexalith.MyNewModule.Application;
using Hexalith.MyNewModule.Application.Services;
using Hexalith.MyNewModule.DocumentContainers;
using Hexalith.MyNewModule.MyNewModuletorages;
using Hexalith.MyNewModule.DocumentTypes;
using Hexalith.MyNewModule.FileTypes;
using Hexalith.MyNewModule.ValueObjects;
using Hexalith.Domain.Events;

using Microsoft.Extensions.Logging;

/// <summary>
/// Service for uploading MyNewModule.
/// </summary>
public partial class DocumentUploadService : IDocumentUploadService
{
    private readonly IAggregateService _aggregateService;
    private readonly ILogger<DocumentUploadService> _logger;
    private readonly ISessionService _sessionService;
    private readonly IWritableFileProvider _writableFileProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentUploadService"/> class.
    /// </summary>
    /// <param name="sessionService">The session service.</param>
    /// <param name="aggregateService">The aggregate service.</param>
    /// <param name="writableFileProvider">The writable file provider.</param>
    /// <param name="logger">The logger.</param>
    public DocumentUploadService(
        [NotNull] ISessionService sessionService,
        [NotNull] IAggregateService aggregateService,
        [NotNull] IWritableFileProvider writableFileProvider,
        [NotNull] ILogger<DocumentUploadService> logger)
    {
        ArgumentNullException.ThrowIfNull(sessionService);
        ArgumentNullException.ThrowIfNull(aggregateService);
        ArgumentNullException.ThrowIfNull(writableFileProvider);
        ArgumentNullException.ThrowIfNull(logger);
        _sessionService = sessionService;
        _aggregateService = aggregateService;
        _writableFileProvider = writableFileProvider;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task UploadDocumentAsync(
        string correlationId,
        string userId,
        string documentContainerId,
        string documentId,
        string documentTypeId,
        string fileTypeId,
        string fileName,
        IEnumerable<DocumentTag> tags,
        Stream fileContent,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentException.ThrowIfNullOrWhiteSpace(documentContainerId);
        ArgumentException.ThrowIfNullOrWhiteSpace(documentId);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(fileContent);
        cancellationToken.ThrowIfCancellationRequested();
        SessionInformation session = await _sessionService
            .GetAsync(userId, cancellationToken)
            .ConfigureAwait(false);
        Task<DocumentType> documentTypeTask = GetDocumentTypeAsync(documentTypeId, session.PartitionId, cancellationToken);
        Task<FileType> fileTypeTask = GetFileTypeAsync(fileTypeId, session.PartitionId, cancellationToken);
        DocumentContainer container = await GetContainerAsync(documentContainerId, session.PartitionId, cancellationToken).ConfigureAwait(false);
        MyNewModuletorage storage = await GetStorageAsync(container.MyNewModuletorageId, session.PartitionId, cancellationToken).ConfigureAwait(false);
        string path = Path.Combine(container.Path, documentId);
        FileType fileType = await fileTypeTask.ConfigureAwait(false);
        DocumentType documentType = await documentTypeTask.ConfigureAwait(false);
        IEnumerable<DocumentTag> fileTags = [
            ..tags,
            ..container.Tags,
            ..documentType.Tags,
            new("UserId", userId, true),
            new("CorrelationId", correlationId, true),
            new("SessionId", session.SessionId, true),
            new("PartitionId", session.PartitionId, true),
            new("DocumentId", documentId, true),
            new("FileName", fileName, true),
            new("FilePath", path, true),
            new("FileContentType", fileType.ContentType, true),
            new("DocumentContainerId", documentContainerId, true),
            new("MyNewModuletorageId", container.MyNewModuletorageId, true)];
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
        await using IWritableFile file = await _writableFileProvider
            .CreateFileAsync(
                storage.StorageType,
                storage.ConnectionString,
                path,
                fileName,
                fileTags.Select(t => (t.Key, t.Value)),
                cancellationToken).ConfigureAwait(false);
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
        await fileContent.CopyToAsync(file.Stream, cancellationToken).ConfigureAwait(false);
        _ = await file.CloseAsync(cancellationToken).ConfigureAwait(false);
        LogFileUploadedInformation(_logger, userId, documentId, fileName, path, session.PartitionId);
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "File '{FileName}' uploaded at '{FilePath}' by '{UserId}' for document '{DocumentId}' in partition '{PartitionId}'.")]
    private static partial void LogFileUploadedInformation(ILogger logger, string userId, string documentId, string fileName, string filePath, string partitionId);

    private async Task<DocumentContainer> GetContainerAsync(string documentContainerId, string partitionId, CancellationToken cancellationToken)
    {
        string globalId = Metadata.CreateAggregateGlobalId(
            partitionId,
            DocumentDomainHelper.DocumentContainerAggregateName,
            documentContainerId);
        SnapshotEvent? snapshot = await _aggregateService
            .GetSnapshotAsync(DocumentDomainHelper.DocumentContainerAggregateName, globalId, cancellationToken)
            .ConfigureAwait(false) ?? throw new InvalidOperationException($"Document container '{documentContainerId}' not found.");
        return snapshot.GetAggregate<DocumentContainer>();
    }

    private async Task<DocumentType> GetDocumentTypeAsync(string documentTypeId, string partitionId, CancellationToken cancellationToken)
    {
        string globalId = Metadata.CreateAggregateGlobalId(
            partitionId,
            DocumentDomainHelper.DocumentTypeAggregateName,
            documentTypeId);
        SnapshotEvent? snapshot = await _aggregateService
            .GetSnapshotAsync(DocumentDomainHelper.DocumentTypeAggregateName, globalId, cancellationToken)
            .ConfigureAwait(false) ?? throw new InvalidOperationException($"Document type '{documentTypeId}' not found.");
        return snapshot.GetAggregate<DocumentType>();
    }

    private async Task<FileType> GetFileTypeAsync(string documentTypeId, string partitionId, CancellationToken cancellationToken)
    {
        string globalId = Metadata.CreateAggregateGlobalId(
            partitionId,
            DocumentDomainHelper.FileTypeAggregateName,
            documentTypeId);
        SnapshotEvent? snapshot = await _aggregateService
            .GetSnapshotAsync(DocumentDomainHelper.FileTypeAggregateName, globalId, cancellationToken)
            .ConfigureAwait(false) ?? throw new InvalidOperationException($"File type '{documentTypeId}' not found.");
        return snapshot.GetAggregate<FileType>();
    }

    private async Task<MyNewModuletorage> GetStorageAsync(string MyNewModuletorageId, string partitionId, CancellationToken cancellationToken)
    {
        string globalId = Metadata.CreateAggregateGlobalId(
            partitionId,
            DocumentDomainHelper.MyNewModuletorageAggregateName,
            MyNewModuletorageId);
        SnapshotEvent? snapshot = await _aggregateService
            .GetSnapshotAsync(DocumentDomainHelper.MyNewModuletorageAggregateName, globalId, cancellationToken)
            .ConfigureAwait(false) ?? throw new InvalidOperationException($"Document storage '{MyNewModuletorageId}' not found.");
        return snapshot.GetAggregate<MyNewModuletorage>();
    }
}