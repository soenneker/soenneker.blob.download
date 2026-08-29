using System.Diagnostics.Contracts;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;

namespace Soenneker.Blob.Download.Abstract;

/// <summary>
/// A utility library for Azure Blob download operations <para/>
/// Typically Scoped IoC.
/// </summary>
public interface IBlobDownloadUtil
{
    /// <summary>
    /// Downloads to a particular file on the host server as a temp file
    /// </summary>
    /// <param name="container">Element that will contain the rendered component.</param>
    /// <param name="relativeUrl">URL of the relative to target.</param>
    /// <param name="publicAccessType">Blob-container public access level to require.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested file Info.</returns>
    [Pure]
    ValueTask<FileInfo> Download(string container, string relativeUrl, PublicAccessType publicAccessType = PublicAccessType.None, CancellationToken cancellationToken = default) ;

    /// <summary>
    /// Ready-to-read MemoryStream (Position 0)
    /// </summary>
    /// <param name="container">Element that will contain the rendered component.</param>
    /// <param name="relativeUrl">URL of the relative to target.</param>
    /// <param name="publicAccessType">Blob-container public access level to require.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested memory Stream.</returns>
    [Pure]
    ValueTask<MemoryStream> DownloadToMemory(string container, string relativeUrl, PublicAccessType publicAccessType = PublicAccessType.None, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads to String.
    /// </summary>
    /// <param name="container">Element that will contain the rendered component.</param>
    /// <param name="relativeUrl">URL of the relative to target.</param>
    /// <param name="publicAccessType">Blob-container public access level to require.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the text returned by download To String.</returns>
    [Pure]
    ValueTask<string> DownloadToString(string container, string relativeUrl, PublicAccessType publicAccessType = PublicAccessType.None, CancellationToken cancellationToken = default);
}
