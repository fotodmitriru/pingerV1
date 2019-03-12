using System;
using AppPinger.DB;

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

        public void ViewLogFromSqLite(string distStorage = "")
        {
            distStorage = CheckIsNullOrEmptyDistStorage(distStorage, GetGlobalDistStorage("globalDistStorageSqLite"));

            var sqliteDb = new DbManager();
            sqliteDb.ViewDb($"Data Source={distStorage}", EnumProviderDb.SqLite);
        }
    }
}
