using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using AppPinger.Protocols.Interfaces;

namespace AppPinger.Protocols.Implements
{
    public class ListConfigProtocols: IListConfigProtocols
    {
        public IList<ConfigProtocol> ListConfProtocols { get; set; }
        public bool ReadConfig(string distSource)
        {
            if (!File.Exists(distSource))
                return false;

            using (Stream reader = new FileStream(distSource, FileMode.Open))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(IList<ConfigProtocol>));
                try
                {
                    ListConfProtocols = ser.ReadObject(reader) as IList<ConfigProtocol>;
                }
                catch (SerializationException e)
                {
                    throw new SerializationException("Проверьте формат файла JSON!", e);
                }
            }

            return (ListConfProtocols ?? throw new InvalidOperationException()).Any();
        }
        public bool WriteConfig(string distSource)
        {
            using (Stream writer = new FileStream(distSource, FileMode.Create))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IList<ConfigProtocol>));
                serializer.WriteObject(writer, ListConfProtocols);
            }

            return true;
        }
    }

}
