using System;
using System.IO;
using AppPinger.Protocols;
using AppPinger.Protocols.Interfaces;
using AppPinger.Protocols.Interfaces.Implements;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace AppPinger
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathAppConfig = "appConfig.json";
            if (args.Length > 0)
                pathAppConfig = args[0];
            if (!File.Exists(pathAppConfig))
            {
                Console.WriteLine("Файл с конфигурациями приложения не найден!");
                return;
            }

            IConfiguration appConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(pathAppConfig, optional: true, reloadOnChange: true)
                .Build();

            Startup.InitPinger(appConfig);
        }
    }
}
