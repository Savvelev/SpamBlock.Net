using System.Net;

namespace SpamBlock
{
    public sealed class NoResult : CheckResult
    {
        public NoResult(IPAddress ipAddress) : base(ipAddress)
        {
        }
    }
}