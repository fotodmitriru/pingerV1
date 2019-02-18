using System;
using AppPinger.Protocols;
using AppPinger.Protocols.Interfaces;
using AppPinger.Protocols.Interfaces.Implements;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppPinger
{
    public class Startup
    {
        public static void InitPinger(IConfiguration appConfig)
        {
            // Create service collection
            var serviceCollection = new ServiceCollection();
            ConfigurePinger(serviceCollection);

            // Create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var app = new ApplicationBuilder(serviceProvider);
            var listConfig = app.ApplicationServices.GetService<IListConfigProtocols>();

            if (listConfig.ReadConfig(appConfig["listHosts"]))
            {
                Console.WriteLine("Список пингуемых хостов загружен!");
            }
            else
            {
                Console.WriteLine("Не удалось загрузить список пингуемых хостов!");
                Console.ReadKey();
                return;
            }

            SaveLogs.GlobalDistStorage = appConfig["fileLogs"];
            new PingProtocols(app, appConfig, true);
            Console.WriteLine("Все пинги завершены.");
            Console.ReadLine();
        }

        private static void ConfigurePinger(IServiceCollection serviceCollection)
        {
            serviceCollection.AddOptions();
            serviceCollection.AddTransient<ConfigProtocol>();
            serviceCollection.AddSingleton<IListConfigProtocols, ListConfigProtocols>();
            serviceCollection.AddTransient<IICMP, ICMP>();
            serviceCollection.AddTransient<IHTTP, HTTP>();

        }
    }
}
