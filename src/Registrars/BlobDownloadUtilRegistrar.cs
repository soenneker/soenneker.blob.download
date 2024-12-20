using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blob.Client.Registrars;
using Soenneker.Blob.Download.Abstract;
using Soenneker.Utils.MemoryStream.Registrars;

namespace Soenneker.Blob.Download.Registrars;

/// <summary>
/// A utility library for Azure Blob storage download operations
/// </summary>
public static class BlobDownloadUtilRegistrar
{
    public static void AddBlobDownloadUtilAsScoped(this IServiceCollection services)
    {
        services.AddMemoryStreamUtil();
        services.AddBlobClientUtilAsSingleton();
        services.TryAddScoped<IBlobDownloadUtil, BlobDownloadUtil>();
    }


    public static void AddBlobDownloadUtilAsSingleton(this IServiceCollection services)
    {
        services.AddMemoryStreamUtil();
        services.AddBlobClientUtilAsSingleton();
        services.TryAddSingleton<IBlobDownloadUtil, BlobDownloadUtil>();
    }
}