using System.Net;

namespace SpamBlock.HttpBL
{
    public sealed class InternalIPResult : CheckResult
    {
        public InternalIPResult(IPAddress ipAddress) : base(ipAddress)
        {
        }
    }
}