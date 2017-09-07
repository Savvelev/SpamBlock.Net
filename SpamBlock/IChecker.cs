using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SpamBlock
{
    public interface IChecker
    {
        Task<CheckResult> Check(IPAddress ipAddress);
    }
}
