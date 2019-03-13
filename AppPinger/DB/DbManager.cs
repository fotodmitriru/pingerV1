using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppPinger.DB.ConfigureProviders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace AppPinger.DB
{
    public class DbManager
    {
        public async Task WriteToDbAsync(string nameProtocol, string dataLog, string dbConnectionString,
            EnumProviderDb enumProvider)
        {
            if (string.IsNullOrEmpty(nameProtocol))
                throw new ArgumentException("Не указано имя протокола!");
            if (string.IsNullOrEmpty(dbConnectionString))
                throw new ArgumentException("Не указана строка подключения!");

            dynamic db = null;
            switch (enumProvider)
            {
                case EnumProviderDb.SqLite:
                    try
                    {
                        db = InitDbProvider(dbConnectionString).ApplicationServices.GetService<ConfigureDbSqLite>();
                    }
                    catch (Exception e)
                    {
                        throw new AccessViolationException(string.Format("Не удалось подключиться к базе данных! {0}",
                            e.Message));
                    }
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
            if (string.IsNullOrEmpty(dbConnectionString))
                throw new ArgumentException("Не указана строка подключения!");

            dynamic db = null;
            switch (enumProvider)
            {
                case EnumProviderDb.SqLite:
                    try
                    {
                        db = InitDbProvider(dbConnectionString).ApplicationServices.GetService<ConfigureDbSqLite>();
                    }
                    catch (Exception e)
                    {
                        throw new AccessViolationException(string.Format("Не удалось подключиться к базе данных! {0}",
                            e.Message));
                    }
                    break;
            }

            if (db != null)
                return new List<LogModel>(db.LogsModel);

            return new List<LogModel>();
        }

        private IApplicationBuilder InitDbProvider(string dbConnectionString)
        {
            IServiceCollection providersCollection = new ServiceCollection();
            providersCollection.AddSingleton(x =>
                ActivatorUtilities.CreateInstance<ConfigureDbSqLite>(x, dbConnectionString));
            var serviceProvider = providersCollection.BuildServiceProvider();
            return new ApplicationBuilder(serviceProvider);
        }
    }
}