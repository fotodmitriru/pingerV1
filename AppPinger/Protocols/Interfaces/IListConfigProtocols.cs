using System.Collections.Generic;
using AppPinger.Protocols.Interfaces.Implements;

namespace AppPinger.Protocols.Interfaces
{
    public interface IListConfigProtocols
    {
        IList<ConfigProtocol> ListConfProtocols { get; set; }
        bool ReadConfig(string distSource);
    }
}
