using System;
using System.Collections.Generic;
using System.Text;

namespace AppPinger.Protocols.Interfaces.Implements
{
    class HTTP : IHTTP
    {
        public string Host { get; set; }
        public int Period { get; set; }
        public string ReplyLog { get; set; }
        public IConfigProtocol ConfigProtocol { get; set; }

        public bool StartPing(IConfigProtocol confProtocol)
        {
            throw new NotImplementedException();
        }

        public event DelegatePingCompleted PingCompleted;
        public int ValidCode { get; set; }
    }
}
