using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpamBlock.HttpBL;

namespace SpamBlock.Tests
{
    [TestClass]
    public class CheckerTests
    {
        private readonly SpamBlocker _checker = new SpamBlocker();

        [TestMethod]
        [DataRow("test")]
        [DataRow("10.0.0.300")]
        public async Task TestInvalidString(string ip)
        {
            await Assert.ThrowsExceptionAsync<CheckException>(async () => await _checker.Check(ip));
        }
    }
}
