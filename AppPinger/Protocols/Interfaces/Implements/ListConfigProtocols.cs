using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace AppPinger.Protocols.Interfaces.Implements
{
    class ListConfigProtocols: IListConfigProtocols
    {
        public IList<ConfigProtocol> ListConfProtocols { get; set; }
        public bool ReadConfig(string distSource)
        {
            if (!File.Exists(distSource))
                return false;

            var json = File.ReadAllText(distSource);
            var jsonDeser = JsonConvert.DeserializeObject<IList<ConfigProtocol>>(json);
            ListConfProtocols = jsonDeser;

            return ListConfProtocols.Any();
        }
    }

}
