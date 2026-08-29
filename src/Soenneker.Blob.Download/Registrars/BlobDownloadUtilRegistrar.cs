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
    /// Registers Blob Download Util with a scoped lifetime.
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddBlobDownloadUtilAsScoped(this IServiceCollection services)
    {
        services.AddMemoryStreamUtilAsSingleton().AddBlobClientUtilAsSingleton().AddPathUtilAsScoped().TryAddScoped<IBlobDownloadUtil, BlobDownloadUtil>();

        return services;
    }

    /// <summary>
    /// Registers Blob Download Util with a singleton lifetime.
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddBlobDownloadUtilAsSingleton(this IServiceCollection services)
    {
        services.AddMemoryStreamUtilAsSingleton().AddBlobClientUtilAsSingleton().AddPathUtilAsSingleton().TryAddSingleton<IBlobDownloadUtil, BlobDownloadUtil>();

        return services;
    }
}
