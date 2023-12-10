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
using Soenneker.Utils.Cancellation.Abstract;
using Soenneker.Utils.FileSync;
using Soenneker.Utils.MemoryStream.Abstract;

namespace Soenneker.Blob.Download;

///<inheritdoc cref="IBlobDownloadUtil"/>
public class BlobDownloadUtil : IBlobDownloadUtil
{
    private readonly ILogger<BlobDownloadUtil> _logger;
    private readonly IBlobClientUtil _blobClientUtil;
    private readonly IMemoryStreamUtil _memoryStreamUtil;
    private readonly ICancellationUtil _cancellationUtil;
    
    public BlobDownloadUtil(IBlobClientUtil blobClientUtil, ILogger<BlobDownloadUtil> logger, IMemoryStreamUtil memoryStreamUtil, ICancellationUtil cancellationUtil)
    {
        _blobClientUtil = blobClientUtil;
        _logger = logger;
        _memoryStreamUtil = memoryStreamUtil;
        _cancellationUtil = cancellationUtil;
    }

    public async ValueTask<FileInfo> Download(string container, string relativeUrl, PublicAccessType publicAccessType = PublicAccessType.None)
    {
        BlobClient blobClient = await GetClient(container, relativeUrl, publicAccessType).NoSync();
        
        CancellationToken cancellationToken = _cancellationUtil.Get();

        string downloadPath = FileUtilSync.GetTempFileName();

        await blobClient.DownloadToAsync(downloadPath, cancellationToken).NoSync();

        _logger.LogDebug("Finished download from {relativeUrl}, stored at {location}", relativeUrl, downloadPath);

        return new FileInfo(downloadPath);
    }

    public async ValueTask<MemoryStream> DownloadToMemory(string container, string relativeUrl, PublicAccessType publicAccessType = PublicAccessType.None)
    {
        BlobClient blobClient = await GetClient(container, relativeUrl, publicAccessType).NoSync();

        MemoryStream memoryStream = await _memoryStreamUtil.Get().NoSync();
        CancellationToken cancellationToken = _cancellationUtil.Get();

        _ = await blobClient.DownloadToAsync(memoryStream, cancellationToken).NoSync();

        _logger.LogDebug("Finished download from {relativeUrl} into MemoryStream", relativeUrl);

        memoryStream.ToStart();

        return memoryStream;
    }

    public async ValueTask<string> DownloadToString(string container, string relativeUrl, PublicAccessType publicAccessType = PublicAccessType.None)
    {
        MemoryStream memoryStream = await DownloadToMemory(container, relativeUrl, publicAccessType).NoSync();

        string result = await memoryStream.ToStr().NoSync();

        return result;
    }

    private async ValueTask<BlobClient> GetClient(string container, string relativeUrl, PublicAccessType publicAccessType = PublicAccessType.None)
    {
        BlobClient blobClient = await _blobClientUtil.GetClient(container, relativeUrl, publicAccessType).NoSync();

        _logger.LogDebug("Beginning to download Blob ({jobId}) ...",  Path.Combine(blobClient.Uri.ToString(), container, relativeUrl));

        if (!await blobClient.ExistsAsync().NoSync())
        {
            // TODO: Handle this better?
            throw new Exception($"Specified blob does not exist in container ({container}) at relative path ({relativeUrl})");
        }

        return blobClient;
    }
}