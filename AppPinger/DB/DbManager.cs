﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppPinger.DB.ConfigureProviders;

namespace AppPinger.DB
{
    public class DbManager
    {
        public async Task WriteToDbAsync(string nameProtocol, string dataLog, IConfigureDb configureDb)
        {
            if (string.IsNullOrEmpty(nameProtocol))
                throw new ArgumentException("Не указано имя протокола!");
            if (configureDb == null)
                throw new ArgumentException(nameof(configureDb));

            dynamic db = configureDb;
            try
            {
                var logMod = new LogModel
                {
                    NameProtocol = nameProtocol,
                    DataLog = dataLog
                };
                db.LogsModel.Add(logMod);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                throw new Exception(e.Message);
            }
        }

        public List<LogModel> ViewDb(string dbConnectionString, IConfigureDb configureDb)
        {
            if (string.IsNullOrEmpty(dbConnectionString))
                throw new ArgumentException("Не указана строка подключения!");
            if (configureDb == null)
                throw new ArgumentException(nameof(configureDb));

            dynamic db = configureDb;

            return new List<LogModel>(db.LogsModel);
        }
    }
}