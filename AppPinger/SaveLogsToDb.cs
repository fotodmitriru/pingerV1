using System;
using AppPinger.DB;
using AppPinger.DB.ConfigureProviders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace AppPinger
{
    public partial class SaveLogs
    {
        public void WriteLogAsyncToSqLite(string nameProtocol, string dataLog, string distStorage = "")
        {
            distStorage = CheckIsNullOrEmptyDistStorage(distStorage, GetGlobalDistStorage("globalDistStorageSqLite"));
            var dbManager = _appBuilder.ApplicationServices.GetService<DbManager>();
            var configureDbSqLite = InitDbProvider($"Data Source={distStorage}").ApplicationServices
                .GetService<ConfigureDbSqLite>();
            dbManager.WriteToDbAsync(nameProtocol, dataLog, configureDbSqLite);
        }

        public bool ViewLogFromSqLite(string distStorage = "")
        {
            distStorage = CheckIsNullOrEmptyDistStorage(distStorage, GetGlobalDistStorage("globalDistStorageSqLite"));
            var _dbManager = _appBuilder.ApplicationServices.GetService<DbManager>();
            var configureDbSqLite = InitDbProvider($"Data Source={distStorage}").ApplicationServices
                .GetService<ConfigureDbSqLite>();
            var logsModel = _dbManager.ViewDb($"Data Source={distStorage}", configureDbSqLite);
            if (!logsModel.Any())
                return false;
            foreach (var logMod in logsModel)
            {
                Console.WriteLine(logMod.NameProtocol + ":" + logMod.DataLog);
            }

            return true;
        }

        private IApplicationBuilder InitDbProvider(string dbConnectionString)
        {
            _serviceCollection.AddSingleton(x =>
                ActivatorUtilities.CreateInstance<ConfigureDbSqLite>(x, dbConnectionString));
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            return new ApplicationBuilder(serviceProvider);
        }
    }
}
