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
    public static IServiceCollection AddBlobDownloadUtilAsScoped(this IServiceCollection services)
    {
        services.AddMemoryStreamUtilAsSingleton()
                .AddBlobClientUtilAsSingleton();
        services.TryAddScoped<IBlobDownloadUtil, BlobDownloadUtil>();

        return services;
    }

    public static IServiceCollection AddBlobDownloadUtilAsSingleton(this IServiceCollection services)
    {
        services.AddMemoryStreamUtilAsSingleton()
                .AddBlobClientUtilAsSingleton();
        services.TryAddSingleton<IBlobDownloadUtil, BlobDownloadUtil>();

        return services;
    }
}