using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace SpamBlock.Tests
{
    public class UnitTest1
    {
        private const string AccessKey = "spneqobsfgua";
        private readonly Checker _checker = new Checker(AccessKey);

        [Theory]
        [InlineData("127.0.0.1")]
        public async Task TestQuery(string ip)
        {
            var ipAddress = IPAddress.Parse(ip);
            var result = await _checker.QueryHoneyPot(ipAddress);
            Assert.Equal(IPAddress.None, result);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("10.0.0.300")]
        public async Task TestInvalidString(string ip)
        {
            var ex = await Assert.ThrowsAsync<CheckException>(async () => await _checker.Check(ip));
            Assert.Equal(ErrorReason.InvalidString, ex.Reason);
        }


        [Theory]
        [InlineData("::1")]
        public async Task TestInvalidAddressFamily(string ip)
        {
            var ex = await Assert.ThrowsAsync<CheckException>(async () => await _checker.Check(ip));
            Assert.Equal(ErrorReason.InvalidAddressFamily, ex.Reason);
        }

        [Theory]
        [InlineData("127.0.0.1")]
        [InlineData("192.168.1.5")]
        [InlineData("10.4.7.20")]
        [InlineData("10.0.0.1")]
        public async Task TestInternalIPResult(string ip)
        {
            var c = new Checker(AccessKey);
            var result = await _checker.Check(ip);
            var ir = Assert.IsType<InternalIPResult>(result);
            Assert.Equal(ip, ir.IPAddress.ToString());
        }

        [Theory]
        [InlineData("127.1.1.0")]
        public async Task TestSearchEngine(string ip)
        {
            var ipAddress = IPAddress.Parse(ip);
            var result = await _checker.Lookup(ipAddress);
            var sr = Assert.IsType<SearchEngineResult>(result);
            Assert.Equal(SearchEngineType.AltaVista, sr.SearchEngine);
            Assert.Equal(ipAddress, sr.IPAddress);
        }

        [Theory]
        [InlineData("127.1.1.3")]
        public async Task TestSpammerTypes(string ip)
        {
            var ipAddress = IPAddress.Parse(ip);
            var result = await _checker.Lookup(ipAddress);
            var spr = Assert.IsType<SpammerResult>(result);
            Assert.Equal(SpammerType.Suspicious | SpammerType.Harvester, spr.Type);
            Assert.Equal(ipAddress, spr.IPAddress);
        }
    }
}
