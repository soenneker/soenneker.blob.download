using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Soenneker.Blob.Client.Abstract;
using Soenneker.Blob.Download.Abstract;
using Soenneker.Extensions.Stream;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.MemoryStream.Abstract;
using Soenneker.Utils.Path.Abstract;

namespace Soenneker.Blob.Download;

///<inheritdoc cref="IBlobDownloadUtil"/>
public sealed class BlobDownloadUtil : IBlobDownloadUtil
{
    private readonly ILogger<BlobDownloadUtil> _logger;
    private readonly IBlobClientUtil _blobClientUtil;
    private readonly IMemoryStreamUtil _memoryStreamUtil;
    private readonly IPathUtil _pathUtil;

    public BlobDownloadUtil(IBlobClientUtil blobClientUtil, ILogger<BlobDownloadUtil> logger, IMemoryStreamUtil memoryStreamUtil, IPathUtil pathUtil)
    {
        _blobClientUtil = blobClientUtil;
        _logger = logger;
        _memoryStreamUtil = memoryStreamUtil;
        _pathUtil = pathUtil;
    }

    public async ValueTask<FileInfo> Download(string container, string relativeUrl, PublicAccessType publicAccessType = PublicAccessType.None, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        BlobClient blobClient = await GetClient(container, relativeUrl, publicAccessType, cancellationToken).NoSync();

        string downloadPath = await _pathUtil.GetRandomTempFilePath(".tmp", cancellationToken).NoSync();

        await blobClient.DownloadToAsync(downloadPath, cancellationToken).NoSync();

        _logger.LogDebug("Finished download from {relativeUrl}, stored at {location}", relativeUrl, downloadPath);

        return new FileInfo(downloadPath);
    }

    public async ValueTask<MemoryStream> DownloadToMemory(string container, string relativeUrl, PublicAccessType publicAccessType = PublicAccessType.None, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        BlobClient blobClient = await GetClient(container, relativeUrl, publicAccessType, cancellationToken).NoSync();

        MemoryStream memoryStream = await _memoryStreamUtil.Get(cancellationToken).NoSync();

        _ = await blobClient.DownloadToAsync(memoryStream, cancellationToken).NoSync();

        _logger.LogDebug("Finished download from {relativeUrl} into MemoryStream", relativeUrl);

        memoryStream.ToStart();

        return memoryStream;
    }

    public async ValueTask<string> DownloadToString(string container, string relativeUrl, PublicAccessType publicAccessType = PublicAccessType.None, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using MemoryStream memoryStream = await DownloadToMemory(container, relativeUrl, publicAccessType, cancellationToken).NoSync();

        return await memoryStream.ToStr(cancellationToken: cancellationToken).NoSync();
    }

    private async ValueTask<BlobClient> GetClient(string container, string relativeUrl, PublicAccessType publicAccessType, CancellationToken cancellationToken)
    {
        BlobClient blobClient = await _blobClientUtil.Get(container, relativeUrl, publicAccessType, cancellationToken).NoSync();

        _logger.LogDebug("Beginning to download Blob ({jobId}) ...",  Path.Combine(blobClient.Uri.ToString(), container, relativeUrl));

        if (!await blobClient.ExistsAsync(cancellationToken).NoSync())
        {
            // TODO: Handle this better?
            throw new Exception($"Specified blob does not exist in container ({container}) at relative path ({relativeUrl})");
        }

        return blobClient;
    }
}