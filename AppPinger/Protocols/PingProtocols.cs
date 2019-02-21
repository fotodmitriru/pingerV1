using System;
using AppPinger.Protocols.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppPinger.Protocols
{
    public class PingProtocols
    {
        //private IBasePingProtocol _pingProtocol;
        /*private readonly ITCP _tcp;
        private readonly IHTTP _http;*/
        private readonly IConfiguration _configuration;

        public PingProtocols(IApplicationBuilder appBuilder, IConfiguration appConfig, bool startPing = false)
        {
            _configuration = appConfig;
            if (startPing)
                StartPing(appBuilder);
        }

        public bool StartPing(IApplicationBuilder appBuilder)
        {
            var listConfigProtocols = appBuilder.ApplicationServices.GetService<IListConfigProtocols>();
            foreach (var listConfProtocol in listConfigProtocols.ListConfProtocols)
            {
                if (listConfProtocol.NameProt == "ICMP")
                {
                    var pingProtocol = appBuilder.ApplicationServices.GetService<IICMP>();
                    pingProtocol.PingCompleted += PrintAnswerLog;
                    pingProtocol.StartPing(listConfProtocol);
                }

                if (listConfProtocol.NameProt == "HTTP")
                {
                    var pingProtocol = appBuilder.ApplicationServices.GetService<IHTTP>();
                    pingProtocol.PingCompleted += PrintAnswerLog;
                    pingProtocol.StartPing(listConfProtocol);
                }
                
                if (listConfProtocol.NameProt == "TCP")
                {
                    var pingProtocol = appBuilder.ApplicationServices.GetService<ITCP>();
                    pingProtocol.PingCompleted += PrintAnswerLog;
                    pingProtocol.StartPing(listConfProtocol);
                }
            }

            return true;
        }

        public void PrintAnswerLog(string replyLog)
        {
            Console.WriteLine(replyLog);
            SaveLogs.WriteLogAsync(replyLog);
        }
    }
}