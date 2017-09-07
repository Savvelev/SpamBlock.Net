using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SpamBlock.HttpBL;
using Xunit;

namespace SpamBlock.Tests
{
    public class HttpBlClientTests
    {
        private readonly HttpBLClient _client;

        public HttpBlClientTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var accessKey = configuration["SpamBlock:AccessKey"];
            _client = new HttpBLClient(accessKey);
        }


        [Theory]
        [InlineData("127.0.0.1")]
        [InlineData("192.168.1.5")]
        [InlineData("10.4.7.20")]
        [InlineData("10.0.0.1")]
        public async Task TestInternalIPResult(string ip)
        {
            var ipAddress = IPAddress.Parse(ip);
            var result = await _client.Check(ipAddress);
            var ir = Assert.IsType<InternalIPResult>(result);
            Assert.Equal(ipAddress, ir.IPAddress);
        }

        [Theory]
        [InlineData("::1")]
        public async Task TestInvalidAddressFamily(string ip)
        {
            var ipAddress = IPAddress.Parse(ip);
            var ex = await Assert.ThrowsAsync<CheckException>(() => _client.Check(ipAddress));
        }

        [Theory]
        [InlineData("127.0.0.1")]
        public async Task TestQuery(string ip)
        {
            var ipAddress = IPAddress.Parse(ip);
            var result = await _client.CheckWithHoneyPot(ipAddress);
            Assert.Null(result);
        }

        [Theory]
        [InlineData("127.1.1.0")]
        public async Task TestSearchEngine(string ip)
        {
            var ipAddress = IPAddress.Parse(ip);
            var result = await _client.CheckWithHoneyPot(ipAddress);
            var sr = Assert.IsType<SearchEngineResult>(result);
            Assert.Equal(SearchEngineType.AltaVista, sr.SearchEngine);
            Assert.Equal(ipAddress, sr.IPAddress);
        }

        [Theory]
        [InlineData("127.1.1.3")]
        public async Task TestSpammerTypes(string ip)
        {
            var ipAddress = IPAddress.Parse(ip);
            var result = await _client.CheckWithHoneyPot(ipAddress);
            var spr = Assert.IsType<SpammerResult>(result);
            Assert.Equal(SpammerType.Suspicious | SpammerType.Harvester, spr.Type);
            Assert.Equal(ipAddress, spr.IPAddress);
        }
    }
}
