using System.Xml.Serialization;

namespace AppPinger.Protocols.Interfaces.Implements
{
    public class ConfigProtocol:IConfigProtocol
    {
        public string Host { get; set; }

        public int Period { get; set; }

        public string NameProt { get; set; }

        public string AdditionalAttribute { get; set; }
    }
}
