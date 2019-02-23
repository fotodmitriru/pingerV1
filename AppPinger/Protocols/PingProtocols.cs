using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppPinger.Protocols.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppPinger.Protocols
{
    public class PingProtocols
    {
        private readonly IConfiguration _configuration;

        public PingProtocols(IApplicationBuilder appBuilder, IConfiguration appConfig, bool startPing = false)
        {
            _configuration = appConfig ??
                             throw new NullReferenceException(string.Format("Параметр {0} не задан!",
                                 (IConfiguration) null));
            if (startPing)
                StartPing(appBuilder);
        }

        public bool StartPing(IApplicationBuilder appBuilder)
        {
            if (appBuilder == null)
                throw new NullReferenceException(string.Format("Параметр {0} не задан!", (IApplicationBuilder) null));

            var listConfigProtocols = appBuilder.ApplicationServices.GetService<IListConfigProtocols>();
            foreach (var confProtocol in listConfigProtocols.ListConfProtocols)
            {
                IBasePingProtocol pingProtocol = null;
                if (confProtocol.NameProt == "ICMP")
                {
                    confProtocol.HeadersAddAttr = _configuration["ICMPConfigListHosts"].Split(",").ToList();
                    pingProtocol = appBuilder.ApplicationServices.GetService<IICMP>();
                }

                if (confProtocol.NameProt == "HTTP")
                {
                    confProtocol.HeadersAddAttr = _configuration["HTTPConfigListHosts"].Split(",").ToList();
                    pingProtocol = appBuilder.ApplicationServices.GetService<IHTTP>();
                }

                if (confProtocol.NameProt == "TCP")
                {
                    confProtocol.HeadersAddAttr = _configuration["TCPConfigListHosts"].Split(",").ToList();
                    pingProtocol = appBuilder.ApplicationServices.GetService<ITCP>();
                }

                if (pingProtocol != null)
                {
                    pingProtocol.Host = (string)confProtocol.AdditionalAttributes[confProtocol.HeadersAddAttr.IndexOf("Host")];
                    pingProtocol.Period = Convert.ToInt32(confProtocol.AdditionalAttributes[confProtocol.HeadersAddAttr.IndexOf("Period")]);
                    pingProtocol.DistStorage = (string)confProtocol.AdditionalAttributes[confProtocol.HeadersAddAttr.IndexOf("DistStorage")] ?? "";
                    pingProtocol.PingCompleted += PrintAnswerLog;
                    
                    dynamic p = pingProtocol;
                    if (p is IHTTP)
                        p.ValidCode = Convert.ToInt32(confProtocol.AdditionalAttributes[confProtocol.HeadersAddAttr.IndexOf("ValidCode")]);
                    if(p is ITCP)
                        p.Port = Convert.ToInt32(confProtocol.AdditionalAttributes[confProtocol.HeadersAddAttr.IndexOf("Port")]);
                    p.StartPing();
                }
            }

            return true;
        }

        public void PrintAnswerLog(string replyLog, string distStorage = "")
        {
            if (replyLog == null)
                throw new ArgumentException("Значение переменной не должно быть null.", nameof(replyLog));
            Console.WriteLine(replyLog);
            SaveLogs.WriteLogAsync(replyLog, distStorage);
        }
    }
}