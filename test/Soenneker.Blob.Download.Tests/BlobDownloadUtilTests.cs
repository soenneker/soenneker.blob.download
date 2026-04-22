using Soenneker.Blob.Download.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Blob.Download.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class BlobDownloadUtilTests : HostedUnitTest
{
    private readonly IBlobDownloadUtil _util;

    public BlobDownloadUtilTests(Host host) : base(host)
    {
        _util = Resolve<IBlobDownloadUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
