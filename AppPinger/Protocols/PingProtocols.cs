using System;
using System.Linq;
using AppPinger.Protocols.Implements;
using AppPinger.Protocols.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppPinger.Protocols
{
    public class PingProtocols
    {
        private readonly IConfiguration _configuration;

        public PingProtocols(IApplicationBuilder appBuilder, IConfiguration appConfig,
            IServiceCollection serviceCollection, bool startPing = false)
        {
            _configuration = appConfig ??
                             throw new NullReferenceException(string.Format("Параметр {0} не задан!",
                                 (IConfiguration) null));
            if (startPing)
                StartPing(appBuilder, serviceCollection);
        }

        public bool StartPing(IApplicationBuilder appBuilder, IServiceCollection serviceCollection)
        {
            if (appBuilder == null)
                throw new NullReferenceException(string.Format("Параметр {0} не задан!", (IApplicationBuilder) null));

            var listConfigProtocols = appBuilder.ApplicationServices.GetService<IListConfigProtocols>();
            foreach (var confProtocol in listConfigProtocols.ListConfProtocols)
            {
                if (confProtocol.NameProt == EnumProtocols.Icmp)
                {
                    confProtocol.HeadersAddAttr = _configuration["ICMPConfigListHosts"].Split(",").ToList();
                    serviceCollection.AddSingleton<IBasePingProtocol>(x => ActivatorUtilities.CreateInstance<ICMP>(x, confProtocol));
                }

                if (confProtocol.NameProt == EnumProtocols.Http)
                {
                    confProtocol.HeadersAddAttr = _configuration["HTTPConfigListHosts"].Split(",").ToList();
                    serviceCollection.AddSingleton<IBasePingProtocol>(x => ActivatorUtilities.CreateInstance<HTTP>(x, confProtocol));
                }

                if (confProtocol.NameProt == EnumProtocols.Tcp)
                {
                    confProtocol.HeadersAddAttr = _configuration["TCPConfigListHosts"].Split(",").ToList();
                    serviceCollection.AddSingleton<IBasePingProtocol>(x => ActivatorUtilities.CreateInstance<TCP>(x, confProtocol));
                }

                var serviceProvider = serviceCollection.BuildServiceProvider();
                var appBuild = new ApplicationBuilder(serviceProvider);
                var pingProtocol = appBuild.ApplicationServices.GetService<IBasePingProtocol>();
                if (pingProtocol != null)
                {
                    pingProtocol.PingCompleted += PrintAnswerLog;
                    pingProtocol.StartPing();
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