using System.Net;

namespace SpamBlock
{

    public abstract class CheckResult
    {
        // ReSharper disable once InconsistentNaming
        public IPAddress IPAddress { get; }

        protected CheckResult(IPAddress ipAddress)
        {
            IPAddress = ipAddress;
        }
    }

}