using Soenneker.Blob.Download.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;


namespace Soenneker.Blob.Download.Tests;

[Collection("Collection")]
public class BlobDownloadUtilTests : FixturedUnitTest
{
    private readonly IBlobDownloadUtil _util;

    public BlobDownloadUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IBlobDownloadUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
