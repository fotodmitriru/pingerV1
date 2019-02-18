using System;

namespace AppPinger.Protocols.Interfaces.Implements
{
    class HTTP : IHTTP
    {
        public string Host { get; set; }
        public int Period { get; set; }
        public string ReplyLog { get; set; }

        public bool StartPing(ConfigProtocol confProtocol)
        {
            throw new NotImplementedException();
        }

        public event DelegatePingCompleted PingCompleted;
        public int ValidCode { get; set; }
    }
}
