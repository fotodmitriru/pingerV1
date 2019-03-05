using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppPinger.Protocols.Interfaces;
using Newtonsoft.Json;

namespace AppPinger.Protocols.Implements
{
    public class ListConfigProtocols: IListConfigProtocols
    {
        public IList<ConfigProtocol> ListConfProtocols { get; set; }
        public bool ReadConfig(string distSource)
        {
            if (!File.Exists(distSource))
                return false;

            var json = File.ReadAllText(distSource);
            try
            {
                var jsonDeser = JsonConvert.DeserializeObject<IList<ConfigProtocol>>(json);
                ListConfProtocols = jsonDeser;
            }
            catch (Exception e)
            {
                throw new NullReferenceException("Ошибка! Проверьте формат файла настроек!");
            }

            return ListConfProtocols.Any();
        }
    }

}
