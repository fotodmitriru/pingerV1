using System;
using System.Collections.Generic;
using System.Text;

namespace AppPinger.Protocols.Interfaces
{
    public interface IPingProtocol
    {
        string Host { get; set; }
        int Period { get; set; }
        string ReplyLog { get; set; }
        bool StartPing();
    }
}
