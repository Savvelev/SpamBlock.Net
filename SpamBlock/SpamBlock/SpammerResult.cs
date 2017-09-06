using System.Net;

namespace SpamBlock
{
    public sealed class SpammerResult : CheckResult
    {
        public SpammerResult(IPAddress ipAddress, SpammerType type, byte daysSince, byte threatScore)
            : base(ipAddress)
        {
            DaysSince = daysSince;
            ThreatScore = threatScore;
            Type = type;
        }
        public SpammerType Type { get; }
        public byte DaysSince { get; }
        public byte ThreatScore { get; }
    }
}