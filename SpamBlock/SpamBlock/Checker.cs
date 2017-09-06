using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SpamBlock
{
    public sealed class Checker
    {
        private readonly string _accessKey;


        public Checker(string accessKey)
        {
            _accessKey = accessKey;
        }


        public async Task<CheckResult> Check(string clientHostAddress)
        {
            try
            {
                if (!IPAddress.TryParse(clientHostAddress, out var ip))
                    throw new CheckException(ErrorReason.InvalidString,
                        $"The supplied value {clientHostAddress} could not be parsed as a valid IP address.");

                //Check if it is v4 address
                if (ip.AddressFamily != AddressFamily.InterNetwork)
                    throw new CheckException(ErrorReason.InvalidAddressFamily,
                        $"The supplied value {clientHostAddress} is not a valid IPv4 address.");

                //Check if is an internal ip
                if (ip.IsInternal())
                    return new InternalIPResult(ip);

                return await Lookup(ip);
            }
            catch (Exception e) when (!(e is CheckException))
            {
                throw new CheckException(ErrorReason.GeneralError, e.Message, e);
            }
        }

        internal async Task<CheckResult> Lookup(IPAddress ip)
        {
            var response = await QueryHoneyPot(ip);
            if (IPAddress.None.Equals(response))
                return new NoResult(ip);
            var parts = response.GetAddressBytes();
            if (parts[0] != 127)
                throw new CheckException(ErrorReason.UnexpectedResponse, $"Honeypot returned unexpected ip {response}");
            if (parts[3] == 0) //Search engine
                return new SearchEngineResult(ip, (SearchEngineType)parts[2]);
            else
                return new SpammerResult(ip, (SpammerType)parts[3], parts[1], parts[2]);
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

    }
}
