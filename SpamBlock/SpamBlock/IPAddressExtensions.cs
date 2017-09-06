using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SpamBlock
{
    // ReSharper disable once InconsistentNaming
    public static class IPAddressExtensions
    {
        /* An IP should be considered as internal when:

       10.0.0.0     -   10.255.255.255  (10/8 prefix)
       127.0.0.0    -   127.255.255.255  (127/8 prefix)
       172.16.0.0   -   172.31.255.255  (172.16/12 prefix)
       192.168.0.0  -   192.168.255.255 (192.168/16 prefix)
     */
        public static bool IsInternal(this IPAddress ip)
        {
            var parts = ip.GetAddressBytes();
            switch (parts[0])
            {
                case 10:
                case 127:
                    return true;
                case 172:
                    return parts[1] >= 16 && parts[1] < 32;
                case 192:
                    return parts[1] == 168;
                default:
                    return false;
            }
        }
    }
}
