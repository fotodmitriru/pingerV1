using System.Collections.Generic;
using AppPinger.Protocols.Interfaces;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace AppPinger.Protocols.Implements
{
    class ListConfigProtocols: IListConfigProtocols
    {
        public IList<IConfigProtocol> ListConfProtocols { get; set; }
        public bool ReadConfig(string distSource)
        {
            if (!File.Exists(distSource))
                return false;
            using (Stream reader = new FileStream(distSource, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ConfigProtocol[]));
                ListConfProtocols = ((ConfigProtocol[])serializer.Deserialize(reader));
            }

            return ListConfProtocols.Any();
        }

        public void WriteConfig(string distSource)
        {
            var confProt = new ConfigProtocol();
            confProt.Host = "ya.ru";
            confProt.Period = 1;
            confProt.AdditionalAttribute = "111";
            confProt.NameProt = "ICMP";

            ListConfProtocols = new List<IConfigProtocol>();
            ListConfProtocols.Add(confProt);
            using (Stream writer = new FileStream(distSource, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ConfigProtocol[]));
                serializer.Serialize(writer,
                    ListConfProtocols.Select(x => new ConfigProtocol() { Host = x.Host,
                        Period = x.Period , AdditionalAttribute = x.AdditionalAttribute,
                        NameProt = x.NameProt}).ToArray());
            }
        }
    }
}
