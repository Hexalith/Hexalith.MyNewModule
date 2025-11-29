// <copyright file="AwsS3BucketStorage.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Servers.Services;

using System;
using System.Threading;

using Hexalith.Documents.Application.Services;

/// <summary>
/// Provides methods to interact with AWS S3 bucket storage.
/// </summary>
public class AwsS3BucketStorage
{
    /// <summary>
    /// Creates a file in the specified AWS S3 bucket.
    /// </summary>
    /// <param name="connectionString">The connection string to the AWS S3 bucket.</param>
    /// <param name="path">The path within the bucket where the file will be created.</param>
    /// <param name="fileName">The name of the file to be created.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the writable file.</returns>
    /// <exception cref="NotImplementedException">This method is not implemented.</exception>
    public Task<IWritableFile> CreateFileAsync(string connectionString, string path, string fileName, CancellationToken cancellationToken) => throw new NotImplementedException();

    /// <summary>
    /// Reads a file from the specified AWS S3 bucket.
    /// </summary>
    /// <param name="connectionString">The connection string to the AWS S3 bucket.</param>
    /// <param name="path">The path within the bucket where the file is located.</param>
    /// <param name="fileName">The name of the file to be read.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the readable file.</returns>
    /// <exception cref="NotImplementedException">This method is not implemented.</exception>
    public Task<IReadableFile> ReadFileAsync(string connectionString, string path, string fileName, CancellationToken cancellationToken) => throw new NotImplementedException();
}