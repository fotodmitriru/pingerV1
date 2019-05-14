using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AppPinger.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace AppPinger
{
    public partial class SaveLogs
    {
        private readonly Dictionary<string, string> _distStorage;
        private readonly IApplicationBuilder _appBuilder;
        private readonly IServiceCollection _serviceCollection;

        public SaveLogs(Dictionary<string, string> distStorage, IServiceCollection serviceCollection)
        {
            _distStorage = distStorage ??
                           throw new ArgumentNullException(nameof(distStorage), "Не указаны пути сохранения логов!");

            _serviceCollection = serviceCollection ??
                                 throw new ArgumentNullException(nameof(serviceCollection));

            var serviceProvider = _serviceCollection.BuildServiceProvider() ??
                                  throw new ArgumentNullException(nameof(DbManager));
            _appBuilder = new ApplicationBuilder(serviceProvider);
        }

        public async void WriteLogAsyncToFile(string dataLog, string distStorage = "")
        {
            distStorage = CheckIsNullOrEmptyDistStorage(distStorage, GetGlobalDistStorage("globalDistStorage"));

            try
            {
                bool fExists = File.Exists(distStorage);
                using (StreamWriter sw = new StreamWriter(distStorage, fExists, Encoding.Default))
                {
                    await sw.WriteLineAsync(dataLog);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Файл логов не удалось сохранить!", e);
            }
        }

        public string CheckIsNullOrEmptyDistStorage(string distStorage, string globalDistStorage)
        {
            if (string.IsNullOrEmpty(distStorage))
            {
                if (string.IsNullOrEmpty(globalDistStorage))
                {
                    throw new ArgumentNullException("distStorage", "Не указан путь сохранения логов!");
                }

                return globalDistStorage;
            }

            return distStorage;
        }

        public string GetGlobalDistStorage(string nameDistStorage)
        {
            if (string.IsNullOrEmpty(nameDistStorage))
                throw new NullReferenceException("Укажите имя атрибута!");

            return (_distStorage.ContainsKey(nameDistStorage)) ? _distStorage[nameDistStorage] : "";
        }

    }
}