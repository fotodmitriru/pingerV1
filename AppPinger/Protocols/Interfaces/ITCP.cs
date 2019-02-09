using System;
using System.Collections.Generic;
using System.Text;

namespace AppPinger.Protocols.Interfaces
{
    interface ITCP : IPingProtocol
    {
        int Port { get; set; }
    }
}
