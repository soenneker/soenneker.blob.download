[![](https://img.shields.io/nuget/v/Soenneker.Blob.Download.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blob.Download/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blob.download/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blob.download/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Blob.Download.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blob.Download/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blob.download/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.blob.download/actions/workflows/codeql.yml)

# Soenneker.Blob.Download

A utility library for Azure Blob download operations Typically Scoped IoC.

## Install

```bash
dotnet add package Soenneker.Blob.Download
```

## Quick start

```csharp
using Soenneker.Blob.Download.Registrars;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
var result = services.AddBlobDownloadUtilAsScoped();
```

Registers Blob Download Util with a scoped lifetime.

## What you get

- `IBlobDownloadUtil` — A utility library for Azure Blob download operations Typically Scoped IoC.
- `BlobDownloadUtilRegistrar` — A utility library for Azure Blob storage download operations.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `IBlobDownloadUtil.Download(container, relativeUrl, publicAccessType, cancellationToken)` | Downloads to a particular file on the host server as a temp file. | A task whose result is the requested file Info. |
| `IBlobDownloadUtil.DownloadToMemory(container, relativeUrl, publicAccessType, cancellationToken)` | Ready-to-read MemoryStream (Position 0). | A task whose result is the requested memory Stream. |
| `IBlobDownloadUtil.DownloadToString(container, relativeUrl, publicAccessType, cancellationToken)` | Downloads to String. | A task whose result is the text returned by download To String. |
| `BlobDownloadUtilRegistrar.AddBlobDownloadUtilAsScoped(services)` | Registers Blob Download Util with a scoped lifetime. | The same service collection, so additional registrations can be chained. |
| `BlobDownloadUtilRegistrar.AddBlobDownloadUtilAsSingleton(services)` | Registers Blob Download Util with a singleton lifetime. | The same service collection, so additional registrations can be chained. |

## Practical notes

- Cancellation stops pending work; it does not undo work that has already completed.
