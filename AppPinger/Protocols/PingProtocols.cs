using System;
using AppPinger.Protocols.Implements;
using AppPinger.Protocols.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace AppPinger.Protocols
{
    public class PingProtocols
    {
        private readonly IApplicationBuilder _appBuilder;
        private readonly IServiceCollection _serviceCollection;

        public PingProtocols(IApplicationBuilder appBuilder, IServiceCollection serviceCollection,
            bool startPing = false)
        {
            _appBuilder = appBuilder ?? 
                                 throw new NullReferenceException(string.Format("Параметр {0} не задан!",
                                     (IApplicationBuilder) null));
            _serviceCollection = serviceCollection ?? 
                                 throw new NullReferenceException(string.Format("Параметр {0} не задан!",
                                     (IServiceCollection) null));

            if (startPing)
                StartPing();
        }

        public bool StartPing()
        {
            if (_appBuilder == null)
                throw new NullReferenceException(string.Format("Параметр {0} не задан!", (IApplicationBuilder) null));

            var listConfigProtocols = _appBuilder.ApplicationServices.GetService<IListConfigProtocols>();
            foreach (var confProtocol in listConfigProtocols.ListConfProtocols)
            {
                if (confProtocol.NameProt == EnumProtocols.Icmp)
                    _serviceCollection.AddSingleton<IBasePingProtocol>(x =>
                        ActivatorUtilities.CreateInstance<ICMP>(x, confProtocol));

                if (confProtocol.NameProt == EnumProtocols.Http)
                    _serviceCollection.AddSingleton<IBasePingProtocol>(x =>
                        ActivatorUtilities.CreateInstance<HTTP>(x, confProtocol));

                if (confProtocol.NameProt == EnumProtocols.Tcp)
                    _serviceCollection.AddSingleton<IBasePingProtocol>(x =>
                        ActivatorUtilities.CreateInstance<TCP>(x, confProtocol));

                var serviceProvider = _serviceCollection.BuildServiceProvider();
                var appBuild = new ApplicationBuilder(serviceProvider);
                var pingProtocol = appBuild.ApplicationServices.GetService<IBasePingProtocol>();
                if (pingProtocol != null)
                {
                    pingProtocol.PingCompleted += PrintAnswerLog;
                    pingProtocol.StartAsyncPing();
                }
            }

            return true;
        }

        public void PrintAnswerLog(string replyLog, string distStorage = "")
        {
            if (replyLog == null)
                throw new ArgumentException("Значение переменной не должно быть null.", nameof(replyLog));
            Console.WriteLine(replyLog);
            _appBuilder.ApplicationServices.GetService<SaveLogs>().WriteLogAsyncToFile(replyLog, distStorage);
        }
    }
}