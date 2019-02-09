using System;
using System.IO;
using System.Text;

namespace AppPinger.Protocols.Implements
{
    class SaveLogs
    {
        public static bool WriteLog(string distStorage, string dataLog)
        {
            try
            {
                bool fExists = File.Exists(distStorage);
                using (StreamWriter sw = new StreamWriter(distStorage, fExists, Encoding.Default))
                {
                    sw.WriteLine(dataLog);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return true;
        }
    }
}
