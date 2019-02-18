using System;
using System.IO;
using Microsoft.Extensions.Configuration;

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
