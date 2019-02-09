using System.Collections.Generic;

namespace AppPinger.Protocols.Interfaces
{
    public interface IListConfigProtocols
    {
        IList<IConfigProtocol> ListConfProtocols { get; set; }
        bool ReadConfig(string distSource);
    }
}
