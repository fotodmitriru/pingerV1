using System;
using System.IO;
using System.Text;

namespace AppPinger
{
    class SaveLogs
    {
        public static string GlobalDistStorage { get; set; }

        public static async void WriteLogAsync(string dataLog, string distStorage = "")
        {
            if (string.IsNullOrEmpty(distStorage))
            {
                if (string.IsNullOrEmpty(GlobalDistStorage))
                {
                    throw new ArgumentNullException("distStorage", "Не указан путь сохранения логов!");
                }

                distStorage = GlobalDistStorage;
            }

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
    }
}
