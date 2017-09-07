using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SpamBlock.HttpBL
{
    public sealed class HttpBLClient : IChecker
    {
        private readonly string _accessKey;

        public HttpBLClient(string accessKey)
        {
            _accessKey = accessKey;
        }

        internal async Task<IPAddress> QueryHoneyPot(IPAddress ip)
        {
            var parts = ip.GetAddressBytes();
            var query = $"{_accessKey}.{parts[3]}.{parts[2]}.{parts[1]}.{parts[0]}.dnsbl.httpbl.org";
            try
            {
                var entry = await Dns.GetHostEntryAsync(query);
                return entry.AddressList.FirstOrDefault();
            }
            catch (SocketException e) when (e.ErrorCode == 11001)
            {
                return IPAddress.None;
            }
        }

        public async Task<CheckResult> Check(IPAddress ip)
        {
            //Check if it is v4 address
            if (ip.AddressFamily != AddressFamily.InterNetwork)
                throw new CheckException($"The supplied value {ip} is not a valid IPv4 address.");

            //Check if is an internal ip
            if (ip.IsInternal())
                return new InternalIPResult(ip);

            return await CheckWithHoneyPot(ip);
        }

        internal async Task<CheckResult> CheckWithHoneyPot(IPAddress ip)
        {
            var response = await QueryHoneyPot(ip);
            if (IPAddress.None.Equals(response))
                return null;
            var parts = response.GetAddressBytes();
            if (parts[0] != 127)
                throw new CheckException($"Honeypot returned unexpected ip {response}");
            if (parts[3] == 0) //Search engine
                return new SearchEngineResult(ip, (SearchEngineType) parts[2]);
            else
                return new SpammerResult(ip, (SpammerType) parts[3], parts[1], parts[2]);
        }
    }
}
