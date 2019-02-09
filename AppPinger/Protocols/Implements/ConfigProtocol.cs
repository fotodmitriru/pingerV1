using System.Xml.Serialization;
using AppPinger.Protocols.Interfaces;

namespace AppPinger.Protocols.Implements
{
    public class ConfigProtocol:IConfigProtocol
    {
        [XmlAttribute]
        public string Host { get; set; }

        [XmlAttribute]
        public int Period { get; set; }

        [XmlAttribute]
        public string NameProt { get; set; }

        [XmlAttribute]
        public string AdditionalAttribute { get; set; }
    }
}
