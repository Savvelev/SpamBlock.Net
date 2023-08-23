using Microsoft.Extensions.Configuration;
using SpamBlock.HttpBL;
using System.Net;

namespace SpamBlock.Tests
{
    [TestClass]
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

        [TestMethod]
        
        [DataRow("127.0.0.1")]
        [DataRow("192.168.1.5")]
        [DataRow("10.4.7.20")]
        [DataRow("10.0.0.1")]
        public async Task TestInternalIPResult(string ip)
        {
            var ipAddress = IPAddress.Parse(ip);
            var result = await _client.Check(ipAddress);
            Assert.IsInstanceOfType(result, typeof(InternalIPResult));
            var ir = result as InternalIPResult;
            Assert.AreEqual(ipAddress, ir.IPAddress);
        }

        [TestMethod]
        [DataRow("::1")]
        public async Task TestInvalidAddressFamily(string ip)
        {
            var ipAddress = IPAddress.Parse(ip);
            await Assert.ThrowsExceptionAsync<CheckException>(() => _client.Check(ipAddress));
        }

        [TestMethod]
        [DataRow("2.59.254.158")]
  /*      [DataRow("127.0.0.1")]*/
        public async Task TestQuery(string ip)
        {
            var ipAddress = IPAddress.Parse(ip);
            var result = await _client.CheckWithHoneyPot(ipAddress);
            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow("127.1.1.0")]
        public async Task TestSearchEngine(string ip)
        {
            var ipAddress = IPAddress.Parse(ip);
            var result = await _client.CheckWithHoneyPot(ipAddress);
            Assert.IsInstanceOfType(result, typeof(SearchEngineResult));
            var sr = result as SearchEngineResult;

            Assert.AreEqual(SearchEngineType.AltaVista, sr.SearchEngine);
            Assert.AreEqual(ipAddress, sr.IPAddress);
        }

        [TestMethod]
        [DataRow("127.1.1.3")]
        public async Task TestSpammerTypes(string ip)
        {
            var ipAddress = IPAddress.Parse(ip);
            var result = await _client.CheckWithHoneyPot(ipAddress);

            Assert.IsInstanceOfType(result, typeof(SpammerResult));
            var spr = result as SpammerResult;

            Assert.AreEqual(SpammerType.Suspicious | SpammerType.Harvester, spr.Type);
            Assert.AreEqual(ipAddress, spr.IPAddress);
        }
    }
}
