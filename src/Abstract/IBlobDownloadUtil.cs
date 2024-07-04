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
    [Pure]
    ValueTask<FileInfo> Download(string container, string relativeUrl, PublicAccessType publicAccessType = PublicAccessType.None, CancellationToken cancellationToken = default) ;

    /// <summary>
    /// Ready-to-read MemoryStream (Position 0)
    /// </summary>
    [Pure]
    ValueTask<MemoryStream> DownloadToMemory(string container, string relativeUrl, PublicAccessType publicAccessType = PublicAccessType.None, CancellationToken cancellationToken = default);

    [Pure]
    ValueTask<string> DownloadToString(string container, string relativeUrl, PublicAccessType publicAccessType = PublicAccessType.None, CancellationToken cancellationToken = default);
}