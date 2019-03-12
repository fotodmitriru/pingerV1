using System;
using System.Collections.Generic;
using AppPinger.DB.ConfigureProviders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace AppPinger.DB
{
    internal class DbManager
    {
        public async void WriteToDbAsync(string nameProtocol, string dataLog, string dbConnectionString,
            EnumProviderDb enumProvider)
        {
            dynamic db = null;
            switch (enumProvider)
            {
                case EnumProviderDb.SqLite:
                    db = InitDbProvider(dbConnectionString).ApplicationServices.GetService<ConfigureDbSqLite>();
                    break;
            }

            try
            {
                var logMod = new LogModel
                {
                    NameProtocol = nameProtocol,
                    DataLog = dataLog
                };
                if (db != null)
                {
                    db.LogsModel.Add(logMod);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                throw new Exception(e.Message);
            }
        }

        public List<LogModel> ViewDb(string dbConnectionString, EnumProviderDb enumProvider)
        {
            dynamic db = null;
            switch (enumProvider)
            {
                case EnumProviderDb.SqLite:
                    db = InitDbProvider(dbConnectionString).ApplicationServices.GetService<ConfigureDbSqLite>();
                    break;
            }

            if (db != null)
                return new List<LogModel>(db.LogsModel);

            return new List<LogModel>();
        }

        public IApplicationBuilder InitDbProvider(string dbConnectionString)
        {
            IServiceCollection providersCollection = new ServiceCollection();
            providersCollection.AddSingleton(x =>
                ActivatorUtilities.CreateInstance<ConfigureDbSqLite>(x, dbConnectionString));
            var serviceProvider = providersCollection.BuildServiceProvider();
            return new ApplicationBuilder(serviceProvider);
        }
    }
}