using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SpamBlock.HttpBL;

namespace SpamBlock
{
    public class SpamBlocker
    {
        readonly List<IChecker> _checkers = new List<IChecker>();
        public SpamBlocker()
        {
        }

        /// <summary>
        /// Checks ip address of visitor. 
        /// </summary>
        /// <param name="clientHostAddress">IP address of visitor</param>
        /// <exception cref="CheckException"></exception>
        /// <returns>Result as <see cref="InternalIPResult"/> or <see cref="SpammerResult"/> or <see cref="SearchEngineResult"/> or <see cref="NoResult"/></returns>
        public async Task<CheckResult> Check(string clientHostAddress)
        {
            if (!IPAddress.TryParse(clientHostAddress, out var ip))
                throw new CheckException($"The supplied value {clientHostAddress} could not be parsed as a valid IP address.");

            return await Check(ip);
        }
        public async Task<CheckResult> Check(IPAddress ipAddress)
        {
            foreach (var checker in _checkers)
            {
                var result = await checker.Check(ipAddress);
                if (result != null)
                    return result;
            }
            return new NoResult(ipAddress);
        }

        public void AddChecker(IChecker checker)
        {
            _checkers.Add(checker);
        }

        public async Task<bool> IsAllowed(string remoteIp, byte thresholdThreatScore, byte maxAgeInDays)
        {
            var result = await Check(remoteIp);
            switch (result)
            {
                case InternalIPResult internalIpResult:
                case NoResult noResult:
                case SearchEngineResult searchEngineResult:
                    return true;
                case SpammerResult spammerResult:
                    return spammerResult.DaysSince > maxAgeInDays || spammerResult.ThreatScore < thresholdThreatScore;
                default:
                    throw new NotSupportedException($"Unexpected result type: {result?.GetType()}");
            }
        }

    }
}
