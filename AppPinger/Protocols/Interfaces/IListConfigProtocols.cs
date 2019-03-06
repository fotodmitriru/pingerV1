using System.Collections.Generic;

namespace AppPinger.Protocols.Interfaces
{
    public interface IListConfigProtocols
    {
        IList<ConfigProtocol> ListConfProtocols { get; set; }
        bool ReadConfig(string distSource);
        bool WriteConfig(string distSource);
    }
}
