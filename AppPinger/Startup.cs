using System;
using System.Collections.Generic;
using System.IO;
using AppPinger.DB;
using AppPinger.DB.ConfigureProviders;
using AppPinger.Protocols;
using AppPinger.Protocols.Interfaces;
using AppPinger.Protocols.Implements;
using Microsoft.AspNetCore.Builder;
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

            CreateExampleConfig(appBuilder, appConfig["listHosts"]);

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

            var saveLogs = appBuilder.ApplicationServices.GetService<SaveLogs>();
            saveLogs.GlobalDistStorage = appConfig["fileLogs"];
            saveLogs.ViewLogFromSqLite(appConfig["fileLogsSQLite"]);
            saveLogs.GlobalDistStorageSqLite = appConfig["fileLogsSQLite"];

            var pingProtocols = new PingProtocols(appBuilder, serviceCollection);
            pingProtocols.StartPing();
            Console.WriteLine("Все пинги запущены, для выхода из программы нажмите любую клавишу.");
            Console.ReadLine();
        }

        private static void ConfigurePinger(IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
                throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddTransient<ConfigProtocol>();
            serviceCollection.AddSingleton<IListConfigProtocols, ListConfigProtocols>();
            serviceCollection.AddSingleton<SaveLogs>();
            serviceCollection.AddSingleton<ConfigureDbSqLite>();
            serviceCollection.AddSingleton<DbManager>();
        }

        private static void CreateExampleConfig(IApplicationBuilder appBuilder, string fileJsonPath)
        {
            if (File.Exists(fileJsonPath))
                return;
            var listConfig = appBuilder.ApplicationServices.GetService<IListConfigProtocols>();
            var itemIcmp = appBuilder.ApplicationServices.GetService <ConfigProtocol>();

            itemIcmp.NameProt = EnumProtocols.Icmp;
            itemIcmp.Host = "ya.ru";
            itemIcmp.Period = 1;
            itemIcmp.AttributesProtocol = new Dictionary<string, string>();
            itemIcmp.AttributesProtocol.Add("DistStorage", "ICMPLog.txt");
            itemIcmp.AttributesProtocol.Add("DistStorageSqLite", "LogsICMP.db");

            listConfig.ListConfProtocols = new List<ConfigProtocol>();
            listConfig.ListConfProtocols.Add(itemIcmp);

            var itemHttp = appBuilder.ApplicationServices.GetService<ConfigProtocol>();
            itemHttp.NameProt = EnumProtocols.Http;
            itemHttp.Host = "rambler.ru";
            itemHttp.Period = 5;
            itemHttp.AttributesProtocol = new Dictionary<string, string>();
            itemHttp.AttributesProtocol.Add("ValidCode", "200");
            itemHttp.AttributesProtocol.Add("DistStorage", "HTTPLog.txt");

            listConfig.ListConfProtocols.Add(itemHttp);

            var itemTcp = appBuilder.ApplicationServices.GetService<ConfigProtocol>();
            itemTcp.NameProt = EnumProtocols.Tcp;
            itemTcp.Host = "mail.ru";
            itemTcp.Period = 10;
            itemTcp.AttributesProtocol = new Dictionary<string, string>();
            itemTcp.AttributesProtocol.Add("Port", "80");
            itemTcp.AttributesProtocol.Add("DistStorage", "TCPLog.txt");

            listConfig.ListConfProtocols.Add(itemTcp);
            listConfig.WriteConfig(fileJsonPath);
        }
    }
}