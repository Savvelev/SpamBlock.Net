using System.Net;

namespace SpamBlock
{
    public sealed class InternalIPResult : CheckResult
    {
        public InternalIPResult(IPAddress ipAddress) : base(ipAddress)
        {
        }
    }
}