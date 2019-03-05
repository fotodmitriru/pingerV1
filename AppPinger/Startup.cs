using System;
using AppPinger.Protocols;
using AppPinger.Protocols.Interfaces;
using AppPinger.Protocols.Implements;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppPinger
{
    public class Startup
    {
        public static void InitPinger(IConfiguration appConfig)
        {
            if (appConfig == null)
                throw new NullReferenceException(string.Format("Параметр {0} не задан!", (IConfiguration) null));
            // Create service collection
            var serviceCollection = new ServiceCollection();
            ConfigurePinger(serviceCollection);

            // Create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var appBuilder = new ApplicationBuilder(serviceProvider);
            var listConfig = appBuilder.ApplicationServices.GetService<IListConfigProtocols>();

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
            //ConfigurePinger(serviceCollection);
            SaveLogs.GlobalDistStorage = appConfig["fileLogs"];
            new PingProtocols(appBuilder, appConfig, serviceCollection, true);
            Console.WriteLine("Все пинги запущены, для выхода из программы нажмите любую клавишу.");
            Console.ReadLine();
        }

        private static void ConfigurePinger(IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddTransient<ConfigProtocol>();
            serviceCollection.AddSingleton<IListConfigProtocols, ListConfigProtocols>();
        }
    }
}