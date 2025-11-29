// <copyright file="WritableFileProvider.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Servers.Services;

using System;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Documents.Application.Services;
using Hexalith.Documents.ValueObjects;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides functionality to create writable files.
/// </summary>
public class WritableFileProvider : IWritableFileProvider, IReadableFileProvider
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="WritableFileProvider"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider instance used to resolve dependencies.</param>
    public WritableFileProvider(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public async Task<IWritableFile> CreateFileAsync(
        DocumentStorageType storageType,
        string? connectionString,
        string path,
        string fileName,
        IEnumerable<(string Key, string? Value)> tags,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        return storageType switch
        {
            DocumentStorageType.AzureStorageContainer
                => await _serviceProvider.GetRequiredService<AzureContainerStorage>().CreateFileAsync(connectionString, path, fileName, cancellationToken).ConfigureAwait(false),
            DocumentStorageType.OneDrive
                => await _serviceProvider.GetRequiredService<OneDriveStorage>().CreateFileAsync(connectionString, path, fileName, cancellationToken).ConfigureAwait(false),
            DocumentStorageType.FileSystem
                => await _serviceProvider.GetRequiredService<FileSystemStorage>().CreateFileAsync(connectionString, path, fileName, tags, cancellationToken).ConfigureAwait(false),
            DocumentStorageType.Dropbox
                => await _serviceProvider.GetRequiredService<DropboxStorage>().CreateFileAsync(connectionString, path, fileName, cancellationToken).ConfigureAwait(false),
            DocumentStorageType.GoogleDrive
                => await _serviceProvider.GetRequiredService<GoogleDriveStorage>().CreateFileAsync(connectionString, path, fileName, cancellationToken).ConfigureAwait(false),
            DocumentStorageType.AwsS3Bucket
                => await _serviceProvider.GetRequiredService<AwsS3BucketStorage>().CreateFileAsync(connectionString, path, fileName, cancellationToken).ConfigureAwait(false),
            DocumentStorageType.Sharepoint
                => await _serviceProvider.GetRequiredService<SharepointStorage>().CreateFileAsync(connectionString, path, fileName, cancellationToken).ConfigureAwait(false),
            _ => throw new NotSupportedException($"Storage type {storageType} is not supported."),
        };
    }

    /// <inheritdoc/>
    public async Task<IReadableFile> OpenFileAsync(DocumentStorageType storageType, string? connectionString, string path, string fileName, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        return storageType switch
        {
            DocumentStorageType.AzureStorageContainer
                => await _serviceProvider.GetRequiredService<AzureContainerStorage>().ReadFileAsync(connectionString, path, fileName, cancellationToken).ConfigureAwait(false),
            DocumentStorageType.OneDrive
                => await _serviceProvider.GetRequiredService<OneDriveStorage>().ReadFileAsync(connectionString, path, fileName, cancellationToken).ConfigureAwait(false),
            DocumentStorageType.FileSystem
                => await _serviceProvider.GetRequiredService<FileSystemStorage>().ReadFileAsync(connectionString, path, fileName, cancellationToken).ConfigureAwait(false),
            DocumentStorageType.Dropbox
                => await _serviceProvider.GetRequiredService<DropboxStorage>().ReadFileAsync(connectionString, path, fileName, cancellationToken).ConfigureAwait(false),
            DocumentStorageType.GoogleDrive
                => await _serviceProvider.GetRequiredService<GoogleDriveStorage>().ReadFileAsync(connectionString, path, fileName, cancellationToken).ConfigureAwait(false),
            DocumentStorageType.AwsS3Bucket
                => await _serviceProvider.GetRequiredService<AwsS3BucketStorage>().ReadFileAsync(connectionString, path, fileName, cancellationToken).ConfigureAwait(false),
            DocumentStorageType.Sharepoint
                => await _serviceProvider.GetRequiredService<SharepointStorage>().ReadFileAsync(connectionString, path, fileName, cancellationToken).ConfigureAwait(false),
            _ => throw new NotSupportedException($"Storage type {storageType} is not supported."),
        };
    }
}