// <copyright file="StorageFile.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Servers.Services;

using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Documents.Application.Services;

/// <summary>
/// Represents a local storage file implementation.
/// </summary>
/// <param name="fileStream">The file stream used for reading and writing.</param>
/// <param name="filePath">The URL that identifies the location of the file.</param>
public class StorageFile(FileStream fileStream, string filePath) : IWritableFile, IReadableFile
{
    private Stream? _stream = fileStream;

    /// <inheritdoc/>
    public string FilePath { get; } = filePath;

    /// <inheritdoc/>
    public long Size { get; private set; }

    /// <inheritdoc/>
    public Stream Stream => _stream ?? throw new InvalidOperationException("The file is closed.");

    /// <inheritdoc/>
    public async Task<long> CloseAsync(CancellationToken cancellationToken)
    {
        if (_stream is null)
        {
            throw new InvalidOperationException("The file is already closed.");
        }

        await _stream.FlushAsync(cancellationToken).ConfigureAwait(false);
        Size = _stream.Length;
        _stream.Close();
        await _stream.DisposeAsync().ConfigureAwait(false);
        _stream = null;
        return Size;
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (_stream is not null)
        {
            _ = await CloseAsync(CancellationToken.None).ConfigureAwait(false);
        }

        GC.SuppressFinalize(this);
    }
}