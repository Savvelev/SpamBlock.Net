#if NET461
using SpamBlock;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Microsoft.Extensions.Logging;
using SpamBlock.HttpBL;

namespace SpamBlock
{
    public class SpamBlockModule : IHttpModule
    {
        private readonly SpamBlocker _checker = new SpamBlocker();

        public SpamBlockModule()
        {
            var accessKey = WebConfigurationManager.AppSettings["AccessKey"];
            _checker.AddChecker(new HttpBLClient(accessKey));
        }

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            var eh = new EventHandlerTaskAsyncHelper(Handler);
            context.AddOnBeginRequestAsync(eh.BeginEventHandler,eh.EndEventHandler);
        }

        private async Task Handler(object sender, EventArgs eventArgs)
        {
            try
            {
                var application = (HttpApplication)sender;
                var ctx = application.Context;
                var ip = ctx.Request.UserHostAddress;
                var allowed = await _checker.IsAllowed(ip, 100, 100);
                if (!allowed)
                {
                    SendNotAllowedResponse(ip, null);
                }
            }
            catch (Exception ex)
            {
            }
        }


        void SendNotAllowedResponse(string ipAddress, HttpResponse response)
        {
            response.StatusCode = 403;
            response.SubStatusCode = 6;
            response.StatusDescription = "403.6 IP Address Rejected.";

            var content = $@"
<html>
<head><title>403.6 IP Address Rejected.</title></head>
<body><center>
<h1>403.6 IP Address Rejected.</h1>
<p>Access from {ipAddress} denied, IP is blacklisted due to bad behavior.</p>
</center></body>
</html>";
            response.Write(content);
        }
    }
}
#endif