[![](https://img.shields.io/nuget/v/Soenneker.Blob.Download.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blob.Download/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blob.download/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blob.download/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Blob.Download.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blob.Download/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blob.download/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.blob.download/actions/workflows/codeql.yml)

# Soenneker.Blob.Download

Downloads Azure blobs to temporary files, memory streams, or strings.

## Installation

```bash
dotnet add package Soenneker.Blob.Download
```

## Configuration

Provide the Azure Storage connection string through configuration:

```json
{
  "Azure": {
    "Storage": {
      "Blob": {
        "ConnectionString": "<connection string>"
      }
    }
  }
}
```

## Registration

```csharp
using Microsoft.Extensions.DependencyInjection;
using Soenneker.Blob.Download.Registrars;

services.AddBlobDownloadUtilAsScoped();
```

`AddBlobDownloadUtilAsSingleton()` is also available when a singleton lifetime fits the application.

## Usage

```csharp
using Soenneker.Blob.Download.Abstract;

public sealed class DocumentStore
{
    private readonly IBlobDownloadUtil _downloads;

    public DocumentStore(IBlobDownloadUtil downloads)
    {
        _downloads = downloads;
    }

    public async ValueTask<string> ReadManifest(CancellationToken cancellationToken)
    {
        return await _downloads.DownloadToString(
            "documents",
            "manifests/latest.json",
            cancellationToken: cancellationToken);
    }
}
```

Download to a temporary file when the content should not be buffered in memory:

```csharp
FileInfo file = await downloads.Download(
    "documents",
    "exports/archive.zip",
    cancellationToken: cancellationToken);

try
{
    // Consume file here.
}
finally
{
    file.Delete();
}
```

For in-memory processing, dispose the returned stream:

```csharp
await using MemoryStream stream = await downloads.DownloadToMemory(
    "documents",
    "images/logo.png",
    cancellationToken: cancellationToken);
```

## Choosing a download method

| Method | Use it when | Ownership |
| --- | --- | --- |
| `Download` | The blob may be large or a file-based API needs the result | Delete the returned temporary file |
| `DownloadToMemory` | A stream-based API needs reasonably sized content | Dispose the returned stream |
| `DownloadToString` | The blob is reasonably sized text | Returns an owned string |

## Behavior

- `DownloadToMemory` and `DownloadToString` buffer the entire blob. Prefer `Download` for large or untrusted content.
- The stream returned by `DownloadToMemory` is positioned at `0`.
- Failed file downloads remove the incomplete temporary file when possible. Failed memory downloads dispose their stream.
- A missing blob is reported through Azure's `RequestFailedException`, including a `404` status when returned by the service.
- `publicAccessType` is used only if the underlying client utility creates a missing container; it does not update an existing container's access level.
- Cancellation is passed to Azure Storage and temporary resource creation.
