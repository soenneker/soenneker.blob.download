using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blob.Client.Registrars;
using Soenneker.Blob.Download.Abstract;
using Soenneker.Utils.Cancellation.Abstract;
using Soenneker.Utils.Cancellation.Registrars;
using Soenneker.Utils.File.Registrars;
using Soenneker.Utils.MemoryStream.Registrars;

namespace Soenneker.Blob.Download.Registrars;

/// <summary>
/// A utility library for Azure Blob storage download operations
/// </summary>
public static class BlobDownloadUtilRegistrar
{
    /// <summary>
    /// Scoped IoC (due to <see cref="ICancellationUtil"/>) Recommended
    /// </summary>
    public static void AddBlobDownloadUtil(this IServiceCollection services)
    {
        services.AddMemoryStreamUtil();
        services.AddBlobClientUtilAsSingleton();
        services.TryAddScoped<IBlobDownloadUtil, BlobDownloadUtil>();
        services.AddCancellationUtil();
    }
}