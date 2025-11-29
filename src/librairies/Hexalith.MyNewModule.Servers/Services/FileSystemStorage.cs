// <copyright file="FileSystemStorage.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Servers.Services;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Threading;

using Hexalith.Documents.Application.Services;
using Hexalith.Documents.Servers.Services;

using Microsoft.Extensions.Logging;

/// <summary>
/// Provides methods for file system storage operations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="FileSystemStorage"/> class.
/// </remarks>
/// <param name="logger">The logger instance.</param>
public partial class FileSystemStorage(ILogger<FileSystemStorage> logger)
{
    private readonly ILogger _logger = logger;

    /// <summary>
    /// Creates a writable file asynchronously.
    /// </summary>
    /// <param name="storageRootPath">The root path of the storage.</param>
    /// <param name="path">The path where the file will be created.</param>
    /// <param name="fileName">The name of the file to be created.</param>
    /// <param name="tags">The tags associated with the file.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the writable file.</returns>
    [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "The Stream is in the Scope of the returned IWritableFile object")]
    public async Task<IWritableFile> CreateFileAsync(string storageRootPath, string path, string fileName, IEnumerable<(string Key, string? Value)> tags, CancellationToken cancellationToken)
    {
        string filePath = GetPath(storageRootPath, path, fileName, true);

        cancellationToken.ThrowIfCancellationRequested();

        await using StreamWriter tagsFile = File.CreateText(filePath + ".tags.json");
        await tagsFile.WriteAsync(JsonSerializer.Serialize(tags)).ConfigureAwait(false);
        tagsFile.Close();

        LogFileCreatedInformation(_logger, filePath);

        // Create the file.
        return new StorageFile(File.Create(filePath), filePath);
    }

    /// <summary>
    /// Reads a file asynchronously.
    /// </summary>
    /// <param name="storageRootPath">The root path of the storage.</param>
    /// <param name="path">The path where the file is located.</param>
    /// <param name="fileName">The name of the file to be read.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the readable file.</returns>
    public Task<IReadableFile> ReadFileAsync(string storageRootPath, string path, string fileName, CancellationToken cancellationToken)
    {
        string filePath = GetPath(storageRootPath, path, fileName, false);

        cancellationToken.ThrowIfCancellationRequested();

        LogFileReadInformation(_logger, filePath);

        // Create the file.
        return Task.FromResult<IReadableFile>(new StorageFile(File.OpenRead(filePath), filePath));
    }

    private static string GetPath([NotNull] string storageRootPath, [NotNull] string path, [NotNull] string fileName, bool create)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(storageRootPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        // Verify that the storage path value is relative.
        if (!Path.IsPathRooted(storageRootPath))
        {
            throw new ArgumentException("The storage root path must be rooted.", nameof(path));
        }

        // Verify that the path value is relative.
        if (Path.IsPathRooted(path))
        {
            throw new ArgumentException("The path must be relative.", nameof(path));
        }

        // Verify that the file name is compliant with the Linux file system.
        if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            throw new ArgumentException("The file name contains invalid characters.", nameof(fileName));
        }

        string filePath = Path.Combine(storageRootPath, path, fileName);
        string? folderPath = Path.GetDirectoryName(filePath);
        if (string.IsNullOrWhiteSpace(folderPath))
        {
            throw new InvalidOperationException($"The folder path for '{filePath}' is invalid.");
        }

        if (create)
        {
            if (!Directory.Exists(folderPath))
            {
                // Create the directory if it does not exist.
                _ = Directory.CreateDirectory(folderPath);
            }

            if (File.Exists(filePath))
            {
                throw new InvalidOperationException($"The file '{filePath}' already exists on the file system.");
            }
        }
        else if (!File.Exists(filePath))
        {
            throw new InvalidOperationException($"The file '{filePath}' does not exist on the file system.");
        }

        return filePath;
    }

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "File '{FilePath}' has been created.")]
    private static partial void LogFileCreatedInformation(ILogger logger, string filePath);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "File '{FilePath}' has been read.")]
    private static partial void LogFileReadInformation(ILogger logger, string filePath);
}