using System;
using System.IO;
using System.Text;

namespace AppPinger
{
    public partial class SaveLogs
    {
        public string GlobalDistStorage { get; set; }
        public string GlobalDistStorageSqLite { get; set; }

        public async void WriteLogAsyncToFile(string dataLog, string distStorage = "")
        {
            distStorage = CheckIsNullOrEmptyDistStorage(distStorage, GlobalDistStorage);

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
                if (string.IsNullOrEmpty(GlobalDistStorageSqLite))
                {
                    throw new ArgumentNullException("distStorage", "Не указан путь сохранения логов!");
                }

                return globalDistStorage;
            }

            return distStorage;
        }
    }
}