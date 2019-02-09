using System;
using System.Collections.Generic;
using System.Text;

namespace AppPinger.Protocols.Interfaces
{
    interface IHTTP : IPingProtocol
    {
        int ValidCode { get; set; }
    }
}
