using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SpamBlock.HttpBL;
using Xunit;

namespace SpamBlock.Tests
{
   public class CheckerTests
    {
        private readonly SpamBlocker _checker = new SpamBlocker();

        [Theory]
        [InlineData("test")]
        [InlineData("10.0.0.300")]
        public async Task TestInvalidString(string ip)
        {
            var ex = await Assert.ThrowsAsync<CheckException>(async () => await _checker.Check(ip));
        }



       


    }
}
