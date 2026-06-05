using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blob.Client.Registrars;
using Soenneker.Blob.Download.Abstract;
using Soenneker.Utils.MemoryStream.Registrars;
using Soenneker.Utils.Path.Registrars;

namespace Soenneker.Blob.Download.Registrars;

/// <summary>
/// A utility library for Azure Blob storage download operations
/// </summary>
public static class BlobDownloadUtilRegistrar
{
    /// <summary>
    /// Adds blob download util as scoped.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The result of the operation.</returns>
    public static IServiceCollection AddBlobDownloadUtilAsScoped(this IServiceCollection services)
    {
        services.AddMemoryStreamUtilAsSingleton().AddBlobClientUtilAsSingleton().AddPathUtilAsScoped().TryAddScoped<IBlobDownloadUtil, BlobDownloadUtil>();

        return services;
    }

    /// <summary>
    /// Adds blob download util as singleton.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The result of the operation.</returns>
    public static IServiceCollection AddBlobDownloadUtilAsSingleton(this IServiceCollection services)
    {
        services.AddMemoryStreamUtilAsSingleton().AddBlobClientUtilAsSingleton().AddPathUtilAsSingleton().TryAddSingleton<IBlobDownloadUtil, BlobDownloadUtil>();

        return services;
    }
}