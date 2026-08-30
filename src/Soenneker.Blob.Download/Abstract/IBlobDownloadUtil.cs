using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;

namespace Soenneker.Blob.Download.Abstract;

/// <summary>
/// Downloads blobs to temporary files, memory, or text.
/// </summary>
public interface IBlobDownloadUtil
{
    /// <summary>
    /// Downloads a blob to a randomly named temporary file.
    /// </summary>
    /// <param name="container">Name of the blob container.</param>
    /// <param name="relativeUrl">Path of the blob within the container.</param>
    /// <param name="publicAccessType">Public access level used if the container must be created.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The downloaded temporary file. The caller is responsible for deleting it.</returns>
    ValueTask<FileInfo> Download(string container, string relativeUrl, PublicAccessType publicAccessType = PublicAccessType.None,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads a blob into a stream positioned at the beginning.
    /// </summary>
    /// <param name="container">Name of the blob container.</param>
    /// <param name="relativeUrl">Path of the blob within the container.</param>
    /// <param name="publicAccessType">Public access level used if the container must be created.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The downloaded content. The caller is responsible for disposing the stream.</returns>
    ValueTask<MemoryStream> DownloadToMemory(string container, string relativeUrl, PublicAccessType publicAccessType = PublicAccessType.None,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads a blob and decodes its content as text.
    /// </summary>
    /// <param name="container">Name of the blob container.</param>
    /// <param name="relativeUrl">Path of the blob within the container.</param>
    /// <param name="publicAccessType">Public access level used if the container must be created.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The decoded blob content.</returns>
    ValueTask<string> DownloadToString(string container, string relativeUrl, PublicAccessType publicAccessType = PublicAccessType.None,
        CancellationToken cancellationToken = default);
}
