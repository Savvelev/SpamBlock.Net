using System.Net;

namespace SpamBlock.HttpBL
{
    public sealed class SearchEngineResult : CheckResult
    {
        public SearchEngineResult(IPAddress ipAddress, SearchEngineType searchEngine) : base(ipAddress)
        {
            SearchEngine = searchEngine;
        }

        public SearchEngineType SearchEngine { get; }

    }
}