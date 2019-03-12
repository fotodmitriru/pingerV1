using System;
using AppPinger.DB;
using Microsoft.EntityFrameworkCore.Internal;

namespace AppPinger
{
    public partial class SaveLogs
    {
        public void WriteLogAsyncToSqLite(string nameProtocol, string dataLog, string distStorage = "")
        {
            distStorage = CheckIsNullOrEmptyDistStorage(distStorage, GetGlobalDistStorage("globalDistStorageSqLite"));

            var sqliteDb = new DbManager();
            sqliteDb.WriteToDbAsync(nameProtocol, dataLog, $"Data Source={distStorage}", EnumProviderDb.SqLite);
        }

        public bool ViewLogFromSqLite(string distStorage = "")
        {
            distStorage = CheckIsNullOrEmptyDistStorage(distStorage, GetGlobalDistStorage("globalDistStorageSqLite"));

            var sqliteDb = new DbManager();
            var logsModel = sqliteDb.ViewDb($"Data Source={distStorage}", EnumProviderDb.SqLite);
            if (!logsModel.Any())
                return false;
            foreach (var logMod in logsModel)
            {
                Console.WriteLine(logMod.NameProtocol + ":" + logMod.DataLog);
            }

            return true;
        }
    }
}
